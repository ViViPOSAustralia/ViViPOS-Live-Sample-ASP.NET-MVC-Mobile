using System;
using System.Linq;
using System.Web.Mvc;
using MvcMobile.Models;
using MvcMobile.Messaging;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client;

namespace MvcMobile.Controllers {
    public class HomeController : Controller {

        //Wait Handle
        AutoResetEvent waitHandle = new AutoResetEvent(false);

        public ViewResult Index() {
            ViewBag.NumSessions = Sessions.All.Count;
            return View();
        }

        public ViewResult AllProducts(string TableNo, string Target)
        {
            Product[] products = null;

            var connection = new HubConnection(ConfigurationManager.AppSettings["viviposliveURL"],
                "app_id=" + ConfigurationManager.AppSettings["app_id"] +
                "&app_key=" + ConfigurationManager.AppSettings["app_key"] +
                "&cid=" + ConfigurationManager.AppSettings["cid"] +
                "&s=" + ConfigurationManager.AppSettings["s"]);

            var proxy = connection.CreateHubProxy("LiveHub");   //Create Proxy for ViViPOS Live Hub

            bool connected = false;
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    connected = true;
                    Console.WriteLine("Connected");
                }
            }).Wait();

            if (connected)
            {

                //Setup onError handler
                proxy.On<string>("onError", (errorMessage) =>
                {
                    ViewBag.ErrorMessage = errorMessage;    //Set error Message to display if there is an error
                    waitHandle.Set();
                });


                //Set up call back handler when product json is received from ViViPOS terminal
                proxy.On<Response>("onProductsReceived", (response) =>
                {
                    products = JsonConvert.DeserializeObject<Product[]>(response.data.ToString());
                    waitHandle.Set();
                });


                var request = new Request()
                {
                    //Replace Target with the MacAddress of the targetted ViViPOS 
                    meta = new Meta() { target = Target },
                    data = null
                };

                //Try to get products from ViViPOS terminal, pass in request object
                proxy.Invoke<string>("GetProducts", request).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}", task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine(task.Result);
                    }
                });


                waitHandle.WaitOne();   //Wait until we receive messages from the ViViPOS

                return View(products);  //return results to the View
            }
            else
            {
                //If there is a connection error, display error
                ViewBag.ErrorMessage = "Error Connecting to ViViPOS Live";
                return View(products);
            }
        }

        public ViewResult AllMachines()
        {
            Machine[] machines = null;

            var connection = new HubConnection(ConfigurationManager.AppSettings["viviposliveURL"],
                "app_id=" + ConfigurationManager.AppSettings["app_id"] +
                "&app_key=" + ConfigurationManager.AppSettings["app_key"] +
                "&cid=" + ConfigurationManager.AppSettings["cid"] +
                "&s=" + ConfigurationManager.AppSettings["s"]);

            var proxy = connection.CreateHubProxy("LiveHub");   //Create Proxy for ViViPOS Live Hub

            bool connected = false;
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    connected = true;
                    Console.WriteLine("Connected");
                }
            }).Wait();

            if (connected)
            {

                //Setup onError handler
                proxy.On<string>("onError", (errorMessage) =>
                {
                    ViewBag.ErrorMessage = errorMessage;    //Set error Message to display if there is an error
                    waitHandle.Set();
                });


                //Set up call back handler when product json is received from ViViPOS terminal
                proxy.On<Response>("onMachinesReceived", (response) =>
                {
                    machines = JsonConvert.DeserializeObject<Machine[]>(response.data.ToString());
                    waitHandle.Set();
                });


                var request = new Request()
                {
                    //Replace Target with the MacAddress of the targetted ViViPOS 
                    meta = new Meta(),
                    data = null
                };


                //Try to get products from ViViPOS terminal, pass in request object
                proxy.Invoke<string>("GetMachines", request).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}", task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine(task.Result);
                    }
                });


                waitHandle.WaitOne();   //Wait until we receive messages from the ViViPOS

                return View(machines);  //return results to the View
            }
            else
            {
                //If there is a connection error, display error
                ViewBag.ErrorMessage = "Error Connecting to ViViPOS Live";
                return View(machines);
            }
        }

        public ViewResult AllTables(Machine machine)
        {
            Table[] tables = null;

            var connection = new HubConnection(ConfigurationManager.AppSettings["viviposliveURL"],
                "app_id=" + ConfigurationManager.AppSettings["app_id"] +
                "&app_key=" + ConfigurationManager.AppSettings["app_key"] +
                "&cid=" + ConfigurationManager.AppSettings["cid"] +
                "&s=" + ConfigurationManager.AppSettings["s"]);

            var proxy = connection.CreateHubProxy("LiveHub");   //Create Proxy for ViViPOS Live Hub

            bool connected = false;
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    connected = true;
                    Console.WriteLine("Connected");
                }
            }).Wait();

            if (connected)
            {

                //Setup onError handler
                proxy.On<string>("onError", (errorMessage) =>
                {
                    ViewBag.ErrorMessage = errorMessage;    //Set error Message to display if there is an error
                    waitHandle.Set();
                });


                //Set up call back handler when product json is received from ViViPOS terminal
                proxy.On<Response>("onTablesReceived", (response) =>
                {
                    tables = JsonConvert.DeserializeObject<Table[]>(response.data.ToString());
                    waitHandle.Set();
                });


                var request = new Request()
                {
                    //Replace Target with the MacAddress of the targetted ViViPOS 
                    meta = new Meta() { target = machine.MacAddress },
                    data = null
                };


                //Try to get products from ViViPOS terminal, pass in request object
                proxy.Invoke<string>("GetTables", request).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}", task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine(task.Result);
                    }
                });


                waitHandle.WaitOne();   //Wait until we receive messages from the ViViPOS
                ViewBag.Target = machine.MacAddress;
                return View(tables);  //return results to the View
            }
            else
            {
                //If there is a connection error, display error
                ViewBag.ErrorMessage = "Error Connecting to ViViPOS Live";
                return View(tables);
            }
        }

        public ViewResult AllSpeakers() {
            var allSpeakers = Sessions.All.SelectMany(x => x.Speakers).Distinct().OrderBy(x => x);
            return View(allSpeakers);
        }

        public ViewResult AllTags() {
            var allTags = Sessions.All.SelectMany(x => x.Tags).Distinct().OrderBy(x => x);
            return View(allTags);
        }

        public ViewResult AllDates() {
            var allDates = Sessions.All.Select(x => x.StartDate).Distinct().OrderBy(x => x);
            return View(allDates);
        }

        public ViewResult SessionsBySpeaker(string speaker) {
            ViewBag.Title = "Sessions by " + speaker;
            var sessions = Sessions.All.Where(session => session.Speakers.Contains(speaker)).OrderBy(x => x.StartDate);
            return View("SessionsTable", sessions);
        }

        public ViewResult SessionsByTag(string tag) {
            ViewBag.Title = "Sessions tagged " + tag;
            var sessions = Sessions.All.Where(session => session.Tags.Contains(tag)).OrderBy(x => x.Title);
            return View("SessionsTable", sessions);
        }

        public ViewResult SessionsByDate(DateTime date) {
            ViewBag.Title = "Sessions on at " + date.ToString("ddd, MMM dd, h:mm tt");
            var sessions = Sessions.All.Where(session => session.StartDate == date).OrderBy(x => x.Title);
            return View("SessionsTable", sessions);
        }

        public ActionResult SessionByCode(string code) {
            if (String.IsNullOrEmpty(code)) {
                return RedirectToAction("AllTags");
            }

            var session = Sessions.All.Single(x => x.Code == code);
            return View(session);
        }
    }
}