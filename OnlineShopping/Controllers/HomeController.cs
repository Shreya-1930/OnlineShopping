using Microsoft.AspNetCore.Mvc;


namespace OnlineShopping.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            string[] countrynames = { "india", "canada", "us", "uk" };
            
                ViewBag.c2 = (countrynames);
            ViewData["c1"] = (countrynames);

            TempData["a"] = "hello";
            TempData.Keep("a");

            return View();
        }
        
        public ViewResult Proclink()
        {
            return View();
        }
        [ActionName("viewdate")]
        public ViewResult ShowDate()

        {
            return View("ShowDate");
        }
        //[ActionName("ind")]
        [NonAction]
        public string india()
        {
            return "welcome to india page";
        }

        public ViewResult Addnums()
        {
            return View();

        }
        [HttpPost]
        public ViewResult Addnums(string txt1,string txt2)
        {
            int res = int.Parse(txt1) + int.Parse(txt2);
            ViewData["v"]=res;
            return View();

        }
        public ViewResult repeatval()
        {
            return View();

        }
        [HttpPost]
        public ViewResult repeatval(string txt1, string txt2)
        {
            
            ViewData["a"] = txt1;
            ViewData["v"] = txt2;

            return View();

        }
    }
}
