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
        // --- CONFIGURACIÓN ---
        private const double REGEN_ENERGIA = 1.8;
        private const int MS_BLOQUEO_ACCION = 1000;
        private const int MS_BLOQUEO_DOLOR = 500;

        private Dictionary<VirtualKey, bool> cooldownsActivos = new Dictionary<VirtualKey, bool>();
        private Dictionary<VirtualKey, double> tiemposRestantes = new Dictionary<VirtualKey, double>();
        private DispatcherTimer gameLoop;
        private DispatcherTimer uiTimer;

        private bool p1Herido = false, p1Cansado = false, p2Herido = false, p2Cansado = false;
        private bool p1Protegido = false, p2Protegido = false;
        private bool p1Ocupado = false, p2Ocupado = false;

        // Registro para que las pociones sean de un solo uso
        private bool p1PocionVidaUsada = false, p1PocionEnergiaUsada = false;
        private bool p2PocionVidaUsada = false, p2PocionEnergiaUsada = false;

        public CombatePage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += OnKeyDown;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            P1.Vida = 100; P1.Energia = 100;
            P2.Vida = 100; P2.Energia = 100;
            InicializarSistemas();
        }

        private void InicializarSistemas()
        {
            VirtualKey[] teclas = {
                VirtualKey.W, VirtualKey.A, VirtualKey.S, VirtualKey.D,
                VirtualKey.Up, VirtualKey.Left, VirtualKey.Down, VirtualKey.Right
            };
            foreach (var t in teclas) { cooldownsActivos[t] = true; tiemposRestantes[t] = 0; }

            gameLoop = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            gameLoop.Tick += (s, e) => {
                GestionarEstadoVisual(P1, ref p1Herido, ref p1Cansado, p1Ocupado);
                GestionarEstadoVisual(P2, ref p2Herido, ref p2Cansado, p2Ocupado);
                if (P1.Energia < 100) P1.Energia += REGEN_ENERGIA;
                if (P2.Energia < 100) P2.Energia += REGEN_ENERGIA;
            };
            gameLoop.Start();

            uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            uiTimer.Tick += (s, e) => ActualizarLabelsUI();
            uiTimer.Start();
        }

        private void GestionarEstadoVisual(iPokemon p, ref bool h, ref bool c, bool ocupado)
        {
            if (p.Vida <= 0) { p.animacionDerrota(); gameLoop.Stop(); return; }
            if (ocupado) return;

            if (p.Vida < 50 && !h) { h = true; p.animacionHerido(); }
            else if (p.Vida >= 50 && h) { h = false; p.animacionNoHerido(); }

            if (p.Energia < 50 && !c) { c = true; p.animacionCansado(); }
            else if (p.Energia >= 50 && c) { c = false; p.animacionNoCansado(); }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            var k = e.VirtualKey;

            // JUGADOR 1
            if (k == VirtualKey.W) ProcesarAccion(P1, P2, k, "flojo", 15, 0.8, txtW);
            if (k == VirtualKey.A) ProcesarAccion(P1, P2, k, "fuerte", 40, 3.0, txtA);
            if (k == VirtualKey.S) ProcesarAccion(P1, P2, k, "escudo", 15, 0, txtS); // CD manual por escudo
            if (k == VirtualKey.D) ProcesarAccion(P1, P2, k, "relax", 0, 5.0, txtD);
            if (k == VirtualKey.Q) UsarPocion(P1, "vida");
            if (k == VirtualKey.E) UsarPocion(P1, "energia");

            // JUGADOR 2
            if (k == VirtualKey.Up) ProcesarAccion(P2, P1, k, "flojo", 15, 0.8, txtUp);
            if (k == VirtualKey.Left) ProcesarAccion(P2, P1, k, "fuerte", 40, 3.0, txtLeft);
            if (k == VirtualKey.Down) ProcesarAccion(P2, P1, k, "escudo", 15, 0, txtDown);
            if (k == VirtualKey.Right) ProcesarAccion(P2, P1, k, "relax", 0, 5.0, txtRight);
            if (k == VirtualKey.O) UsarPocion(P2, "vida");
            if (k == VirtualKey.P) UsarPocion(P2, "energia");
        }

        private async void ProcesarAccion(iPokemon user, iPokemon target, VirtualKey k, string tipo, double coste, double cd, TextBlock txt)
        {
            bool ocupado = (user == P1) ? p1Ocupado : p2Ocupado;
            if (ocupado || !cooldownsActivos[k]) return;
            if (user.Energia < coste) { FeedbackError(txt); return; }

            // Si intenta poner escudo pero ya tiene uno, ignoramos
            if (tipo == "escudo" && ((user == P1 && p1Protegido) || (user == P2 && p2Protegido))) return;

            SetOcupado(user, true);
            user.Energia -= coste;

            switch (tipo)
            {
                case "flojo":
                    user.animacionAtaqueFlojo();
                    AplicarDano(user, target, 10);
                    break;
                case "fuerte":
                    user.animacionAtaqueFuerte();
                    AplicarDano(user, target, 25);
                    break;
                case "escudo":
                    if (user == P1) p1Protegido = true; else p2Protegido = true;
                    user.verEscudo(true);
                    txt.Opacity = 0.4; // "Modo Cooldown" visual mientras el escudo esté activo
                    cooldownsActivos[k] = false;
                    break;
                case "relax":
                    user.Vida += 15; user.Energia += 25;
                    user.animacionDescasar();
                    break;
            }

            if (tipo != "escudo") IniciarCooldownIndividual(k, cd, txt);

            await Task.Delay(MS_BLOQUEO_ACCION);
            SetOcupado(user, false);
        }

        private async void AplicarDano(iPokemon user, iPokemon target, double cantidad)
        {
            bool protegido = (target == P1) ? p1Protegido : p2Protegido;
            if (protegido)
            {
                // ELIMINAR ESCUDO AL RECIBIR DAÑO
                if (target == P1)
                {
                    p1Protegido = false;
                    txtS.Opacity = 1.0; // Vuelve a la normalidad
                    cooldownsActivos[VirtualKey.S] = true;
                }
                else
                {
                    p2Protegido = false;
                    txtDown.Opacity = 1.0; // Vuelve a la normalidad
                    cooldownsActivos[VirtualKey.Down] = true;
                }
                target.verEscudo(false);
                txtConsola.Text = "¡ESCUDO ROTO!";
            }
            else
            {
                bool estabaHeridoAntes = (target == P1) ? p1Herido : p2Herido;
                target.Vida -= cantidad;
                SetOcupado(target, true);
                if (target.Vida < 50 && !estabaHeridoAntes)
                {
                    target.animacionHerido();
                    if (target == P1) p1Herido = true; else p2Herido = true;
                }
                await Task.Delay(MS_BLOQUEO_DOLOR);
                SetOcupado(target, false);
            }
        }

        private void UsarPocion(iPokemon p, string tipo)
        {
            if (p == P1)
            {
                if (tipo == "vida" && !p1PocionVidaUsada)
                {
                    P1.Vida += 40; p1PocionVidaUsada = true;
                    P1.verPocionVida(false); txtQ.Opacity = 0.2; // Apagado permanente
                }
                else if (tipo == "energia" && !p1PocionEnergiaUsada)
                {
                    P1.Energia += 50; p1PocionEnergiaUsada = true;
                    P1.verPocionEnergia(false); txtE.Opacity = 0.2; // Apagado permanente
                }
            }
            else
            {
                if (tipo == "vida" && !p2PocionVidaUsada)
                {
                    P2.Vida += 40; p2PocionVidaUsada = true;
                    P2.verPocionVida(false); txtO.Opacity = 0.2;
                }
                else if (tipo == "energia" && !p2PocionEnergiaUsada)
                {
                    P2.Energia += 50; p2PocionEnergiaUsada = true;
                    P2.verPocionEnergia(false); txtP.Opacity = 0.2;
                }
            }
        }

        private void SetOcupado(iPokemon p, bool estado)
        {
            if (p == P1) p1Ocupado = estado; else p2Ocupado = estado;
        }

        private async void IniciarCooldownIndividual(VirtualKey k, double seg, TextBlock txt)
        {
            if (seg <= 0) return;
            cooldownsActivos[k] = false;
            tiemposRestantes[k] = seg;
            txt.Opacity = 0.4;
            await Task.Delay((int)(seg * 1000));
            cooldownsActivos[k] = true;
            txt.Opacity = 1.0;
        }

        private async void FeedbackError(TextBlock txt)
        {
            var col = txt.Foreground;
            txt.Foreground = new SolidColorBrush(Colors.Red);
            await Task.Delay(250);
            txt.Foreground = col;
        }

        private void ActualizarLabelsUI()
        {
            ActualizarTexto(VirtualKey.W, txtW, "[W] FLOJO (15 EP)");
            ActualizarTexto(VirtualKey.A, txtA, "[A] FUERTE (40 EP)");
            ActualizarTexto(VirtualKey.S, txtS, "[S] ESCUDO (15 EP)");
            ActualizarTexto(VirtualKey.D, txtD, "[D] RELAX");
            ActualizarTexto(VirtualKey.Up, txtUp, "[↑] FLOJO (15 EP)");
            ActualizarTexto(VirtualKey.Left, txtLeft, "[←] FUERTE (40 EP)");
            ActualizarTexto(VirtualKey.Down, txtDown, "[↓] ESCUDO (15 EP)");
            ActualizarTexto(VirtualKey.Right, txtRight, "[→] RELAX");
        }

        private void ActualizarTexto(VirtualKey k, TextBlock txt, string baseT)
        {
            if (tiemposRestantes[k] > 0)
            {
                tiemposRestantes[k] -= 0.1;
                txt.Text = $"{baseT} | {Math.Max(0, tiemposRestantes[k]):F1}s";
            }
            else txt.Text = baseT;
        }
    }
}