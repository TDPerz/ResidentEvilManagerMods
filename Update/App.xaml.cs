using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Update
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var pa = System.Text.Json.JsonSerializer.Deserialize<VersionJson>(File.ReadAllText("data/update.json"));
                MainWindow main = new MainWindow(pa.download);
                main.Show();
                main.StartUpdate();
            }catch(Exception o)
            {
                MessageBox.Show("Algo salio mal: " + o);
                System.Windows.Application.Current.Shutdown();
            }
        }

        public struct VersionJson
        {
            public string version { get; set; }
            public string download { get; set; }
        }
    }
}
