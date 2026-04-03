using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoVacioUWP_Base
{
    public interface iPokemon
    {
        //Todas las propiedades deben devolver la información correspondiente (get). Además debe poderse actualizar el valor, el menos, de la Vida y la Energía (set)
        double Vida { get; set; }
        double Energia { get; set; }
        string Nombre { get; set; } //Nombre del Pokemon
        string Categoría { get; set; } //Gas, Murciélago, Ratón..
        string Tipo { get; set; } //Eléctrico, Veneno, Volador...
        double Altura { get; set; } // En metros
        double Peso { get; set; } // En kilos
        string Evolucion { get; set; } // Nombre de la evolución o evoluciones
        string Descripcion { get; set; } // Entre 200 y 500 caracteres

        void verFondo(bool ver);
        void verFilaVida(bool ver);
        void verFilaEnergia(bool ver);
        void verPocionVida(bool ver);
        void verPocionEnergia(bool ver);
        void verNombre(bool ver);
        void verEscudo(bool ver); //Sirve principalmente para activar/desactivar el escudo

        void activarAniIdle(bool activar);
        void animacionAtaqueFlojo();
        void animacionAtaqueFuerte();
        void animacionDefensa(); //Debe dejar algún tipo de elemento que represente escudo visible
        void animacionDescasar();

        void animacionCansado();
        void animacionNoCansado();
        void animacionHerido();
        void animacionNoHerido();
        void animacionDerrota();
    }
}
