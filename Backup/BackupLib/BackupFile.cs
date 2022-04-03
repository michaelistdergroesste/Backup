using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupLib
{
    public class BackupFile
    {
        /// <summary>
        /// Hierhin wird das Backup gespeichert
        /// </summary>
        string destPath;

        // declaring an event using built-in EventHandler
        public delegate void delChange(int e);
        public event delChange Change;

        /// <summary>
        /// Hole eine Liste mit allen Namen, die zu dem Parameter source passen
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dirs"></param>
        private void GetListOfAllFiles(string source, out string[] dirs)
        {
            string lastItem = LastItem(source);
            string finePattern = "*" + lastItem + "*.zip";
            dirs = Directory.GetFiles(destPath, finePattern);
            Array.Sort(dirs);
        }

        private void DeleteFiles(string[] dirs, int leave)
        {
            // die Anzahl der zu löschenden Dateien
            int numberToDelete = dirs.Length - leave;
            for (int i = 0; i < numberToDelete; i++)
            {
                try { File.Delete(dirs[i]); }
                catch { }
            }
        }

        private void DeleteDir(string dir)
        {
            try
            {
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dir);
                SetAttributesNormal(dirInfo);
                Directory.Delete(dir, true);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        internal static void SetAttributesNormal(DirectoryInfo path)
        {
            // BFS folder permissions normalizer
            Queue<DirectoryInfo> dirs = new Queue<DirectoryInfo>();
            dirs.Enqueue(path);
            while (dirs.Count > 0)
            {
                var dir = dirs.Dequeue();
                dir.Attributes = FileAttributes.Normal;
                Parallel.ForEach(dir.EnumerateFiles(), e => e.Attributes = FileAttributes.Normal);
                foreach (var subDir in dir.GetDirectories())
                    dirs.Enqueue(subDir);
            }
        }


        /// <summary>
        /// Erzeuge den Name der Zip Datei abhaengig von dem Quellnamen und dem aktuellem Datum
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        private void CreateFileName(string source, out string dest)
        {
            DateTime localDate = DateTime.Now;
            string lastItem = LastItem(source);
            string timeFormat = localDate.ToString("yyMMddhhmmss");
            dest = destPath + "\\" + lastItem + "_" + timeFormat;
        }
        /// <summary>
        /// Hole den namen des tiefsten Verzeichnisses
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string LastItem(string source)
        {
            string[] arrayOfSource = source.Split('\\');
            return (arrayOfSource[arrayOfSource.Length - 1]);
        }



        /// <summary>
        /// Kopiere das Verzeichnis rekursiv
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private void DirectoryCopy(string dirCopySource, string dirCopyTarget)
        {
            //int posGit = dirCopySource.IndexOf(".git");
            //if (posGit >= 0)
            //{
            //    ;
            //}
            //else
            {
                // alle Unterverzeichnisse des aktuellen Verzeichnisses ermitteln
                string[] subDirectories = Directory.GetDirectories(dirCopySource);

                // Zielpfad erzeugen
                StringBuilder newTargetPath = new StringBuilder();
                {
                    newTargetPath.Append(dirCopyTarget);
                    newTargetPath.Append(dirCopySource.Substring(dirCopySource.LastIndexOf(@"\")));
                }

                // wenn aktueller Ordner nicht existiert -> ersstellen
                if (!Directory.Exists(newTargetPath.ToString()))
                    Directory.CreateDirectory(newTargetPath.ToString());


                // Unterverzeichnise durchlaufen und Funktion mit dazu gehörigen Zielpfad erneut aufrufen (Rekursion)
                foreach (string subDirectory in subDirectories)
                {
                    string newDirectoryPath = subDirectory;

                    // wenn ''/'' an letzter Stelle dann entfernen
                    if (newDirectoryPath.LastIndexOf(@"\") == (newDirectoryPath.Length - 1))
                        newDirectoryPath = newDirectoryPath.Substring(0, newDirectoryPath.Length - 1);

                    // rekursiever Aufruf
                    try { DirectoryCopy(newDirectoryPath, newTargetPath.ToString()); }
                    catch { }

                }


                // alle enthaltenden Dateien des aktuellen Verzeichnisses ermitteln
                string[] fileNames = Directory.GetFiles(dirCopySource);
                foreach (string fileSource in fileNames)
                {
                    // Zielpfad + Dateiname
                    StringBuilder fileTarget = new StringBuilder();
                    {
                        fileTarget.Append(newTargetPath);
                        fileTarget.Append(fileSource.Substring(fileSource.LastIndexOf(@"\")));
                    }
                    // Datei kopieren, wenn schon vorhanden überschreiben
                    File.Copy(fileSource, fileTarget.ToString(), true);
                }
            }
        }
        public bool IsPathNameValid(string source)
        {
            try
            {
                if (source == null)
                    return false;
                else if (source.Length < 5)
                    return false;
                else
                    return true;
            }
            catch
            { return false; }

        }
        /// <summary>
        /// Kopiere das zu sichernde Verzeichnis
        /// Zippe das Verzeichnis 
        /// Lösche das soeben kopierte Verzeichnis
        /// Lösche die alten Backupas
        /// </summary>
        /// <param name="source">das zu sichernde Verzeichnis</param>
        /// <param name="leave"></param>
        public void ZipFolder(string source, int leave)
        {
            if (IsPathNameValid(source))
            {

                try
                {
                    string[] dirs;
                    GetListOfAllFiles(source, out dirs);
                    OnChange(17); //No event data
                    string dest;
                    CreateFileName(source, out dest);
                    OnChange(33); //No event data
                    // Copy from the current directory, include subdirectories.
                    DirectoryCopy(source, dest);
                    OnChange(49); //No event data
                    // Mache ein Zip File vom soeben kopierten Verzeichnis
                    ZipFile.CreateFromDirectory(dest, dest + ".zip");
                    OnChange(64); //No event data
                    // Loesche dassoeben kopierten Verzeichnis nachdem es gezippt wurde
                    DeleteDir(dest);
                    OnChange(82); //No event data
                    // Lösche alle alten *.zip - Files
                    DeleteFiles(dirs, leave);
                    OnChange(100); //No event data
                }
                catch (Exception ex)
                { }

            }

        }


        public BackupFile(string destPath)
        {
            this.destPath = destPath;
        }

        private void OnChange(int number)
        {
            Change?.Invoke(number);
        }
    }
}
