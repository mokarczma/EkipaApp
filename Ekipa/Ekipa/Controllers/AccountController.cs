using Ekipa.Models;
using Ekipa.Models.ViewModel;
using Ekipa.Models.ViewModel.Company;
using Ekipa.Models.ViewModel.Customer;
using Ekipa.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ekipa.Controllers
{
    public class AccountController : Controller
    {
        public LogedUserVM LogedUser()
        {
            var userLogged = User as MPrincipal;
            LogedUserVM logedUserVM = new LogedUserVM() { Loged = false };

            if (userLogged != null)
            {
                var login = userLogged.UserDetails.Login;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    if (cust != null)
                    {
                        logedUserVM.UserId = cust.ID;
                        logedUserVM.UserName = cust.Login;
                        logedUserVM.UserRole = cust.RoleId;
                        logedUserVM.Loged = true;
                    }
                    var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                    if (comp != null)
                    {
                        logedUserVM.UserId = comp.Id;
                        logedUserVM.UserName = comp.Login;
                        logedUserVM.UserRole = comp.RoleId;
                        logedUserVM.Loged = true;
                    }
                }
            }
            return logedUserVM;
        }


        [HttpGet]
        [ActionName("Logout")]
        public ActionResult Loginout()
        {
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [ActionName("RegisterCustomer")]
        public ActionResult RegisterCustomer()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                return RedirectToAction("Logout");
            }

            return View();
        }

        [HttpPost]
        [ActionName("RegisterCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCustomer(CustomerAccountVM _model)
        {
            if (ModelState.IsValid)
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var compEmail = db.Companies.FirstOrDefault(x => x.Email == _model.Email);
                    var compLogin = db.Companies.FirstOrDefault(x => x.Login == _model.Login);
                    var custEmail = db.Customers.FirstOrDefault(x => x.Email == _model.Email);
                    var custLogin = db.Customers.FirstOrDefault(x => x.Login == _model.Login);
                    if ((compEmail == null) && (compLogin == null) && (custEmail == null) && (custLogin == null))
                    {
                        Customer customer = new Customer();
                        customer.Name = _model.Name;
                        customer.Surname = _model.Surname;
                        customer.Login = _model.Login;
                        customer.PhoneNumber = _model.PhoneNumber;
                        customer.Email = _model.Email;
                        customer.Password = Security.sha512encrypt(_model.Password);
                        customer.RoleId = 3;
                        customer.IsDelete = false;
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("LoginCustomer");
                    }
                    else if ((compEmail != null) || (custEmail != null))
                    {
                        ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                    }
                    else if ((compLogin != null) || (custLogin != null))
                    {
                        ModelState.AddModelError("Login", "Użytkownik o podanym loginie już istnieje");
                    }
                }
            }

            return View(_model);
        }

        [HttpGet]
        [ActionName("LoginCustomer")]
        [Route("LoginCustomer")]
        public ActionResult LoginCustomer()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                return RedirectToAction("Logout");
            }

            return View();
        }

        [HttpPost]
        [ActionName("LoginCustomer")]
        public ActionResult LoginCustomer(CustomerAccountVM _model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                bool validEmail = db.Customers.Any(x => x.Email == _model.Email);
                bool validLogin = db.Customers.Any(x => x.Login == _model.Login);


                if (!(validEmail || validLogin))
                {
                    ModelState.AddModelError("Password", "Niepoprawny login lub hasło");
                    return View(_model);
                }

                _model.Password = Security.sha512encrypt(_model.Password);

                Customer customer = db.Customers.FirstOrDefault(u => u.Login.Equals(_model.Login) && u.Password.Equals(_model.Password));

                string authId = Guid.NewGuid().ToString();

                Session["AuthID"] = authId;
                var cookie = new HttpCookie("AuthID");
                cookie.Value = authId;
                Response.Cookies.Add(cookie);

                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(customer.Login, false);
                    var authTicket = new FormsAuthenticationTicket(1, customer.Login, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30), false, "");
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    authCookie.Expires = DateTime.UtcNow.AddMinutes(30);
                    Response.SetCookie(authCookie);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Niepoprawny login lub hasło");
                return View(_model);

            }

        }

        [HttpGet]
        [ActionName("EditCustomer")]
        public ActionResult EditCustomer()
        {
            var userCustomer = User as MPrincipal;
            var login = userCustomer.UserDetails.Login;
            ViewBag.UserName = userCustomer.UserDetails.Login;
            ViewBag.UserRole = 3;
            CustomerAccountEditVM customerVM = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));

                customerVM = new CustomerAccountEditVM();
                customerVM.Name = cust.Name;
                customerVM.Surname = cust.Surname;
                customerVM.PhoneNumber = cust.PhoneNumber;
                customerVM.Email = cust.Email;
            }

            return View("EditCustomer", customerVM);
        }

        [HttpPost]
        [ActionName("EditCustomer")]
        public ActionResult EditCustomer(CustomerAccountEditVM model)
        {
            var userCustomer = User as MPrincipal;
            var login = userCustomer.UserDetails.Login;
            ViewBag.UserName = userCustomer.UserDetails.Login;
            ViewBag.UserRole = 3;

           
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var baseCompEmail = db.Companies.FirstOrDefault(x => x.Email == model.Email);
                    var baseCustEmail = db.Customers.FirstOrDefault(x => x.Email == model.Email);


                    var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    if (cust != null)
                    {
                        // loginu nie można zmienić, więc tutaj tylko email nie może się powtórzyć z takim jaki jużjest w bazie
                        if ((baseCompEmail == null && baseCustEmail == null) || model.Email == cust.Email)
                        {
                            cust.Name = model.Name ?? "";
                            cust.Surname = model.Surname ?? "";
                            cust.PhoneNumber = model.PhoneNumber ?? "";
                            cust.Email = model.Email ?? "";
                            if (!string.IsNullOrEmpty(model.Password) && model.Password == cust.Password)
                            {
                                cust.Password = model.NewPassword;
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                        }
                    }
                }

                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [ActionName("RegisterCompany")]
        public ActionResult RegisterCompany()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                return RedirectToAction("Logout");
            }

            return View();

        }

        [HttpPost]
        [ActionName("RegisterCompany")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCompany(CompanyAccountVM _model)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var compEmail = db.Companies.FirstOrDefault(x => x.Email == _model.Email);
                    var compLogin = db.Companies.FirstOrDefault(x => x.Login == _model.Login);
                    var custEmail = db.Customers.FirstOrDefault(x => x.Email == _model.Email);
                    var custLogin = db.Customers.FirstOrDefault(x => x.Login == _model.Login);
                    if ((compEmail == null) && (compLogin == null) && (custEmail == null) && (custLogin == null))
                    {
                        Company company = new Company();
                        company.CompanyName = _model.CompanyName;
                        company.Login = _model.Login;
                        company.PhoneNumer = _model.PhoneNumber;
                        company.Email = _model.Email;
                        company.Password = Security.sha512encrypt(_model.Password);
                        company.RoleId = 4;
                        company.IsDelete = false;
                        company.CityId = 1;
                        db.Companies.Add(company);
                        db.SaveChanges();
                        return RedirectToAction("LoginCompany");
                    }
                    else if ((compEmail != null) || (custEmail != null))
                    {
                        ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                    }
                    else if ((compLogin != null) || (custLogin != null))
                    {
                        ModelState.AddModelError("Login", "Użytkownik o podanym loginie już istnieje");
                    }
                }
            }

            return View(_model);
        }

        [HttpGet]
        [ActionName("LoginCompany")]
        [Route("LoginCompany")]
        public ActionResult LoginCompany()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                return RedirectToAction("Logout");
            }

            return View();
        }

        [HttpPost]
        [ActionName("LoginCompany")]
        public ActionResult LoginCompany(CompanyAccountVM _model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                bool validEmail = db.Companies.Any(x => x.Email == _model.Email);
                bool validLogin = db.Companies.Any(x => x.Login == _model.Login);

                if (!(validEmail || validLogin))
                {
                    ModelState.AddModelError("Password", "Niepoprawny login lub hasło");
                    return View(_model);
                }

                _model.Password = Security.sha512encrypt(_model.Password);
                ViewBag.Title = "Logowanie klienta";

                Company company = db.Companies.FirstOrDefault(u => u.Login.Equals(_model.Login) && u.Password.Equals(_model.Password));

                string authId = Guid.NewGuid().ToString();

                Session["AuthID"] = authId;
                var cookie = new HttpCookie("AuthID");
                cookie.Value = authId;
                Response.Cookies.Add(cookie);

                if (company != null)
                {
                    FormsAuthentication.SetAuthCookie(company.Login, false);
                    var authTicket = new FormsAuthenticationTicket(1, company.Login, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(60), false, "");
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    authCookie.Expires = DateTime.UtcNow.AddMinutes(60);
                    Response.SetCookie(authCookie);
                    return RedirectToAction("Index", "Home");
                }
                return View(_model);
            }
        }
        [HttpGet]
        [ActionName("IndexCompany")]
        public ActionResult IndexCompany()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                ViewBag.UserName = comp.Login;
                ViewBag.IconNumber = 0;
                ViewBag.Role = comp.RoleId;
            }
            return View();
        }

        [HttpGet]
        [ActionName("EditCompany")]
        public ActionResult EditCompany()
        {
            ViewBag.CityList = CitiesQuery();
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;


            CompanyAccountEditVM companyEditVM = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                ViewBag.UserName = comp.Login;
                ViewBag.Role = comp.RoleId;
                companyEditVM = new CompanyAccountEditVM();
                companyEditVM.CompanyName = comp.CompanyName;
                companyEditVM.PhoneNumber = comp.PhoneNumer;
                companyEditVM.Email = comp.Email;
            }

            return View("EditCompany", companyEditVM);
        }
        [HttpPost]
        [ActionName("EditCompany")]
        public ActionResult EditCompany(CompanyAccountEditVM model)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var baseCompEmail = db.Companies.FirstOrDefault(x => x.Email == model.Email);
                var baseCustEmail = db.Customers.FirstOrDefault(x => x.Email == model.Email);

                var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                if (comp != null)
                {
                    // loginu nie można zmienić, więc tutaj tylko email nie może się powtórzyć z takim jaki jużjest w bazie
                    if ((baseCompEmail == null && baseCustEmail == null) || model.Email == comp.Email)
                    {
                        comp.CompanyName = model.CompanyName ?? "";
                        comp.PhoneNumer = model.PhoneNumber ?? "";
                        comp.Email = model.Email ?? "";
                        if (!string.IsNullOrEmpty(model.Password) && model.Password == comp.Password)
                        {
                            comp.Password = model.NewPassword;
                        }
                        db.SaveChanges();
                        return RedirectToAction("EditCompany");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                    }
                }
            }
            return RedirectToAction("EditCompany");
        }
        private static List<SelectListItem> CitiesQuery()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                items = (from li in db.Cities
                         select new SelectListItem
                         {
                             Text = li.Name,
                             Value = li.ID.ToString()
                         }).ToList();
            }
            return items;
        }
    }
}