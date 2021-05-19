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

namespace ResidentEvilManagerMods
{
    /// <summary>
    /// Lógica de interacción para Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        private int id;
        Mod mod;

        public Item(Object modOb, int idS)
        {
            InitializeComponent();
            id = idS;
            mod = (Mod)modOb;
            this.DataContext = mod;
            try
            {
                if (mod.descripcion.Length > 50)
                {
                    Descripcion_s.Text = mod.descripcion.Substring(0, 50) + "...";
                }
                else
                {
                    Descripcion_s.Text = mod.descripcion;
                }
                Console.WriteLine("Listo con la descripcion");
                if (mod.titulo.Length > 30)
                {
                    Titulo.Text = mod.titulo.Substring(0, 30) + "...";
                }
                else
                {
                    Titulo.Text = mod.titulo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
