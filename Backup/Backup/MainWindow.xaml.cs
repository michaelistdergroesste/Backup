using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Backup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Jeder Pfad hat ein Tag. Der Pfad, wo die Daten abgelegt werden hat diese Nummer. 
        /// </summary>
        const int destinationTag = -1;


        const int borderRight = 90;
        const int borderHight = 34;
        /// <summary>
        /// Die Texte im Mainpanel die vor den Quell - Pfaden stehen
        /// </summary>
        System.Windows.Controls.Label[] labelSource;

        System.Windows.Data.Binding[] bindingSource;
        /// <summary>
        /// Der Text im Mainpanel der vor der Auswahlbox für den Zielpfad steht
        /// </summary>
        System.Windows.Controls.Label labelDestination;

        public string Text { get; set; }

        /// <summary>
        /// Hier stehen allle Daten drin die für das Backup gebraucht werden. Beispiel: Dateipfade, Anzahl Sicherungen, usw
        /// </summary>
        public IniData iniData { get; set; }

        /// <summary>
        /// Dateihandler zum Speichern und lesen des INI - Files
        /// </summary>
        FileHandle fileHandle;

        /// <summary>
        /// True, wenn jemand auf einen der Knoepfe gedrückt hat. In diesem Fall wird beim Beenden gefragt, ob die Önderungen gespeiuchert werden sollen.
        /// </summary>
        bool changeSome = false;


        Thread newThread;
        Thread[] newThread2;
        ProcessTimer w;

        public MainWindow()
        {
            InitializeComponent();


            this.iniData = new IniData();

            labelSource = new System.Windows.Controls.Label[iniData.PathNumber];
            bindingSource = new System.Windows.Data.Binding[iniData.PathNumber];

            fileHandle = new FileHandle(iniData, iniData.PathNumber);
            fileHandle.Load();

            CreateFolderBackup();
            CreateFolderList();
            CreateBorderSettings();


            this.DataContext = iniData;

            iniData.ActualHandle = "Sichern von Daten ";

            progressBar.Value = 0;
            w = new ProcessTimer();
            w.fertig += new ProcessTimer.berechnungFertig(DoSome);


        }

        private void IncreaseProcessBar()
        {
            progressBar.Value += 20;
            if (progressBar.Value >= 100)
                buttonStart.IsEnabled = true;
        }

        private void DoSome(int e)
        {
            Dispatcher.BeginInvoke(new Action(IncreaseProcessBar));
        }




        #region crerateUserElements

        /// <summary>
        /// Erzeuge das Eingabefeld ganz oben mit dem Ziel - Verzeichnis
        /// </summary>
        private void CreateFolderBackup()
        {
            var border = CreateBoarder(4);
            GridBackupFolder.Children.Add(border);

            labelDestination = CreateLabel(destinationTag);
            border.Child = labelDestination;

            var folderButton = CreateFolderButton(10, destinationTag);
            GridBackupFolder.Children.Add(folderButton);

            var binding = new System.Windows.Data.Binding("DestPath");
            binding.Source = iniData;
            labelDestination.SetBinding(System.Windows.Controls.Label.ContentProperty, binding);
        }

        /// <summary>
        /// Den Rahmen für die Settings machen (2. von oben)
        /// </summary>
        private void CreateBorderSettings()
        {
            SetBorderSettings(BorderSettings, 45);
        }


        /// <summary>
        /// Erzeuge das UI für die Eingabe der Quell - Verzeichnisse
        /// </summary>
        private void CreateFolderList()
        {
            for (int i = 0; i < iniData.PathNumber; i++)
            {
                var border = CreateBoarder(4 + i * 40);
                GridFolder.Children.Add(border);

                labelSource[i] = CreateLabel(i);
                labelSource[i].Name = "LabelBackup";
                border.Child = labelSource[i];

                var folderButton = CreateFolderButton(10 + i * 40, i);
                GridFolder.Children.Add(folderButton);
                var deleteButton = CreateDeleteButton(10 + i * 40, i);
                GridFolder.Children.Add(deleteButton);
                string pathName = "SourcePath" + i.ToString();
                var binding = new System.Windows.Data.Binding(pathName);
                binding.Source = iniData;
                labelSource[i].SetBinding(System.Windows.Controls.Label.ContentProperty, binding);
            }
        }

        System.Windows.Controls.Label CreateLabel(int tag)
        {
            var label = new System.Windows.Controls.Label();
            label.Tag = tag;
            label.Content = "Verzeichnis";

            return label;
        }

        /// <summary>
        /// Erzeuge den Rahmen um das Verzeichnis
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        Border CreateBoarder(int top)
        {
            var border = new Border();
            SetBorderSettings(border, top);

            return border;
        }

        void SetBorderSettings(Border border, int top)
        {
            border.BorderBrush = System.Windows.Media.Brushes.Black;
            border.BorderThickness = new System.Windows.Thickness(1);
            border.Height = borderHight;
            border.Margin = new Thickness(4, top, borderRight, 0);
            border.VerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// Erzeuge den Button um die neuen Verzeichnisse auszuwählen
        /// </summary>
        /// <param name="top">Y - Position des Buttons</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        System.Windows.Controls.Button CreateFolderButton(int top, int tag)
        {
            var button = new System.Windows.Controls.Button();
            button.Tag = tag;
            button.Content = "...";
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            button.Width = 29;
            button.Margin = new Thickness(4, top, 44, 0);
            button.Click += new System.Windows.RoutedEventHandler(ButtonCreateFolder_Click);

            return button;
        }

        /// <summary>
        /// Erzeuge den Button um ein ausgewähltes Verzeichnis zu entfernen
        /// </summary>
        /// <param name="top">Y - Position des Buttons</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        System.Windows.Controls.Button CreateDeleteButton(int top, int tag)
        {
            int number = tag;
            bool isSelected = false;
            isSelected = iniData.SourcePath[number].Length > 0;

            var button = new System.Windows.Controls.Button();
            button.Tag = tag;
            button.Content = "X";
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            button.IsEnabled = true;
            button.Width = 29;
            button.Margin = new Thickness(34, top, 4, 0);
            button.Click += new System.Windows.RoutedEventHandler(ButtonDeleteFolder_Click);
            string pathName = "IsSomeInSourcePath" + number.ToString();
            var binding = new System.Windows.Data.Binding(pathName);
            binding.Source = iniData;
            button.SetBinding(System.Windows.Controls.Button.IsEnabledProperty, binding);

            return button;
        }


        #endregion crerateUserElements

        /// <summary>
        /// Auswahl der Pfade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;

            int number = (int)button.Tag;

            string foldername;
            foldername = "Verzeichnisname für Backup (" + number.ToString() + ")";

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                string selectedPath = fbd.SelectedPath;

                if (number == destinationTag)
                    iniData.DestPath = selectedPath;
                else
                {
                    switch ((int)number)
                    {
                        case 0:
                            iniData.SourcePath0 = selectedPath;
                            break;
                        case 1:
                            iniData.SourcePath1 = selectedPath;
                            break;
                        case 2:
                            iniData.SourcePath2 = selectedPath;
                            break;
                        case 3:
                            iniData.SourcePath3 = selectedPath;
                            break;
                        case 4:
                            iniData.SourcePath4 = selectedPath;
                            break;
                        case 5:
                            iniData.SourcePath5 = selectedPath;
                            break;
                        case 6:
                            iniData.SourcePath6 = selectedPath;
                            break;
                        case 7:
                            iniData.SourcePath7 = selectedPath;
                            break;
                        case 8:
                            iniData.SourcePath8 = selectedPath;
                            break;
                        case 9:
                            iniData.SourcePath9 = selectedPath;
                            break;
                        default:
                            break;

                    }

                    //iniData.SourcePath[number] = selectedPath;
                }

            }
            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }

        private void ButtonDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;

            int number = (int)button.Tag;

            switch ((int)number)
            {
                case 0:
                    iniData.SourcePath0 = "";
                    break;
                case 1:
                    iniData.SourcePath1 = "";
                    break;
                case 2:
                    iniData.SourcePath2 = "";
                    break;
                case 3:
                    iniData.SourcePath3 = "";
                    break;
                case 4:
                    iniData.SourcePath4 = "";
                    break;
                case 5:
                    iniData.SourcePath5 = "";
                    break;
                case 6:
                    iniData.SourcePath6 = "";
                    break;
                case 7:
                    iniData.SourcePath7 = "";
                    break;
                case 8:
                    iniData.SourcePath8 = "";
                    break;
                case 9:
                    iniData.SourcePath9 = "";
                    break;
                default:
                    break;

            }

            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }

        /// <summary>
        /// Inkrementieren oder decrementieren der Anzahl der Generationen
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buttonAdd = (System.Windows.Controls.Button)sender;
            if (buttonAdd.Tag.ToString() == "1")
                iniData.NumberOfGenerations = iniData.NumberOfGenerations + 1;
            if (buttonAdd.Tag.ToString() == "-1")
                iniData.NumberOfGenerations = iniData.NumberOfGenerations - 1;
            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.iniData.NumberOfGenerations = 8;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.fileHandle.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changeSome)
            {
                string message = "Backup";
                string caption = "Vor dem Schliessen speichern?";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                var result = System.Windows.MessageBox.Show(caption, message, buttons);
                if (result == MessageBoxResult.Yes)
                    this.fileHandle.Save();
            }
        }


        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;
            progressBar.Value = 0;
            for (int i = 0; i < iniData.PathNumber; i++)
                newThread = new Thread(w.Backup);
            newThread.Start();

        }



        private void Backup()
        {
            IniData iniData = new IniData();
            FileHandle fileHandle = new FileHandle(iniData, iniData.PathNumber);
            fileHandle.Load();
            BackupFile backupFile = new BackupFile(iniData.destPath);

            for (int i = 0; i < iniData.PathNumber; i++)
            {
                if (iniData.SourcePath[i].Length > 10)
                {
                    iniData.ActualHandle = "backup " + iniData.SourcePath[i];
                    backupFile.ZipFolder(iniData.SourcePath[i]);
                }

            }
        }

        private void ComboInterval_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }

        private void ComboInterval_Copy_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }

        private void ComboInterval_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Merken, dass auf dem Knopf gedrückt hat.
            changeSome = true;
        }
    }
}
