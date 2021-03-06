﻿using FreshMart.Data;
using FreshMart.Models;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public class ProductService
    {
        private CartService cartService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ProductService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            cartService = new CartService(httpContextAccessor, _context);
        }


        //Products
        public List<Product> GetAllProducts()
        {
            var products = _context.Products.Include(c => c.Category).Include(c => c.District).Include(p => p.Seller).OrderByDescending(c => c.CreatedAt).ToList();
            return products;
        }
        public List<Product> GetProductByCategoryID(int id)
        {
            var pro = _context.Products
                .Include(c => c.Category)
                .Include(c => c.District)
                .Where(c => c.CategoryId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return pro;
        }


        public ProductViewModel ProductVMWithCartCount(int id)
        {

            var products = GetProductByCategoryID(id);

            var all = GetAllProducts();

            var categories = GetAllCategories();
            var districts = GetAllDistricts();
            var domains = GetCategoryByDomain();

            var cartCount = cartService.GetCartCount();
            var productView = new ProductViewModel
            {
                Products = products,
                Category = categories,
                District = districts,
                DistinctCat = domains,
                BaseProduct = all,  //all variable is created because //it will always remain same as it is inherited
                CartCount = cartCount
            };

            return productView;
        }


        public List<District> GetAllDistricts()
        {
            var districts = _context.Districts.ToList();
            return districts;
        }


        //Category
        public List<Category> GetAllCategories()
        {
            var category = _context.Categories.ToList();
            return category;
        }
        public List<string> GetCategoryByDomain()
        {
            var domains = _context.Categories.Select(c => c.Domain).Distinct().ToList();
            return domains;
        }
        public string GetCategoryByDomainID(int id)
        {
            var domainName = _context.Categories.Where(c => c.Id == id).Select(c => c.Domain).First();
            return domainName;
        }




    }
}
