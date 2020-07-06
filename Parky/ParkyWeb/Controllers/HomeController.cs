﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger,
            INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM llistOfParkAndTrails = new IndexVM()
            {
                NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath,HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken"))
            };
            return View(llistOfParkAndTrails);
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

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(NewUser obj)
        public ActionResult Login(User obj)
        {
            User user = new User { Password = obj.Password, Username = obj.Username };
            //User objUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            //if (objUser.Token == null)
            //{
            //    return View();
            //}

            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim(ClaimTypes.Name,objUser.Username));
            //identity.AddClaim(new Claim(ClaimTypes.Role,objUser.Role));
            //var principal = new ClaimsPrincipal(identity);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //HttpContext.Session.SetString("JWToken", objUser.Token);
            //TempData["alert"] = "Welcome " + objUser.Username;

           return Json(obj, new Newtonsoft.Json.JsonSerializerSettings());
           // return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accountRepository.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (!result)
            {
                return View();
            }
            TempData["alert"] = "Registeration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken","");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
