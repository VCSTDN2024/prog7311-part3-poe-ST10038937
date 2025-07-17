using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using ST10038937_prog7311_poe1.Data;
using ST10038937_prog7311_poe1.Models;

namespace ST10038937_prog7311_poe1.Controllers;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Extensions.Localization;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<HomeController> _localizer;

    public HomeController(
        ILogger<HomeController> logger, 
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        IStringLocalizer<HomeController> localizer) // Injected
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<IActionResult> Index()
    {
        // If user is authenticated, show role-specific dashboard
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (User.IsInRole("Farmer"))
            {
                // Get farmer profile
                var farmer = user != null ? await _context.Farmers
                    .FirstOrDefaultAsync(f => f.UserId == user.Id) : null;
                
                if (farmer != null)
                {
                    // Get farmer's products
                    var products = await _context.Products
                        .Where(p => p.FarmerId == farmer.FarmerId)
                        .ToListAsync();
                    
                    ViewBag.Farmer = farmer;
                    ViewBag.ProductCount = products.Count;
                    ViewBag.Categories = products
                        .Select(p => p.Category)
                        .Distinct()
                        .ToList();
                    
                    return View("FarmerDashboard");
                }
            }
            else if (User.IsInRole("Employee"))
            {
                // Get counts for employee dashboard
                ViewBag.FarmerCount = await _context.Farmers.CountAsync();
                ViewBag.ProductCount = await _context.Products.CountAsync();
                ViewBag.Categories = await _context.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();
                
                // Get recent products
                ViewBag.RecentProducts = await _context.Products
                    .Include(p => p.Farmer)
                    .OrderByDescending(p => p.ProductionDate)
                    .Take(5)
                    .ToListAsync();
                
                return View("EmployeeDashboard");
            }
        }
        
        // Default landing page for unauthenticated users
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult About()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
        );

        // For AJAX calls, we don't want to redirect.
        // We can check if the request is an AJAX request.
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Ok();
        }

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public IActionResult GetTranslations([FromQuery] string[] keys)
    {
        if (keys == null || !keys.Any())
        {
            return BadRequest("No keys provided.");
        }

        var translations = keys.ToDictionary(key => key, key => _localizer[key].Value);
        return new JsonResult(translations);
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
