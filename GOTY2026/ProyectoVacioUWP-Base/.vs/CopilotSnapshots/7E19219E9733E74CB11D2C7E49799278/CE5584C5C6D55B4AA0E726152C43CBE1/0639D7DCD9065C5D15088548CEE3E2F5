using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace ProyectoVacioUWP_Base
{
    public sealed partial class ComputerrorDLM : UserControl, iPokemon
    {
        private DispatcherTimer healthTimer;
        private DispatcherTimer staminaTimer;
        private double healthIncrement = 0;
        private double staminaIncrement = 0;
        private Storyboard sbIdle;

        // Mantenemos estas dos a nivel de clase para que no las borre el sistema
        private MediaPlayer fondo = new MediaPlayer();
        private MediaPlayer efectos = new MediaPlayer();

        public double Vida
        {
            get { return pbHealth.Value; }
            set { pbHealth.Value = value; }
        }

        public double Energia
        {
            get { return pbStamina.Value; }
            set { pbStamina.Value = value; }
        }
        public string Nombre { get; set; } = "Computerror";
        public string Categoría { get; set; } = "Pokemon Ordenador Fantasma";
        public string Tipo { get; set; } = "Fantasma/Electrico";
        public double Altura { get; set; } = 1.60;
        public double Peso { get; set; } = 80;
        public string Evolucion { get; set; } = "PhantomServer";
        public string Descripcion { get; set; } = "Un antiguo ordenador poseído por espíritus digitales.";

        public ComputerrorDLM()
        {
            this.InitializeComponent();
            this.Focus(FocusState.Programmatic);
            //this.KeyDown += ControlTeclas;

            sbIdle = (Storyboard)this.Resources["AniIdle"];
            sbIdle.Begin();

            // Música de fondo
           // CargarMusicaFondo();
        }

        public async void CargarMusicaFondo()
        {
            try
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await folder.GetFileAsync("AssetsComputerrorDLM\\fondoDEFINITIVO.wav");
                fondo.Source = MediaSource.CreateFromStorageFile(file);
                fondo.IsLoopingEnabled = true;
            }
            catch { }
        }

        // Método centralizado para sonidos de efectos
        private async void ReproducirSonido(string filename)
        {
            try
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await folder.GetFileAsync("AssetsComputerrorDLM\\" + filename);
                efectos.Source = MediaSource.CreateFromStorageFile(file);
                efectos.Play();
            }
            catch { }
        }

        // --- IMPLEMENTACIÓN DE LA INTERFAZ (Lo que llama MainPage) ---

        void iPokemon.animacionAtaqueFlojo()
        {
            ReproducirSonido("aranazoFlojoDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniAranazoFlojo"]).Begin();
        }

        void iPokemon.animacionAtaqueFuerte()
        {
            ReproducirSonido("rayoDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniRayoDeLuz"]).Begin();
        }

        void iPokemon.animacionDefensa()
        {
            ReproducirSonido("escudoDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniProteccion"]).Begin();
        }

        void iPokemon.animacionDescasar()
        {
            ReproducirSonido("curaDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniDescanso"]).Begin();
        }

        void iPokemon.animacionCansado()
        {
            ReproducirSonido("apagadoDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniCanso"]).Begin();
        }

        void iPokemon.animacionHerido()
        {
            ReproducirSonido("explosionDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniHerido"]).Begin();
        }

        void iPokemon.animacionNoHerido()
        {
            ReproducirSonido("desexplosionDEFINITIVO.wav");
            ((Storyboard)this.Resources["AniDesHerido"]).Begin();
        }

        void iPokemon.animacionDerrota()
        {
            ReproducirSonido("muerteDEFINITIVA.wav");
            ((Storyboard)this.Resources["AniDerrotado"]).Begin();
            ((Storyboard)this.Resources["AniMeMataste"]).Begin();
        }

        void iPokemon.verEscudo(bool ver)
        {
            if (ver)
            {
                ReproducirSonido("escudoDEFINITIVO.wav");
                ((Storyboard)this.Resources["AniProteccion"]).Begin();
            }
            else
            {
                ReproducirSonido("desescudoDEFINITIVO.wav");
                ((Storyboard)this.Resources["AniDesProteccion"]).Begin();
            }
        }

        // Métodos de visibilidad y otros (Sin cambios)
        void iPokemon.verFondo(bool ver) { grid.Background = ver ? new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///AssetsComputerrorDLM/fondoPokemon.jpg")) } : null; }
        void iPokemon.verFilaVida(bool ver) { pbHealth.Visibility = ImCorazon.Visibility = imRedPotion.Visibility = ver ? Visibility.Visible : Visibility.Collapsed; }
        void iPokemon.verFilaEnergia(bool ver) { pbStamina.Visibility = ImEnergia.Visibility = imYellowPotion.Visibility = ver ? Visibility.Visible : Visibility.Collapsed; }
        void iPokemon.verPocionVida(bool ver) { imRedPotion.Visibility = ver ? Visibility.Visible : Visibility.Collapsed; }
        void iPokemon.verPocionEnergia(bool ver) { imYellowPotion.Visibility = ver ? Visibility.Visible : Visibility.Collapsed; }
        void iPokemon.verNombre(bool ver) { TextNombre.Visibility = ver ? Visibility.Visible : Visibility.Collapsed; }
        void iPokemon.activarAniIdle(bool activar) { if (activar) sbIdle.Begin(); else sbIdle.Stop(); }
        void iPokemon.animacionNoCansado() { ((Storyboard)this.Resources["AniDescanso"]).Begin(); }

        public void activarMusicaIdle(bool activar)
        {
            if (activar) fondo.Play(); else fondo.Pause();
        }

        // Control por teclado (reutiliza los métodos de la interfaz para no repetir)
        private void ControlTeclas(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Number1: ((iPokemon)this).animacionAtaqueFlojo(); break;
                case Windows.System.VirtualKey.Number2: ((iPokemon)this).animacionAtaqueFuerte(); break;
                case Windows.System.VirtualKey.Number3: ((iPokemon)this).animacionDefensa(); break;
                case Windows.System.VirtualKey.Number4: ((iPokemon)this).animacionHerido(); break;
                case Windows.System.VirtualKey.Number9: ((iPokemon)this).animacionDerrota(); break;
            }
        }

        private void useRedPotion_PointerReleased(object sender, PointerRoutedEventArgs e)

        {

            healthTimer = new DispatcherTimer();

            healthTimer.Interval = TimeSpan.FromMilliseconds(100);

            healthTimer.Tick += increaseHealth;

            healthTimer.Start();

            imRedPotion.Visibility = Visibility.Collapsed;

        }



        private void useYellowPotion_PointerReleased(object sender, PointerRoutedEventArgs e)

        {

            staminaTimer = new DispatcherTimer();

            staminaTimer.Interval = TimeSpan.FromMilliseconds(100);

            staminaTimer.Tick += increaseStamina;

            staminaTimer.Start();

            imYellowPotion.Visibility = Visibility.Collapsed;

        }



        private void increaseHealth(object sender, object e)

        {

            pbHealth.Value += 0.5;

            healthIncrement += 0.5;

            if (pbHealth.Value >= 100 || healthIncrement >= 40)

            {

                healthTimer.Stop();

                healthIncrement = 0;

            }

        }



        private void increaseStamina(object sender, object e)

        {

            pbStamina.Value += 0.5;

            staminaIncrement += 0.5;

            if (pbStamina.Value >= 100 || staminaIncrement >= 40)

            {

                staminaTimer.Stop();

                staminaIncrement = 0;

            }

        }

    }
}
