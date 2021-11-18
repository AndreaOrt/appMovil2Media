using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PM2Media.Controller
{
    public class EmpleController
    {
        public EmpleController()
        {
        }

        public async static Task SubirImagen(byte[] imagen)
        {
            HttpClient cliente = new HttpClient();
            MultipartFormDataContent contenido = new MultipartFormDataContent();

            ByteArrayContent MapaBytes = new ByteArrayContent(imagen);
            contenido.Add(MapaBytes, "IMAGE", "filename.jpg");

            var respuesta = await cliente.PostAsync("url...", contenido);

            Debug.WriteLine(respuesta.Content.ReadAsStringAsync().Result);
        }
    }
}
