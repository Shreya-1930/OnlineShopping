using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class OnlineshopController : Controller
        
    {//home,login,register,viewproduct,buy,feedback,search,error

        OnlineshopdbContext dc = new OnlineshopdbContext();
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login( string txt1 ,string txt2)
        {
            var res = (from t in dc.Registers
                      where t.Uname == txt1 && t.Password == txt2
                      select t).Count();
            if (res > 0)
            {
                HttpContext.Session.SetString("uid", txt1);
                return RedirectToAction("Viewproduct");
            }

                ViewData["msg"] = "invalid credentials...please try again!!!";
            
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register r)
        {
            if (ModelState.IsValid)
            {
                dc.Registers.Add(r);
                int i = dc.SaveChanges();
                if (i > 0)
                {
                    ViewData["msg"] = "New User Created Successfully!!";
                }
                else
                {
                    ViewData["msg"] = " couldnt create try again!!";
                }
            }
            return View();
        }
        public IActionResult Viewproduct()
        {
            var res = (from t in dc.Products
                      select t).ToList();
           
            return View(res);
        }
        public IActionResult Buy(Product P)
        {
            if(HttpContext.Session.GetString("uid")==null)
            {
                return RedirectToAction("Login");
            }
            var res = (from t in dc.Products
                       where t.Pid == P.Pid
                       select t).ToList();
            return View(res);
            
        }
        [HttpPost]
        public IActionResult Buy( string qyt)
        {
            Userorder ob = new Userorder();
            ob.Pid = Request.Query["pid"];
            ob.Username= HttpContext.Session.GetString("uid");
            ob.Transdate = DateOnly.FromDateTime(DateTime.Now);
            ob.Qty = int.Parse(qyt);
            Feedbacktbl ob1 = new Feedbacktbl();
            ob1.Username= HttpContext.Session.GetString("uid");
            ob1.Pid= Request.Query["pid"];
            ob1.Fstatus = false;

            dc.Userorders.Add(ob);
            dc.Feedbacktbls.Add(ob1);
            int i = dc.SaveChanges();
            if (i > 0)
            {
                ViewData["msg"] = "Added Successfully!!";
            }
            else
            {
                ViewData["msg"] = "  try again!!";
            }
            return View();
            
        }
        public IActionResult Feedback()
        {
            Feedbacktbl ob1 = new Feedbacktbl();


            //if (HttpContext.Session.GetString("uid") == null)
            //{
            //    return RedirectToAction("Login");
            //}
            var items = new List<SelectListItem>();
            var res = dc.Feedbacktbls.Where(t=>t.Username== HttpContext.Session.GetString("uid") && t.Fstatus==false).Select(t => new { t.Pid }).ToList();
           
            foreach (var item in res)
            {
                items.Add(new SelectListItem { Text= item.Pid, Value = item.Pid });
            }

            ViewBag.DropDownData = items;

            return View();

        }
        [HttpPost]
        public IActionResult Feedback(Feedbacktbl f)
        {
            string st  = HttpContext.Session.GetString("uid");
            string pid = f.Pid;

            var res = (from t in dc.Feedbacktbls
                       where t.Username == st && t.Pid == pid
                       && t.Fstatus == false
                       select t).First();

            res.Fstatus = true;
            res.Usermessage = f.Usermessage;
            res.Ratings = f.Ratings;


            // var res = dc.Feedbacktbls.Where(t => t.Username == HttpContext.Session.GetString("uid") && t.Pid==f.Pid && t.Fstatus == false).Select(t => new { t.Pid }).First();
            dc.Feedbacktbls.Update(res);

            int i = dc.SaveChanges();
            if (i > 0)
            {
                ViewData["msg2"] = ("Thanks for submitting the  feedback  ");
            }
            else
            {
                ViewData["msg2"] = (" something error occured ");
            }
            return View();
        }

        public IActionResult Search(string txtsearch)
        {
            var res1= (from t in dc.Products
                     where t.Pname.Contains(txtsearch)
                     select t).Count();
            var res2 = (from t in dc.Products
                       where t.Pname.Contains(txtsearch)
                       select t).ToList();
            if (res1 > 0)
            {
                ViewData["msg2"] =( $"no of items are {res1} " );
            }
            else
            {
                ViewData["msg2"] = ("no items matched ") ;
            }
            return View(res2);
        }

        public IActionResult Error()
        {
            return View();
        }
       
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("uid");
            return View();
        }

        //public PartialViewResult Testpartial()
        //{
        //    return PartialView();
        //}
        //[HttpPost]
        //public PartialViewResult Testpartial(Feedbacktbl f)
        //{
        //    f.Username = HttpContext.Session.GetString("uid");
        //    dc.Feedbacktbls.Add(f);
        //    int i = dc.SaveChanges();
        //    if (i > 0)
        //    {
        //        ViewData["msg2"] = ("Thanks for submitting the  feedback  ");
        //    }
        //    else
        //    {
        //        ViewData["msg2"] = (" something error occured ");
        //    }
        //    return PartialView();
        //}
        [CustomExceptionFilter]
        public IActionResult testing()
        {
            throw new DivideByZeroException();
            ViewBag.testing = DateTime.Now;
            return View();
        }
    }
}
