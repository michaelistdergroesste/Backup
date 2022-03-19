using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupLib
{
    public class DoWork
    {
        private static Thread backupThread;
        IniData iniData;
        /// <summary>
        /// Die Anzahl der erledigten Jobs
        /// </summary>
        private int jobCounter;
        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        static bool doJob;
        /// <summary>
        /// alter Prozentwert um zu erkennen, dass sich der Prozentwert geaendert hat.
        /// </summary>
        int oldPercent;

        // declaring an event using built-in EventHandler
        public event EventHandler Change;

        public bool DoJob
        {
            get { return doJob; }   
            set { doJob = value; }
        }

        

        public DoWork()
        {
            backupThread = new Thread(backup);
            backupThread.Start();
        }

        public int GetPercent()
        {
            int retVal = 0;

            if (iniData == null)
                retVal = 0;
            else if (iniData.PathNumber == 0)
                retVal = 0;
            else if (jobCounter == iniData.PathNumber)
                retVal = 100;
            else 
            {
                double dRetVal = ((double)jobCounter + 1.0) / (double)iniData.PathNumber * 100.0;
                retVal = (int)dRetVal;                
            }
            return retVal;
        }
        /// <summary>
        /// Der Tread, der das Backup macht.
        /// </summary>
        /// <param name="thradNumber">Eindeutige Nummer des Threads</param>
        private void backup()
        {
            while (true)
            {
                try
                {
                    if (doJob)
                    {
                        this.iniData = new IniData();
                        FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
                        fileHandle.Load();
                        BackupFile backupFile = new BackupFile(iniData.destPath);

                        for (jobCounter = 0; jobCounter < iniData.PathNumber; jobCounter++)
                        {
                            OnChange(EventArgs.Empty); //No event data
                            iniData.ActualHandle = "backup " + iniData.SourcePath[jobCounter];
                            backupFile.ZipFolder(iniData.SourcePath[jobCounter]);
                        }
                        doJob = false;
                        OnChange(EventArgs.Empty); //No event data

                    }
                }
                catch 
                {
                    doJob = false;
                }
                Thread.Sleep(1000);
            }
        }

        private void OnChange(EventArgs e)
        {
            Change?.Invoke(this, e);
        }
    }
}
