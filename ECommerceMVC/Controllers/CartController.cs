﻿using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Helpers;

namespace ECommerceMVC.Controllers
{
    public class CartController : Controller
    {
        public readonly Hshop2023Context db;
        public CartController(Hshop2023Context context) 
        {
            db = context;
        }
        const string CART_KEY = "MYCART";
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1) 
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=>p.MaHh ==id);
            if (item == null) 
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p=>p.MaHh ==id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Can't found the product with id {id}";
                    return Redirect("/404");
				}
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity,
                };
                gioHang.Add(item);
			}

            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=>p.MaHh==id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
    }
}
