using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using WebApiContrib.Formatting;
using Newtonsoft.Json;

namespace ConsoleRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ListarRepositorios();
            Console.ReadKey();
        }

        static void  ListarRepositorios()
        {
            try
            {
                Console.WriteLine("Login:");
                var login = Console.ReadLine();

                Console.WriteLine("Senha:");
                var senha = Console.ReadLine();

                Console.Clear();

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient("https://api.github.com");
                client.Authenticator = new HttpBasicAuthenticator(login, senha);
                client.Authenticator.Authenticate(client, new RestRequest("/user/repos"));
                var response = client.Execute(new RestRequest("/user/repos"));

                if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                {
                    Console.WriteLine("Usuário e/ou senha inválidos");
                }

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    JsonConvert.DeserializeObject<List<Repositorio>>(response.Content).Select(x => x.name).ToList()
                    .ForEach(x => { Console.WriteLine(x); });
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                throw;

            }
           


        }
    }
}
