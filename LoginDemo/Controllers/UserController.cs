using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginDemo.Controllers
{
    public class UserController : Controller
    {
        LogindatabaseEntities db = new LogindatabaseEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.User inloggning)
        {
            if (inloggning.Username == null || inloggning.Password == null)
            {
                ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                return View();
            }
            bool validUser = false;

            //Två metoder för att kontrollera att det är rätt användare. Använd en av dem!

            //1. Kontrollera mot lösenord i web.config
            //validUser = System.Web.Security.FormsAuthentication.Authenticate(inloggning.Username, inloggning.Password);

            //2. Kontrollera mot databas
            validUser = CheckUser(inloggning.Username, inloggning.Password);

            if (validUser == true)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(inloggning.Username, false);
            }
            ModelState.AddModelError("", "Inloggningen ej godkänd");
            return View();
        }

        private bool CheckUser(string username, string password)
        {
            var anvandare = from rader in db.Users
                            where rader.Username.ToUpper() == username.ToUpper()
                            && rader.Password == password
                            select rader;
            if (anvandare.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}