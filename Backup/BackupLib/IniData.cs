using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    public class IniData : INotifyPropertyChanged
    {
        FileHandle fileHandle;
        
        const int PATHNUMBER = 10;
        /// <summary>
        /// Die maximale Anzahl der Pfade, die gesichert wird. Dies entspricht der Anzahl der Pfade auf dem Main Screen
        /// </summary>
        private int pathNumber;

        /// <summary>
        /// Die maximale Anzahl der Quellpfade. Dies entspricht der Anzahl der Pfade auf dem Main Screen
        /// </summary>
        public int PathNumber
        {
            get { return pathNumber; }
            //set { pathNumber = value;}
        }

        /// <summary>
        /// Hier wird das Backup gespeichert
        /// </summary>
        public string destPath;
        /// <summary>
        /// Hierhin wird das Backup gespeichert
        /// </summary>
        public string DestPath
        {
            get { return destPath; }
            set
            {
                destPath = value;
                string nameOfPath = nameof(DestPath);
                OnPropertyChanged(nameOfPath);
            }
        }



        /// <summary>
        /// Der Pfad der zu sichernden Daten
        /// </summary>
        public string[] sourcePath;
        /// <summary>
        /// Der Pfad der zu sichernden Daten
        /// </summary>
        public string[] SourcePath
        {
            get { return sourcePath; }
            set
            {
                for (int i = 0; i < pathNumber; i++)
                    SetSourcePath(i, value[i]);
            }
        }





        public IniData()
        {
            fileHandle = new FileHandle(this);


            this.pathNumber = PATHNUMBER;
            sourcePath = new string[pathNumber];
            isSomeInSourcePath = new bool[pathNumber];
            for (int i = 0; i < pathNumber; i++)
            {
                sourcePath[i] = "";
                isSomeInSourcePath[i] = false;
            }
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
        /// Der Text, der im Main Screen angezeigt wird
        /// </summary>
        public string actualHandle;

        /// <summary>
        /// Der Text, der im Main Screen angezeigt wird
        /// </summary>
        public string ActualHandle
        {
            get { return actualHandle; }
            set
            {
                actualHandle = value;
                OnPropertyChanged(nameof(ActualHandle));
            }
        }

        /// <summary>
        /// die Anzahl der Dateien, die Behalten werden soll
        /// </summary>
        public int numberOfGenerations;

        /// <summary>
        /// die Anzahl der Dateien, die Behalten werden soll
        /// </summary>
        public int NumberOfGenerations
        {
            get { return numberOfGenerations; }
            set
            {
                numberOfGenerations = value;
                if (numberOfGenerations > pathNumber)
                    numberOfGenerations = 10;
                if (numberOfGenerations < 1)
                    numberOfGenerations = 1;
                OnPropertyChanged(nameof(NumberOfGenerations));
            }
        }

        /// <summary>
        /// Interval der Sicherungen in Sekunden
        /// </summary>
        public int interval;
        /// <summary>
        /// Interval der Sicherungen in Sekunden
        /// </summary>
        public int Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                OnPropertyChanged(nameof(Interval));
            }
        }

        public void Load()
        {
            fileHandle.Load();
        }

        public void Save()
        {
            fileHandle.Save();
        }

        /// <summary>
        /// Interval der Sicherungen in Sekunden
        /// </summary>
        public int probier;
        /// <summary>
        /// Interval der Sicherungen in Sekunden
        /// </summary>
        public int Probier
        {
            get { return probier; }
            set
            {
                probier = value;
                OnPropertyChanged(nameof(Interval));
            }
        }
        #region SourcePath
        public string SourcePath0
        {
            get
            {
                return sourcePath[0];
            }
            set { SetSourcePath(0, value); }
        }

        public string SourcePath1
        {
            get
            {
                return sourcePath[1];
            }
            set { SetSourcePath(1, value); }
        }

        public string SourcePath2
        {
            get { return sourcePath[2]; }
            set { SetSourcePath(2, value); }
        }

        public string SourcePath3
        {
            get { return sourcePath[3]; }
            set { SetSourcePath(3, value); }
        }

        public string SourcePath4
        {
            get { return sourcePath[4]; }
            set { SetSourcePath(4, value); }
        }

        public string SourcePath5
        {
            get { return sourcePath[5]; }
            set { SetSourcePath(5, value); }
        }

        public string SourcePath6
        {
            get { return sourcePath[6]; }
            set
            {
                SetSourcePath(6, value);
            }
        }

        public string SourcePath7
        {
            get { return sourcePath[7]; }
            set { SetSourcePath(7, value); }
        }

        public string SourcePath8
        {
            get { return sourcePath[8]; }
            set { SetSourcePath(8, value); }
        }

        public string SourcePath9
        {
            get { return sourcePath[9]; }
            set { SetSourcePath(9, value); }
        }

        #endregion SourcePath

        #region IsSomeInSourcePath

        /// <summary>
        /// Gibt an, ob sich ein Eintrag im passenden SourcePath befindet
        /// </summary>
        public bool[] isSomeInSourcePath;
        /// <summary>
        /// Der Pfad der zu sichernden Daten
        /// </summary>
        public bool[] IsSomeInSourcePath
        {
            get { return isSomeInSourcePath; }
        }

        public bool IsSomeInSourcePath0
        {
            get
            {
                // probier
                return isSomeInSourcePath[0];
            }
        }

        public bool IsSomeInSourcePath1
        {
            get { return isSomeInSourcePath[1]; }
        }

        public bool IsSomeInSourcePath2
        {
            get { return isSomeInSourcePath[2]; }
        }

        public bool IsSomeInSourcePath3
        {
            get { return isSomeInSourcePath[3]; }
        }

        public bool IsSomeInSourcePath4
        {
            get { return isSomeInSourcePath[4]; }
        }

        public bool IsSomeInSourcePath5
        {
            get { return isSomeInSourcePath[5]; }
        }

        public bool IsSomeInSourcePath6
        {
            get { return isSomeInSourcePath[6]; }
        }

        public bool IsSomeInSourcePath7
        {
            get { return isSomeInSourcePath[7]; }
        }

        public bool IsSomeInSourcePath8
        {
            get { return isSomeInSourcePath[8]; }
        }

        public bool IsSomeInSourcePath9
        {
            get { return isSomeInSourcePath[9]; }
        }

        #endregion IsSomeInSourcePath
        public void SetSourcePath(int index, string pathName)
        {
            sourcePath[index] = pathName;
            isSomeInSourcePath[index] = pathName.Length > 0;
            string nameOfProperty = "SourcePath" + index.ToString();
            OnPropertyChanged(nameOfProperty);
            nameOfProperty = "IsSomeInSourcePath" + index.ToString();
            OnPropertyChanged(nameOfProperty);
        }
    }
}
