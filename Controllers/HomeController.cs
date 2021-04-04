using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using BridgeMonitor.Models;


namespace BridgeMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var closed = GetBoatFromApi();
            closed.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach(var boat in closed){
                if(boat.ClosingDate > now){
                   return View(boat); 
                } else {
                    continue;
                }
            }
            return View();
        }

        public IActionResult Allclosed()
        {
            var boats = GetBoatFromApi();
            var boats_before = new List<Boat>();
            var boats_after = new List<Boat>();
            var boat_result = new AllClosed(){
                BoatsBefore = boats_before,
                BoatsAfter = boats_after
            };
            boats.Sort((x, y) => DateTime.Compare(x.ClosingDate, y.ClosingDate));
            DateTime now = DateTime.Now;
            foreach(var boat in boats){
                if (boat.ClosingDate > now){
                    boats_after.Add(boat);
                } else {
                    boats_before.Add(boat);
                }
            }
            
            return View(boat_result);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

         private static List<Boat> GetBoatFromApi()
        {
            //Création un HttpClient (= outil qui va permettre d'interroger une URl via une requête HTTP)
            using (var client = new HttpClient())
            {
                //Interrogation de l'URL censée me retourner les données
                var response = client.GetAsync("https://api.alexandredubois.com/pont-chaban/api.php");
                //Récupération du corps de la réponse HTTP sous forme de chaîne de caractères
                var stringResult = response.Result.Content.ReadAsStringAsync();
                //Conversion de mon flux JSON (string) en une collection d'objets BikeStation
                //d'un flux de données vers des objets => Déserialisation
                //d'objets vers un flux de données => Sérialisation
                var result = JsonConvert.DeserializeObject<List<Boat>>(stringResult.Result);
                return result;
            }
        }
    }
}
