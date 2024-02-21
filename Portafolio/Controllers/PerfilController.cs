using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Models.BD1;
using Portafolio.ViewModel;

namespace Portafolio.Controllers
{


    public class PerfilController : Controller
    {

        private readonly PortafolioDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PerfilController(PortafolioDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult InformacionHome()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InformacionHome(InfHomeVM infHomeVM)
        {
            var usuario = _httpContextAccessor.HttpContext.Session.GetInt32("Usuario");

            if (ModelState.IsValid)
            {
                try
                {
                    if (usuario != null)
                    {
                        InfHome inf = new InfHome();

                        inf.TituloDeBienvenida = infHomeVM.TituloDeBienvenida;
                        inf.DescripcionHome = infHomeVM.DescripcionHome;
                        inf.ID_Usuario = usuario;

                        _dbContext.Add(inf);
                        await _dbContext.SaveChangesAsync();

                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorInfH = "Error";
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Error(ex.Message);
                }
            }
            return View();
        }


        public IActionResult ListaDeProyectos()
        {
            var img = _httpContextAccessor.HttpContext.Session.GetString("imgP");
            var usuario = _httpContextAccessor.HttpContext.Session.GetInt32("Usuario");

            if (img != null)
            {
                ViewBag.ImgPerfil = img;
            }

            var proyectos = _dbContext.Proyectos
                .Where(p => p.ID_Usuario == usuario)
                .Join(_dbContext.Lenguaje,
                 proyecto => proyecto.ID_Lenguaje,
                 lenguaje => lenguaje.ID_Lenguaje,
                 (proyecto, lenguaje) => new ProyectosVM
                 {
                     TituloProy = proyecto.TituloProy,
                     DescripcionProy = proyecto.DescripcionProy,
                     LinkProy = proyecto.LinkProy,
                     ImgProy = proyecto.ImgProy,
                     NombreLenguaje = lenguaje.NombreLenguaje
                 }).ToList();
            return View(proyectos);
        }

        public IActionResult CrearProyectos()
        {
            var img = _httpContextAccessor.HttpContext.Session.GetString("imgP");

            if (img != null)
            {
                ViewBag.ImgPerfil = img;
            }

            var len = _dbContext.Lenguaje.ToList();
            ViewBag.Lenguaje = len;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CrearProyectos(ProyectosVM proyectosVM, IFormFile ImgProyecto)
        {
            var usuario = _httpContextAccessor.HttpContext.Session.GetInt32("Usuario");

            if (ModelState.IsValid)
            {
                Stream image = ImgProyecto.OpenReadStream();
                string urlimagen = await SubirImagenProyecto(image, ImgProyecto.FileName);

                try
                {
                    Proyectos p = new Proyectos();
                    p.TituloProy = proyectosVM.TituloProy;
                    p.DescripcionProy = proyectosVM.DescripcionProy;
                    p.LinkProy = proyectosVM.LinkProy;
                    p.ImgProy = urlimagen;
                    p.ID_Usuario = usuario;
                    p.ID_Lenguaje = proyectosVM.ID_Lenguaje;

                    _dbContext.Add(p);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("Home", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error(ex.Message);
                }
            }
            return View();
        }



        public async Task<string> SubirImagenProyecto(Stream archivo, string nombre)
        {
            string email = "";
            string clave = "";
            string ruta = "";
            string api_key = "";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("Fotos_Proyectos")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;

            return downloadURL;
        }




    }
}
