using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetEtsyApp.Models;
using dotnetEtsyApp.Data;
using dotnetEtsyApp.Middleware;

namespace dotnetEtsyApp.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly CacheData _cacheData;
    private readonly ApplicationDbContext _context;
    public HomeController(ILogger<HomeController> logger, CacheData cacheData, ApplicationDbContext context)
    {
        _logger = logger;
        _cacheData = cacheData;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        
        ViewBag.Stores = _cacheData.GetStoresWithUserUserPermission(await RequestHandler.GetCurrentUserId(HttpContext));
        return View();
    }
    [Route("EtsyAccess")]
    public IActionResult GrantAccessToEtsy(string code,string state)
    {
        
        ViewBag.tokenValid = _cacheData.GetToken(code) == null ? false : true;
        return View();
    }
    [HttpPost]
    public bool ActivateStore(int storeId)
    {
        _cacheData.ChangeActiveStore(storeId);
        return true;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
