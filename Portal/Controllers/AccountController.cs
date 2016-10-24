using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Portal.Models;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Security;

namespace Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public AccountController()
            : this( new ApplicationUserManager( new UserStore<Person>( new ApplicationDbContext() ) ) )
        {
            db = new ApplicationDbContext();
        }

        public AccountController( ApplicationUserManager applicationUserManager )
        {
            userManager = applicationUserManager;
            db = new ApplicationDbContext();
        }

        public ApplicationUserManager userManager { get; private set; }
        public ApplicationDbContext db { get; set; }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login( string returnUrl )
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login( LoginViewModel model, string returnUrl )
        {
            if( ModelState.IsValid ) {
                var user = await userManager.FindAsync( model.UserName, model.Password );
                if( user != null ) {
                    // if checkbox is ticked
                    if (model.RememberMe)
                    {
                        // Clear any other tickets that are already in the response
                        Response.Cookies.Clear();

                        // Set the new expiry date - to thirty days from now
                        DateTime expiryDate = DateTime.Now.AddDays(30);

                        // Create a new forms auth ticket
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, model.UserName, DateTime.Now, expiryDate, true, String.Empty);

                        // Encrypt the ticket
                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                        // Create a new authentication cookie - and set its expiration date
                        HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authenticationCookie.Expires = ticket.Expiration;

                        // Add the cookie to the response.
                        Response.Cookies.Add(authenticationCookie);
                    }

                    await SignInAsync( user, true );
                    return RedirectToLocal( returnUrl );
                } else {
                    ModelState.AddModelError( "", "Invalid username or password." );
                }
            }

            // If we got this far, something failed, redisplay form
            return View( model );
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        protected void SendVerificationEmail( Person user )
        {
            try {
                NameValueCollection mailingSection = ( NameValueCollection )ConfigurationManager.GetSection( "adminMailingSettings" );
                string adminEmail = mailingSection["TargetEmailAddress"].ToString(); // address receiving the confirmation request

                string senderEmail = mailingSection["FromEmailAddress"].ToString(); // address that makes all the mailing
                string senderPasswd = mailingSection["FromEmailPassword"].ToString();
                string senderDisplayName = mailingSection["FromEmailDisplayName"].ToString();

                MailMessage mail = new MailMessage();
                mail.To.Add( adminEmail );
                mail.From = new MailAddress( senderEmail, senderDisplayName, System.Text.Encoding.UTF8 );
                mail.Subject = "[ABBYY Portal] New teacher is waiting for approval";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                string bodyTemplate = "Hello!\r\n\r\n" +
                "User {0} has just registered as a teacher and wants to be verified to start working on courses.\r\n\r\n" +
                "If you are sure that you know the user, please, approve it by admin interface. Otherwise you may contact the user at email {1}. \r\n\r\n" +
                "Yours, ABBYY Portal Team.";
                string fullName = String.Format( "{0} {1} {2}", user.First_Name, user.Middle_Name, user.Second_Name );

                mail.Body = String.Format( bodyTemplate, fullName, user.Email );
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential( senderEmail, senderPasswd );
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
            
                client.Send( mail );
            } catch( Exception ex ) {
                Exception ex2 = ex;
                List<string> errorMessages = new List<string>();
                while( ex2 != null ) {
                    errorMessages.Add( ex2.ToString() );
                    ex2 = ex2.InnerException;
                }
                IdentityResult result = new IdentityResult( errorMessages );
                AddErrors( result );
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register( RegisterViewModel model )
        {
            if( ModelState.IsValid ) {
                var user = new Person() {
                    UserName = model.UserName,
                    Email = model.UserName,
                    First_Name = model.First_Name,
                    Second_Name = model.Second_Name,
                    Middle_Name = model.Middle_Name,
                    Registration_Date = DateTime.Now,
                    Last_Date_Was_Online = DateTime.Now,
                    PhoneNumber = model.PhoneNumber,
                    Exists = true,
                    Person_Type = model.Person_Type
                };
                var result = await userManager.CreateAsync( user, model.Password );
                db.SaveChanges();
                if( result.Succeeded ) {
                    Person pers = db.Users.Where( p => p.UserName == model.UserName ).FirstOrDefault();
                    pers.Picture = db.Picture.Where( p => p.Name == "DefaultPicture" ).FirstOrDefault();
                    userManager.AddToRole( pers.Id, "user" );
                    db.SaveChanges();
                    await SignInAsync( user, isPersistent: false );

                    // Here mail admins if teacher needs to be approved
                    if( user.Person_Type == "Teacher" ) {
                        pers.Person_Type = "Student"; // roll the status back before the confirmation
                        db.SaveChanges();

                        SendVerificationEmail( user );
                    }

                    List<ModelError> errors = new List<ModelError>();
                    foreach( ModelState modelState in ViewData.ModelState.Values ) {
                        foreach( ModelError error in modelState.Errors ) {
                            errors.Add( error );
                        }
                    }

                    TempData["errors"] = errors;
                    if ( errors.Count() != 0 ) {
                        return RedirectToAction( "Index", "Error" );
                    }

                    return RedirectToAction( "Index", "Home" );
                } else {
                    AddErrors( result );
                }

            }

            // If we got this far, something failed, redisplay form
            return View( model );
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate( string loginProvider, string providerKey )
        {
            ManageMessageId? message = null;
            IdentityResult result = await userManager.RemoveLoginAsync( User.Identity.GetUserId(), new UserLoginInfo( loginProvider, providerKey ) );
            if( result.Succeeded ) {
                message = ManageMessageId.RemoveLoginSuccess;
            } else {
                message = ManageMessageId.Error;
            }
            return RedirectToAction( "Manage", new { Message = message } );
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage( ManageMessageId? message )
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action( "Manage" );
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage( ManageUserViewModel model )
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action( "Manage" );
            if( hasPassword ) {
                if( ModelState.IsValid ) {
                    IdentityResult result = await userManager.ChangePasswordAsync( User.Identity.GetUserId(), model.OldPassword, model.NewPassword );
                    if( result.Succeeded ) {
                        return RedirectToAction( "Manage", new { Message = ManageMessageId.ChangePasswordSuccess } );
                    } else {
                        AddErrors( result );
                    }
                }
            } else {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if( state != null ) {
                    state.Errors.Clear();
                }

                if( ModelState.IsValid ) {
                    IdentityResult result = await userManager.AddPasswordAsync( User.Identity.GetUserId(), model.NewPassword );
                    if( result.Succeeded ) {
                        return RedirectToAction( "Manage", new { Message = ManageMessageId.SetPasswordSuccess } );
                    } else {
                        AddErrors( result );
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View( model );
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin( string provider, string returnUrl )
        {
            // Request a redirect to the external login provider
            return new ChallengeResult( provider, Url.Action( "ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl } ) );
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback( string returnUrl )
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if( loginInfo == null ) {
                return RedirectToAction( "Login" );
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await userManager.FindAsync( loginInfo.Login );
            if( user != null ) {
                await SignInAsync( user, isPersistent: false );
                return RedirectToLocal( returnUrl );
            } else {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View( "ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName } );
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin( string provider )
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult( provider, Url.Action( "LinkLoginCallback", "Account" ), User.Identity.GetUserId() );
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync( XsrfKey, User.Identity.GetUserId() );
            if( loginInfo == null ) {
                return RedirectToAction( "Manage", new { Message = ManageMessageId.Error } );
            }
            var result = await userManager.AddLoginAsync( User.Identity.GetUserId(), loginInfo.Login );
            if( result.Succeeded ) {
                return RedirectToAction( "Manage" );
            }
            return RedirectToAction( "Manage", new { Message = ManageMessageId.Error } );
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation( ExternalLoginConfirmationViewModel model, string returnUrl )
        {
            if( User.Identity.IsAuthenticated ) {
                return RedirectToAction( "Manage" );
            }

            if( ModelState.IsValid ) {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if( info == null ) {
                    return View( "ExternalLoginFailure" );
                }
                var user = new Person() { UserName = model.UserName };
                var result = await userManager.CreateAsync( user );
                if( result.Succeeded ) {
                    result = await userManager.AddLoginAsync( user.Id, info.Login );
                    if( result.Succeeded ) {
                        await SignInAsync( user, isPersistent: false );
                        return RedirectToLocal( returnUrl );
                    }
                }
                AddErrors( result );
            }

            ViewBag.ReturnUrl = returnUrl;
            return View( model );
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction( "Index", "Home" );
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = userManager.GetLogins( User.Identity.GetUserId() );
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return ( ActionResult )PartialView( "_RemoveAccountPartial", linkedAccounts );
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing && userManager != null ) {
                userManager.Dispose();
                userManager = null;
            }
            base.Dispose( disposing );
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync( Person user, bool isPersistent )
        {
            AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
            var identity = await userManager.CreateIdentityAsync( user, DefaultAuthenticationTypes.ApplicationCookie );
            AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, identity );
        }

        private void AddErrors( IdentityResult result )
        {
            foreach( var error in result.Errors ) {
                ModelState.AddModelError( "", error );
            }
        }

        private bool HasPassword()
        {
            var user = userManager.FindById( User.Identity.GetUserId() );
            if( user != null ) {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal( string returnUrl )
        {
            if( Url.IsLocalUrl( returnUrl ) ) {
                return Redirect( returnUrl );
            } else {
                return RedirectToAction( "Index", "Home" );
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult( string provider, string redirectUri ) : this( provider, redirectUri, null )
            {
            }

            public ChallengeResult( string provider, string redirectUri, string userId )
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult( ControllerContext context )
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if( UserId != null ) {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge( properties, LoginProvider );
            }
        }
        #endregion
    }
}