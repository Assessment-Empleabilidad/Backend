using System;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Mailersend
{
    public class MailersendServices : IMailersendServices
    {
        private readonly HttpClient _httpClient;
        public MailersendServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(100); 
        }

        public void SendMail(string para, string usuario)
        {
            Console.WriteLine("SendMail");
            try
            {
                var requestBody = new
                {
                    from = new
                    {
                        email = "MS_9vUjoq@trial-z86org8zd6ngew13.mlsender.net"
                    },
                    to = new[]
                    {
                        new {
                            email = para
                        }
                    },
                    subject = "¡Registro Exitoso en Serve-Books!",
                    variables = new[]{
                        new {
                            email = para,
                            substitutions = new []{
                                new {
                                    var = "Usuario",
                                    value =  $"{usuario}"
                                }
                                
                            }
                        }
                    },
                    template_id = "0p7kx4xxxp849yjr"
                };

                var body = System.Text.Json.JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("Authorization", $"Bearer mlsn.db7d2ba06e95530684bc64935056ef4c9768ed64e0aee9e8f2b25f2f85907320");
                HttpResponseMessage response = _httpClient.Send(request);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al enviar el correo: {e.Message}");
            }
        }

         public void SendLoanNotification(string para, string user, string admin, string genre, string title, string autor, string loanDate, string returnDate)
        {
            Console.WriteLine("SendLoanNotification");
            try
            {
                var requestBody = new
                {
                    from = new
                    {
                        email = "MS_9vUjoq@trial-z86org8zd6ngew13.mlsender.net"
                    },
                    to = new[]
                    {
                        new {
                            email = para
                        }
                    },
                    subject = "Notificación de Préstamo de Libro",
                    variables = new[]{
                        new {
                            email = para,
                            substitutions = new []{
                                new {
                                    var = "User",
                                    value =  $"{user}"
                                },
                                new {
                                    var = "Admin",
                                    value =  $"{admin}"
                                },
                                new {
                                    var = "Genre",
                                    value =  $"{genre}"
                                },
                                new {
                                    var = "Title",
                                    value =  $"{title}"
                                },
                                new {
                                    var = "Author",
                                    value =  $"{autor}"
                                },
                                new {
                                    var = "LoanDate",
                                    value =  $"{loanDate}"
                                },
                                new {
                                    var = "ReturnDate",
                                    value =  $"{returnDate}"
                                }
                            }
                        }
                    },
                    template_id = "your-template-id-for-loan-notification"
                };

                var body = System.Text.Json.JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("Authorization", $"Bearer mlsn.db7d2ba06e95530684bc64935056ef4c9768ed64e0aee9e8f2b25f2f85907320");
                HttpResponseMessage response = _httpClient.Send(request);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al enviar el correo: {e.Message}");
            }
        }
    }
}
