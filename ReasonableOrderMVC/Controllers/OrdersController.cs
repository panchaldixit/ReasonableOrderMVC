using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReasonableOrderMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static ReasonableOrderMVC.Models.Order;
using System.IO;

namespace ReasonableOrderMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Order Order { get; set; }

        public Sale Sale { get; set; }

        public Product Product { get; set; }

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            Task.WaitAll(Task.Delay(1000));
            System.Threading.Thread.Sleep(1000);
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            @ViewBag.productList = _db.Products.ToList();
            Order = new Order();
            if (id == null)
            {
                //create
                return View(Order);
            }
            //update
            Order = _db.Orders.FirstOrDefault(u => u.Id == id);
            if (Order == null)
                return NotFound();

            return View(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Order.Id == 0)
                {
                    var existingOrder = _db.Orders.Where(order => order.OrderID == 0 && order.Name == Order.Name).ToList();

                    if (existingOrder.Count > 0 || existingOrder != null)
                        _db.Orders.Add(Order);
                    else
                        @ViewBag.ExistingOrderMessage = "Item already added. Please update order!";
                }
                else
                {
                    _db.Orders.Update(Order);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Order);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Orders.Where(x => x.OrderID == 0).ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var orderFromDb = await _db.Orders.FirstOrDefaultAsync(u => u.Id == id);
            if (orderFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Orders.Remove(orderFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public IActionResult GetPrice(string itemName)
        {
            try
            {
                var productPrice = _db.Products.Where(x => x.Name == itemName)?.FirstOrDefault()?.Price;
                return Json(new { success = true, price = productPrice });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = $"Something went wrong - {ex.Message}" });
            }
        }


        [HttpGet]
        public IActionResult SubmitOrder(string orderValue)
        {
            try
            {
                var dep = _db.Orders.Where(order => order.OrderID == 0).ToList();
                Sale sale = new Sale();
                sale.Total_Order_Value = orderValue;
                _db.Sales.Add(sale);
                _db.SaveChanges();
                dep.ForEach(c => c.OrderID = int.Parse(sale.Id.ToString()));
                _db.SaveChanges();
                return Json(new { success = true, message = int.Parse(sale.Id.ToString()) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Something went wrong - {ex.Message}" });
            }
        }
        #endregion
    }
}
