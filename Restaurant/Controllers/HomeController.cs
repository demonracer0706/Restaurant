using System;
using System.Linq;
using System.Web.Mvc;
using Restaurant.Models.EF;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private RestaurantDbContext entity = new RestaurantDbContext();
        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.DrinkList = entity.Products.Where(drinks => drinks.ID_Type == "TU").ToList();
            model.FoodList = entity.Products.Where(foods => foods.ID_Type == "MA").ToList();
            model.TableList = entity.Tables.Where(table => table.Status == false).ToList();
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult BookATable(string name, string email, string tableId, string date)
        {
            // update table
            if (name != null && email != null && tableId != null && date != null)
            {
                OrderTable item = new OrderTable()
                {
                    FullName = name,
                    Email = email,
                    Number = Int32.Parse(tableId),
                    Date = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                };
                entity.OrderTables.Add(item);
                entity.SaveChanges();
                // send mail
            }
            return View();
        }
    }
}