using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Firebase.Auth;

namespace Agri_Energy_Connect.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly Poe2Context _context;

        public UserAccountController(Poe2Context context)
        {
            _context = context;
        }

        // This method returns all of the users
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                var poe2Context = _context.UserAccounts.Include(u => u.UserRoleNavigation);


                return View(await poe2Context.ToListAsync());
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        //This method returns all users that are pending approval to be farmers.
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public async Task<IActionResult> PendingUsers()
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                var poe2Context = _context.UserAccounts.Where(u => u.UserRole.Equals("Requested")).Include(u => u.UserRoleNavigation);


            return View(await poe2Context.ToListAsync());
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        // This method returns a view that provides details about a specific user, including the products they sell.
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public async Task<IActionResult> Details(string id)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.UserRoleNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        //This method returns a view that allows the user to create a new admin user.
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole");
                return View();
            }

            else
            {
                return RedirectToAction("NoAccess", "Category");
            }
        }

        //This method saves data to the database
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,FullName,UserRole")] UserAccount userAccount)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(userAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole", userAccount.UserRole);
                return View(userAccount);
            }
        
      else { return RedirectToAction("NoAccess", "Category");
    }
}

        // This method provides a view for the admin to edit a user, here the admin ocan change the role of a user.
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public async Task<IActionResult> Edit(string id)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }
            ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole", userAccount.UserRole);
            return View(userAccount);
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        // This method saves the data to the database. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,Email,FullName,UserRole")] UserAccount userAccount)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                if (id != userAccount.UserId)
            {
                return NotFound();
            }
            var Role = userAccount.UserRole;
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccount);
                    await _context.SaveChangesAsync();             
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountExists(userAccount.UserId))
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
            ViewData["UserRole"] = new SelectList(_context.Roles, "UserRole", "UserRole", userAccount.UserRole);
            return View(userAccount);
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        // THis method provides a view for the admin user to delete a user 
        //Only if the user is an admin then they can access this view, otherwise they're redirected to the no access view.
        public async Task<IActionResult> Delete(string id)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole.Equals("Admin"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.UserRoleNavigation)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
            }
            else { return RedirectToAction("NoAccess", "Category"); }
        }

        // POST: UserAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //This method deletes the user

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount != null)
            {
                _context.UserAccounts.Remove(userAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(string id)
        {
            return _context.UserAccounts.Any(e => e.UserId == id);
        }
    }
}
