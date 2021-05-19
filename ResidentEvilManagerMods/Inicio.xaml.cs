using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using MaterialDesignThemes.Wpf;

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : Window
    {
        struct path
        {
            public string Game { get; set; }
            public string Download { get; set; }
        }

        public Inicio()
        {
            InitializeComponent();
            DownloadPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+ @"\Downloads\ModRE4";
            GamePath.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\ResidentEvil4\ResidenEvild4.exe";
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if(folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DownloadPath.Text = folder.SelectedPath;
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog files = new OpenFileDialog();
            files.Filter = "Juego(.exe)|*.exe";
            if (files.ShowDialog() == true)
            {
                GamePath.Text = files.FileName;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var pa = System.Text.Json.JsonSerializer.Deserialize<path>(File.ReadAllText("path/path.json"));
            if (!(Directory.Exists(DownloadPath.Text)))
            {
                Directory.CreateDirectory(DownloadPath.Text);
            }
            pa.Download = DownloadPath.Text;
            if (File.Exists(GamePath.Text))
            {
                pa.Game = GamePath.Text;
                var texto = System.Text.Json.JsonSerializer.Serialize(pa);
                File.WriteAllText("path/path.json", texto);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                contenmensage.Content = "El juego no existe o no esta instalado\nInstala el juego antes de continuar";
                await Dialog.ShowDialog(GUiDialog);
            }
        }
    }
}
