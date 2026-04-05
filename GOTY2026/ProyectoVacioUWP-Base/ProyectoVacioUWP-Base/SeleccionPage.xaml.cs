using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoVacioUWP_Base
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SeleccionPage : Page
    {
        private List<PokemonSeleccion> _catalogo;
        private int _indexJ1 = 0; // Posición del selector J1
        private int _indexJ2 = 1; // Posición del selector J2
        private bool _listoJ1 = false;
        private bool _listoJ2 = false;

        public SeleccionPage()
        {
            this.InitializeComponent();
            CargarDatos();
            Window.Current.CoreWindow.KeyDown += OnKeyDown; // Escuchamos el teclado
            ActualizarVisual();
        }

        private void CargarDatos()
        {
            // Usamos la misma ruta de imagen para 'RutaCara' y 'RutaGrande'
            _catalogo = new List<PokemonSeleccion>
    {
        new PokemonSeleccion("Torchic",
                             "ms-appx:///Assets/torchic.png", // Cara
                             "ms-appx:///Assets/torchic.png", // Grande
                             typeof(TorchicJFBR)),

        new PokemonSeleccion("Computerror",
                             "ms-appx:///Assets/computerror.png",
                             "ms-appx:///Assets/computerror.png",
                             typeof(ComputerrorDLM)),

        new PokemonSeleccion("Ditto",
                             "ms-appx:///Assets/ditto.png",
                             "ms-appx:///Assets/ditto.png",
                             typeof(DittoAEJ))
    };
            gvCaras.ItemsSource = _catalogo;
        }

        private void ActualizarVisual()
        {
            // Jugador 1
            var p1 = _catalogo[_indexJ1];
            txtNombreJ1.Text = p1.Nombre;
            imgGrandeJ1.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(p1.RutaGrande));
            txtEstadoJ1.Text = _listoJ1 ? "¡LISTO PARA LUCHAR!" : "ELIGIENDO...";
            txtEstadoJ1.Foreground = _listoJ1 ? new SolidColorBrush(Windows.UI.Colors.Lime) : new SolidColorBrush(Windows.UI.Colors.White);

            // Jugador 2
            var p2 = _catalogo[_indexJ2];
            txtNombreJ2.Text = p2.Nombre;
            imgGrandeJ2.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(p2.RutaGrande));
            txtEstadoJ2.Text = _listoJ2 ? "¡LISTO PARA LUCHAR!" : "ELIGIENDO...";
            txtEstadoJ2.Foreground = _listoJ2 ? new SolidColorBrush(Windows.UI.Colors.Lime) : new SolidColorBrush(Windows.UI.Colors.White);
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (_listoJ1 && _listoJ2) return; // Si ambos están listos, ya no se mueve nada

            // --- CONTROL JUGADOR 1 (A / D para mover, Espacio para confirmar) ---
            if (!_listoJ1)
            {
                if (e.VirtualKey == VirtualKey.A && _indexJ1 > 0) _indexJ1--;
                if (e.VirtualKey == VirtualKey.D && _indexJ1 < _catalogo.Count - 1) _indexJ1++;
                if (e.VirtualKey == VirtualKey.Space) _listoJ1 = true;
            }

            // --- CONTROL JUGADOR 2 (Flechas para mover, Enter para confirmar) ---
            if (!_listoJ2)
            {
                if (e.VirtualKey == VirtualKey.Left && _indexJ2 > 0) _indexJ2--;
                if (e.VirtualKey == VirtualKey.Right && _indexJ2 < _catalogo.Count - 1) _indexJ2++;
                if (e.VirtualKey == VirtualKey.Enter) _listoJ2 = true;
            }

            ActualizarVisual();
            VerificarComienzo();
        }


        private void VerificarComienzo()
        {
            if (_listoJ1 && _listoJ2)
            {
                // Creamos un objeto con los dos elegidos para pasárselo al combate
                var pareja = new List<Type> { _catalogo[_indexJ1].ClasePokemon, _catalogo[_indexJ2].ClasePokemon };
                Frame.Navigate(typeof(CombatePage), pareja);
            }
        }
    }
    public class PokemonSeleccion
    {
        public string Nombre { get; set; }
        public string RutaCara { get; set; }   // El PNG pequeño (ej: ditto_cara.png)
        public string RutaGrande { get; set; } // El PNG grande (ej: ditto_full.png)
        public Type ClasePokemon { get; set; } // Tu UserControl (ej: typeof(DittoAEJ))

        public PokemonSeleccion(string nombre, string cara, string grande, Type clase)
        {
            Nombre = nombre;
            RutaCara = cara;
            RutaGrande = grande;
            ClasePokemon = clase;
        }
    }
}
