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
using System.Collections.ObjectModel;
using System.Threading;

namespace UsuariosAsain
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlUsuario controlUsuario;
        private ViewModel modeloVista;

        public MainWindow()
        {
            modeloVista = (ViewModel)FindResource("modeloVista");
            controlUsuario = (ControlUsuario)FindResource("controlUsuario");
            InitializeComponent();
        }

        private void DniColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            controlUsuario.ReordenarListaDeUsuarios(Criterio.Dni);
        }

        private void NombreColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            controlUsuario.ReordenarListaDeUsuarios(Criterio.Nombre);
        }


    }
}
