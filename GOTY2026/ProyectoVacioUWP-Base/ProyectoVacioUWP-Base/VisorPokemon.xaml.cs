using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoVacioUWP_Base
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class VisorPokemon : Page
    {
        private Type pokemonType;
        private Dictionary<string, string> typeColors = new Dictionary<string, string>()
            {
                { "fuego", "#F08030" },
                { "agua", "#6890F0" },
                { "planta", "#78C850" },
                { "eléctrico", "#F8D030" },
                { "volador", "#A890F0" },
                { "normal", "#A8A878" },
                { "lucha", "#C03028" },
                { "veneno", "#A040A0" },
                { "tierra", "#E0C068" },
                { "roca", "#B8A038" },
                { "bicho", "#A8B820" },
                { "fantasma", "#705898" },
                { "hielo", "#98D8D8" },
                { "dragón", "#7038F8" },
                { "siniestro", "#705848" },
                { "acero", "#B8B8D0" },
                { "hada", "#EE99AC" },
                { "electrico", "#F8D030" },
                 { "dragon", "#7038F8" }
            };
        public VisorPokemon()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            pokemonType= e.Parameter as Type;

            if (pokemonType != null)
            {
                var instance = Activator.CreateInstance(pokemonType) as iPokemon;
                instance.activarAniIdle(false);
                instance.verFilaVida(false);
                instance.verFilaEnergia(false);
                instance.verNombre(false);
                instance.verFondo(false);
                PokemonHost.Content = instance;
                tbNombre.Text = instance.Nombre;
                tbCategoria.Text = instance.Categoría;
                var tipos = GetTipos(instance.Tipo);
                SetTypeImages(tipos);
                tbAltura.Text = instance.Altura + " m";
                tbPeso.Text = instance.Peso + " kg";
                tbEvolucion.Text = instance.Evolucion;
                tbDescripcion.Text = instance.Descripcion;
            }
        }

        public List<string> GetTipos(string tipoRaw)
        {
            if (string.IsNullOrWhiteSpace(tipoRaw))
                return new List<string>();

            var normalized = Regex.Replace(tipoRaw, @"[\/,]+", " ");
            return normalized
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim().ToLower())
                        .ToList();
        }
        private void cbAnimacion_Checked(object sender, RoutedEventArgs e)
        {
            var instance = PokemonHost.Content as iPokemon;
            instance.activarAniIdle(true);
        }

        private void cbAnimacion_Unchecked(object sender, RoutedEventArgs e)
        {
            var instance = PokemonHost.Content as iPokemon;
            instance.activarAniIdle(false);
        }
        private void SetTypeImages(List<string> tipos)
        {
            imgTipo1.Source = null;
            imgTipo2.Source = null;
            txtTipo1.Text = "";
            txtTipo2.Text = "";
            borderTipo1.Background = null;
            borderTipo2.Background = null;

            if (tipos.Count > 0)
            {
                imgTipo1.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{tipos[0]}.png"));
                txtTipo1.Text = Capitalize(tipos[0]);
                borderTipo1.Background = new SolidColorBrush(GetTypeColor(tipos[0]));
            }

            if (tipos.Count > 1)
            {
                imgTipo2.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{tipos[1]}.png"));
                txtTipo2.Text = Capitalize(tipos[1]);
                borderTipo2.Background = new SolidColorBrush(GetTypeColor(tipos[1]));
            }
        }

        private Color GetTypeColor(string tipo)
        {
            if (typeColors.TryGetValue(tipo.ToLower(), out var hex))
            {
                return Windows.UI.ColorHelper.FromArgb(
                    255,
                    Convert.ToByte(hex.Substring(1, 2), 16),
                    Convert.ToByte(hex.Substring(3, 2), 16),
                    Convert.ToByte(hex.Substring(5, 2), 16)
                );
            }
            return Colors.Gray; 
        }

        private string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }
    }
}
