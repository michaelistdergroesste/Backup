using BackupLib;
using System.Reflection;

namespace BackupBackground
{
    public partial class FormBackupBackground : Form
    {
        //Thread newThread;

        //DoWork doWork;

        //public FormBackupBackground()
        //{
        //    InitializeComponent();
        //}

        //private void Backup()
        //{
        //    doWork.DoJob = true;
        //    WriteLastStoreTime();
        //}

        //private void WriteLastStoreTime()
        //{
        //    long timeInSecounds = GetCurrentTime();
        //    string fileName = OwnIniFileName();
        //    IniFile ownIniFile = new IniFile(fileName);
        //    ownIniFile.IniWriteValue("LastStore", timeInSecounds.ToString());
        //}

        //private string OwnIniFileName()
        //{
        //    string currentDirectory = GetApplicationsPath();
        //    return (currentDirectory + "\\ownbackup.ini");
        //}

        //public string GetApplicationsPath()
        //{
        //    FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
        //    return fi.DirectoryName;
        //}

        //public long GetCurrentTime()
        //{
        //    DateTime localDate = DateTime.Now;
        //    return (localDate.Ticks / 10000000);
        //}


        //private void button1_Click(object sender, EventArgs e)
        //{

        //}
    }
}