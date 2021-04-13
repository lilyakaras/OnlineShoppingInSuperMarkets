using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingInSupermarkets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShoppingInSupermarkets.Controllers
{
    [Authorize(Roles = "admin, buyer")]
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;
        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "admin, buyer")]
        public async Task<IActionResult> Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role == "admin")
            {
                var allOrders = _context.Orders.Include(o => o.Customer).Include(o => o.SuperMarket);
                return View(await allOrders.ToListAsync());
            }
            var userId = Int32.Parse(User.FindFirst(x => x.Type == "userId").Value);
            var userOrders = _context.Orders.Where(x => x.Customer.UserId == userId).Include(o => o.Customer).Include(o => o.SuperMarket);
            return View(await userOrders.ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "admin, buyer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "LastName");
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "Name", order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "Name", order.SuperMarketId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "ID", order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        [Authorize(Policy = "VIPBuyer")]
        public IActionResult Discount(int id)
        {
            return View(_context.Customers.FirstOrDefault(x => x.UserId == id));
        }
    }
}
