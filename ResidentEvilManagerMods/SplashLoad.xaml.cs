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
using System.Windows.Shapes;

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para SplashLoad.xaml
    /// </summary>
    public partial class SplashLoad : Window
    {
        public SplashLoad()
        {
            InitializeComponent();
        }

        public void setMenssage(string mensaje) { logs.Text = mensaje; }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
