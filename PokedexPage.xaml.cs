    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
namespace ProyectoVacioUWP_Base
    {
        public sealed partial class PokedexPage : Page
        {
            private List<Pokemon> _allPokemon = new List<Pokemon>();
            private List<Pokemon> _filteredPokemon = new List<Pokemon>();

            private HashSet<string> selectedTypes = new HashSet<string>();
            private DispatcherTimer _searchTimer;
        private readonly Dictionary<string, Color> typeColors = new Dictionary<string, Color>()
        {
            { "fuego", Color.FromArgb(255, 240, 128, 48) },
            { "agua", Color.FromArgb(255, 104, 144, 240) },
            { "planta", Color.FromArgb(255, 120, 200, 80) },
            { "electrico", Color.FromArgb(255, 248, 208, 48) },
            { "volador", Color.FromArgb(255, 168, 144, 240) },
            { "normal", Color.FromArgb(255, 168, 168, 120) },
            { "lucha", Color.FromArgb(255, 192, 48, 40) },
            { "veneno", Color.FromArgb(255, 160, 64, 160) },
            { "tierra", Color.FromArgb(255, 224, 192, 104) },
            { "roca", Color.FromArgb(255, 184, 160, 56) },
            { "bicho", Color.FromArgb(255, 168, 184, 32) },
            { "fantasma", Color.FromArgb(255, 112, 88, 152) },
            { "hielo", Color.FromArgb(255, 152, 216, 216) },
            { "dragon", Color.FromArgb(255, 112, 56, 248) },
            { "siniestro", Color.FromArgb(255, 112, 88, 72) },
            { "acero", Color.FromArgb(255, 184, 184, 208) },
            { "psiquico", Color.FromArgb(255, 248, 88, 136) },
            { "hada", Color.FromArgb(255, 238, 153, 172) }
        };
        public PokedexPage()
            {
                this.InitializeComponent();

                _searchTimer = new DispatcherTimer();
                _searchTimer.Interval = TimeSpan.FromMilliseconds(300);
                _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); ApplyFilters(); };

                LoadPokemon();
                ApplyFilters();
                InitTypeButtons();
            }
            private void LoadPokemon()
            {
            _allPokemon = new List<Pokemon>
            {
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),

                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
                new Pokemon("Torchic", "fuego", "ms-appx:///Assets/torchic.png", typeof(TorchicJFBR)),
                new Pokemon("Computerror", "eléctrico/fantasma", "ms-appx:///Assets/computerror.png", typeof(ComputerrorDLM)),
                new Pokemon("Ditto", "normal", "ms-appx:///Assets/ditto.png", typeof(DittoAEJ)),
            };
            }

            private void ApplyFilters()
            {

                string query = NormalizeType(sb.Text ?? "");

                _filteredPokemon = _allPokemon.Where(p =>
                {
                    var pokemonTypes = GetNormalizedTypes(p.Tipo);

                    bool matchesType = selectedTypes.Count == 0 ||
                                       selectedTypes.All(t => pokemonTypes.Contains(t));

                    bool matchesName = string.IsNullOrWhiteSpace(query) ||
                            NormalizeType(p.Nombre).Contains(query);
                    return matchesType && matchesName;

                }).ToList();

                PokemonList.ItemsSource = _filteredPokemon;
            }

            private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    _searchTimer.Stop();
                    _searchTimer.Start();
                }
            }

            private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
            {
                ApplyFilters();
            }

        private void TypeFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is StackPanel stack)
            {
                var textBlock = stack.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock == null) return;
                var type = NormalizeType(textBlock.Text.Trim());
                bool isSelected;

                if (selectedTypes.Contains(type))
                {
                    selectedTypes.Remove(type);
                    isSelected = false;
                }
                else
                {
                    selectedTypes.Add(type);
                    isSelected = true;
                }

                UpdateButtonVisual(button, type, isSelected);
                ApplyFilters();
            }
        }
        private void UpdateButtonVisual(Button button, string type, bool isSelected)
        {
            if (!typeColors.TryGetValue(type, out var baseColor))
                baseColor = Colors.Gray;

            if (isSelected)
            {
                button.Background = new SolidColorBrush(baseColor);
                button.BorderBrush = new SolidColorBrush(Colors.White);
                button.BorderThickness = new Thickness(3);
            }
            else
            {
                button.Background = new SolidColorBrush(
                    Color.FromArgb(120, baseColor.R, baseColor.G, baseColor.B));

                button.BorderBrush = new SolidColorBrush(baseColor);
                button.BorderThickness = new Thickness(1);
            }
        }

        private string NormalizeType(string type)
            {
                if (string.IsNullOrWhiteSpace(type)) return "";

                string normalized = type.Normalize(NormalizationForm.FormD);
                var sbText = new StringBuilder();

                foreach (var c in normalized)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        sbText.Append(c);
                }

                return sbText.ToString().ToLowerInvariant();
            }

        private void InitTypeButtons()
        {
            TypeFilterStack.Items.Clear();

            var types = new[]
            {
       new { Name="Fuego", Bg="#88FF0000", Border="#F08030", Icon="Assets/fuego.png" },
        new { Name="Agua", Bg="#8800A0FF", Border="#6890F0", Icon="Assets/agua.png" },
        new { Name="Planta", Bg="#8800FF00", Border="#78C850", Icon="Assets/planta.png" },
        new { Name="Psíquico", Bg="#88F85888", Border="#F85888", Icon="Assets/psiquico.png" },
        new { Name="Volador", Bg="#88A890F0", Border="#A890F0", Icon="Assets/volador.png" },
        new { Name="Normal", Bg="#88A8A878", Border="#A8A878", Icon="Assets/normal.png" },
        new { Name="Lucha", Bg="#88C03028", Border="#C03028", Icon="Assets/lucha.png" },
        new { Name="Veneno", Bg="#88A040A0", Border="#A040A0", Icon="Assets/veneno.png" },
        new { Name="Tierra", Bg="#88E0C068", Border="#E0C068", Icon="Assets/tierra.png" },
        new { Name="Roca", Bg="#88B8A038", Border="#B8A038", Icon="Assets/roca.png" },
        new { Name="Bicho", Bg="#88A8B820", Border="#A8B820", Icon="Assets/bicho.png" },
        new { Name="Fantasma", Bg="#88705898", Border="#705898", Icon="Assets/fantasma.png" },
        new { Name="Hielo", Bg="#8898D8D8", Border="#98D8D8", Icon="Assets/hielo.png" },
        new { Name="Dragón", Bg="#887038F8", Border="#7038F8", Icon="Assets/dragon.png" },
        new { Name="Siniestro", Bg="#88705848", Border="#705848", Icon="Assets/siniestro.png" },
        new { Name="Acero", Bg="#88B8B8D0", Border="#B8B8D0", Icon="Assets/acero.png" },
        new { Name="Hada", Bg="#88EE99AC", Border="#EE99AC", Icon="Assets/hada.png" },
        new { Name="Eléctrico", Bg="#88F8D030", Border="#F8D030", Icon="Assets/electrico.png" }
    };

            foreach (var type in types)
            {
                var btn = new Button
                {
                    Style = (Style)Resources["NoGrayButtonStyle"],
                    Background = new SolidColorBrush(ColorHelper.FromArgb(
                        Convert.ToByte(type.Bg.Substring(1, 2), 16),
                        Convert.ToByte(type.Bg.Substring(3, 2), 16),
                        Convert.ToByte(type.Bg.Substring(5, 2), 16),
                        Convert.ToByte(type.Bg.Substring(7, 2), 16)
                    )),
                    BorderBrush = new SolidColorBrush(ColorHelper.FromArgb(
                        255,
                        Convert.ToByte(type.Border.Substring(1, 2), 16),
                        Convert.ToByte(type.Border.Substring(3, 2), 16),
                        Convert.ToByte(type.Border.Substring(5, 2), 16)
                    )),
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 4,
                        Children =
                {
                    new Image { Source = new BitmapImage(new Uri("ms-appx:///" + type.Icon)), Width=24, Height=24 },
                    new TextBlock { Text = type.Name, Foreground = new SolidColorBrush(Colors.White), FontWeight = Windows.UI.Text.FontWeights.Bold }
                }
                    }
                };

                btn.PointerEntered += (s, e) => btn.RenderTransform = new ScaleTransform { ScaleX = 1.1, ScaleY = 1.1 };
                btn.PointerExited += (s, e) => btn.RenderTransform = new ScaleTransform { ScaleX = 1.0, ScaleY = 1.0 };

                var textBlock = ((btn.Content as StackPanel).Children.OfType<TextBlock>().FirstOrDefault());
                if (textBlock != null)
                {
                    var normalizedType = NormalizeType(textBlock.Text);
                    UpdateButtonVisual(btn, normalizedType, false);
                }

                btn.Click += TypeFilter_Click;
                TypeFilterStack.Items.Add(btn);
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e) { base.OnNavigatedTo(e); }
            private void PokemonList_ItemClick(object sender, ItemClickEventArgs e)
            {
                if (e.ClickedItem is Pokemon selected)
                {
                    Frame.Navigate(typeof(VisorPokemon), selected.PokemonType);
                }
            }
            private List<string> GetNormalizedTypes(string tipoRaw)
            {
                if (string.IsNullOrWhiteSpace(tipoRaw))
                    return new List<string>();

                return tipoRaw
                    .Split('/', ',', ' ')
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Select(t => NormalizeType(t.Trim()))
                    .ToList();
            }
        }
    }