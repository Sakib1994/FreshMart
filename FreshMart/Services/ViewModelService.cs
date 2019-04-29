using FreshMart.Data;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public class ViewModelService
    {

        private readonly ApplicationDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;


        public ViewModelService(ApplicationDbContext con, IHttpContextAccessor hca)
        {
            _context = con;
            _httpContextAccessor = hca;
        }

        public ProductViewModel GetProductViewModel()
        {

            var categories = _context.Categories.ToList();
            var districts = _context.Districts.ToList();
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();

            CartService cs = new CartService(_httpContextAccessor, _context);
            var totalPrice = cs.GetCartTotalPrice();
            var cartCount = cs.GetCartCount();
            var viewmodel = new ProductViewModel
            {
                District = districts,
                Category = categories,
                DistinctCat = domains,
                CartCount = cartCount,
                TotalPrice = totalPrice,
                Sellers = _context.Sellers.ToList()

            };


            return viewmodel;

        }



    }
}
