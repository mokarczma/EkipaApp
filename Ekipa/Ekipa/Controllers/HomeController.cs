﻿using Ekipa.Models;
using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
            }
            return View();
        }
        [HttpGet]
        public ActionResult MainView()
        {
            MainViewVM model = new MainViewVM();
            var user = User as MPrincipal;
            model.LogUser = false;
            if (user != null)
            {
                model.LogUser = true;
                model.UserName = user.UserDetails.Login;

            }
            return View();
        }
    }
}