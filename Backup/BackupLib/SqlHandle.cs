using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BackupLib
{
    internal class SqlHandle
    {
        /// <summary>
        /// Hier stehen allle Daten drin die für das Backup gebraucht werden. Beispiel: Dateipfade, Anzahl Sicherungen, usw
        /// </summary>
        IniData iniData;
        /// <summary>
        /// Die Routienen zum schreiben und Lesen des Ini - Files
        /// </summary>
        string filePath;
        string cs;

        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public SqlHandle(IniData iniData)
        {
            this.iniData = iniData;
            //this.pathNumber = iniData.PathNumber;

            string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = currentDirectory + "\\backup.db";
            


            cs = @"URI=file:" + filePath;

            CreateDb();
            Save();


        }

        /// <summary>
        /// Create database and table
        /// </summary>
        private void CreateDb()
        {
            if (!System.IO.File.Exists(filePath))
            {
                SQLiteConnection.CreateFile(filePath);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + filePath))
                {
                    sqlite.Open();
                    string sql = "create table backup(name varchar(20), value varchar(100))";
                    var cmd = new SQLiteCommand(sql, sqlite);
                    cmd.ExecuteNonQuery();

                }
            }
            else
            {
                //MessageBox.Show("Database cannot create");
                return;
            }

        }

        /// <summary>
        /// Speicher die Informationen was gesopeichert werden soll
        /// </summary>
        public void Save()
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            DateTime localDate = DateTime.Now;
            string name = "Datum";
            string value = localDate.ToString("dd.MM.yyyy HH:mm");
            string cmdText = "INSERT INTO backup(name, value) VALUES('" + name + "', '" + value + "')";
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
            con.Close();

            //DateTime localDate = DateTime.Now;
            //string headder = localDate.ToString("dd.MM.yyyy HH:mm");
            //iniFile.IniWriteValue("Date", headder);
            //iniFile.IniWriteValue("destPath", iniData.DestPath);
            //iniFile.IniWriteValue("numberOfSavings", iniData.NumberOfGenerations);
            //iniFile.IniWriteValue("Interval", iniData.Interval);
            //iniFile.IniWriteValue("LastStore", iniData.LastStore);
            //for (int i = 0; i < iniData.PathNumber; i++)
            //    iniFile.IniWriteValue("sourceSave" + i.ToString(), iniData.SourcePath[i]);
        }
    }
}
