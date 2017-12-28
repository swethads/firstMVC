using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

using ASPNET_MVC_Application.Models;
using Newtonsoft.Json;

namespace ASPNET_MVC_Application.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            var clients = Client.GetClients();
            return View(clients);
        }

        public ActionResult create()
        {
            ViewBag.Submitted = false;
            var created = false;

            if (HttpContext.Request.RequestType=="POST")
            {
                var id = Request.Form["id"];
                var name = Request.Form["name"];
                var address = Request.Form["address"];
                var trusted = false;

                if (Request.Form["trusted"] == "on")
                {
                    trusted = true;
                }

                Client client = new Client()
                {
                    ID = Convert.ToInt16(id),
                    Name = name,
                    Address = address,
                    Trusted = Convert.ToBoolean(trusted)
                };

                var ClientFile = Client.ClientFile;
                var ClientData = System.IO.File.ReadAllText(ClientFile);

                List<Client> ClientList = new List<Client>();
                ClientList = JsonConvert.DeserializeObject<List<Client>>(ClientData);

                if(ClientList==null)
                {
                    ClientList = new List<Client>();
                }
                ClientList.Add(client);

                System.IO.File.WriteAllText(ClientFile,JsonConvert.SerializeObject(ClientList));

                created = true;
            }

            if(created)
            {
                ViewBag.Message = "Client Successfully Created";
            }
            else
            {
                ViewBag.Message = "Error Creating Client";
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                var name = Request.Form["name"];
                var address = Request.Form["address"];
                var trusted = false;
                if (Request.Form["trusted"] == "on")
                    trusted = true;
                

                var clints = Client.GetClients();

                foreach (Client client in clints)
                {
                    if (client.ID == id)
                    {
                        client.Name = name;
                        client.Address = address;
                        client.Trusted = Convert.ToBoolean(trusted);
                        break;
                    }
                }

                System.IO.File.WriteAllText(Client.ClientFile, JsonConvert.SerializeObject(clints));
                Response.Redirect("~/Client/Index?Message=Client_Updated");
            }

            var clnt = new Client();
            var clients = Client.GetClients();

            foreach(Client client in clients)
            {
                if(client.ID==id)
                {
                    clnt = client;
                    break;
                }
            }

            if (clnt == null)
            {
                ViewBag.Message = "No Client Was Found";
            }
            return View(clnt);
        }

        public ActionResult delete(int id)
        {
            var client = Client.GetClients();
            var deleted = false;
            // adding a comment

            foreach(Client clients in client)
            {
                if(clients.ID==id)
                {
                    var index = client.IndexOf(clients);
                    client.RemoveAt(index);

                    System.IO.File.WriteAllText(Client.ClientFile,JsonConvert.SerializeObject(client));
                    deleted = true;
                    break;
                }
            }

            //if(deleted)
            //{
            //    ViewBag.Message = "Client Deleted Successfully";
            //}
            //else
            //{
            //    ViewBag.Message = "Error Deleting Client";
            //}
            Response.Redirect("~/Client/Index?Message=Client_Deleted");
            return View();
        }
    }
}