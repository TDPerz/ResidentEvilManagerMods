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

namespace ResidentEvilManagerMods.Paneles
{
    /// <summary>
    /// Lógica de interacción para Ajustes.xaml
    /// </summary>
    public partial class Ajustes : UserControl
    {
        public int result = -1;
        public string pathOri;

        public Ajustes(string path)
        {
            InitializeComponent();
            pathOri = path;
            pathDownload.Text = path;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            result = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            result = 0;
        }

        private void openFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathDownload.Text = folder.SelectedPath;
            }
        }
    }
}
