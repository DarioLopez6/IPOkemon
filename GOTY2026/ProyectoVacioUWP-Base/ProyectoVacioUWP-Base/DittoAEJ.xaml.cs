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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace ProyectoVacioUWP_Base
   
{
    public sealed partial class DittoAEJ : UserControl, iPokemon
    {
        DispatcherTimer _healthTimer;
        DispatcherTimer _energyTimer;

        double healthIncrement = 0;
        double energyIncrement = 0;
        private bool escudoActivo = false;
        private TimeSpan duracionTotal = TimeSpan.FromSeconds(1);
        private TimeSpan mitadDuracion = TimeSpan.FromSeconds(0.5);
        private bool heridoActivo = false;
        private bool cansadoActivo = false;
        private bool derrotadoActivo = false;
        private bool estaticoActivo = false;
        private bool _quierePocionVida = false;
        private bool _filaVidaVisible = false;
        private bool _filaEnergiaVisible = false;
        private bool _quierePocionEnergia = false;
        public Slider sliderVidaExterno;
        public Slider sliderEnergiaExterno;
        public CheckBox chkPocimaVidaExterno;
        public CheckBox chkPocimaEnergiaExterno;


        DispatcherTimer tProj1Off;
        DispatcherTimer tProj2On;
        DispatcherTimer tProj2Off;
        DispatcherTimer tProj3On;
        DispatcherTimer tProj3Off;
        DispatcherTimer tProj4On;
        DispatcherTimer tProj4Off;

        DispatcherTimer tProyGordoOn;
        DispatcherTimer tProyGordoOff;
        MediaPlayer mpEstatico = new MediaPlayer();
        DispatcherTimer timerSonido;
        MediaPlayer mpFuerte = new MediaPlayer();
        MediaPlayer mpEscudo = new MediaPlayer();
        MediaPlayer mpHerido = new MediaPlayer();
        MediaPlayer mpCurar = new MediaPlayer();
        MediaPlayer mpDormir = new MediaPlayer();
        MediaPlayer mpEnergia = new MediaPlayer();
        MediaPlayer mpMuerte = new MediaPlayer();

        public double Vida
        {
            get { return pbHealth.Value; }
            set { pbHealth.Value = value; }
        }

        public double Energia
        {
            get { return pbEnergy.Value; }
            set { pbEnergy.Value = value; }
        }


        public string Nombre { get; set; } = "Ditto";

        public string Categoría { get; set; } = "Transformación";
        public string Tipo { get; set; } = "Normal";
        public double Altura { get; set; } = 0.3;
        public double Peso { get; set; } = 4.0;
        public string Evolucion { get; set; } = "Ninguna";
        public string Descripcion { get; set; } = "Puede reorganizar su estructura celular para adoptar la forma de cualquier otro ser vivo.";

        public DittoAEJ()
        {
            this.InitializeComponent();

            mpEstatico.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/vote.mp3"));
            mpFuerte.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/fuerte.mp3"));
            mpEscudo.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/escudo.mp3"));
            mpHerido.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/herido.mp3"));
            mpCurar.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/curar.mp3"));
            mpDormir.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/dormir.mp3"));
            mpEnergia.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/energia.mp3"));
            mpMuerte.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/muerte.mp3"));
            mpEstatico.IsLoopingEnabled = false;

            timerSonido = new DispatcherTimer();
            timerSonido.Interval = TimeSpan.FromSeconds(1);
            timerSonido.Tick += (s, e) =>
            {
                mpEstatico.PlaybackSession.Position = TimeSpan.Zero;
                mpEstatico.Play();
            };

            OcultarExtras();

            this.IsTabStop = true;
        }


        private void Fade(UIElement elemento, double from, double to, double segundos)
        {
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(segundos)
            };

            var sb = new Storyboard();
            sb.Children.Add(anim);

            Storyboard.SetTarget(anim, elemento);
            Storyboard.SetTargetProperty(anim, "Opacity");

            sb.Begin();
        }

        private void OcultarExtras()
        {
            herida.Visibility = Visibility.Collapsed;
            herida2.Visibility = Visibility.Collapsed;
            herida3.Visibility = Visibility.Collapsed;
            tirita1.Visibility = Visibility.Collapsed;
            tirita2.Visibility = Visibility.Collapsed;

            Zs.Visibility = Visibility.Collapsed;
        }
        private void ComprobarRetornoAlEstadoBase()
        {
            if (!derrotadoActivo)
            {
                estatico.Begin();
                mpEstatico.Play();
                timerSonido.Start();
            }
        }
        private void ToggleHerido()
        {
            if (derrotadoActivo) return;

            estatico.Stop();
            timerSonido.Stop();
            mpEstatico.Pause();

            if (!heridoActivo)
            {
                mpHerido.PlaybackSession.Position = TimeSpan.Zero;
                mpHerido.Play();

                pasarAHerido.Begin();
                pasarAHerido.Completed -= HeridoFin;
                pasarAHerido.Completed += HeridoFin;

                herida.Visibility = Visibility.Visible;
                herida2.Visibility = Visibility.Visible;
                herida3.Visibility = Visibility.Visible;
                tirita1.Visibility = Visibility.Visible;
                tirita2.Visibility = Visibility.Visible;

                Fade(herida, 0, 1, 1);
                Fade(herida2, 0, 1, 1);
                Fade(herida3, 0, 1, 1);
                Fade(tirita1, 0, 1, 1);
                Fade(tirita2, 0, 1, 1);

                heridoActivo = true;
            }
            else
            {
                mpCurar.PlaybackSession.Position = TimeSpan.Zero;
                mpCurar.Play();

                quitarHerido.Begin();
                quitarHerido.Completed -= HeridoFin;
                quitarHerido.Completed += HeridoFin;

                Fade(herida, 1, 0, 1);
                Fade(herida2, 1, 0, 1);
                Fade(herida3, 1, 0, 1);
                Fade(tirita1, 1, 0, 1);
                Fade(tirita2, 1, 0, 1);

                var t = new DispatcherTimer();
                t.Interval = TimeSpan.FromSeconds(1);
                t.Tick += (s, e) =>
                {
                    herida.Visibility = Visibility.Collapsed;
                    herida2.Visibility = Visibility.Collapsed;
                    herida3.Visibility = Visibility.Collapsed;
                    tirita1.Visibility = Visibility.Collapsed;
                    tirita2.Visibility = Visibility.Collapsed;
                    ((DispatcherTimer)s).Stop();
                };
                t.Start();

                heridoActivo = false;
            }
        }

        private void HeridoFin(object sender, object e)
        {
            if (estaticoActivo) ComprobarRetornoAlEstadoBase();
        }

        private void ToggleCansado()
        {
            if (derrotadoActivo) return;

            estatico.Stop();
            timerSonido.Stop();
            mpEstatico.Pause();

            if (!cansadoActivo)
            {
                mpDormir.PlaybackSession.Position = TimeSpan.Zero;
                mpDormir.Play();

                pasarACansado.Begin();
                pasarACansado.Completed -= CansadoFin;
                pasarACansado.Completed += CansadoFin;

                Zs.Visibility = Visibility.Visible;
                Fade(Zs, 0, 1, 1);

                cansadoActivo = true;
            }
            else
            {
                mpEnergia.PlaybackSession.Position = TimeSpan.Zero;
                mpEnergia.Play();

                quitarCansado.Begin();
                quitarCansado.Completed -= CansadoFin;
                quitarCansado.Completed += CansadoFin;

                Fade(Zs, 1, 0, 1);

                var t = new DispatcherTimer();
                t.Interval = TimeSpan.FromSeconds(1);
                t.Tick += (s, e) =>
                {
                    Zs.Visibility = Visibility.Collapsed;
                    ((DispatcherTimer)s).Stop();
                };
                t.Start();

                cansadoActivo = false;
            }
        }

        private void CansadoFin(object sender, object e)
        {
            if (estaticoActivo) ComprobarRetornoAlEstadoBase();
        }

        private void Derrotado()
        {
            if (derrotadoActivo) return;

            derrotadoActivo = true;

            estatico.Stop();
            timerSonido.Stop();
            mpEstatico.Pause();

            mpHerido.Pause();
            mpDormir.Pause();
            mpEscudo.Pause();

            mpMuerte.PlaybackSession.Position = TimeSpan.Zero;
            mpMuerte.Play();

            OcultarExtras();

            ojoIz.Visibility = Visibility.Collapsed;
            ojoDer.Visibility = Visibility.Collapsed;

            hojoMuerto.Visibility = Visibility.Visible;
            hojoMuerto2.Visibility = Visibility.Visible;

            hojoMuerto.Opacity = 0;
            hojoMuerto2.Opacity = 0;

            Fade(hojoMuerto, 0, 1, 2);
            Fade(hojoMuerto2, 0, 1, 2);

            derrotado.Begin();

        }


        private void AtaqueDebil()
        {
            if (derrotadoActivo) return;

            estatico.Stop();
            mpEstatico.Pause();
            timerSonido.Stop();

            ataqueDebil.Begin();

            ataqueDebil.Completed -= AtaqueDebilFin;
            ataqueDebil.Completed += AtaqueDebilFin;

            ojoIz_Enfadado.Visibility = Visibility.Visible;
            ojoDer_Enfadado.Visibility = Visibility.Visible;
            boca_Enfadada.Visibility = Visibility.Visible;

            var t1 = new DispatcherTimer();
            t1.Interval = TimeSpan.FromSeconds(0.5);
            t1.Tick += (s, e) =>
            {
                proyectil1.Visibility = Visibility.Visible;
                ReproducirSonidoPew();
                ((DispatcherTimer)s).Stop();
            };
            t1.Start();

            var t1off = new DispatcherTimer();
            t1off.Interval = TimeSpan.FromSeconds(3);
            t1off.Tick += (s, e) =>
            {
                proyectil1.Visibility = Visibility.Collapsed;
                ((DispatcherTimer)s).Stop();
            };
            t1off.Start();

            var t2 = new DispatcherTimer();
            t2.Interval = TimeSpan.FromSeconds(1);
            t2.Tick += (s, e) =>
            {
                proyectil2.Visibility = Visibility.Visible;
                ReproducirSonidoPew();
                ((DispatcherTimer)s).Stop();
            };
            t2.Start();

            var t2off = new DispatcherTimer();
            t2off.Interval = TimeSpan.FromSeconds(3);
            t2off.Tick += (s, e) =>
            {
                proyectil2.Visibility = Visibility.Collapsed;
                ((DispatcherTimer)s).Stop();
            };
            t2off.Start();

            var t3 = new DispatcherTimer();
            t3.Interval = TimeSpan.FromSeconds(1.5);
            t3.Tick += (s, e) =>
            {
                proyectil3.Visibility = Visibility.Visible;
                ReproducirSonidoPew();
                ((DispatcherTimer)s).Stop();
            };
            t3.Start();

            var t3off = new DispatcherTimer();
            t3off.Interval = TimeSpan.FromSeconds(3.5);
            t3off.Tick += (s, e) =>
            {
                proyectil3.Visibility = Visibility.Collapsed;
                ((DispatcherTimer)s).Stop();
            };
            t3off.Start();

            var t4 = new DispatcherTimer();
            t4.Interval = TimeSpan.FromSeconds(2);
            t4.Tick += (s, e) =>
            {
                proyectil4.Visibility = Visibility.Visible;
                ReproducirSonidoPew();
                ((DispatcherTimer)s).Stop();
            };
            t4.Start();

            var t4off = new DispatcherTimer();
            t4off.Interval = TimeSpan.FromSeconds(3.5);
            t4off.Tick += (s, e) =>
            {
                proyectil4.Visibility = Visibility.Collapsed;
                ((DispatcherTimer)s).Stop();
            };
            t4off.Start();
        }

        private void AtaqueDebilFin(object sender, object e)
        {
            ojoIz_Enfadado.Visibility = Visibility.Collapsed;
            ojoDer_Enfadado.Visibility = Visibility.Collapsed;
            boca_Enfadada.Visibility = Visibility.Collapsed;

            if (estaticoActivo) ComprobarRetornoAlEstadoBase();
        }
        private void ReproducirSonidoPew()
        {
            MediaPlayer mpPew = new MediaPlayer();
            mpPew.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///AssetsDittoAEJ/pew.mp3"));
            mpPew.Play();
        }


        private void AtaqueFuerte()
        {
            if (derrotadoActivo) return;
            estatico.Stop();
            timerSonido.Stop();
            mpEstatico.Pause();

            ataqueFuerte.Begin();

            ataqueFuerte.Completed -= AtaqueFuerteFin;
            ataqueFuerte.Completed += AtaqueFuerteFin;

            ojoIz_Enfadado.Visibility = Visibility.Visible;
            ojoDer_Enfadado.Visibility = Visibility.Visible;

            var ton = new DispatcherTimer();
            ton.Interval = TimeSpan.FromSeconds(0.5);
            ton.Tick += (s, e) =>
            {
                proyectilGordo.Visibility = Visibility.Visible;

                mpFuerte.PlaybackSession.Position = TimeSpan.Zero;
                mpFuerte.Play();

                ((DispatcherTimer)s).Stop();
            };
            ton.Start();

            var toff = new DispatcherTimer();
            toff.Interval = TimeSpan.FromSeconds(2);
            toff.Tick += (s, e) =>
            {
                proyectilGordo.Visibility = Visibility.Collapsed;
                ((DispatcherTimer)s).Stop();
            };
            toff.Start();
        }

        private void AtaqueFuerteFin(object sender, object e)
        {
            ojoIz_Enfadado.Visibility = Visibility.Collapsed;
            ojoDer_Enfadado.Visibility = Visibility.Collapsed;

            if (estaticoActivo) ComprobarRetornoAlEstadoBase();
        }


        private void ToggleEscudo()
        {
            if (derrotadoActivo) return;

            estatico.Stop();
            timerSonido.Stop();
            mpEstatico.Pause();

            mpEscudo.PlaybackSession.Position = TimeSpan.Zero;
            mpEscudo.Play();

            if (!escudoActivo)
            {
                pasarAEscudo.Begin();
                pasarAEscudo.Completed -= EscudoAnimacionFin;
                pasarAEscudo.Completed += EscudoAnimacionFin;
                escudoActivo = true;
            }
            else
            {
                quitarEscudo.Begin();
                quitarEscudo.Completed -= EscudoAnimacionFin;
                quitarEscudo.Completed += EscudoAnimacionFin;
                escudoActivo = false;
            }
        }
        private void EscudoAnimacionFin(object sender, object e)
        {
            if (estaticoActivo) ComprobarRetornoAlEstadoBase();
        }
        private void useHealthPotion(object sender, PointerRoutedEventArgs e)
        {
            if (pbHealth.Value >= 100) return;

            imHealthPotion.Visibility = Visibility.Collapsed;
            healthIncrement = 0;

            _healthTimer = new DispatcherTimer();
            _healthTimer.Interval = TimeSpan.FromMilliseconds(50);
            _healthTimer.Tick += IncreaseHealth;
            _healthTimer.Start();
        }

        private void IncreaseHealth(object sender, object e)
        {
            pbHealth.Value += 1;
            healthIncrement += 1;

            if (chkPocimaVidaExterno != null)
                chkPocimaVidaExterno.IsChecked = false;

            if (sliderVidaExterno != null)
                sliderVidaExterno.Value = this.pbHealth.Value;


            if (pbHealth.Value >= 100 || healthIncrement >= 40)
            {
                _healthTimer.Stop();
                healthIncrement = 0;
            }
        }


        private void useEnergyPotion(object sender, PointerRoutedEventArgs e)
        {
            if (pbEnergy.Value >= 100) return;

            imEnergyPotion.Visibility = Visibility.Collapsed;
            energyIncrement = 0;

            _energyTimer = new DispatcherTimer();
            _energyTimer.Interval = TimeSpan.FromMilliseconds(50);
            _energyTimer.Tick += IncreaseEnergy;
            _energyTimer.Start();
        }

        private void IncreaseEnergy(object sender, object e)
        {
            pbEnergy.Value += 1;
            energyIncrement += 1;
            if (chkPocimaEnergiaExterno != null)
                chkPocimaEnergiaExterno.IsChecked = false;

            if (sliderEnergiaExterno != null)
                sliderEnergiaExterno.Value = this.pbEnergy.Value;



            if (pbEnergy.Value >= 100 || energyIncrement >= 40)
            {
                _energyTimer.Stop();
                energyIncrement = 0;
            }
        }

        public void verFondo(bool ver)
        {
            if (gridGeneral.Background != null)
            {
                gridGeneral.Background.Opacity = ver ? 1.0 : 0.0;
            }
        }

        public void verFilaVida(bool ver)
        {
            _filaVidaVisible = ver;
            simboloHeart.Visibility = ver ? Visibility.Visible : Visibility.Collapsed;
            pbHealth.Visibility = ver ? Visibility.Visible : Visibility.Collapsed;
            ActualizarVisibilidadPocionVida();
        }

        public void verPocionVida(bool ver)
        {
            _quierePocionVida = ver;
            ActualizarVisibilidadPocionVida();
        }

        private void ActualizarVisibilidadPocionVida()
        {
            imHealthPotion.Visibility = (_filaVidaVisible && _quierePocionVida)
                                        ? Visibility.Visible : Visibility.Collapsed;
        }

        public void verFilaEnergia(bool ver)
        {
            _filaEnergiaVisible = ver;
            simboloEnergy.Visibility = ver ? Visibility.Visible : Visibility.Collapsed;
            pbEnergy.Visibility = ver ? Visibility.Visible : Visibility.Collapsed;
            ActualizarVisibilidadPocionEnergia();
        }

        public void verPocionEnergia(bool ver)
        {
            _quierePocionEnergia = ver;
            ActualizarVisibilidadPocionEnergia();
        }

        private void ActualizarVisibilidadPocionEnergia()
        {
            imEnergyPotion.Visibility = (_filaEnergiaVisible && _quierePocionEnergia)
                                        ? Visibility.Visible : Visibility.Collapsed;
        }

        public void verNombre(bool ver)
        {
            txtNombre.Visibility = ver ? Visibility.Visible : Visibility.Collapsed;
        }

        public void verEscudo(bool ver)
        {
            if (ver && !escudoActivo)
            {
                ToggleEscudo();
            }
            else if (!ver && escudoActivo)
            {
                ToggleEscudo();
            }
        }

        public void activarAniIdle(bool activar)
        {
            estaticoActivo = activar;
            if (activar)
            {
                estatico.Begin();
                mpEstatico.Play();
                timerSonido.Start();
            }
            else
            {
                estatico.Stop();
                mpEstatico.Pause();
                timerSonido.Stop();
            }
        }

        public void animacionAtaqueFlojo()
        {
            AtaqueDebil();
        }

        public void animacionAtaqueFuerte()
        {
            AtaqueFuerte();
        }

        public void animacionDefensa()
        {
            if (!escudoActivo) ToggleEscudo();
        }

        public void animacionDescasar()
        {
            cansadoActivo = true;
            ToggleCansado();
        }

        public void animacionCansado()
        {
            if (!cansadoActivo) ToggleCansado();
        }

        public void animacionNoCansado()
        {
            if (cansadoActivo) ToggleCansado();
        }

        public void animacionHerido()
        {
            if (!heridoActivo) ToggleHerido();
        }

        public void animacionNoHerido()
        {
            if (heridoActivo) ToggleHerido();
        }

        public void animacionDerrota()
        {
            Derrotado();
        }
    }
}
