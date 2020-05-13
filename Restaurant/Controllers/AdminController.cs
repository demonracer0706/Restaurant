using Restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Common;
using System.Web.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class AdminController : Controller
    {
        private RestaurantDbContext entity = new RestaurantDbContext();
        // GET: Admin
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            AdminModel model = new AdminModel();
            model.RevenueList = new List<long>();
            DateTime currentDate = DateTime.Today;
            // RevenueList
            for (var i = 9; i < 13; i++)
            {
                var listInvoice
                    = entity.Invoices.Where(
                        invoice => invoice.Date.Month == i
                        && invoice.Date.Year == currentDate.Year
                        && invoice.Status).ToList();
                long total = 0;
                if (listInvoice != null && listInvoice.Count > 0)
                {
                    foreach (var item in listInvoice)
                    {
                        total += item.Payable;
                    }
                }
                model.RevenueList.Add(total);
            }
            return View("Index", model);
        }

        // GET: RevenueList and display to Index
        [HttpPost]
        public ActionResult Index(string start, string end)
        {
            AdminModel model = new AdminModel();
            JsonResult result = new JsonResult();
            DateTime currentDate = DateTime.Today;
            if (start.Equals("") || start == null)
            {
                start = currentDate.Date.ToString("dd/MM/yyyy").Replace("-", "/");
            }
            if (end.Equals("") || end == null)
            {
                end = currentDate.Date.ToString("dd/MM/yyyy").Replace("-", "/");
            }
            // RevenueByMenuItemList
            var startDate
                = DateTime.ParseExact(start, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(end, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime fullEndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            var totalEstimate = (from orderDetail in entity.InvoiceDetails
                                 join invoice in entity.Invoices on orderDetail.ID_Invoice equals invoice.ID_Invoice
                                 where DateTime.Compare(invoice.Date, startDate) >= 0
                                 && DateTime.Compare(invoice.Date, fullEndDate) <= 0
                                 && invoice.Status
                                 group orderDetail by orderDetail.ID_Product into g
                                 select new RevenueByMenuItem()
                                 {
                                     ID_THUCPHAM = g.Key,
                                     SOLUONG = g.Sum(orderDetail => orderDetail.Quantity),
                                     DOANHTHU = g.Sum(orderDetail => orderDetail.TotalPrice),
                                 }).ToList();
            var revenueByMenuItemList
                = (from menu in entity.Products
                   join detail in entity.InvoiceDetails on menu.ID_Product equals detail.ID_Product
                   join type in entity.ProductTypes on menu.ID_Type equals type.TypeName
                   join invoice in entity.Invoices on detail.ID_Invoice equals invoice.ID_Invoice
                   where DateTime.Compare(invoice.Date, startDate) >= 0
                   && DateTime.Compare(invoice.Date, fullEndDate) <= 0
                   && invoice.Status
                   select new RevenueByMenuItem()
                   {
                       ID_THUCPHAM = menu.ID_Product,
                       LOAI = type.TypeName,
                       TEN_THUCPHAM = menu.ProductName,
                       DONGIA = menu.Price,
                   }).Distinct().ToList();
            int index = 1;
            foreach (var item in revenueByMenuItemList)
            {
                item.SOLUONG = totalEstimate.FirstOrDefault(i => i.ID_THUCPHAM == item.ID_THUCPHAM).SOLUONG;
                item.DOANHTHU = totalEstimate.FirstOrDefault(i => i.ID_THUCPHAM == item.ID_THUCPHAM).DOANHTHU;
                item.ID = index;
                index++;
            }
            model.RevenueByMenuItemList = revenueByMenuItemList;
            if (model.RevenueByMenuItemList != null && model.RevenueByMenuItemList.Count > 0)
            {
                result = Json(new
                {
                    recordsTotal = model.RevenueByMenuItemList.Count,
                    recordsFiltered = model.RevenueByMenuItemList.Count,
                    data = model.RevenueByMenuItemList
                }, JsonRequestBehavior.AllowGet);
                return result;
            }
            return Json(new
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                data = model.RevenueByMenuItemList
            }, JsonRequestBehavior.AllowGet); ;
        }
        #endregion

        #region UserManagement
        [HttpGet]
        public ActionResult UserManagement()
        {
            AdminModel model = new AdminModel();
            var userList = entity.Accounts.ToList();
            var roleList = entity.Roles.Where(role => role.ID_Role != "").ToList();
            model.UserList = userList;
            model.RoleList = roleList;
            return View("UserManagement", model);
        }

        [HttpPost]
        public ActionResult UserManagement(string fullname, string roleId, string username, string password)
        {
            // init data
            Account user = new Account()
            {
                Username = username,
                Password = Encryptor.MD5Hash(password),
                ID_Role = roleId,
                FullName = fullname,
            };

            var currentUser = entity.Accounts.SingleOrDefault(u => u.Username == user.Username);
            if (currentUser == null)
            {
                // create
                entity.Accounts.Add(user);
            }
            else
            {
                // update user
                currentUser.Username = user.Username;
                currentUser.Password = user.Password;
                currentUser.ID_Role = user.ID_Role;
                currentUser.FullName = user.FullName;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var userList = entity.Accounts.ToList();
                var roleList = entity.Roles.Where(role => role.ID_Role != "AD").ToList();
                model.UserList = userList;
                model.RoleList = roleList;
                return View("UserManagement", model);
            }
            return null;
        }

        public ActionResult DeleteUser(string username)
        {
            if (!username.Equals(""))
            {
                var currentUser = entity.Accounts.SingleOrDefault(u => u.Username == username);
                entity.Entry(currentUser).State = System.Data.Entity.EntityState.Deleted;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var userList = entity.Accounts.ToList();
                var roleList = entity.Roles.Where(role => role.ID_Role != "AD").ToList();
                model.UserList = userList;
                model.RoleList = roleList;
                return View("UserManagement", model);
            }
            return null;
        }
        #endregion

        #region TableManagement
        [HttpGet]
        public ActionResult TableManagement()
        {
            AdminModel model = new AdminModel();
            var tableList = entity.Tables.ToList();
            var zoneList = entity.Zones.ToList();
            model.TableList = tableList;
            model.ZoneList = zoneList;
            return View("TableManagement", model);
        }

        [HttpPost]
        public ActionResult TableManagement(string tableId, string tableName, string zoneId)
        {
            if (tableId.Equals(""))
            {
                // create
                Table table = new Table()
                {
                    Number = getLastTableId(),
                    TableName = tableName,
                    ID_Zone = zoneId,
                    Status = false
                };
                entity.Tables.Add(table);
            }
            else
            {
                int id = Int32.Parse(tableId);
                // update
                var currentTable = entity.Tables.SingleOrDefault(t => t.Number == id);
                currentTable.ID_Zone = zoneId;
                currentTable.TableName = tableName;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var tableList = entity.Tables.ToList();
                var zoneList = entity.Zones.ToList();
                model.TableList = tableList;
                model.ZoneList = zoneList;
                return View("TableManagement", model);
            }
            return null;
        }

        public ActionResult DeleteTable(string tableId)
        {
            if (!tableId.Equals(""))
            {
                int id = Int32.Parse(tableId);
                var item = entity.Tables
                    .Where(t => t.Number == id)
                    .FirstOrDefault();
                entity.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var tableList = entity.Tables.ToList();
                var zoneList = entity.Zones.ToList();
                model.TableList = tableList;
                model.ZoneList = zoneList;
                return View("TableManagement", model);
            }
            return null;
        }

        public ActionResult ChangeStatus(string tableId)
        {
            int id = Int32.Parse(tableId);
            var currentTable = entity.Tables.SingleOrDefault(t => t.Number == id);
            if (currentTable.Status == true)
            {
                currentTable.Status = false;
            }
            else
            {
                if (currentTable.Status == false)
                {
                    currentTable.Status = true;
                }
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var tableList = entity.Tables.ToList();
                var zoneList = entity.Zones.ToList();
                model.TableList = tableList;
                model.ZoneList = zoneList;
                return RedirectToAction("TableManagement","Admin");
            }
            return null;
        }
        #endregion

        #region ListOrder
        public ActionResult ListOrder()
        {
            AdminModel model = new AdminModel();
            var bookTableList = entity.OrderTables.ToList();
            model.BookTableList = bookTableList;
            return View("ListOrder", model);
        }
        #endregion

        #region MenuManagement
        public ActionResult MenuManagement()
        {
            AdminModel model = new AdminModel();
            var menuList = entity.Products.Join(
                entity.ProductTypes,
                menu => menu.ID_Type,
                type => type.ID_Type,
                (menu, type) => new MenuItem()
                {
                    ID_THUCPHAM = menu.ID_Product,
                    ID_LOAI = menu.ID_Type,
                    LOAI = type.TypeName,
                    TEN_THUCPHAM = menu.ProductName,
                    DONGIA = menu.Price,
                    HINH_ANH = menu.Image,
                }).ToList();
            var menuTypeList = entity.ProductTypes.ToList();
            model.MenuList = menuList;
            model.MenuTypeList = menuTypeList;
            return View("MenuManagement", model);
        }

        [HttpPost]
        public ActionResult MenuManagement(string menuId, string menuName, string typeId, string unitPrice, string fileUrl)
        {
            long price = long.Parse(unitPrice);
            var currentMenu = entity.Products.SingleOrDefault(m => m.ID_Product == menuId);
            if (currentMenu == null)
            {
                // create
                Product newMenu = new Product()
                {
                    ID_Product = getLastMenuId(),
                    ProductName = menuName,
                    ID_Type = typeId,
                    Price = price,
                    Image = fileUrl,
                };
                entity.Products.Add(newMenu);
            }
            else
            {
                // update
                if (fileUrl != "")
                {
                    currentMenu.Image = fileUrl;
                }
                currentMenu.ProductName = menuName;
                currentMenu.ID_Type = typeId;
                currentMenu.Price = price;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var menuList = entity.Products.Join(
                    entity.ProductTypes,
                    menu => menu.ID_Type,
                    type => type.ID_Type,
                    (menu, type) => new MenuItem()
                    {
                        ID_THUCPHAM = menu.ID_Product,
                        LOAI = type.TypeName,
                        ID_LOAI = menu.ID_Type,
                        TEN_THUCPHAM = menu.ProductName,
                        DONGIA = menu.Price,
                        HINH_ANH = menu.Image,
                    }).ToList();
                var menuTypeList = entity.ProductTypes.ToList();
                model.MenuList = menuList;
                model.MenuTypeList = menuTypeList;
                return View("MenuManagement", model);
            }
            return null;
        }
        public ActionResult DeleteMenu(string menuId)
        {
            if (!menuId.Equals(""))
            {
                var currentMenu = entity.Products.SingleOrDefault(m => m.ID_Product == menuId);
                entity.Entry(currentMenu).State = System.Data.Entity.EntityState.Deleted;
            }
            if (entity.SaveChanges() > 0)
            {
                AdminModel model = new AdminModel();
                var menuList = entity.Products.Join(
                    entity.ProductTypes,
                    menu => menu.ID_Type,
                    type => type.ID_Type,
                    (menu, type) => new MenuItem()
                    {
                        ID_THUCPHAM = menu.ID_Product,
                        LOAI = type.TypeName,
                        ID_LOAI = menu.ID_Type,
                        TEN_THUCPHAM = menu.ID_Product,
                        DONGIA = menu.Price,
                        HINH_ANH = menu.Image,
                    }).ToList();
                var menuTypeList = entity.ProductTypes.ToList();
                model.MenuList = menuList;
                model.MenuTypeList = menuTypeList;
                return View("MenuManagement", model);
            }
            return null;
        }
        #endregion

        #region Helper
        public int getLastTableId()
        {
            int lastIndex = entity.Tables.ToList().Count;
            return lastIndex + 1;
        }

        public string getLastMenuId()
        {
            int lastIndex = entity.Products.ToList().Count;
            if (lastIndex >= 10)
            {
                return "T" + (lastIndex + 1);
            }
            else
            {
                return "T0" + (lastIndex + 1);
            }
        }
        #endregion
    }
}