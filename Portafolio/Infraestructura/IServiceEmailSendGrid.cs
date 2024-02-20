using Portafolio.ViewModel;

namespace Portafolio.Infraestructura
{
    public interface IServiceEmailSendGrid
    {
        Task Enviar(ContactoVM contactoVM);
    }
}
