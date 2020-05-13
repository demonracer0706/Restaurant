using Restaurant.Models.EF;
using System.Linq;
using System.Web.Mvc;
using Restaurant.Common;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        private RestaurantDbContext entity = new RestaurantDbContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string userName, string passWord)
        {
            var user = entity.Accounts.SingleOrDefault(u => u.Username == userName);
            if (user == null)
            {
                return Content("<script language='javascript' type='text/javascript'>"
                   + "alert('No Username Is Found!');"
                   + "location.href = '/Account/Login';"
                   + "</script>");
            }
            else if (user.Password == Encryptor.MD5Hash(passWord))
            {
                Session["USERNAME"] = user.Username;
                Session["ROLE"] = user.ID_Role;
                if (user.ID_Role.Equals("CT"))
                {
                    return RedirectToAction("Index", "Counter");
                }
                else if (user.ID_Role.Equals("AD"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.ID_Role.Equals("US"))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>"
                   + "alert('Invalid Username Or Password!');"
                   + "location.href = '/Account/Login';"
                   + "</script>");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>"
                   + "alert('Login Error!');"
                   + "location.href = '/Account/Login';"
                   + "</script>");
            }
            return View();
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session["USERNAME"] = null;
            Session["ROLE"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}