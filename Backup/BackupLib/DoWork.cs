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
        static int noBackupThread = 8;
        private static Thread?[]? backupThread = new Thread?[noBackupThread];
        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        static bool doJob;
        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        public bool DoJob
        {
            get { return doJob; }   
            set { doJob = value; }
        }

        public DoWork()
        {
            for (int i = 0; i < noBackupThread; i++)
            {
                backupThread[i] = new Thread(backup);
                backupThread[i].Start(i);

            }

        }

        private void backup(object thradNumber)
        {
            while (true)
            {
                if (doJob)
                {


                    IniData iniData = new IniData();
                    FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
                    fileHandle.Load();
                    BackupFile backupFile = new BackupFile(iniData.destPath);

                    //int i = iniData.PathNumber;
                    int i = (int)thradNumber;
                    string sourcePath = iniData.SourcePath[i];
                    if (sourcePath.Length > 10)
                    {
                        backupFile.ZipFolder(sourcePath);
                    }

                    doJob = false;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
