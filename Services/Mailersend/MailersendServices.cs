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

        //Correo de registro    
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

        //Correo de notificacion de prestamo para admin  
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
                    template_id = "zr6ke4nmm0mgon12"
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

        //Correo solicitud de presatamo para user 
        public void BookLoan(string para, string user, string genre, string title, string autor, string loanDate, string returnDate)
        {
            Console.WriteLine("BookLoan");
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
                    subject = "Solicitud de Préstamo de Libro",
                    variables = new[]{
                        new {
                            email = para,
                            substitutions = new []{
                                new {
                                    var = "User",
                                    value =  $"{user}"
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
                    template_id = "3yxj6ljww81gdo2r"
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

        //Correo para recordar la fecha de vencimiento del préstamo
        public void LoanAboutToMature(string para, string user, string genre, string title, string autor, string loanDate, string returnDate)
        {
            Console.WriteLine("LoanAboutToMature");
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
                    subject = "Préstamo de libro a punto de vencer",
                    html = $@"
                        <html>
                        <body>
                            <p>Estimado/a {user},</p>
                            <p>Le informamos que se ha realizado un nuevo préstamo de libro. A continuación, se detallan los datos:</p>
                            <p>Detalles del Préstamo:</p>
                            <ul>
                                <li>Título del Libro: {title}</li>
                                <li>Genero: {genre}</li>
                                <li>Autor: {autor}</li>
                                <li>Fecha del Préstamo: {loanDate}</li>
                                <li>Fecha de Devolución: {returnDate}</li>
                            </ul>
                            <p>Por favor, asegúrese de devolver el libro antes de la fecha de vencimiento para evitar multas. Si necesita más tiempo, puede renovar su préstamo a través de nuestro sistema en línea, sujeto a disponibilidad.</p>
                            <p>Gracias por su atención y cooperación.</p>
                            <p>Saludos cordiales,<br/>Serve-Books</p>
                        </body>
                        </html>"
                };

                var body = System.Text.Json.JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("Authorization", "Bearer mlsn.db7d2ba06e95530684bc64935056ef4c9768ed64e0aee9e8f2b25f2f85907320");
                HttpResponseMessage response = _httpClient.Send(request);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al enviar el correo: {e.Message}");
            }       
        }
        
        //Correo de autorización préstamo
        public void AuthorizedLoan(string para, string user, string genre, string title, string autor, string loanDate, string returnDate)
        {
            Console.WriteLine("AuthorizedLoan");
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
                    subject = "Autorización de Préstamo de Libro",
                    html = $@"
                        <html>
                        <body>
                            <p>Estimado/a {user},</p>
                            <p>Nos complace informarle que su solicitud de préstamo de libro ha sido aprobada. A continuación, se detallan los datos del préstamo:</p>
                            <p>Detalles del Préstamo:</p>
                            <ul>
                                <li>Título del Libro: {title}</li>
                                <li>Género: {genre}</li>
                                <li>Autor: {autor}</li>
                                <li>Fecha del Préstamo: {loanDate}</li>
                                <li>Fecha de Devolución: {returnDate}</li>
                            </ul>
                            <p>Por favor, asegúrese de recoger el libro en la biblioteca antes de la fecha de vencimiento.</p>
                            <p>Gracias por utilizar Serve-Books.</p>
                        </body>
                        </html>"
                };

                var body = System.Text.Json.JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("Authorization", "Bearer mlsn.db7d2ba06e95530684bc64935056ef4c9768ed64e0aee9e8f2b25f2f85907320");
                HttpResponseMessage response = _httpClient.Send(request);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al enviar el correo: {e.Message}");
            }       
        }
    }
}
