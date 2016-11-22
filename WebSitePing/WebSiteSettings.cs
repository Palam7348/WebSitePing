using System.Windows.Forms;


namespace WebSitePing
{
    class WebSiteSettings
    {
        private int interval;
        private int timeout;
        private int numberOfErrors;
        private string url;
        private string errorText;
        private string repairedText;
        private Notification notificationSettings;

        private const int oneSecond = 1000;

        #region Properies
        public int Interval { get { return interval; } set { interval = value; } }
        public int Timeout { get { return timeout; } set { timeout = value; } }
        public int NumberOfErrors { get { return numberOfErrors; } private set { numberOfErrors = value; } }
        public string Url { get { return url; } set { url = value; } }
        public string ErrorText { get { return errorText; } set { errorText = value; } }
        public string RepairedText { get { return repairedText; } set { repairedText = value; } }
        public Notification NotificationSettings { get { return notificationSettings; } set { notificationSettings = value; } }

        #endregion

        public WebSiteSettings(int interval, int timeout, string url, string errorText, string repairedText, bool messagebox, bool console, string email)
        {
            this.interval = interval;
            this.timeout = timeout;
            this.numberOfErrors = 0;
            this.url = url;
            this.errorText = errorText;
            this.repairedText = repairedText;
            this.notificationSettings = new Notification { MessageBox = messagebox, ConsoleAndSound = console, Email = email };
        }

        public WebSiteSettings(string url)
        {
            this.interval = 300 * oneSecond;
            this.timeout = 7 * oneSecond;
            this.numberOfErrors = 0;
            this.url = url;
            this.errorText = "ERROR has been occurred at the website";
            this.repairedText = "Website has been repaired";
            this.notificationSettings = new Notification { MessageBox = true, ConsoleAndSound = false, Email = null };
        }

        public void IncrementErrorCounter()
        {
            numberOfErrors++;
        }

        public void SendNotification(string type)
        {
            if (type.Equals("ERROR"))
            {
                if (notificationSettings.MessageBox)
                {
                    MessageBox.Show(errorText);
                }
                if (notificationSettings.ConsoleAndSound)
                {
                    System.Console.WriteLine(errorText);
                    System.Console.Beep();
                }
                if (notificationSettings.Email != null)
                {
                    /* http://stackoverflow.com/questions/449887/sending-e-mail-using-c-sharp */
                }
            }
            if (type.Equals("REPAIRED"))
            {
                if (notificationSettings.MessageBox)
                {
                    MessageBox.Show(repairedText);
                }
                if (notificationSettings.ConsoleAndSound)
                {
                    System.Console.WriteLine(repairedText);
                    System.Console.Beep();
                }
                if (notificationSettings.Email != null)
                {
                    /* http://stackoverflow.com/questions/449887/sending-e-mail-using-c-sharp */
                }
            }
        }
    }
}
