using CodeGeneration.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeGeneration.Controllers
{
    [Authorize]
    public class CodesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CodesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var codes = _context.Codes.Include(x => x.User).Include(x => x.Service).ToList();
            return View(codes);
        }

        public IActionResult GenerateCode()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(currentUserID);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            DateTime baseDate = DateTime.Now;
            List<int> years = new List<int>();
            List<int> months = new List<int>();
            months.Add(DateTime.Now.Month);
            for(int i = 1; i <= 12; i++)
            {
                if(DateTime.Now.Month != i)
                {
                    months.Add(i);
                }
            }
            years.Add(DateTime.Now.Year);

            // Show dates of previous fifteen years.
            for (int ctr = -1; ctr >= -2; ctr--)
            {
                var x = baseDate.AddYears(ctr);
                years.Add(x.Year);
            }

            ViewData["Years"] = new SelectList(years);
            ViewData["Months"] = new SelectList(months);

            return View();
        }
        public async Task<JsonResult> GenerateCodes(long service, int years, int months) 
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(currentUserID);


            var thirdChar = user.LastName[0].ToString();

            var year = years.ToString();
            var month = months;
            var newMonth = month.ToString();
            if(month < 10)
            {
                newMonth = "0" + month.ToString();
            }

            var Oservice = await _context.Services.FindAsync(service);

            var serial = Oservice.Serial.ToString();


            var CodesCounterInmonth = (_context.Codes.Where(x => x.Date_Created.Year == DateTime.Now.Year && x.Date_Created.Month == DateTime.Now.Month).ToList().Count() + 1).ToString();

            var CodeGenerated = user.UserSerial + year + newMonth + serial + CodesCounterInmonth;


            var isCodeExist = _context.Codes.FirstOrDefault(x => x.Codes == CodeGenerated);
            if (isCodeExist != null)
            {
                CodeGenerated = thirdChar + CodeGenerated;
                var isCodeExistWithThree = _context.Codes.FirstOrDefault(x => x.Codes == CodeGenerated);
                if (isCodeExistWithThree != null)
                {
                    return Json("Code AlreadyExist");
                }
                else
                {
                    var newCode = new Code
                    {
                        Codes = CodeGenerated,
                        Date_Created = DateTime.Now,
                        ServiceId = service,
                        Status = 0,
                        UserId = currentUserID
                    };
                    _context.Codes.Add(newCode);
                    _context.SaveChanges();
                    return Json(CodeGenerated);
                }
                
            }
            else
            {
                var newCode = new Code
                {
                    Codes = CodeGenerated,
                    Date_Created = DateTime.Now,
                    ServiceId = service,
                    Status = 0,
                    UserId = currentUserID
                };
                _context.Codes.Add(newCode);
                _context.SaveChanges();
                return Json(CodeGenerated);

            }

            
        }
    }
}
