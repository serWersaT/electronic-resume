using System;

namespace WindowsFormsApp1
{
    class LOG
    {
        public void write(string msg)
        {
            DateTime currtime = DateTime.Now;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log.txt", true))
            {
                string tmptxt = String.Format("{0:dd.mm.yy hh:mm:ss} {1}", currtime, msg);
                file.WriteLine(tmptxt);
                file.Close();
            }
        }
    }
}
