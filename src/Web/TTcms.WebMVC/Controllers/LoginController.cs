using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using TTcms.WebMVC.ViewModels.Pagination;
using TTcms.WebMVC.ViewModels.CatalogViewModels;
using TTcms.WebMVC.Services;

namespace TTcms.WebMVC.Controllers
{
    public class LoginController : Controller
    {
        private ICatalogService _catalogSvc;

        public LoginController(ICatalogService catalogSvc) => 
            _catalogSvc = catalogSvc;

        public  IActionResult Index()
        {
            return View();
        }
    }
}