namespace BackupWork
{
    internal static class Program
    {



        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frm = new Form1();   //einfach nur Form1 initalisieren und dann
            //Application.Run(new Form1());
            Application.Run();
        }
    }
}