using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Models.BD1;
using Portafolio.ViewModel;

namespace Portafolio.Controllers
{
    public class ProyectosController : Controller
    {

        private readonly PortafolioDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProyectosController(PortafolioDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Proyectos() 
        {

            var img = _httpContextAccessor.HttpContext.Session.GetString("imgP");
            var usuario = _httpContextAccessor.HttpContext.Session.GetInt32("Usuario");
            var CualquierUsuario = _httpContextAccessor.HttpContext.Session.GetInt32("CualquierUsuario");

            if (img != null)
            {
                ViewBag.ImgPerfil = img;
            }


            if (usuario != null)
            {
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
                    NombreLenguaje = lenguaje.NombreLenguaje,
                    
                }).ToList();
                return View(proyectos);
            }
            else 
            {
                if (CualquierUsuario != null)
                {
                    var proyectosCualquierUsuario = _dbContext.Proyectos
                        .Where(pu => pu.ID_Usuario == CualquierUsuario)
                        .Join(_dbContext.Lenguaje,
                        proyectosCualquierUsuario => proyectosCualquierUsuario.ID_Lenguaje,
                        lenguajeCU => lenguajeCU.ID_Lenguaje,
                        (proyectosCualquierUsuario, lenguajeCU) => new ProyectosVM
                        {
                            TituloProy = proyectosCualquierUsuario.TituloProy,
                            DescripcionProy = proyectosCualquierUsuario.DescripcionProy,
                            LinkProy = proyectosCualquierUsuario.LinkProy,
                            ImgProy = proyectosCualquierUsuario.ImgProy,
                            NombreLenguaje = lenguajeCU.NombreLenguaje
                        }).ToList();
                    return View(proyectosCualquierUsuario);

                }
                else 
                {
                    return NotFound();
                }
            }


            return View();
        }



    }
}
