﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    internal class FileHandle
    {

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
        public FileHandle(IniData iniData)
        {
            this.iniData = iniData;
            //this.pathNumber = iniData.PathNumber;

            string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = currentDirectory + "\\backup.ini";
            iniFile = new IniFile(filePath);



        }



        /// <summary>
        /// Speicher die Informationen was gesopeichert werden soll
        /// </summary>
        public void Save()
        {
            DateTime localDate = DateTime.Now;
            string headder = localDate.ToString("dd.MM.yyyy HH:mm");
            iniFile.IniWriteValue("Date", headder);
            iniFile.IniWriteValue("destPath", iniData.DestPath);
            iniFile.IniWriteValue("numberOfSavings", iniData.NumberOfGenerations);
            iniFile.IniWriteValue("Interval", iniData.Interval);
            iniFile.IniWriteValue("LastStore", iniData.LastStore);
            for (int i = 0; i < iniData.PathNumber; i++)
                iniFile.IniWriteValue("sourceSave" + i.ToString(), iniData.SourcePath[i]);
        }
        /// <summary>
        /// Lade die Informationen was gespeichert werden soll.
        /// </summary>
        public void Load()
        {
            iniData.DestPath = iniFile.IniReadValue("destPath");
            int number;
            iniFile.IniReadValue("numberOfSavings", out number);
            iniData.NumberOfGenerations = number;
            string[] sourcePath = new string[iniData.PathNumber];
            for (int i = 0; i < iniData.PathNumber; i++)
                sourcePath[i] = iniFile.IniReadValue("sourceSave" + i.ToString());
            iniData.SourcePath = sourcePath;
            iniFile.IniReadValue("Interval", out number);
            iniData.Interval = number;
            iniFile.IniReadValue("LastStore", out number);
            iniData.LastStore = number;
        }


    }
}
