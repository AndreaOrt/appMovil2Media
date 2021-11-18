using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;

namespace PM2Media
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnTomarFoto_Clicked(System.Object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Alerta", "Cámara no disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //Directory = "Sample",
                //Name = "test.jpg"
                SaveToAlbum = true
            });

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            pathFoto.Text = file.AlbumPath;
            
            //convertir a arreglo de bytes
            /*byte[] fileByte = System.IO.File.ReadAllBytes(file.AlbumPath);
            Debug.WriteLine("En Bytes: " + fileByte);*/

            //convertir a base64
            //string pathBase64 = Convert.ToBase64String(fileByte);

            //txtAnother.Text = Convert.ToString(Convert.FromBase64String(pathBase64));

            fotografia.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void btnElegirFoto_Clicked(System.Object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Alerta", "No se puede elegir una foto", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            pathFoto.Text = file.Path;

            fotografia.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void btnRestApi_Clicked(System.Object sender, System.EventArgs e)
        {
            byte[] fileByte = System.IO.File.ReadAllBytes(pathFoto.Text);
            Debug.WriteLine("En Bytes: " + fileByte);

            //convertir a base64
            string pathBase64 = Convert.ToBase64String(fileByte);

            var user = new Models.ApiMovil2.Empleado
            {
                Name = "Test2",
                Location = 1,
                user_image = pathBase64
            };

            await CreateEmple(user);
        }

        //Metodo POST
        public async static Task CreateEmple(Models.ApiMovil2.Empleado user)
        {
            String jsonContent = JsonConvert.SerializeObject(user);
            StringContent contenido = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage response = await cliente.PostAsync("https://movil2.herokuapp.com/principal/insert/user/account/", contenido);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Empleado creado");
                }
                else
                {
                    Debug.WriteLine("Error!");
                }
            }
        }
    }
}
