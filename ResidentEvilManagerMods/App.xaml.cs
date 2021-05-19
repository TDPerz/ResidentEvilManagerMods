using ResidentEvilManagerMods.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashLoad lo = new SplashLoad();
            lo.Show();
            lo.setMenssage("Verificando ruta de descarga...");
            var pa = System.Text.Json.JsonSerializer.Deserialize<path>(File.ReadAllText("path/path.json"));
            if (pa.Download == "none")
            {
                lo.Hide();
                if(new Inicio().ShowDialog() == true)
                {
                    lo.Show();
                }
                else
                {
                    Current.Shutdown();
                }
            }
            lo.setMenssage("Buscando Mods...");
            GoogleDrive go = new GoogleDrive();
            Hashtable page = go.getTable();
            lo.setMenssage("Buscando atualizacion...");
            var vers = System.Text.Json.JsonSerializer.Deserialize<Versiones>(go.getUpdate());
            if (getActualization(vers.version))
            {
                lo.setMenssage("Iniciando aplicacion...");
                new MainWindow(pa.Download, go, File.Exists(@"data\Logs.txt")).Show();
                lo.Close();
            }
            else
            {
                Console.WriteLine("existe? " + (File.Exists(Environment.CurrentDirectory + @"\Update.exe") ? "Si ": "no") );
                MessageBox.Show("Se ha encontrado una nueva version, Se iniiciara a descargar", "Actualizacion encontrada", MessageBoxButton.OK, MessageBoxImage.Information);
                ProcessStartInfo pros = new ProcessStartInfo(Environment.CurrentDirectory + @"\Update.exe");
                Process p = new Process();
                p.StartInfo = pros;
                p.Start();
                lo.Close();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private struct path
        {
            public string Game { get; set; }
            public string Download { get; set; }
        }

        public struct Versiones
        {
            public string version { set; get; }
            public string link { set; get; }
        }

        private bool getActualization(string versionNueva)
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(assem.Location);
            if(ver.FileVersion == versionNueva)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
