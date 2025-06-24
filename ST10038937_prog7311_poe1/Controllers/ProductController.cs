using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ST10038937_prog7311_poe1.Data;
using ST10038937_prog7311_poe1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10038937_prog7311_poe1.Services;

namespace ST10038937_prog7311_poe1.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly Services.ProductNotifier _productNotifier;
        private readonly IAuditService _auditService;

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, Services.ProductNotifier productNotifier, IAuditService auditService)
        {
            _context = context;
            _userManager = userManager;
            _productNotifier = productNotifier;
            _auditService = auditService;
        }

        // GET: Product
        public async Task<IActionResult> Index(int? farmerId, string category, DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Create query
            var productsQuery = _context.Products.Include(p => p.Farmer).AsQueryable();
            
            // Filter by farmer if specified
            if (farmerId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.FarmerId == farmerId);
            }
            // For farmers, only show their own products
            else if (User.IsInRole("Farmer"))
            {
                if (user == null)
                {
                    return Forbid();
                }
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                if (farmer == null)
                {
                    return NotFound("Farmer profile not found");
                }
                
                productsQuery = productsQuery.Where(p => p.FarmerId == farmer.FarmerId);
            }
            
            // Apply category filter if specified
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }
            
            // Apply date range filter if specified
            if (startDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= endDate.Value);
            }
            
            // Get available categories for filter dropdown
            ViewBag.Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync();
            
            // For employees, get farmers for filter dropdown
            if (User.IsInRole("Employee"))
            {
                ViewBag.Farmers = new SelectList(await _context.Farmers.ToListAsync(), "FarmerId", "Name");
            }
            
            // Set filter values for the view
            ViewBag.SelectedFarmerId = farmerId;
            ViewBag.SelectedCategory = category;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            return View(await productsQuery.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
                
            if (product == null)
            {
                return NotFound();
            }
            
            // Check if the user is authorized to view this product
            if (User.IsInRole("Farmer"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Forbid();
                }
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                
                if (farmer == null || product.FarmerId != farmer.FarmerId)
                {
                    return Forbid();
                }
            }

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            
            if (farmer == null)
            {
                return NotFound("Farmer profile not found");
            }
            
            ViewBag.FarmerId = farmer.FarmerId;
            ViewBag.FarmerName = farmer.Name;
            
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create([Bind("Name,Category,ProductionDate,Price,Description,QuantityAvailable,FarmerId")] Product product)
        {
            // Verify the farmer is adding a product to their own profile
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Forbid();
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                // Audit log
                await _auditService.LogActionAsync(user.Id, "Create Product", $"Product ID: {product.ProductId}, Name: {product.Name}");
                _productNotifier.NotifyProductCreated(product);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.FarmerId = farmer.FarmerId;
            ViewBag.FarmerName = farmer.Name;
            
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int? id)
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
            
            // Verify the farmer is editing their own product
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Forbid();
            }
            
            ViewBag.FarmerId = farmer.FarmerId;
            ViewBag.FarmerName = farmer.Name;
            
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,ProductionDate,Price,Description,QuantityAvailable,FarmerId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            
            // Verify the farmer is editing their own product
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                                        _context.Update(product);
                    await _context.SaveChangesAsync();
                    await _auditService.LogActionAsync(user.Id, "Edit Product", $"Product ID: {product.ProductId}, Name: {product.Name}");
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
            
            ViewBag.FarmerId = farmer.FarmerId;
            ViewBag.FarmerName = farmer.Name;
            
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
                
            if (product == null)
            {
                return NotFound();
            }
            
            // Verify the farmer is deleting their own product
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
            
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Forbid();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product != null)
            {
                // Verify the farmer is deleting their own product
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Forbid();
                }
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                
                if (farmer == null || product.FarmerId != farmer.FarmerId)
                {
                    return Forbid();
                }
                
                                var productName = product.Name;
                var productId = product.ProductId;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                await _auditService.LogActionAsync(user.Id, "Delete Product", $"Product ID: {productId}, Name: {productName}");
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
