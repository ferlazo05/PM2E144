using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E144.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PM2E144
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
            llenarDatos();
            Localizacion();
        }

        private async void Localizacion()
        {
            try
            {
                var localizacion = await Geolocation.GetLocationAsync();
            }
            catch (PermissionException ex)
            {
                await DisplayAlert("Advertencia", "Debe activar el permiso de gps.", "Ok");
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Advertencia", "Debe activar el gps.", "Ok");
                System.Environment.Exit(0);
            }

        }

        public async void llenarDatos()
        {
            var siteList = await App.DBase.GetSitesAsync();
            if (siteList != null)
            {
                lstSites.ItemsSource = siteList;
            }
        }

        private async void lstSites_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (Sites)e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.id.ToString()))
            {
                var site = await App.DBase.GetSitesByIdAsync(obj.id);
                if (site != null)
                {
                    ViewSite sitio = new ViewSite();
                    sitio.BindingContext = site;
                    await Navigation.PushAsync(sitio);
                }
            }
        }
    }
}