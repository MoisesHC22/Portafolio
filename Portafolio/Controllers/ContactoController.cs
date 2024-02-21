using Microsoft.AspNetCore.Mvc;
using Portafolio.Infraestructura;
using Portafolio.ViewModel;

namespace Portafolio.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IServiceEmailSendGrid _emailSendGrid;

        public ContactoController(IServiceEmailSendGrid emailSendGrid) 
        {
            _emailSendGrid = emailSendGrid;
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
