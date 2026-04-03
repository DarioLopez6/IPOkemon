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
        public sealed partial class TorchicJFBR : UserControl, iPokemon
        {
            Storyboard sbaux;
            double subidaVida = 0, subidaEnergia = 0;
            DispatcherTimer timer, timer2;
            MediaPlayer mp = new MediaPlayer();
            String categoria = "Polluelo", tipo = "Fuego", evolucion = "Combusken", descripcion = "Posee una saca de fuego en el abdomen, cuya llama arderá durante toda su vida. El calor que desprende resulta muy agradable al abrazarlo.";
            double altura = 0.4, peso = 2.5;

            public double Vida { get => this.pbVida.Value; set => this.pbVida.Value = value; }
            public double Energia { get => this.pbEnergia.Value; set => this.pbEnergia.Value = value; }
            public string Nombre { get => this.tbNombre.Text; set => this.tbNombre.Text = value; }
            public string Categoría { get => this.categoria; set => this.categoria = value; }
            public string Tipo { get => this.tipo; set => this.tipo = value; }
            public double Altura { get => this.altura; set => this.altura = value; }
            public double Peso { get => this.peso; set => this.peso = value; }
            public string Evolucion { get => this.evolucion; set => this.evolucion = value; }
            public string Descripcion { get => this.descripcion; set => this.descripcion = value; }

            public TorchicJFBR()
            {
                this.InitializeComponent();
                //PRIMERO: Hacer interactivo nuestro Pokemon para que atienda a eventos de teclado
                this.IsTabStop = true;
                this.Loaded += (s, e) =>
                {
                    this.Focus(FocusState.Programmatic);
                };
                Storyboard sbIdle = (Storyboard)this.Resources["sbAleteo"];
                sbIdle.Begin();
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Ember.mp3"));
                mp.Play();  // briefly play
                mp.Pause();
            }

            private void pocionVida_PointerReleased(object sender, PointerRoutedEventArgs e)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += increaseHealth;
                timer.Start();
                this.pocionVida.Visibility = Visibility.Collapsed;
            }

            private void increaseHealth(object sender, object e)
            {
                if (subidaVida < 40 && pbVida.Value < 100)
                {
                    subidaVida += 0.5;
                    pbVida.Value += 0.5;
                }
                else
                {
                    timer.Stop();
                }
            }

            private void pocionEnergia_PointerReleased(object sender, PointerRoutedEventArgs e)
            {
                timer2 = new DispatcherTimer();
                timer2.Interval = TimeSpan.FromMilliseconds(100);
                timer2.Tick += increaseEnergy;
                timer2.Start();
                pocionEnergia.Visibility = Visibility.Collapsed;
            }

            private void increaseEnergy(object sender, object e)
            {
                if (subidaEnergia < 40 && pbEnergia.Value < 100)
                {
                    subidaEnergia += 0.5;
                    pbEnergia.Value += 0.5;
                }
                else
                {
                    timer2.Stop();
                }
            }

            public void verFondo(bool ver)
            {
                if (ver)
                    this.imFondo.Visibility = Visibility.Visible;
                else
                    this.imFondo.Visibility = Visibility.Collapsed;
            }

            public void verFilaVida(bool ver)
            {
                if (ver)
                    this.gd.RowDefinitions[0].Height = new GridLength(50);
                else
                    this.gd.RowDefinitions[0].Height = new GridLength(0);
            }

            public void verFilaEnergia(bool ver)
            {
                if (ver)
                    this.gd.RowDefinitions[1].Height = new GridLength(50);
                else
                    this.gd.RowDefinitions[1].Height = new GridLength(0);
            }

            public void verPocionVida(bool ver)
            {
                if (ver)
                    this.pocionVida.Visibility = Visibility.Visible;
                else
                    this.pocionVida.Visibility = Visibility.Collapsed;
            }

            public void verPocionEnergia(bool ver)
            {
                if (ver)
                    this.pocionEnergia.Visibility = Visibility.Visible;
                else
                    this.pocionEnergia.Visibility = Visibility.Collapsed;
            }

            public void verNombre(bool ver)
            {
                if (ver)
                    this.tbNombre.Visibility = Visibility.Visible;
                else
                    this.tbNombre.Visibility = Visibility.Collapsed;
            }

            public void verEscudo(bool ver)
            {
                if (ver)
                {
                    sbaux = (Storyboard)this.Resources["sbProteger"];
                    mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Protect.mp3"));
                    mp.Play();
                    sbaux.Begin();
                }
                else
                {
                    sbaux = (Storyboard)this.Resources["sbDesproteger"];
                    mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Flash.mp3"));
                    mp.Play();
                    sbaux.Begin();
                }
            }

            public void activarAniIdle(bool activar)
            {
                Storyboard sbIdle = (Storyboard)this.Resources["sbAleteo"];
                if (activar)
                {
                    sbIdle.Begin();
                }
                else
                    sbIdle.Stop();
            }

            public void animacionAtaqueFlojo()
            {
                sbaux = (Storyboard)this.Resources["sbAtaqueDebil"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Ember.mp3"));
                mp.Play();
                sbaux.Begin();
                DispatcherTimer stopSound = new DispatcherTimer();
                stopSound.Interval = TimeSpan.FromSeconds(0.7);
                stopSound.Tick += (s, ev) =>
                {
                    mp.Pause();
                    stopSound.Stop();
                };
                stopSound.Start();
            }

            public void animacionAtaqueFuerte()
            {
                sbaux = (Storyboard)this.Resources["sbAtaqueFuerte"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Ember.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionDefensa()
            {
                sbaux = (Storyboard)this.Resources["sbProteger"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Protect.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionDescasar()
            {
                sbaux = (Storyboard)this.Resources["sbDespertar"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Torchic.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionCansado()
            {
                sbaux = (Storyboard)this.Resources["sbDormir"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Sleep.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionNoCansado()
            {
                sbaux = (Storyboard)this.Resources["sbDespertar"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Torchic.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionHerido()
            {
                sbaux = (Storyboard)this.Resources["sbHerido"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Damage.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionNoHerido()
            {
                sbaux = (Storyboard)this.Resources["sbNoHerido"];
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Healing.mp3"));
                mp.Play();
                sbaux.Begin();
            }

            public void animacionDerrota()
            {
                sbaux = (Storyboard)this.Resources["sbDerrota"];
                Storyboard sbIdle = (Storyboard)this.Resources["sbAleteo"];
                sbIdle.Stop();
                mp.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///TorchicAssets/Faint.mp3"));
                mp.Play();
                sbaux.Begin();
            }
        }
}
