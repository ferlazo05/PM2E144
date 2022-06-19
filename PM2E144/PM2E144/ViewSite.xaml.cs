using PM2E144.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E144
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSite : ContentPage
    {
        public ViewSite()
        {
            InitializeComponent();
        }

        private async void btnmapa_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtid.Text))
            {
                var site = await App.DBase.GetSitesByIdAsync(Int32.Parse(txtid.Text));
                if (site != null)
                {
                    MapPage mapa = new MapPage();
                    mapa.BindingContext = site;
                    await Navigation.PushAsync(mapa);
                }
            }
        }
        private async void btneliminar_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("ADVERTENCIA", "Desea eliminar el sitio?", "Yes", "No");
            if (action)
            {
                var site = await App.DBase.GetSitesByIdAsync(Int32.Parse(txtid.Text));
                if (site != null)
                {
                    await App.DBase.DeleteSiteAsync(site);
                    await DisplayAlert("Eliminado", "Se elimino de manera exitosa!", "Ok");
                    await Navigation.PopToRootAsync();
                }
            }
        }

        private async void btncompartirubi_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Sitio",
                Title = "Sitio visitado",
                Text = "Te comparto este sitio",
                Uri = "http://maps.google.com/?q="+txtlatitud.Text+","+txtlongitud.Text
            });
        }

        private async void btncompartirimg_Clicked(object sender, EventArgs e)
        {
            var image = await MediaPicker.PickPhotoAsync();

            if(image == null)
            {
                return;
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Sitio visitado",
                File = new ShareFile(image)
            }); ; ;
        }
    }
}