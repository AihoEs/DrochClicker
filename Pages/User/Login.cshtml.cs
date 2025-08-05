using Drochclicker.Pages.Database_info;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Drochclicker.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly IPasswordHasher<UserContext> _hasher;
        private readonly DbContextInfo _db;
        [BindProperty]
        [Required]
        public string Login { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PasswordHash { get; set; }
        public LoginModel(DbContextInfo db, IPasswordHasher<UserContext> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _db.DataBase.FirstOrDefaultAsync(o => o.Login == Login);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверные логин или пароль");
                return Page();
            }
            var result = _hasher.VerifyHashedPassword(user, user.Password, Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Неверные логин или пароль");
                return Page();
            }
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login)

        };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToPage("/Index");
        }
    }
}
