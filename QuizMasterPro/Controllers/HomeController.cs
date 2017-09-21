using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizMasterPro.Models;
using QuizMasterPro.Data;
using QuizMasterPro.ViewModels.QuizViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;

namespace QuizMasterPro.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public HomeController(ApplicationDbContext context,

            IStringLocalizer<HomeController> localizer,
            IHtmlLocalizer<HomeController> htmlLocalizer,

            IStringLocalizerFactory factory,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;

            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;

            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            var localizer1 = factory.Create(type);
            var localizer2 = factory.Create(nameof(SharedResource), assemblyName.Name);

            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["Quiz Master Pro Dashboard"];

            var quizes = _context.Quizzes
                .OrderByDescending(x => x.Created)
                .Take(5)
                .Select(x => new DashboardQuizItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Created = x.Created
                });
            var model = new DashboardViewModel
            {
                LatestQuizzes = quizes
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }
    }


    public class SharedResource
    {
    }
}
