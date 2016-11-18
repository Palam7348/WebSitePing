using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSitePing
{
    class Notification
    {
        private bool messageBox;
        private bool consoleAndSound;
        private string email;

        public bool MessageBox { get; set; }
        public bool ConsoleAndSound { get; set; }
        public string Email { get; set; }
    }
}
