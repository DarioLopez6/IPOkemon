using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ProyectoVacioUWP_Base
{
    public sealed partial class CombatePage : Page
    {
        // Diccionarios para control de tiempos y estados
        private Dictionary<VirtualKey, bool> cooldownsActivos = new Dictionary<VirtualKey, bool>();
        private Dictionary<VirtualKey, double> tiemposRestantes = new Dictionary<VirtualKey, double>();

        private DispatcherTimer gameLoop;
        private DispatcherTimer uiTimer;

        private bool p1Herido = false, p1Cansado = false, p2Herido = false, p2Cansado = false;
        private bool p1Protegido = false, p2Protegido = false;

        public CombatePage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += OnKeyDown;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Valores iniciales
            P1.Vida = 100; P1.Energia = 100;
            P2.Vida = 100; P2.Energia = 100;

            InicializarCombate();
        }

        private void InicializarCombate()
        {
            VirtualKey[] claves = {
                VirtualKey.W, VirtualKey.A, VirtualKey.S, VirtualKey.D,
                VirtualKey.Up, VirtualKey.Left, VirtualKey.Down, VirtualKey.Right
            };

            foreach (var k in claves)
            {
                cooldownsActivos[k] = true;
                tiemposRestantes[k] = 0;
            }

            // Bucle Lógica y Regeneración Rápida
            gameLoop = new DispatcherTimer();
            gameLoop.Interval = TimeSpan.FromMilliseconds(200);
            gameLoop.Tick += (s, e) => {
                ValidarEstados(P1, ref p1Herido, ref p1Cansado);
                ValidarEstados(P2, ref p2Herido, ref p2Cansado);

                // Regeneración (aprox 7.5 puntos por segundo)
                if (P1.Energia < 100) P1.Energia += 1.5;
                if (P2.Energia < 100) P2.Energia += 1.5;
            };
            gameLoop.Start();

            // Bucle UI para actualizar cronómetros de texto
            uiTimer = new DispatcherTimer();
            uiTimer.Interval = TimeSpan.FromMilliseconds(100);
            uiTimer.Tick += (s, e) => ActualizarLabelsCooldown();
            uiTimer.Start();
        }

        private void ValidarEstados(iPokemon p, ref bool h, ref bool c)
        {
            if (p.Vida <= 0) { p.animacionDerrota(); gameLoop.Stop(); uiTimer.Stop(); return; }

            // Umbral de Vida
            if (p.Vida < 50 && !h) { h = true; p.animacionHerido(); }
            else if (p.Vida >= 50 && h) { h = false; p.animacionNoHerido(); }

            // Umbral de Energía
            if (p.Energia < 50 && !c) { c = true; p.animacionCansado(); }
            else if (p.Energia >= 50 && c) { c = false; p.animacionNoCansado(); }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            var k = e.VirtualKey;

            // --- JUGADOR 1 ---
            if (k == VirtualKey.W) IntentarAtaque(P1, P2, k, "flojo", 15, 0.8, txtW);
            if (k == VirtualKey.A) IntentarAtaque(P1, P2, k, "fuerte", 40, 3.0, txtA);
            if (k == VirtualKey.S) IntentarEscudo(P1, k, 15, 4.0, txtS);
            if (k == VirtualKey.D) { if (cooldownsActivos[k]) { P1.Vida += 15; P1.Energia += 25; ((iPokemon)P1).animacionDescasar(); AplicarCD(k, 5.0, txtD); } }
            if (k == VirtualKey.Q) ((iPokemon)P1).verPocionVida(false);
            if (k == VirtualKey.E) ((iPokemon)P1).verPocionEnergia(false);

            // --- JUGADOR 2 ---
            if (k == VirtualKey.Up) IntentarAtaque(P2, P1, k, "flojo", 15, 0.8, txtUp);
            if (k == VirtualKey.Left) IntentarAtaque(P2, P1, k, "fuerte", 40, 3.0, txtLeft);
            if (k == VirtualKey.Down) IntentarEscudo(P2, k, 15, 4.0, txtDown);
            if (k == VirtualKey.Right) { if (cooldownsActivos[k]) {  P2.Vida += 15; P2.Energia += 25; ((iPokemon)P2).animacionDescasar(); AplicarCD(k, 5.0, txtRight); } }
            if (k == VirtualKey.O) ((iPokemon)P2).verPocionVida(false);
            if (k == VirtualKey.P) ((iPokemon)P2).verPocionEnergia(false);
        }

        private void IntentarAtaque(iPokemon user, iPokemon target, VirtualKey k, string tipo, double coste, double cd, TextBlock txt)
        {
            if (!cooldownsActivos[k]) return;

            if (user.Energia < coste) { FeedbackError(txt); return; }

            user.Energia -= coste;
            if (tipo == "flojo") ((iPokemon)user).animacionAtaqueFlojo();
            else ((iPokemon)user).animacionAtaqueFuerte();

            bool protegido = (target == P1) ? p1Protegido : p2Protegido;
            if (protegido)
            {
                if (target == P1) p1Protegido = false; else p2Protegido = false;
                target.verEscudo(false);
                txtConsola.Text = "¡ESCUDO ROTO!";
            }
            else
            {
                target.Vida -= (tipo == "flojo" ? 10 : 25);
            }
            AplicarCD(k, cd, txt);
        }

        private void IntentarEscudo(iPokemon user, VirtualKey k, double coste, double cd, TextBlock txt)
        {
            bool yaProtegido = (user == P1) ? p1Protegido : p2Protegido;
            if (!cooldownsActivos[k] || yaProtegido) return;

            if (user.Energia < coste) { FeedbackError(txt); return; }

            user.Energia -= coste;
            if (user == P1) p1Protegido = true; else p2Protegido = true;
            user.verEscudo(true);
            AplicarCD(k, cd, txt);
        }

        private async void AplicarCD(VirtualKey k, double seg, TextBlock txt)
        {
            cooldownsActivos[k] = false;
            tiemposRestantes[k] = seg;
            txt.Opacity = 0.4;
            await Task.Delay((int)(seg * 1000));
            cooldownsActivos[k] = true;
            txt.Opacity = 1.0;
        }

        private async void FeedbackError(TextBlock txt)
        {
            var original = txt.Foreground;
            txt.Foreground = new SolidColorBrush(Colors.Red);
            await Task.Delay(250);
            txt.Foreground = original;
        }

        private void ActualizarLabelsCooldown()
        {
            UpdateText(VirtualKey.W, txtW, "[W] FLOJO (15 EP)");
            UpdateText(VirtualKey.A, txtA, "[A] FUERTE (40 EP)");
            UpdateText(VirtualKey.S, txtS, "[S] ESCUDO (15 EP)");
            UpdateText(VirtualKey.D, txtD, "[D] RELAX");
            UpdateText(VirtualKey.Up, txtUp, "[↑] FLOJO (15 EP)");
            UpdateText(VirtualKey.Left, txtLeft, "[←] FUERTE (40 EP)");
            UpdateText(VirtualKey.Down, txtDown, "[↓] ESCUDO (15 EP)");
            UpdateText(VirtualKey.Right, txtRight, "[→] RELAX");
        }

        private void UpdateText(VirtualKey k, TextBlock txt, string baseT)
        {
            if (tiemposRestantes[k] > 0)
            {
                tiemposRestantes[k] -= 0.1;
                if (tiemposRestantes[k] < 0) tiemposRestantes[k] = 0;
                txt.Text = $"{baseT} | {tiemposRestantes[k]:F1}s";
            }
            else
            {
                txt.Text = baseT;
            }
        }
    }
}