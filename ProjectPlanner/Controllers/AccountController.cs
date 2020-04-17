using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectPlanner.Data;
using ProjectPlanner.Email;
using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ProjectPlanner.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ITodoRepository _todoRepository;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IProjectRepository projectRepository,
                                 ITodoRepository todoRepository,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _projectRepository = projectRepository;
            _todoRepository = todoRepository;
            _emailSender = emailSender;
        }
        // GET: /<controller>/

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(true);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(model == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Name = model.Name,
                    LastName = model.LastName,
                    Email = model.Email,
                    Position = model.Position,
                    Company = model.Company,
                    Experience = model.Experience,
                };
                var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(true);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(true);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                     await _emailSender.SendEmailAsync(user.Email, "Confirm your account", $"<b>Please click in the link below to confirm your account</b> <br> <a href='{confirmationLink}'>Confirm Now</a>").ConfigureAwait(true);



                    return RedirectToAction("SuccessRegistration");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);
            if (user == null)
            {
                return View("Error");

            }

            var result = await _userManager.ConfirmEmailAsync(user, token).ConfigureAwait(true);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {

            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);


            if (user == null)
            {
                return Json(true);
            }
            else
            {   
                return Json($"Email {email} is already in use.");
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> UserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);

            if (user == null)
            {
                return Json($"The email does not match with an existing account.");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        [AllowAnonymous]

        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(model == null)
            {
                return RedirectToAction("Login");
            }


            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(true);
              
                if (user.IsExternal)
                {
                    ModelState.AddModelError("Email", "The email is associated with an external provider");

                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                    model.RememberMe, false).ConfigureAwait(true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Projects");
                }
                else 
                {
                    var isConfirmed = await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(true);

                    if (!isConfirmed)
                    {
                        ModelState.AddModelError("Email", "Email was not confirmed.");
                    }

                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect password");
                    }
                }
            }
           
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]

        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            var returnUrl = Url.Content("~/");

            var model = new LoginViewModel();

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}.");

                return View("Login", model);
            }
            var info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(true);

            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", model);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if(email != null) {
                var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);

                if(user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email", "Email not confirmed yet.");

                    return View("Login", model);
                }
            }

            var singInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true).ConfigureAwait(true);

            if (singInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {              
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Name = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            IsExternal = true,
                        };

                        await _userManager.CreateAsync(user).ConfigureAwait(true);
                    }

                    await _userManager.AddLoginAsync(user, info).ConfigureAwait(true);
                    await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(true);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on ezerivas92@asd.com";

                return View("Error");

            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditAccount()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            var projects = _projectRepository.GetAllProjects(user);

            var todos = new List<Todo>();

            if(projects == null)
            {

            }

            foreach (var project in projects)
            {
                var todos2 = project.Todos;

                foreach (var todo in todos2)
                {
                    todos.Add(todo);
                }
            }

            var model = new EditAccountViewModel()
            {
                User = user,
                Projects = projects,
                Todos = todos,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if(model == null)
            {
                return RedirectToAction("EditAccount");
            }

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);

            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.Name = model.User.Name;
                    user.LastName = model.User.LastName;
                    user.Position = model.User.Position;
                    user.Company = model.User.Company;
                    user.Experience = model.User.Experience;

                    await _userManager.UpdateAsync(user).ConfigureAwait(true);
                }
                return RedirectToAction("EditAccount");
            }

            return RedirectToAction("EditAccount");
        }

        [Authorize]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if(model == null)
            {
                return RedirectToAction("EditAccount");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword).ConfigureAwait(true);
             
                return Json(result);

            }

            return RedirectToAction("EditAccount");
        }     
    }
}