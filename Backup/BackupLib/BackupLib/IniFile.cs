using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    internal class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);

        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public void IniWriteValue(string Key, string Value)
        {
            WritePrivateProfileString("", Key, Value, this.path);
        }

        public void IniWriteValue(string Key, int Value)
        {
            WritePrivateProfileString("", Key, Value.ToString(), this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
        public string IniReadValue(string Key)
        {
            return IniReadValue("", Key);
        }
        public string IniReadValue(string Key, out int number)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString("", Key, "", temp, 255, this.path);
            Int32.TryParse(temp.ToString(), out number);
            return temp.ToString();
        }
        public string IniReadValue(string Key, out long number)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString("", Key, "", temp, 255, this.path);
            Int64.TryParse(temp.ToString(), out number);
            return temp.ToString();
        }
    }
}
