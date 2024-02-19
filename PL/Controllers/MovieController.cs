using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PL.Controllers
{
    //inyeccion de dependencias
    //patrones de diseño
    //tipo de dato dynamic 
    //DTO's -- crear modelos solo con las propiedades que tu quieres en una determinada peticioin
    public class MovieController : Controller
    {
        public IActionResult GetPopulares()
        {
            Models.Movie movie = new Models.Movie();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("movie/popular?api_key=29829358e000b2dec2f1e8dfc8226db9");
                responseTask.Wait(); // Llamada al metodo de la api
                var respuesta = responseTask.Result;
                if (respuesta.IsSuccessStatusCode)
                {
                    var readTask = respuesta.Content.ReadAsStringAsync();
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    dynamic JsonObject = JObject.Parse(readTask.Result);

                    foreach (var registro in JsonObject.results)
                    {
                        Models.Movie movieobj = new Models.Movie();
                        movieobj.IdMovie = registro.id;
                        movieobj.Nombre = registro.original_title;
                        movieobj.Poster = "https://image.tmdb.org/t/p/w300_and_h450_bestv2/" + registro.poster_path;

                        movie.Movies.Add(movieobj);
                    }
                }
                else
                {
                    return View();
                }


            }
            return View(movie);
        }
        [HttpGet]
        public IActionResult AgregarFavorito(int idPelicula)
        {
            using (HttpClient client = new HttpClient())
            {
                //un objeto anonimo
                //media_type ="movie"
                //"media_id" = idPelicula,
                var movie = new
                {
                    media_type = "movie",
                    media_id = idPelicula,
                    favorite = true
                };

                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responseTask = client.PostAsJsonAsync("account/20961193/favorite?api_key=&session_id=", movie);
                responseTask.Wait(); // Llamada al metodo de la api
                var respuesta = responseTask.Result;
                if (respuesta.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Pelicula agregada a favoritos!";
                    return PartialView("Modal");
                }
                else
                {
                    var readTask = respuesta.Content.ReadAsStringAsync();
                    readTask.Wait();
                    dynamic JsonObject = JObject.Parse(readTask.Result);
                    ViewBag.Mensaje = JsonObject.status_message;
                    return PartialView("Modal");
                }
            }
        }
    }
}
