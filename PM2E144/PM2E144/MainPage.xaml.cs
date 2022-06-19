using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PM2E144.Models;
using Plugin.Media;
using Xamarin.Essentials;

namespace PM2E144
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        double latitud;
        double longitud;

        Plugin.Media.Abstractions.MediaFile FileFoto = null;
        public MainPage()
        {
            InitializeComponent();
            Localizacion();
        }

        private async void Localizacion()
        {
            try
            {
                var localizacion = await Geolocation.GetLocationAsync();
                if (localizacion != null)
                {
                    latitud = localizacion.Latitude;
                    longitud = localizacion.Longitude;
                }
                txtlatitud.Text = latitud.ToString();
                txtlongitud.Text = longitud.ToString();
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

        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
            return null;
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "MySites",
                    Name = "sitio.jpg",
                    SaveToAlbum = true
                });

                //await DisplayAlert("Directorio", FileFoto.Path, "Ok");

                if (FileFoto != null)
                {
                    Foto.Source = ImageSource.FromStream(() =>
                    {
                        return FileFoto.GetStream();
                    });
                }
            }
            catch (PermissionException ex)
            {
                await DisplayAlert("Advertencia", "Debe activar el permiso de camara.", "Ok");
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "Debe usar la camara desde la aplicacion.", "Ok");
                System.Environment.Exit(0);
            }
            
        }

        private async void btnagregar_Clicked(object sender, EventArgs e)
        {
            if (FileFoto == null)
            {
                await DisplayAlert("Aviso", "Necesita tomar una fotografia", "OK");
                return;
            }
            else if(!string.IsNullOrEmpty(txtlatitud.Text) && !string.IsNullOrEmpty(txtlongitud.Text) && !string.IsNullOrEmpty(txtdescripcion.Text))
            {
                var site = new Sites
                {
                    id = 0,
                    latitud = txtlatitud.Text,
                    longitud = txtlongitud.Text,
                    descripcion = txtdescripcion.Text,
                    foto = ConvertImageToByteArray()
                };

                try
                {
                    var result = await App.DBase.SaveSiteAsync(site);
                    if (result > 0)
                    {
                        await DisplayAlert("Registro", "Sitio registrado con exito!", "OK");
                        LimpiarTxt();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", "No se pudo registrar, intente de nuevo.", "OK");
                }

                /*
                var result = await App.DBase.SaveSiteAsync(site);

                if (result > 0)
                {
                    await DisplayAlert("Registro", "Sitio registrado con exito!", "OK");
                    LimpiarTxt();
                }
                else
                {
                    await DisplayAlert("Aviso", "No se pudo registrar.", "OK");
                }

                */
            }
            else
            {
                await DisplayAlert("Aviso", "Debe llenar los campos.", "Ok");
                return;
            }
        }

        private async void btnlista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPage());
        }

        private void LimpiarTxt()
        {
            Foto.Source = null;
            Localizacion();
            txtdescripcion.Text = "";
        }
    }
}
