using CodeGeneration.Data;
using CodeGeneration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CodeGeneration.Configuration;
using MailKit.Security;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LoginRegister.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {

            if (_signInManager.IsSignedIn(User)) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

           
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {



            if (ModelState.IsValid)
            {
                var e = model.Email.Trim().ToLower() + "@icon-creations" + model.Domain;
                e.Trim();
                if (_context.Users.FirstOrDefault(x => x.Email == e) != null)
                {

                    ModelState.AddModelError("", "Email is already used");
                }
                long serialnumberuser = 1;
                var LastUser = _context.Users.OrderByDescending(x => x.SerialId).FirstOrDefault();
                if (LastUser != null)
                {
                    serialnumberuser = LastUser.SerialId + 1;
                }

                var user = new ApplicationUser
                {
                    UserName = e,
                    Email = e,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    SerialId = serialnumberuser,
                    UserSerial = model.FirstName[0].ToString() + model.MiddleName[0].ToString() + serialnumberuser.ToString()

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                   await _userManager.AddToRoleAsync(user, "User");

                    var id = _context.Users.FirstOrDefault(x => x.Email == e).Id;


                    var send = InsertEmailConfirmAsync(id, e, e);
                    if (await send)
                    {
                        TempData["success"] = "you are registered successfully, please check your email to activate your account";
                        //add role here
                        await _userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Register Successfully but we can't send activation to your email");
                        ViewData["Roles"] = new SelectList(_context.Roles, "NormalizedName", "Name");
                        return View(model);
                    }
                    
                }
            }
          
                ModelState.AddModelError("", "Please entre strong password or add a valid email");
            

            return View(model);
        }
        public async Task<bool> InsertEmailConfirmAsync(string userid, string username, string email)
        {
            try
            {



                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("info@fintech-egypt.com", "Code generation Service"),
                    Subject = "Code Generation Registration Confirm",
                    PlainTextContent = "Hello," + email,
                    HtmlContent = "<h3 style='text-align:center;'>Hi </h3>"+"You're almost ready to get started. Please click on the button below to verify your email address and enjoy exclusive cleaning services with us!<br /> <br />"+ "<p style='text-align:center;'><a style='text-transform:uppercase;border-radius:10px;background-color:#FF6600;color:#fff;font-size:12px;text-decoration: none;padding:5px 20px;;width:fit-content;'  href='https://codegeneration20220511133853.azurewebsites.net/Account/AccountValidate?id=" + userid + "'>Verify your email</a>"+ "<p style='text-align:center;'>Thanks,<br />Icon-Creations</p>"
            };
                msg.AddTo(new EmailAddress(email, email));
                var response = await client.SendEmailAsync(msg);


                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public IActionResult Login()
        {

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["comeSuccess"] = TempData["success"];
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            
            await HttpContext.SignOutAsync();
            
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccountValidate()
        {
            var id = HttpContext.Request.Query["id"].ToString();
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.EmailConfirmed = true;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["success"] = "your acount activated successfully";
                return RedirectToAction("Login");

            }

            return NotFound();
        }
        [HttpGet]
        public IActionResult OTPPage()
        {
            var value = TempData.Get<ApplicationUser>("user");
            TempData.Put("newUser", value);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email.Trim().ToLower());
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid ID or Password");
                    return View(model);
                }

                if (user.EmailConfirmed)
                {

                    var res = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                    if (res.Succeeded)
                    {
                        var getUser = _context.Users.Where(x => x.Email == user.Email && x.PasswordHash == user.PasswordHash);
                        await _signInManager.SignInAsync(user, isPersistent: false);
           
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid ID or Password");
                        ViewData["Roles"] = new SelectList(_context.Roles, "NormalizedName", "Name");
                        return View(model);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Please check your email to confirm your account");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid ID or Password");
            return View(model);
        }
        
    }
}