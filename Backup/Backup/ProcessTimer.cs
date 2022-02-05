using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BackupLib;

namespace Backup
{
    internal class ProcessTimer
    {
        public delegate void berechnungFertig(int e);
        public event berechnungFertig fertig;

        public void DoWork()
        {

            //progressBar.Value = 0;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                fertig(0);
            }

        }

        public void Backup()
        {
            try
            {
                IniData iniData = new IniData();
                FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
                fileHandle.Load();
                BackupFile backupFile = new BackupFile(iniData.destPath);

                for (int i = 0; i < iniData.PathNumber; i++)
                {
                    fertig(0);
                    if (iniData.SourcePath[i].Length > 10)
                    {
                        iniData.ActualHandle = "backup " + iniData.SourcePath[i];
                        backupFile.ZipFolder(iniData.SourcePath[i]);
                    }


                }
            }
            catch { }

            fertig(0);
        }

        public void Backup2(object str)
        {
            IniData iniData = new IniData();
            FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
            fileHandle.Load();
            BackupFile backupFile = new BackupFile(iniData.destPath);
            string sourthPath = (string)str;
            iniData.ActualHandle = "backup " + sourthPath;
            backupFile.ZipFolder(sourthPath);
        }

    }
}
