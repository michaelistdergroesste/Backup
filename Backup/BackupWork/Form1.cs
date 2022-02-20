using BackupLib;
using System.Reflection;

namespace BackupWork
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Zeit des letzten Speicherns in Sekunden
        /// </summary>
        long lastStore;
        /// <summary>
        /// Gewünschtes Intervall in Sekunden
        /// </summary>
        long interval;

        int progressBar;

        Thread newThread;
        Thread backupThread;

        string applicationPath;

        /// <summary>
        /// true wenn das Backup läuft.
        /// </summary>
        bool doJob;

        public Form1()
        {
            InitializeComponent();


            applicationPath = GetApplicationsPath();

            newThread = new Thread(work);
            backupThread = new Thread(backup);
            newThread.Start();
            backupThread.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                ShowIcon = false;
                //Backup.Visible = true;
                //Backup.ShowBalloonTip(1000);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backup()
        {
            while (true)
            {
                if (doJob)
                {
             

                    IniData iniData = new IniData();
                    FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
                    fileHandle.Load();
                    BackupFile backupFile = new BackupFile(iniData.destPath);

                    for (int i = 0; i < iniData.PathNumber; i++)
                    {
                        if (iniData.SourcePath[0].Length > 10)
                        {
                            backupFile.ZipFolder(iniData.SourcePath[i]);
                        }
                    }
                    doJob = false;
                }
                Thread.Sleep(1000);
            }
        }

        private void work()
        {

            const int fiveMinutes = 5 * 60 * 1000;
            while (true)
            {
                Thread.Sleep(fiveMinutes);
                GetInterval();
                LoadLastStoreTime();
                long timeInSecounds = GetCurrentTime();
                long diff = timeInSecounds - this.lastStore;

                string text = "diff = " + diff.ToString();

                if (diff > this.interval)
                    StartBackup();

            }
        }

        private void StartBackup()
        {
            //Backup.BalloonTipText = "Backup wurde gestartet";
            //Backup.BalloonTipTitle = "Backup";
            //Backup.ShowBalloonTip(1000);

            doJob = true;
           
            WriteLastStoreTime();
        }

        private void GetInterval()
        {
            IniData iniData = new IniData();
            FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
            fileHandle.Load();
            this.interval = (long)iniData.Interval;
        }



        #region laden und Speichern der Zeit des letzten Backups
        private void WriteLastStoreTime()
        {
            long timeInSecounds = GetCurrentTime();
            string fileName = OwnIniFileName();
            IniFile ownIniFile = new IniFile(fileName);
            ownIniFile.IniWriteValue("LastStore", timeInSecounds.ToString());
        }

        private void LoadLastStoreTime()
        {
            long lastStore;
            string fileName = OwnIniFileName();
            IniFile ownIniFile = new IniFile(fileName);
            ownIniFile.IniReadValue("LastStore", out lastStore);
            this.lastStore = lastStore;
        }

        private string OwnIniFileName()
        {
            string currentDirectory = GetApplicationsPath();
            return (currentDirectory + "\\ownbackup.ini");
        }

        public string GetApplicationsPath()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            return fi.DirectoryName;
        }


        public long GetCurrentTime()
        {
            DateTime localDate = DateTime.Now;
            return (localDate.Ticks / 10000000);
        }



        #endregion // laden und Speichern der Zeit des letzten Backups

        private void button1_Click(object sender, EventArgs e)
        {
            StartBackup();
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {

        }
    }
}