using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Models;
using Firebase.Auth;

namespace Agri_Energy_Connect.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Poe2Context _context;

        public CategoryController(Poe2Context context)
        {
            _context = context;
        }

        //This method returns all the products in a specific category
        public async Task<IActionResult> CategoryProducts(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                //This linq query selects all the products that belong to that category.
                var products =  _context.Products.Where(p => p.Category.CategoryId == id)
                    .Include(p => p.Category).Include(p =>p.User);
                if (products == null)
                {
                    return NotFound();
                }
                return View(products);
            }
            else
            {
                return View("NoAccess");
            }
        }

        //This method returns all categories
        public async Task<IActionResult> Index()
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                var categories = await _context.Categories.ToListAsync();

                return View(categories);
            }
            else
            {
                return View("NoAccess");
            }
        }

        //This meethod returns the view with the details of a specific category.
        public async Task<IActionResult> Details(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            if (userID != null)
            {
                if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
            }
            else
            {
                return View("NoAccess");
            }
        }

        // This method returns the view that allows a user to create a category
        public IActionResult Create()
        {//if the user is an admin then they can create  a category

            //If the user is not an admind then they will be redireccted to the no acess view.
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                return View();
            }
            else
            {
                return View("NoAccess");
            }
        }

        // This method creates the category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryImg")] Category category)
        {//If the user is an admin then the category will be saved
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            else
            {//If the user is not an admin then the user will be redirected to the no access view 
                return View("NoAccess");
            }
        }

        // This method allows admins to edit categories.
        public async Task<IActionResult> Edit(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            else
            {//If the user is not an admin then the user cannot access the edit view and will be redirected to no access.
                return View("NoAccess");
            }
        }

        //This method saves the edited category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                if (id != category.CategoryId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(category);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.CategoryId))
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
                return View(category);
            }
            else
            {//If the user is not an admin then they will be redirected to the no access view.
                return View("NoAccess");
            }
        }

        // This method deletes a category.
        //If the user is an admin then the user can delete the admin 
        //If the user is not an admin then they cannot delete the category.
        public async Task<IActionResult> Delete(int? id)
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(m => m.CategoryId == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            else
            {
                return View("NoAccess");
            }
        }

        // This method actaully deletes the category from the database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userID = HttpContext.Session.GetString("userId");
            var userRole = HttpContext.Session.GetString("userRole");
            if (userID != null && userRole.Equals("Admin"))
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("NoAccess");

            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
