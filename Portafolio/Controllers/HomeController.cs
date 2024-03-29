﻿using Firebase.Auth;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, PortafolioDBContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {

            _logger = logger;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public IActionResult Home()
        {

            var usuario = _httpContextAccessor.HttpContext.Session.GetInt32("Usuario");



            var Datos = _dbContext.Usuario.FirstOrDefault(u => u.ID_Usuario == usuario);

            var FechaActual = DateTime.Today;

            if (Datos != null)
            {
                var Especialidad = _dbContext.Especialidad.FirstOrDefault(s => s.ID_Especialidad == Datos.ID_Especialidad);

                int edad = FechaActual.Year - Datos.FechaNacimiento.Year;

                if (Datos.FechaNacimiento.Date > FechaActual.AddYears(-edad))
                {
                    edad--;
                }
                ViewBag.ImgPerfil = Datos.ImgUsuario;
                HttpContext.Session.SetString("imgP", Datos.ImgUsuario);

                ViewBag.Edad = edad;
                ViewBag.Especialidad = Especialidad.NombreEspecialidad;

                var InfHome = _dbContext.InfHome.FirstOrDefault(i => i.ID_Usuario == Datos.ID_Usuario);

                if (InfHome != null)
                {
                    ViewBag.titulo = InfHome.TituloDeBienvenida;
                    ViewBag.Descripcion = InfHome.DescripcionHome;
                }

                var Proyecto = _dbContext.Proyectos.Where(p => p.ID_Usuario == Datos.ID_Usuario)
                         .OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                if (Proyecto != null)
                {
                    ViewBag.ImgPro = Proyecto.ImgProy;
                    ViewBag.TituloPro = Proyecto.TituloProy;
                    ViewBag.DescPro = Proyecto.DescripcionProy;
                }

                return View(Datos);
            }
            else
            {
                var cualquierRegistro = _dbContext.Usuario.OrderBy(c => Guid.NewGuid()).FirstOrDefault();

                if (cualquierRegistro != null)
                {
                    var EspecialidadDeUsuario = _dbContext.Especialidad.FirstOrDefault(es => es.ID_Especialidad == cualquierRegistro.ID_Especialidad);

                    int EdadCualquierRegistro = FechaActual.Year - cualquierRegistro.FechaNacimiento.Year;

                    if (cualquierRegistro.FechaNacimiento.Date > FechaActual.AddYears(-EdadCualquierRegistro))
                    {
                        EdadCualquierRegistro--;
                    }

                    ViewBag.Edad = EdadCualquierRegistro;
                    ViewBag.Especialidad = EspecialidadDeUsuario != null ? EspecialidadDeUsuario.NombreEspecialidad : "Sin especialidad";

                    var InfHCualquierU = _dbContext.InfHome.FirstOrDefault(inf => inf.ID_Usuario == cualquierRegistro.ID_Usuario);

                    if (InfHCualquierU != null)
                    {
                        ViewBag.titulo = InfHCualquierU.TituloDeBienvenida;
                        ViewBag.Descripcion = InfHCualquierU.DescripcionHome;
                    }

                    var Proyecto = _dbContext.Proyectos.Where(p => p.ID_Usuario == cualquierRegistro.ID_Usuario)
                        .OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    if (Proyecto != null)
                    {
                        ViewBag.ImgPro = Proyecto.ImgProy;
                        ViewBag.TituloPro = Proyecto.TituloProy;
                        ViewBag.DescPro = Proyecto.DescripcionProy;
                    }


                    HttpContext.Session.SetInt32("CualquierUsuario", cualquierRegistro.ID_Usuario);


                    return View(cualquierRegistro);

                }
                else 
                {
                    return NotFound();
                }   
            }
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

                }
                catch (Exception ex)
                {

                    ViewBag.Error(ex.Message);
                }
            }
            return View();
        }



        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            string email = _configuration.GetValue<string>("STORAGE_EMAIL");
            string clave = _configuration.GetValue<string>("STORAGE_CLAVE");
            string ruta = _configuration.GetValue<string>("STORAGE_RUTA");
            string api_key = _configuration.GetValue<string>("STORAGE_APIKEY");

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
