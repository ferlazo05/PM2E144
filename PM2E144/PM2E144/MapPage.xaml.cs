using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E144.Models;
using Plugin.Media;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E144
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var localizacion = await Geolocation.GetLocationAsync();

            if (localizacion != null)
            {
                var pin = new Pin()
                {
                    Position = new Position(localizacion.Latitude, localizacion.Longitude),
                    Label = txtdescripcion.Text
                };
                mapa.Pins.Add(pin);
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(localizacion.Latitude, localizacion.Longitude), Distance.FromMeters(100.00)));
            }
        }
    }
}