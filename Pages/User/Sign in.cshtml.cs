using Drochclicker.Pages.Database_info;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;


namespace Drochclicker.Pages.User
{
    public class Sign_inModel : PageModel
    {
        private readonly DbContextInfo _db;
        private readonly IPasswordHasher<UserContext> _hasher;
        [BindProperty]
        [Required]
        public string Login { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
       [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }


        public Sign_inModel(DbContextInfo db, IPasswordHasher<UserContext> hasher)
        {
            _db = db;
            _hasher = hasher;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var User = await _db.DataBase.FirstOrDefaultAsync(o => o.Login == Login);
            if(User != null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким логином уже существует");
                return Page();
            
            }
            if(Password == null)
            {
                ModelState.AddModelError(string.Empty, "Введите пароль");
                return Page();
            }
            var user = new UserContext
            {
                Login = Login,
                

            };
            user.Password = _hasher.HashPassword(user, Password);
            _db.DataBase.Add(user);
            await _db.SaveChangesAsync();
            await SignInUser(user.Login);
            return RedirectToPage("/Index");

        }
        public async Task SignInUser(string login)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, login)
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
        }
        private string Hash(string input)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    }
}
