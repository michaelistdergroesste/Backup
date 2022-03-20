using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    public class FileHandle : Common
    {
        /// <summary>
        /// Der Pfad, wo die Daten abgelegt werden
        /// </summary>
        string currentDirectory;
        /// <summary>
        /// Hier stehen allle Daten drin die für das Backup gebraucht werden. Beispiel: Dateipfade, Anzahl Sicherungen, usw
        /// </summary>
        IniData iniData;
        /// <summary>
        /// Die Routienen zum schreiben und Lesen des Ini - Files
        /// </summary>
        IniFile iniFile;
        /// <summary>
        /// Behandelt das Schreiben und Lesen des Inifiles
        /// </summary>
        /// <param name="iniData">die Klasse mit den Inidaten die geschrieben oder gelesen werden soll</param>
        /// <param name="pathNumber">die maximale Anzahl der Pfade die gesichert werden soll</param>
        public FileHandle(IniData iniData, int pathNumber)
        {
            this.iniData = iniData;
            this.pathNumber = pathNumber;

            currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = currentDirectory + "\\backup.ini";
            iniFile = new IniFile(filePath);


        }




        public void Save()
        {
            DateTime localDate = DateTime.Now;
            string headder = localDate.ToString("dd.MM.yyyy HH:mm");
            iniFile.IniWriteValue("Date", headder);
            iniFile.IniWriteValue("destPath", iniData.DestPath);
            iniFile.IniWriteValue("numberOfSavings", iniData.NumberOfGenerations);
            iniFile.IniWriteValue("Interval", iniData.Interval);
            iniFile.IniWriteValue("Probier", iniData.Probier);
            for (int i = 0; i < pathNumber; i++)
                iniFile.IniWriteValue("sourceSave" + i.ToString(), iniData.SourcePath[i]);
        }

        public void Load()
        {
            iniData.DestPath = iniFile.IniReadValue("destPath");
            int number;
            iniFile.IniReadValue("numberOfSavings", out number);
            iniData.NumberOfGenerations = number;
            string[] sourcePath = new string[this.pathNumber];
            for (int i = 0; i < this.pathNumber; i++)
                sourcePath[i] = iniFile.IniReadValue("sourceSave" + i.ToString());
            iniData.SourcePath = sourcePath;
            iniFile.IniReadValue("Interval", out number);
            iniData.Interval = number;
            iniFile.IniReadValue("Probier", out number);
            iniData.Probier = number;
        }

        /// <summary>
        /// Schreibe die aktuelle Zeit um zu wissen, wann das letze Backup gemacht wurde.
        /// </summary>
        internal void WriteLastStoreTime()
        {
            DateTime localDate = DateTime.Now;
            long timeInSecounds = localDate.Ticks / 10000000;
            iniFile.IniWriteValue("LastStore", timeInSecounds.ToString());

        }
    }
}
