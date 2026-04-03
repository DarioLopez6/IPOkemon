using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ProyectoVacioUWP_Base
{
    public class Pokemon
    {
        string nombre, tipo;
        BitmapImage imagen;
        Type pokemonType;
        public Pokemon(string nombre, string tipo, string url, Type type)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.imagen = new BitmapImage(new Uri(url));
            this.pokemonType = type;
        }
        public string Nombre { get => this.nombre; set => this.nombre = value; }
        public string Tipo { get => this.tipo; set => this.tipo = value; }

        public BitmapImage Imagen { get => this.imagen; set => this.imagen = value; }
        public Type PokemonType { get => this.pokemonType; set => this.pokemonType = value; }

    }
}
