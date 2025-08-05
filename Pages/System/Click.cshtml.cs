using Drochclicker.Pages.Database_info;
using Drochclicker.Pages.Database_info;
using DrochClicker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Drochclicker.Pages.System
{
    [Authorize]
    public class ClickModel : PageModel
    {
        private readonly DbContextInfo _db;
        
        [BindProperty]
        public int Click { get; set; } = 0;
        public int Boost { get; set; } = 1;

        public List<UserInfo> Inventory { get; set; } = new List<UserInfo>();
        public List<UserInfo> UserUpgrades { get; set; } = new List<UserInfo>();


        public ClickModel(DbContextInfo db)
        {
            _db = db;
            
        }
        public async Task OnGetAsync()
        {
            
            var UserName = GetUserName();
            Inventory = await _db.UserUpgrades.Include(u => u.Upgrade).Where(u => u.UserLogin == UserName).ToListAsync();
            UserUpgrades = await _db.UserUpgrades.Include(u => u.Upgrade).Where(u => u.UserLogin == UserName).ToListAsync();
            var entry = await _db.DataBase.FindAsync(UserName);

            if(entry == null)
            {
                entry = new UserContext { Login = UserName, ClickCount = 1 };
                _db.DataBase.Add(entry);
                await _db.SaveChangesAsync();
            }
            Click = entry.ClickCount;
            


        }
        public async Task<IActionResult> OnPost()
        {
            Boost = 1;
            var UserName = GetUserName();
            Inventory = await _db.UserUpgrades.Include(u => u.Upgrade).Where(u => u.UserLogin == UserName).ToListAsync();
            var upgrades = _db.UserUpgrades.Where(u => u.UserLogin == UserName);
            
            var entry = await _db.DataBase.FindAsync(UserName);
            
            

            if (entry == null)
            {
                entry = new UserContext { Login = UserName, ClickCount = 1 };
                _db.DataBase.Add(entry);
                
            }
            else
            {
                foreach (var upgrader in upgrades)
                {
                    
                    Boost += upgrader.Level * upgrader.Upgrade.Value;
                }
            }
            entry.ClickCount += Boost;
            await _db.SaveChangesAsync();
            Click = entry.ClickCount;
            return Page();

        }
        public string GetUserName()
        {
            return User.Identity?.Name ?? "anon";
        }

    }
}
