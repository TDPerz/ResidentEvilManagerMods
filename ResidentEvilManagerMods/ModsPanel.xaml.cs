using ResidentEvilManagerMods.Driver;
using ResidentEvilManagerMods.Estructuras;
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
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;
using System.Media;

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para ModsPanel.xaml
    /// </summary>
    public partial class ModsPanel : System.Windows.Controls.UserControl
    {
        private List<Mod> listMod;
        private string dirD;
        private GoogleDrive drive;
        private WebClient cliente;
        private System.Windows.Forms.WebBrowser web;
        private bool descargandoArchivo;

        public ModsPanel()
        {
            InitializeComponent();
            web = new System.Windows.Forms.WebBrowser();
            cliente = new WebClient();
            cliente.DownloadProgressChanged += new DownloadProgressChangedEventHandler(descargando);
            cliente.DownloadFileCompleted += new AsyncCompletedEventHandler(completo);
            web.ScriptErrorsSuppressed = true;
            web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(preparado);
        }

        private void preparado(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var resp = web.Document.GetElementById("downloadButton").GetAttribute("href");
                descargandoArchivo = true;
                cliente.DownloadFileAsync(new Uri(resp), dirD + @"\" + resp.Split('/')[resp.Split('/').Length-1]);
            }
            catch (Exception o)
            {
                Console.WriteLine(o);
            }
        }

        private void completo(object sender, AsyncCompletedEventArgs e)
        {
            descargandoArchivo = false;
            Progreso.Visibility = Visibility.Collapsed;
            OneButton.Visibility = Visibility.Visible;
            mensaje.Text = "Descarga Finalizada!";
        }

        private void descargando(object sender, DownloadProgressChangedEventArgs e)
        {
            contador.Content = e.ProgressPercentage;
            barraProgreso.Value = e.ProgressPercentage;
            Console.WriteLine(e.ProgressPercentage);
        }

        public void setData(string dirDownload, Object listMods, Object gd)
        {
            dirD = dirDownload;
            listMod = (List<Mod>)listMods;
            drive = (GoogleDrive)gd;
            drive = (GoogleDrive)gd;
            int a = 0;
            foreach (var i in listMod)
            {
                Item it = new Item(i, a);
                it.Margin = new Thickness(0);
                it.infoButton.Click += InfoButton_Click;
                it.Width = viewListPanel.Width;
                viewListPanel.Children.Add(it);
                a++;
            }
        }


        public async void OpenDrawHost(string men)
        {

        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            drawHostUi.DataContext = ((Mod)(((System.Windows.Controls.Button)sender).DataContext));
            Drawhost.IsRightDrawerOpen = true;
        }

        public async void openDialog(string men)
        {
            mensaje.Text = men;
            await log.ShowDialog(UILog);
        }

        public bool isOpenDialog()
        {
            return log.IsOpen;
        }

        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            YesNoStack.Visibility = Visibility.Collapsed;
            Progreso.Visibility = Visibility.Visible;
            log.IsOpen = true;
            web.Navigate(((Mod)drawHostUi.DataContext).link);
        }

        private void log_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!descargandoArchivo)
            {
                YesNoStack.Visibility = Visibility.Visible;
                Progreso.Visibility = Visibility.Collapsed;
                OneButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                eventArgs.Cancel();
                SystemSounds.Beep.Play();
            }
        }
    }
}
