using AIM.Controllers;
using AIM.Data;
using AIM.Models;
using ExternalLogin.Extensions;
using ExternalLogin.Interfaces;
using LSM_PN;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;

namespace EmployeeProject.UI.Controllers
{
    public class ExternalController : BaseController
    {
        private readonly IExternalLoginService externalLoginService;
        private readonly AimDbContext contextDB;
        private readonly LDAPSettings ldapSettings;

        public ExternalController(IExternalLoginService externalLoginService, AimDbContext contextDB, LDAPSettings ldapSettings) : base(contextDB)
        {
            this.externalLoginService = externalLoginService;
            this.contextDB = contextDB;
            this.ldapSettings = ldapSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string token) // make sure to add the parameter in the endpoint
        {

                var ipaddress = HttpContext.IpAddress();
                var usercode = string.Empty;

                if (externalLoginService.TryValidateToken(token, ipaddress, out usercode))
                    return await AuthenticateUser(usercode); // if validated, this is where you setup the user session 

                return RedirectToAction("Login", "Users");

        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return Redirect(externalLoginService.PortalUrl);
 
        }

        private async Task<IActionResult> AuthenticateUser(string usercode)
        {
            try
            {

                var user = await contextDB.tbl_aim_users.FirstOrDefaultAsync(a => a.UserCode == usercode);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserCode);
                    HttpContext.Session.SetString("UserPass", user.UserPass);
                    HttpContext.Session.SetString("FullName", user.UserFullName);
                    HttpContext.Session.SetString("ProfileId", user.UserProfile.ToString());

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return RedirectToAction("Login", "Users");

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Employee");
            }
        }
    }
}
