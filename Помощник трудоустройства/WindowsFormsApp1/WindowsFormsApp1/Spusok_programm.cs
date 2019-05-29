using System;
using Microsoft.Win32;



namespace WindowsFormsApp1
{
    class Spisok_programm
    {
        LOG lg = new LOG();
        public string spisok()
        {
            try
            {
                string displayName;
                string spisok = "";
                RegistryKey key;
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string +
                        subkey.GetValue("InstallLocation") as string;
                    spisok += displayName + "\r\n";
                }
                return spisok;
            }
            catch (Exception e1)
            {
                lg.write("ошибка: " + e1);
                return "error";
            }
        }
    }
}
