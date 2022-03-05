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
        static int noBackupThread = 2;
        private static Thread?[]? backupThread;
        private static bool[] doSingleJob;
        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        static bool doJob;
        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        public bool DoJob
        {
            get 
            {

                doJob = false;
                for (int i = 0; i < noBackupThread; i++)
                    doJob &= doSingleJob[i];               
                return doJob; 
            }   
            set 
            { 
                doJob = value;
                for (int i = 0; i < noBackupThread; i++)
                    doSingleJob[i] = doJob;
            }
        }

        public DoWork()
        {
            backupThread = new Thread?[noBackupThread];
            for (int i = 0; i < noBackupThread; i++)
            {
                backupThread[i] = new Thread(backup);
                backupThread[i].Start(i);
                doSingleJob = new bool[noBackupThread];
                doSingleJob[i] = false;

            }

        }

        private void backup(object thradNumber)
        {
            while (true)
            {
                int number = (int)thradNumber;
                if (doSingleJob[number])
                {


                    IniData iniData = new IniData();
                    FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
                    fileHandle.Load();
                    BackupFile backupFile = new BackupFile(iniData.destPath);

                    
                    for (int i = number; i < iniData.PathNumber; i += noBackupThread)
                    {
                        if (iniData.SourcePath[i].Length > 10)
                        {
                            iniData.ActualHandle = "backup " + iniData.SourcePath[i];
                            backupFile.ZipFolder(iniData.SourcePath[i]);
                        }
                    }
                    doSingleJob[number] = false;
                    
                }
                Thread.Sleep(1000);
            }
        }
    }
}
