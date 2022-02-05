using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    public class OwnIniData : Common, INotifyPropertyChanged
    {
        public OwnIniData()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Der letzte Backup in Sekunden seit 1.1.1970
        /// </summary>
        public int lastBackup;
        public int LastBackup
        {
            get { return lastBackup; }
            set
            {
                lastBackup = value;
            }
        }
    }
}
