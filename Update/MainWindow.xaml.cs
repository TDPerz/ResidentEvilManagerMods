using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Update
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string linkUpdate;
        private System.Windows.Forms.WebBrowser web;
        private WebClient cliente;

        public MainWindow(string arg)
        {
            InitializeComponent();
            linkUpdate = arg;
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
                string dirD = Environment.CurrentDirectory + @"\data";
                var resp = web.Document.GetElementById("downloadButton").GetAttribute("href");
                cliente.DownloadFileAsync(new Uri(resp), dirD + @"\Update.zip");
            }
            catch (Exception o)
            {
                Console.WriteLine("Ocurrio un error: \n"+ o);
            }
        }

        private void completo(object sender, AsyncCompletedEventArgs e)
        {
            progreso.IsIndeterminate = true;
            logs.Text = "Aplicando actualizacion";
            ZipFile.ExtractToDirectory(Environment.CurrentDirectory + @"\data\Update.zip", Environment.CurrentDirectory + @"\data");
            AplicarUpdate();
        }

        private void AplicarUpdate()
        {
            CopyAll(Environment.CurrentDirectory, Environment.CurrentDirectory + @"\data\Update");
            Directory.Delete(Environment.CurrentDirectory + @"\data\Update", true);
            File.Delete(Environment.CurrentDirectory + @"\data\Update.zip");
            System.Windows.Forms.MessageBox.Show("Actualizacion Instalada correctamente\nIniciando aplicacion", "Actualizacion completa", MessageBoxButtons.OK);
            ProcessStartInfo pros = new ProcessStartInfo(Environment.CurrentDirectory + @"\ResidentEvilManagerMods.exe");
            Process p = new Process();
            p.StartInfo = pros;
            p.Start();
            this.Close();
        }

        private void descargando(object sender, DownloadProgressChangedEventArgs e)
        {
            progreso.Value = e.ProgressPercentage;
            logs.Text = "Descargando actualizacion: " + e.ProgressPercentage + " % ";
        }

        private void CopyAll(string destDir, string source)
        {
            DirectoryInfo path = new DirectoryInfo(source);
            DirectoryInfo[] dirs = path.GetDirectories();
            foreach(var d in dirs)
            {
                string dest = System.IO.Path.Combine(source, d.Name);
                Console.WriteLine("Ruta : " + destDir + @"\" + d.Name);
                if (!Directory.Exists(destDir+@"\"+d.Name))
                {
                    Directory.CreateDirectory(destDir + @"\" + d.Name);
                }
                CopyAll(destDir + @"\" + d.Name, dest);
            }
            FileInfo[] file = path.GetFiles();
            foreach(var f in file)
            {
                string fpath = System.IO.Path.Combine(destDir, f.Name);
                f.CopyTo(fpath, true);
            }
        }

        public void StartUpdate()
        {
            web.Navigate(linkUpdate);
        }
    }
}
