using Microsoft.AspNetCore.Mvc;
using Portafolio.Infraestructura;
using Portafolio.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Portafolio.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IServiceEmailSendGrid _emailSendGrid;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactoController(IServiceEmailSendGrid emailSendGrid, IHttpContextAccessor httpContextAccessor) 
        {
            _emailSendGrid = emailSendGrid;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Contacto() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contacto(ContactoVM contactoVM)
        {
            await _emailSendGrid.Enviar(contactoVM);


            return RedirectToAction("Home", "Home");
        }
    }
}
