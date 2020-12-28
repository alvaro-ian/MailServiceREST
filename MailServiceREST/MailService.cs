using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace MailServiceREST
{
    public class MailService
    {
        public string url = "http://localhost:8080/"; 
        MensagemController msgControl;

        public void ServiceStart()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Seu sistema nao suporta a classe HttpListener e seus recursos...");
                return;
            }

            msgControl = new MensagemController();

            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add(url + "mail/"); // Adicionando url para escuta

                // Iniciando Servidor
                listener.Start();
                Console.WriteLine("Escutando na porta 8080...");
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest req = context.Request;

                    string resposta = "";

                    // Analisando metodo HTTP para resposta
                    switch (req.HttpMethod)
                    {
                        case "POST":
                            PostMessage(req);
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            break;
                        case "GET":
                            resposta = GetMessage(req);
                            break;
                        case "DELETE":
                            DeleteMessage(req);
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            break;
                    }

                    if (resposta != "")
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(resposta);

                        context.Response.ContentLength64 = buffer.Length;
                        using (System.IO.Stream output = context.Response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                    Console.WriteLine(context.Response.StatusCode + " " + context.Response.StatusDescription);
                    context.Response.Close();
                }
            }
        }

        // POST
        // Corpo da request eh um json
        public void PostMessage(HttpListenerRequest request)
        {
            Mensagem m;
            string json;

            // Extraindo corpo da REQUEST
            using (System.IO.Stream input = request.InputStream)
            {
                System.IO.StreamReader body = new System.IO.StreamReader(input, request.ContentEncoding);
                json = body.ReadToEnd();
                body.Close();
            }

            Console.WriteLine(json);
            // Convertendo de JSON para objeto Mensagem
            m = JsonConvert.DeserializeObject<Mensagem>(json);
            Console.WriteLine(m.ToString());
            // Inserindo mensagem no db
            msgControl.Insert(m);
        }

        // GET
        public string GetMessage(HttpListenerRequest request)
        {
            string resposta = "";
            List<Mensagem> mail = new List<Mensagem>();

            Console.WriteLine(request.QueryString.Count);

            if (request.QueryString.Count == 0) // Caso nao haja parametros na url, retornar todas as mensagens
            {
                mail = msgControl.GetAll();
                resposta = JsonConvert.SerializeObject(mail);
            }
            else if (request.QueryString.GetKey(0) == "id") // Caso o parametro seja um id, retornar aquela mensagem
            {
                mail.Add(msgControl.Get(int.Parse(request.QueryString.Get("id"))));
                resposta = JsonConvert.SerializeObject(mail);
            }
            else if (request.QueryString.GetKey(0) == "username")   // Caso o parametro seja um nome de usuario, retornar todas as suas mensagens
            {
                mail = msgControl.GetUser(request.QueryString.Get("username"));
                resposta = JsonConvert.SerializeObject(mail);
            }

            Console.WriteLine(resposta);
            return resposta;
        }

        // DELETE
        // Recebe como parametro da url um id de mensagem
        public void DeleteMessage(HttpListenerRequest request)
        {
            int id = int.Parse(request.QueryString.Get("id"));

            msgControl.Delete(msgControl.Get(id));
        }
    }
}
