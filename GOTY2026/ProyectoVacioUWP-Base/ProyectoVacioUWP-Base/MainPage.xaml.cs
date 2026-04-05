using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace ProyectoVacioUWP_Base
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            frCentral.Navigate(typeof(HomePage), this);

            SystemNavigationManager.GetForCurrentView().BackRequested += OpcionVolver;
            frCentral.Navigated += FrCentral_Navigated;
        }

        private void FrCentral_Navigated(object sender, NavigationEventArgs e)
        {

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                frCentral.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }

        private void OpcionVolver(object sender, BackRequestedEventArgs e)
        {
            if (frCentral.CanGoBack)
            {
                e.Handled = true;
                frCentral.GoBack();
            }
        }

        private void NavViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = sender as NavigationViewItem;
            if (item == null) return;

            var currentPageType = frCentral.CurrentSourcePageType;

            switch (item.Tag)
            {
                case "Inicio":
                    if (currentPageType != typeof(HomePage))
                    {
                        frCentral.BackStack.Clear();
                        frCentral.ForwardStack.Clear();

                        frCentral.Navigate(typeof(HomePage), this);

                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            AppViewBackButtonVisibility.Collapsed;
                        frCentral.BackStack.Clear();
                        frCentral.ForwardStack.Clear();

                    }
                    break;

                case "Pokedex":
                    if (currentPageType != typeof(PokedexPage))
                    {
                        frCentral.Navigate(typeof(PokedexPage), this);
                    }
                    break;

                case "Combate":
                    if (currentPageType != typeof(SeleccionPage))
                    {
                        frCentral.Navigate(typeof(SeleccionPage), this);
                    }
                    break;

                case "AcercaDe":
                    if (currentPageType != typeof(AcercaDePage))
                    {
                        frCentral.Navigate(typeof(AcercaDePage), this);
                    }
                    break;
            }
        }
    }
}