using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Timers;


namespace WebSitePing
{
    

    class CheckPageTimer
    {
        private int maxErrorsCount = 10;
        private string NotificationTypeError = "error";
        private string NotificationTypeRepaired = "repaired";
        private int oneHour = 60 * 1000;

        private System.Timers.Timer intervalTimer = new System.Timers.Timer();
        private System.Timers.Timer getFullHtmlPageTimer = new System.Timers.Timer();

        private WebSiteSettings settings;
        HtmlAgilityPack.HtmlDocument doc;

        Stopwatch timeoutTimer;
        

        public CheckPageTimer(bool setFullOrShortSettings)
        {
            if (setFullOrShortSettings)
            {
                setFullSetting();
            }
            else
            {
                setDafaultSetting();
            }

            doc = new HtmlDocument();
            intervalTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            intervalTimer.Interval = 5000;//settings.Interval;
            intervalTimer.Start();


            getFullHtmlPageTimer.Elapsed += GetFullHtmlPageTimer_Elapsed;
            getFullHtmlPageTimer.Interval = oneHour;
            getFullHtmlPageTimer.Start();

            timeoutTimer = new Stopwatch();
        }

        private void GetFullHtmlPageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                doc.LoadHtml(settings.Url);
                Console.WriteLine(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                settings.IncrementErrorCounter();
            }            
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                timeoutTimer.Start();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(settings.Url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                timeoutTimer.Stop();
                TimeSpan sp = timeoutTimer.Elapsed;
                if (response.StatusCode == HttpStatusCode.OK && sp.Seconds < settings.Timeout)
                {
                    Console.WriteLine("HEY IT'S OK");
                }
                else
                {
                    Console.WriteLine("ERROR");
                    settings.IncrementErrorCounter();
                    if (settings.NumberOfErrors >= maxErrorsCount) settings.SendNotification(NotificationTypeError);
                }
                
            }
            catch (Exception ex)
            {
                settings.IncrementErrorCounter();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                response.Close();
            }
           
        }    

        private void setFullSetting()
        {
            try
            {
                WebSiteSettings full;
                int interval = int.Parse(Console.ReadLine());
                int timeout = int.Parse(Console.ReadLine());
                string url = Console.ReadLine();
                string errorText = Console.ReadLine();
                string repairedText = Console.ReadLine();
                Console.Write("Enable messagebox notifications?: (yes/no)");
                string messageboxAnserw = Console.ReadLine();

                bool messageBoxNotification = (messageboxAnserw.Equals("yes")) ? true : false;

                Console.Write("Enable console and sound notifications?: (yes/no)");
                string consoleAnserw = Console.ReadLine();
                bool consoleAndSoundNotification = (consoleAnserw.Equals("yes")) ? true : false;

                string email = Console.ReadLine();

                full = new WebSiteSettings(interval, timeout, url, errorText,
                    repairedText, messageBoxNotification, consoleAndSoundNotification, email);
                this.settings = full;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private void setDafaultSetting()
        {
            try
            {
                WebSiteSettings defaultSettings;
                /*WHILE TESTING, URL WILL BE DEFAULT lesyadraw.ru*/
                /*Console.Write("Enter url: ");
                string url = Console.ReadLine();*/
                string url = "http://www.lesyadraw.ru/";
                defaultSettings = new WebSiteSettings(url);
                this.settings = defaultSettings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
        }
    }
}
