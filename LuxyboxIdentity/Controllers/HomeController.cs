using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuxyboxIdentity.Models;
using LuxyboxIdentity.Data;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using System.Data.Entity.Core;

namespace LuxyboxIdentity.Controllers
{
    public class HomeController : BaseController
    {


        public ActionResult Index()
        {

            //Helper.BusinessHelper.AddCategory(new Data.Category { Name = "Men" });
            var categories = dbContext.Categories.ToList();//Helper.BusinessHelper.GetCategories();
            var products = dbContext.Products.ToList();
            var cartitems = dbContext.CartItems.ToList();
            var model = new HomeModel(categories, products, cartitems);

            return View(model);
        }
        public ActionResult Products(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var products = dbContext.Products.Where(c => c.CategoryId == id).ToList();
            if (products == null)
            {
                return HttpNotFound();
            }

            var categories = dbContext.Categories.ToList();
            var cartitems = dbContext.CartItems.ToList();
            var model = new HomeModel(categories, products, cartitems);

            return View(model);
        }
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = dbContext.Products.SingleOrDefault(q => q.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //public ActionResult IncreaseQuantity(int productId) //sayfa yönlendirme yaparak arttırma
        //{
        //    var sessionId = Session["sessionId"].ToString();
        //    var currentCart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);

        //    if (HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        currentCart.MemberId = User.Identity.GetUserId();
        //    }

        //    CartItem item = currentCart.CartItems.SingleOrDefault(q=>q.ProductId == productId);
        //    item.Quantity++;
        //    dbContext.SaveChanges();
        //    return RedirectToAction("cart");
        //}

        [HttpPost]
        public JsonResult CartItemQuantityUpdate(int productId, int quantity)
        {
            var sessionId = Session["sessionId"].ToString();
            var currentCart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                currentCart.MemberId = User.Identity.GetUserId();
            }

            CartItem item = currentCart.CartItems.SingleOrDefault(q => q.ProductId == productId);
            item.Quantity = quantity;
            dbContext.SaveChanges();
            RedirectToAction("cart");
            return Json(new { result = true });
        }
        [HttpPost]

        public JsonResult CartItemDeleteUpdate(int productId)
        {
            var sessionId = Session["sessionId"].ToString();
            var currentCart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                currentCart.MemberId = User.Identity.GetUserId();
            }

            CartItem item = currentCart.CartItems.SingleOrDefault(q => q.ProductId == productId);
            dbContext.CartItems.Remove(item);
            if (item == null)
            {
                return Json(new { result = false });
            }

            dbContext.SaveChanges();
            return Json(new { result = true });

        }
        public ActionResult AddToCart(int id)
        {
            ViewBag.Message = "Ürün Sepete Eklendi";
            var sessionId = Session["sessionId"].ToString();
            var currentCart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);
            if (currentCart == null)
            {
                currentCart = new Cart { CreateDate = DateTime.Now, SessionId = Session["sessionId"].ToString() };
                dbContext.Carts.Add(currentCart);
                dbContext.SaveChanges();
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                currentCart.MemberId = User.Identity.GetUserId();
            }
            var item = currentCart.CartItems.SingleOrDefault(q => q.ProductId == id);
            if (item == null)
            {
                item = new CartItem { CartId = currentCart.Id, CreateDate = DateTime.Now, ProductId = id, Quantity = 1 };
                currentCart.CartItems.Add(item);
            }
            else {
                item.Quantity++;
            }

            dbContext.SaveChanges();
            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
        {

            string sessionId = Session["sessionId"].ToString();


            Cart cart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
        public ActionResult Delete(int? id)
        {
            string sessionId = Session["sessionId"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = dbContext.Products.SingleOrDefault(q => q.Id == id);
            Cart cart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);
            dbContext.Carts.Remove(cart);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string sessionId = Session["sessionId"].ToString();
            Cart cart = dbContext.Carts.SingleOrDefault(q => q.SessionId == sessionId);
            dbContext.Carts.Remove(cart);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Shopping()
        {
            ViewBag.Message = "Bizi tercih ettiğiniz için teşekkür ederiz.";

            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }

        [HttpPost]

        public JsonResult Contact(string name, string message, string subject, string mail)
           {
        
            dbContext.Contacts.Add(new Data.Contact { Mail = mail, Name = name, Message = message, Subject=subject });
            try
            {
                
                dbContext.SaveChanges();
                
            }
            catch(Exception ex)
            {
                return Json(new { result = false });
            }
            return Json(new { result = true });
            

        }
        //[HttpPost]

        //public JsonResult Contact(Data.Contact contact)
        //{
        //    dbContext.Contacts.Add(contact);
        //    dbContext.SaveChanges();
        //    return Json(new { result = true });


        //}

        public ActionResult CheckOrder()
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return Redirect("/account/login?returnUrl=/home/checkorder");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOrder([Bind(Include = "Id,ShipmentAdress,InvoiceAdress,CreateDate,NameSurname,InvoiceName,SessionId")] CheckOrder checkorder)
        {
            var cartitems = dbContext.CartItems.ToList();
            
            if (ModelState.IsValid)
            {

                string sessionId = Session["sessionId"].ToString();
                checkorder.SessionId = sessionId;
                checkorder.CreateDate = DateTime.Now;
                var cart = (Cart)ViewBag.CurrentCart;
                decimal? totalPrice = 0;
                foreach (var item in cart.CartItems)
                {
                    totalPrice += item.Product.Price;
                }
                checkorder.TotalPrice = totalPrice.Value;
                dbContext.CheckOrders.Add(checkorder);

                dbContext.Carts.Remove(cart);
                dbContext.SaveChanges();
                return RedirectToAction("Shopping");
            }

            return View(checkorder);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}