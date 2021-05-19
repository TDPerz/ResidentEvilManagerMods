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
using MaterialDesignThemes.Wpf;
using ResidentEvilManagerMods.Driver;
using ResidentEvilManagerMods.Estructuras;
using ResidentEvilManagerMods.Paneles;
using System.Collections;
using System.Text.Json;
using System.IO;
using System.Media;

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path;
        Driver.GoogleDrive drive;
        private Ajustes setti;
        private bool resultData = false;
        struct pathStruct
        {
            public string Game { get; set; }
            public string Download { get; set; }
        }

        public MainWindow(string p, Object db, bool existChange)
        {
            InitializeComponent();
            path = p;
            drive = (Driver.GoogleDrive)db;
            Hashtable hash = drive.getTable();
            var doc = JsonDocument.Parse(drive.getPage(((Google.Apis.Drive.v3.Data.File)hash[0]).Id));
            JsonElement root = doc.RootElement;
            var MisMods = root.EnumerateArray();
            List<Mod> moders = new List<Mod>();
            while (MisMods.MoveNext())
            {
                var modTemp = JsonSerializer.Deserialize<Mod>(MisMods.Current.ToString());
                moders.Add(modTemp);
            }
            modsPanel.setData(path, moders, drive);
            setti = new Ajustes(p);
            setti.Cancelar.Command = DialogHost.CloseDialogCommand;
            setti.Aplicar.Command = DialogHost.CloseDialogCommand;
            Dialogos.DialogClosing += Dialogos_DialogClosing;
            if (existChange)
            {
                Dialogos.CloseOnClickAway = true;
                showChanges();
                File.Delete(Environment.CurrentDirectory + @"\data\Logs.txt");
            }
        }

        public async void showChanges()
        {
            mensaje.Text = "Lista de Cambios";
            Cambios.Text = File.ReadAllText(Environment.CurrentDirectory+@"\data\Logs.txt");
            VerCambios.Visibility = Visibility.Visible;
            await Dialogos.ShowDialog(UILog);
            VerCambios.Visibility = Visibility.Collapsed;
            Dialogos.CloseOnClickAway = false;
        }

        private async void Dialogos_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            switch (setti.result)
            {
                case 0:
                    if(setti.pathDownload.Text != setti.pathOri)
                    {
                        respuestasS_N.Visibility = Visibility.Collapsed;
                        BarradeProgreso.IsEnabled = true;
                        BarradeProgreso.Visibility = Visibility.Visible;
                        var pa = System.Text.Json.JsonSerializer.Deserialize<pathStruct>(File.ReadAllText("path/path.json"));
                        if (!(Directory.Exists(setti.pathDownload.Text)))
                        {
                            Directory.CreateDirectory(setti.pathDownload.Text);
                        }
                        pa.Download = setti.pathDownload.Text;
                        var texto = System.Text.Json.JsonSerializer.Serialize(pa);
                        File.WriteAllText("path/path.json", texto);
                        respuestasS_N.Visibility = Visibility.Visible;
                        BarradeProgreso.IsEnabled = false;
                        BarradeProgreso.Visibility = Visibility.Collapsed;
                    }
                    break;
                case 1:
                    if(setti.pathDownload.Text != setti.pathOri)
                    {
                        mensaje.Text = "Seguro que quieres cerrar sin guardar?";
                        true_button.Content = "si";
                        false_button.Content = "no";
                        await log.ShowDialog(UILog);
                        if (!resultData)
                        {
                            eventArgs.Cancel();
                        }
                        else
                        {
                            setti.result = -1;
                            setti.pathDownload.Text = setti.pathOri;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            resultData = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            resultData = false;
        }

        private async void Open_Settings(object sender, RoutedEventArgs e)
        {
            if (modsPanel.isOpenDialog())
            {
                SystemSounds.Beep.Play();
            }
            else
            {
                await Dialogos.ShowDialog(setti);
            }
        }

        private void Open_Info(object sender, RoutedEventArgs e)
        {
            if (modsPanel.isOpenDialog())
            {
                SystemSounds.Beep.Play();
            }
            else
            {
            }
        }
    }
}
