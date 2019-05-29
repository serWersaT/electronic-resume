using System;
using System.Net.NetworkInformation;


namespace WindowsFormsApp1
{
    class enternet_ping
    {
        public bool status_enternet = false;
        string status = "";
        LOG lg = new LOG();

        public void ping()
        {           
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    status = "Статус подключения к сети интернет: Подключено";
                }
            }

            catch
            {
                status = "Статус подключения к сети интернет: Ошибка подключения";
                lg.write("нет подключения к сети");
            }
        }
        
        public string ret()
        {
            return status;
        }

    }
}
