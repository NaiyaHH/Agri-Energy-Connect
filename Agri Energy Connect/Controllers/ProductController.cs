using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Models;
using Microsoft.AspNetCore.Authorization;

namespace Agri_Energy_Connect.Controllers
{

    public class ProductController : Controller
    {
        private readonly Poe2Context _context;

        public ProductController(Poe2Context context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(DateOnly StartDate, DateOnly EndDate, int Category, string searchString)
        {//This sessions variable and if statement ensures that the user is logged in to view products.
            var userID = HttpContext.Session.GetString("userId");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            if (userID != null)
            {
                //In this linq query, all the products are selected.
                var products = from p in _context.Products
                               select p;

                //In this linq query, the data from foreign keys are included in the list of information will be displayed to the user
                products = products
                    .Include(p => p.User)
                    .Include(p => p.Category).Where(p => !p.UserId.Equals(userID)); ;

                //THe code in this if statement checks for the category only, no start date, no end date, no farmer or product.
                if (StartDate == DateOnly.MinValue && EndDate == DateOnly.MinValue && string.IsNullOrEmpty(searchString) && Category != 0)
                {
                    products = products.Where(p =>
                         p.CategoryId == (Category)
                     );
                }

                //The code in this if statement checks for a search string only, no start or end date. 
                if (StartDate == DateOnly.MinValue && EndDate == DateOnly.MinValue && Category == 0 && !string.IsNullOrEmpty(searchString))
                {

                    products = products.Where(p =>
                        p.ProductName.Contains(searchString) ||
                        p.User.FullName.Contains(searchString)
                    );

                }

                //The code in this if statement checks for the start and end date only.
                if (StartDate != DateOnly.MinValue && EndDate != DateOnly.MinValue && Category == 0 && string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p =>
                                p.ProdDate >= StartDate && p.ProdDate <= EndDate);
                }

                //The code in this if statement checks for the start and end date and a productName/UserName
                if (StartDate != DateOnly.MinValue && EndDate != DateOnly.MinValue && Category != 0 && !string.IsNullOrEmpty(searchString))
                {

                    products = products.Where(p =>

                        p.ProductName.Contains(searchString) ||
                        p.User.FullName.Contains(searchString) &&
                        p.ProdDate >= StartDate && p.ProdDate <= EndDate &&
                        p.CategoryId == (Category)
                    );

                }

                //The code in this if statement checks for the productname/userName and a category
                if (StartDate == DateOnly.MinValue && EndDate == DateOnly.MinValue && Category != 0 && !string.IsNullOrEmpty(searchString))
                {

                    products = products.Where(p =>

                        p.ProductName.Contains(searchString) ||
                        p.User.FullName.Contains(searchString) &&
                        p.CategoryId == (Category)
                    );

                }

                //The code in this if statement checks for the start and end date and a category
                if (StartDate != DateOnly.MinValue && EndDate != DateOnly.MinValue && Category != 0 && string.IsNullOrEmpty(searchString))
                {

                    products = products.Where(p =>
                        p.CategoryId == (Category) &&
                        p.ProdDate >= StartDate && p.ProdDate <= EndDate
                    );

                }
                //The view with the selected information is returned. 
                return View(await products.ToListAsync());
            }
            else { return View("NoAccess"); }

        }

        //This method is returns a view that shows the 
        //the current users' products in it.
        public async Task<IActionResult> MyProduct()
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Farmer"))
            {

                //This selects the current user's email address. 
                var user = HttpContext.Session.GetString("theUser");

                //This linq query selects all the products that the current user has selected. 
                var products = from p in _context.Products
                               where p.User.Email == user
                               select p;

                //This linq query attaches the category of the product to the list of the information that will be displayed to the user.
                products = products
                    .Include(p => p.Category);

                //The view with the selected information is returned. 
                return View(await products.ToListAsync());
            }
            else { return View("NoAccess"); }
        }

        //This method returns the view to allow a user to message another farmer. 
        public async Task<IActionResult> ContactFarmer(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }
                //This linq query selects the selected product's information.
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            //if the user is not logged in then they will be redirected to the no access view.
            else { return View("NoAccess"); }
        }


        //This method returns a view thanking the user for sending a message to a specific farmer regarding a product.
        public async Task<IActionResult> MessageSent()
        {
            return View();
        }


        // This method  gets the details of a specific product.
        public async Task<IActionResult> Details(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            //If the user is not logged in, then they will be redirected to the no access view. 
            else { return View("NoAccess"); }
        }

        // This method returns the view to create a product
        public IActionResult Create()
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            //if the user is logged in and the user role is farmer then the user can access the view and create the product.
            if (userID != null && userRole.Equals("Farmer"))
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                return View();
            }
            //If the user is not a farmer then they will be redirected to the no access view 
            else { return View("NoAccess"); }
        }

        // This method saves the product to the database. .
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProdDate, ProductImg, CategoryId,UserId")] Product product)
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Farmer"))
            {
                product.UserId = userID;
                if (!ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
            else { return View("NoAccess"); }
        }

        //This method allows logged in users to edit products.
        public async Task<IActionResult> Edit(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
            else { return View("NoAccess"); }
        }

        // This method saves the edited product to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProdDate,productImg, CategoryId,UserId")] Product product)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                product.UserId = userID;
                if (id != product.ProductId)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.ProductId))
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
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
            else { return View("NoAccess"); }
        }

        //This method provides the view that deletes a product, and ensures that a user is logged in; in order to do so. 
        public async Task<IActionResult> Delete(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);

            }
            else
            {
                return View("NoAccess");
            }
        }

        // The product is deleted from the database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else { return View("NoAccess"); }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
