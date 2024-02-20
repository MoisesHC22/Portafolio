using Portafolio.Infraestructura;
using Portafolio.ViewModel;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Portafolio.Servicios
{
    public class ServiceEmailSendGrid : IServiceEmailSendGrid
    {
        private readonly IConfiguration _configuration;

        public ServiceEmailSendGrid(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Enviar(ContactoVM contactoVM)
        {
            var apiKey = _configuration.GetValue<string>("SENDGRIP_API_KEY");
            var email = _configuration.GetValue<string>("SENGRID_FROM");
            var nombre = _configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);

            var from = new EmailAddress(email, nombre);
            var subject = $"El cliente {contactoVM.Email} quiere contactarte";
            var to = new EmailAddress(email, nombre);
            var mensajeTexto = contactoVM.Mensaje;
            var contenidoHtml = $@"De: {contactoVM.Nombre} - Email: {contactoVM.Email} Mensaje: {contactoVM.Mensaje}";


            var singleEmail = MailHelper.CreateSingleEmail(from, to, subject, mensajeTexto, contenidoHtml);

            var respuesta = await cliente.SendEmailAsync(singleEmail);
        }
    }
}
