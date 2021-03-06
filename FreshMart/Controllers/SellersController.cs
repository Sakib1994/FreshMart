﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Data;
using FreshMart.Models;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FreshMart.Helper;
using Microsoft.AspNetCore.Hosting;

namespace FreshMart.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public SellersController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        // GET: Sellers
        [Route("Sellers/{id}")]
        public IActionResult Index(int id)
        {

            var vm = new SellerVM
            {
                Seller = _context.Sellers.Find(id),
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList()
            };

            return View(vm);
        }



        // GET: Sellers/Details/5
        [Route("Sellers/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }





        [HttpPost]
        [Route("Sellers/Update/{id}")]
        public IActionResult Update(int id, SellerVM sellerVm)
        {
            if (id != sellerVm.Seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Sellers.Update(sellerVm.Seller);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(sellerVm.Seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", sellerVm.Seller.DistrictId);

            return View(sellerVm.Seller);
        }



        // GET: Sellers/Create
        [Route("Sellers/Create")]
        public IActionResult Create()
        {
            var vm = new SellerVM
            {
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList()
            };


            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [Route("Sellers/RequestForSell")]
        public ActionResult RequestForSell(SellerVM sellerVm)
        {

            var emailCheck = _context.SellerRequests.Where(c => c.Email == User.Identity.Name);
            if (emailCheck.ToList().Count > 0)
            {

                ViewBag.err = "You have already requested. Please have patience to be approved !";
                var vm = new SellerVM
                {
                    Districts = _context.Districts.ToList(),
                    Sellers = _context.Sellers.ToList(),
                    Error = ViewBag.err
                };


                return View("Create", vm);
            }


            if (sellerVm.SellerRequest.SellerName == null || sellerVm.SellerRequest.DistrictId == null ||
                sellerVm.SellerRequest.DateOfBirth == null)
            {


                var vm = new SellerVM
                {
                    Districts = _context.Districts.ToList(),
                    Sellers = _context.Sellers.ToList(),
                    Error = "You can not ignore required fields"
                };


                return View("Create", vm);
            }
            else
            {
                var vm = new SellerRequest
                {
                    SellerName = sellerVm.SellerRequest.SellerName,
                    Email = User.Identity.Name,
                    Phone = sellerVm.SellerRequest.Phone,
                    DistrictId = sellerVm.SellerRequest.DistrictId,
                    DateOfBirth = sellerVm.SellerRequest.DateOfBirth,
                    CompanyName = sellerVm.SellerRequest.CompanyName
                };
                _context.SellerRequests.Add(vm);
                _context.SaveChanges();
            }
            var vms = new SellerVM
            {
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList(),
                Error = "Your request has been sent! Wait for approval."
            };

            return RedirectToAction("Create", "Sellers", vms);
        }



        [Route("Seller/SellProduct/{id}")]
        public ActionResult SellProduct(int? id)
        {
            var data = _context.Sellers.Where(c => c.Email == User.Identity.Name);

            if (id == null || data.SingleOrDefault() == null)
            {
                NotFound();
            }
            var vm = new SellerVM
            {
                Products = _context.Products.Where(c => c.SellerId == id).ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList(),
                Sellers = _context.Sellers.ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [Route("Seller/SellProduct/{id?}")]
        public ActionResult SellProduct(int id, IFormFile file, SellerVM vm)
        {
            var data = _context.Sellers.Where(c => c.Email == User.Identity.Name);

            if (id == null || data.SingleOrDefault() == null)
            {
                NotFound();
            }

            if (vm.Product.Title == null ||
                vm.Product.CategoryId == null ||
                vm.Product.DistrictId == null ||
                vm.Product.Price == null ||
                vm.Product.ItemInStock == null ||
                vm.Product.Unit == null)

            {
                ViewBag.entryerr = "Please don't ignore required fields";

                return View(vm);
            }


            var Seller = _context.Sellers.Where(s => s.Email.Contains(User.Identity.Name));
            if (Seller.SingleOrDefault() == null)
            {
                return RedirectToAction("RequestForSell", "Products");
            }


            ImgUploader img = new ImgUploader(_environment);

            var imgPath = img.ImageUrl(file);   //function working here


            var products = new Product
            {
                Title = vm.Product.Title,
                Description = vm.Product.Description,
                Price = vm.Product.Price,
                SellerId = id,
                CategoryId = vm.Product.CategoryId,
                DistrictId = vm.Product.DistrictId,
                IsPublished = vm.Product.IsPublished,  //manually values
                Unit = vm.Product.Unit,
                ItemInStock = vm.Product.ItemInStock,
                CreatedAt = DateTime.Now,              //manually valuess
                UpdatedAt = DateTime.Now,              //manually valuess
                OfferExpireDate = DateTime.Now,        //manually valuess
                ImagePath = imgPath,
                OfferPrice = 1     //need to change
            };

            _context.Products.Add(products);
            _context.SaveChanges();

            var vms = new SellerVM
            {
                Products = _context.Products.Where(c => c.SellerId == id).ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList(),
                Sellers = _context.Sellers.ToList(),
                Error = "Your Product has been added"
            };
            return View(vms);
        }


        [Route("Sellers/SellerProducts/{id}")]
        public ActionResult SellerProducts(int id)
        {
            var prosList = _context.Products.Where(c => c.SellerId == id).ToList();
            var vm = new SellerVM
            {
                Products = prosList,
                Sellers = _context.Sellers.ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList()
            };

            return View(vm);
        }




        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }




        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
