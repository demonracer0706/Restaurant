using Restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Restaurant.Models;
using Rotativa.MVC;
using Rotativa.Core;
using Rotativa.Core.Options;

namespace Restaurant.Controllers
{
    public class CounterController : Controller
    {
        private RestaurantDbContext entity = new RestaurantDbContext();
        // GET: Counter
        public ActionResult Index()
        {
            CounterModel model = new CounterModel();
            model.TableList = entity.Tables.ToList();
            return View("Index", model);
        }

        public void payment(int tableId)
        {
            CounterModel model = new CounterModel();
            var currentTable = entity.Tables.SingleOrDefault(table => table.Number == tableId);
            var currentInvoice
                = entity.Invoices.SingleOrDefault(invoice => invoice.Number == tableId && !invoice.Status);
            if (currentInvoice == null)
            {
                model.TableList = entity.Tables.ToList();
                RedirectToAction("Index", model);
            }
            if (currentInvoice != null)
            {
                currentTable.Status = false;
                var invoiceDetail = entity.InvoiceDetails.Where(i => i.ID_Invoice == currentInvoice.ID_Invoice).ToList();
                foreach (var element in invoiceDetail)
                {
                    var item = element;
                    entity.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                entity.Entry(currentInvoice).State = System.Data.Entity.EntityState.Deleted;
                entity.SaveChanges();
            }
        }

        public ActionResult PrintBill(int tableId)
        {
            CounterModel model = new CounterModel();
            model.Invoice = entity.Invoices.SingleOrDefault(i => i.Number == tableId && !i.Status);
            model.TableId = tableId;
            var currentTable = entity.Tables.SingleOrDefault(table => table.Number == tableId);
            if (currentTable != null && currentTable.Status)
            {
                var currentInvoice = entity.Invoices.SingleOrDefault(invoice => invoice.Number == tableId && !invoice.Status);
                if (currentInvoice != null)
                {
                    var list = entity.InvoiceDetails.Where(i => i.ID_Invoice == currentInvoice.ID_Invoice).ToList();
                    List<OrderDetailModel> listOrder = new List<OrderDetailModel>();
                    foreach (var element in list)
                    {
                        var currentMenu
                            = entity.Products.SingleOrDefault(menu => menu.ID_Product == element.ID_Product);
                        var orderDetail = new OrderDetailModel()
                        {
                            ID_HOADON = element.ID_Invoice,
                            ID_THUCPHAM = element.ID_Product,
                            SOLUONG = element.Quantity,
                            TONGTIEN = element.TotalPrice,
                            URL = currentMenu.Image,
                            TEN_THUCPHAM = currentMenu.ProductName
                        };
                        listOrder.Add(orderDetail);
                    }
                    model.OrderList = listOrder;
                }
            }
            payment(tableId);
            var report = new PartialViewAsPdf("~/Views/Shared/_PrintBill.cshtml", model)
            {
                RotativaOptions = new DriverOptions()
                {
                    PageSize = Size.A6,
                }
            };
            return report;
        }

        public ActionResult Detail(string tableId)
        {
            int id = Int32.Parse(tableId);
            CounterModel model = new CounterModel();
            model.TableId = id;
            model.TableActiveList = entity.Tables.Where(x => x.Status == false && x.Number != id).ToList();
            model.MenuList = entity.Products.Where(i => i.ID_Product != null).ToList();
            var currentTable = entity.Tables.SingleOrDefault(table => table.Number == id);
            if (currentTable != null)
            {
                var currentInvoice = entity.Invoices.SingleOrDefault(invoice => invoice.Number == id);
                if (currentInvoice != null)
                {
                    var list = entity.InvoiceDetails.Where(i => i.ID_Invoice == currentInvoice.ID_Invoice).ToList();
                    List<OrderDetailModel> listOrder = new List<OrderDetailModel>();
                    foreach (var element in list)
                    {
                        var currentMenu
                            = entity.Products.SingleOrDefault(menu => menu.ID_Product == element.ID_Product);
                        var orderDetail = new OrderDetailModel()
                        {
                            ID_HOADON = element.ID_Invoice,
                            ID_THUCPHAM = element.ID_Product,
                            SOLUONG = element.Quantity,
                            TONGTIEN = element.TotalPrice,
                            URL = currentMenu.Image,
                            TEN_THUCPHAM = currentMenu.ProductName,
                            DONGIA = currentMenu.Price
                        };
                        listOrder.Add(orderDetail);
                    }
                    model.OrderList = listOrder;
                }
            }
            return View("Detail", model);
        }

        [HttpPost]
        public ActionResult SwitchTable(int oldTable, int newTable)
        {
            var currentInvoice = entity.Invoices.SingleOrDefault(invoice => invoice.Number == oldTable && !invoice.Status);
            var oldTableRecord = entity.Tables.SingleOrDefault(t => t.Number == oldTable);
            var newTableRecord = entity.Tables.SingleOrDefault(t => t.Number == newTable);
            currentInvoice.Number = newTable;
            oldTableRecord.Status = false;
            newTableRecord.Status = true;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddItem(string tableId, string itemId, string quantity)
        {
            int id = Int32.Parse(tableId);
            int quan = Int32.Parse(quantity);
            var unit = entity.Products.SingleOrDefault(p => p.ID_Product == itemId);
            long unitPrice = unit.Price;
            long totalPrice = unitPrice * quan; ;
            var currentTable = entity.Tables.SingleOrDefault(t => t.Number == id);
            var currentInvoice = entity.Invoices.SingleOrDefault(invoice => invoice.Number == currentTable.Number);
            if (currentInvoice != null)
            {
                var invoiceDetail = entity.InvoiceDetails.SingleOrDefault(x => x.ID_Invoice == currentInvoice.ID_Invoice && x.ID_Product == itemId);
                if (invoiceDetail == null)
                {
                    var newInvoiceDatail = new InvoiceDetail()
                    {
                        ID_Product = itemId,
                        ID_Invoice = currentInvoice.ID_Invoice,
                        Quantity = quan,
                        TotalPrice = totalPrice,
                    };
                    currentInvoice.Payable = currentInvoice.Payable + totalPrice;
                    entity.InvoiceDetails.Add(newInvoiceDatail);
                }
                if (invoiceDetail != null)
                {
                    totalPrice = unitPrice * quan;
                    invoiceDetail.Quantity = invoiceDetail.Quantity + quan;
                    invoiceDetail.TotalPrice = invoiceDetail.TotalPrice + totalPrice;
                    currentInvoice.Payable = invoiceDetail.TotalPrice;
                }
            }
            if (currentInvoice == null)
            {
                string lastIndex = getLastinvoiceId();
                Invoice newInvoice = new Invoice()
                {
                    ID_Invoice = lastIndex,
                    Number = id,
                    Date = DateTime.Now,
                    Payable = totalPrice,
                    Status = false
                };
                
                entity.Invoices.Add(newInvoice);
                InvoiceDetail newDetail = new InvoiceDetail()
                {
                    ID_Product = itemId,
                    ID_Invoice = lastIndex,
                    Quantity = quan,
                    TotalPrice = totalPrice
                };
                entity.InvoiceDetails.Add(newDetail);
            }
            currentTable.Status = true;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteItem(string menuId, string tableId)
        {
            int id = Int32.Parse(tableId);
            var currentTable = entity.Tables.SingleOrDefault(t => t.Number == id);
            if (!menuId.Equals(""))
            {
                var currentInvoice = entity.Invoices.SingleOrDefault(invoice => invoice.Number == id);
                var invoiceItem = entity.InvoiceDetails.FirstOrDefault(x => x.ID_Invoice == currentInvoice.ID_Invoice && x.ID_Product == menuId);
                if (invoiceItem != null)
                {
                    long itemPrice = invoiceItem.TotalPrice;
                    currentInvoice.Payable = currentInvoice.Payable - itemPrice;
                    entity.Entry(invoiceItem).State = System.Data.Entity.EntityState.Deleted;
                }
                if (currentInvoice.Payable == 0)
                {
                    currentTable.Status = false;
                    entity.Entry(currentInvoice).State = System.Data.Entity.EntityState.Deleted;
                }
            }
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public string getLastinvoiceId()
        {
            int lastIndex = entity.Invoices.ToList().Count;
            return "" + (lastIndex + 1);
        }
    }
}