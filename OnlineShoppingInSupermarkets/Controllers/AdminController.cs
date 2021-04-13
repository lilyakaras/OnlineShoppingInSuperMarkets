using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingInSupermarkets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ShoppingContext _context;
        public AdminController(ShoppingContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var users = _context.Users.Include(x => x.Role).ToList();
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role.Name,
                    Type = user.Type
                });
            }
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            var result = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Type = user.Type,
                RoleId = user.RoleId,
                Roles = await _context.Roles.ToListAsync()
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Type,RoleId")] EditUserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.Id == id);
                    user.RoleId = userViewModel.RoleId;
                    user.Type = userViewModel.Type;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userViewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}
