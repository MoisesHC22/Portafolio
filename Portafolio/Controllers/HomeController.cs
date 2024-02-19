using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Models.BD1;
using Portafolio.ViewModel;
using System.Diagnostics;

namespace Portafolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PortafolioDBContext _dbContext;
        public HomeController(ILogger<HomeController> logger, PortafolioDBContext dbContext)
        {

            _logger = logger;
            _dbContext = dbContext;
        }

       public IActionResult Home()
        {
            return View();
        }

        public IActionResult CrearCuenta()
        {
            var esp = _dbContext.Especialidad.ToList();
            ViewBag.Especialidad = esp;  
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(CuentaVM cuentaVM, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                Stream image = Imagen.OpenReadStream();
                string urlimagen = await SubirStorage(image, Imagen.FileName);

                try 
                {
                    Usuario u = new Usuario();
                       u.Nombre = cuentaVM.Nombre;
                       u.ApellidoMaterno = cuentaVM.ApellidoMaterno;
                       u.ApellidoPaterno = cuentaVM.ApellidoPaterno;
                       u.FechaNacimiento = cuentaVM.FechaNacimiento;
                       u.Telefono = cuentaVM.Telefono;
                       u.ImgUsuario = urlimagen;
                       u.ID_Especialidad = cuentaVM.ID_Especialidad;

                    _dbContext.Add(u);
                    await _dbContext.SaveChangesAsync();

                    Cuenta c = new Cuenta();
                       c.Correo = cuentaVM.Correo;
                       c.Contrasena = cuentaVM.Contrasena;
                       c.ID_Usuario = u.ID_Usuario;
                    _dbContext.Add(c);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("Home");

                }catch (Exception ex) 
                {

                    ViewBag.Error(ex.Message);
                } 
            }
            return View();
        }

        public IActionResult Login() 
        {
            return View();
        }



        public async Task<string> SubirStorage(Stream archivo, string nombre)
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
                .Child("Fotos_Perfil")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;

            return downloadURL;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
