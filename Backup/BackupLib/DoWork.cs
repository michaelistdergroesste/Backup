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

        /// <summary>
        /// die Anzahl der gültigen Verzeichniseinträge = die Anzahl der Jobs
        /// </summary>
        int numberOfJobs = 0;

        // declaring an event using built-in EventHandler
        //public event EventHandler Change;

        public delegate void delChange(int e);
        public event delChange Change;

        BackupFile backupFile;
        

        public bool DoJob
        {
            get { return doJob; }   
            set { doJob = value; }
        }

        

        public DoWork()
        {
            this.iniData = new IniData();
            FileHandle fileHandle = new FileHandle(iniData);
            fileHandle.Load();
            fileHandle.WriteLastStoreTime();

            backupFile = new BackupFile(iniData.destPath);
            backupFile.Change += doBackupChange; // register with an event

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
                double divident = (double)jobCounter / 6.0 + 1.0;
                double dRetVal = divident / (double)iniData.PathNumber * 100.0;
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
                        for (jobCounter = 0; jobCounter < iniData.PathNumber; jobCounter++)
                        {
                            string sourcePath = iniData.SourcePath[jobCounter];
                            if (backupFile.IsPathNameValid(sourcePath))
                            {
                                numberOfJobs++;
                            }
                        }
                        for (jobCounter = 0; jobCounter < iniData.PathNumber; jobCounter++)
                        {
                            OnChange(jobCounter); //No event data
                            iniData.ActualHandle = "backup " + iniData.SourcePath[jobCounter];
                            backupFile.ZipFolder(iniData.SourcePath[jobCounter], iniData.NumberOfGenerations);

                        }
                        doJob = false;
                        OnChange(100); //No event data
                        


                    }
                }
                catch 
                {
                    doJob = false;
                }
                Thread.Sleep(1000);
            }
        }

        private void doBackupChange(int e)
        {
            double val = ((double)jobCounter + (double)e / 100.0) / (double)numberOfJobs;
            OnChange(val); 
        }

        private void OnChange(double e)
        {
            int valInPercent = (int)(e * 100.0);
            if (valInPercent > 100)
                valInPercent = 100;
            Change?.Invoke(valInPercent);
        }
    }
}
