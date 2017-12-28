using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace ASPNET_MVC_Application.Models
{
    public class Client
    {
        public static string ClientFile = HttpContext.Current.Server.MapPath("~/App_Data/Clients.json");

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Trusted { get; set; }

        public static List<Client> GetClients()
        {
          List<Client>  clients = new List<Client>();
            if (File.Exists(ClientFile))
            {
                string content = File.ReadAllText(ClientFile);

                clients = JsonConvert.DeserializeObject<List<Client>>(content);

                return clients;
            }
            else
            {
                File.Create(ClientFile).Close();
                File.WriteAllText(ClientFile,"[]");
                GetClients();
            }

            return clients;
        }
    }
}