using System;
using System.Collections.Generic;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;

namespace PM2Media.Views
{
    public partial class UploadFilePage : ContentPage
    {
        public UploadFilePage()
        {
            InitializeComponent();
        }

        private async void btnfoto_Clicked(System.Object sender, System.EventArgs e)
        {
            var tomarfoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MiApp",
                Name = "Image.jpg"
            });

            await DisplayAlert("Ubicación del archivo", tomarfoto.Path, "OK");

            if(tomarfoto != null)
            {
                Imagen.Source = ImageSource.FromStream(() => { return tomarfoto.GetStream(); });
            }

            byte[] ImageBytes = null;

            using(var stream = new MemoryStream())
            {
                tomarfoto.GetStream().CopyTo(stream);
                tomarfoto.Dispose();
                ImageBytes = stream.ToArray();
            }
        }
    }
}
