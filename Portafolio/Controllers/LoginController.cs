using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.ViewModel;

namespace Portafolio.Controllers
{
    public class LoginController : Controller
    {

        private readonly PortafolioDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LoginController(PortafolioDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult login() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CuentaVM cuentaVM)
        {
            if (ModelState.IsValid)
            {
                var cuenta = _dbContext.Cuenta.FirstOrDefault(c => c.Correo == cuentaVM.Correo && c.Contrasena == cuentaVM.Contrasena);

                if (cuenta != null)
                {
                    var usuario = _dbContext.Usuario.FirstOrDefault(u => u.ID_Usuario == cuenta.ID_Usuario);

                    if (usuario != null)
                    {
                        HttpContext.Session.SetInt32("Usuario", usuario.ID_Usuario);
                    }
                    else
                    {
                        return RedirectToAction("CrearCuenta");
                    }


                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    ViewBag.MsjError = "Las credenciales ingresadas no son válidas.";
                    return View(cuentaVM);
                }
            }
            else
            {
                return View(cuentaVM);
            }

        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("Usuario");

            return RedirectToAction("Home", "Home");
        }


    }
}
