using BackupLib;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;



namespace BackupWork
{

    public delegate void emptyFunction();

    public partial class Form1 : Form
    {
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ReferenceConverter))]


        /// <summary>
        /// Zeit des letzten Speicherns in Sekunden
        /// </summary>
        long lastStore;
        /// <summary>
        /// Gewünschtes Intervall in Sekunden
        /// </summary>
        long interval;

        int progressBar;

        Thread workthread;

        DoWork doWork;

        string applicationPath;



        public Form1()
        {
            InitializeComponent();


            doWork = new DoWork();


            applicationPath = GetApplicationsPath();

            workthread = new Thread(new ThreadStart(work));
            doWork.Change += doWorkChange; // register with an event
            workthread.Start();

        }

        private void doWorkChange(int e)
        {
            UpdateProcessBar(e);
        }



        //remove the entire system menu:
        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
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

 
        /// <summary>
        /// Mache das Backup
        /// </summary>
        private void Backup()
        {
            doWork.DoJob = true;
            notifyIcon.BalloonTipText = "Backup wurde gestartet";
            notifyIcon.BalloonTipTitle = "Backup";
            notifyIcon.ShowBalloonTip(1000);
        }

        /// <summary>
        /// regelmaessig wird hier geprueft, ob ein neues Backup gemacht werden muss.
        /// </summary>
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
                {
                    Backup();
                }
                

            }
        }

        /// <summary>
        /// Thread um den Prozessbaklken upzudaten.
        /// </summary>
        private void UpdateProcessBar(int e)
        {


            try
            {
                int percent = e;

                this.Invoke(new emptyFunction(delegate ()
                {
                    label2.Text = percent.ToString();
                    progressBarSuccess.Value = percent;
                    button1.Enabled = ((percent <= 0) || (percent >= 100));
                }));
                // siehe https://www.computerbase.de/forum/threads/ungueltiger-threaduebergreifender-vorgang.1107200/
            }
            catch 
            {

            }

        }

        /// <summary>
        /// Abfragen, wie oft das Backup gemacht werden muss
        /// </summary>
        private void GetInterval()
        {
            IniData iniData = new IniData();
            iniData.Load();
            this.interval = (long)iniData.Interval;
        }



        #region laden und Speichern der Zeit des letzten Backups

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
            return (currentDirectory + "\\" + doWork.IniFileName);
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

        #region Klicks auf der Benutzeroberflaeche
        private void button1_Click(object sender, EventArgs e)
        {
            Backup();
        }

        private void notifyIcon1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                System.Diagnostics.Process.Start("BackupHmi.exe");
            if (e.Button == MouseButtons.Right)
                this.Show();
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        #endregion


    }
}