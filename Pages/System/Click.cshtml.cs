using Drochclicker.Pages.Database_info;
using Drochclicker.Pages.Database_info;
using DrochClicker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Drochclicker.Pages.System
{
    [Authorize]
    public class ClickModel : PageModel
    {
        private readonly DbContextInfo _db;
        
        [BindProperty]
        public int Click { get; set; } = 0;
        public int Boost { get; set; } = 0;

        public List<UserInfo> Inventory { get; set; } = new List<UserInfo>();
        public List<UserInfo> UserUpgrades { get; set; } = new List<UserInfo>();
        public int Rebirth { get; set; }
        public double finalPrice { get; set; }

        public double RebirthPrice { get; set; }







        public ClickModel(DbContextInfo db)
        {
            _db = db;
            
        }
        public List<ShopInfo> ShopUpgrades { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                
                return RedirectToPage("/Login");
            }

            var login = User.Identity.Name;
            var user = await _db.DataBase.FindAsync(login);
            var Rebirths = await _db.UserUpgrades.Where(u => u.UserLogin == login).Select(u => u.Rebirth).ToListAsync();
            var RebirthCount = Rebirths.Count > 0 ? Rebirths.Max() : 0;
            Rebirth = RebirthCount;
            var UserName = GetUserName();
            Inventory = await _db.UserUpgrades.Include(u => u.Upgrade).Where(u => u.UserLogin == UserName).ToListAsync();
            UserUpgrades = await _db.UserUpgrades.Include(u => u.Upgrade).Where(u => u.UserLogin == UserName).ToListAsync();
            double basePrice = 15000; 
            RebirthPrice = basePrice * Math.Pow(1.5, RebirthCount + 1);

            var entry = await _db.DataBase.FindAsync(UserName);
            ShopUpgrades = _db.Upgrades.ToList();

            if(entry == null)
            {
                
                return RedirectToPage("/User/Login");
            }
            Click = entry.ClickCount;
            return Page();
            


        }
        public async Task<IActionResult> OnPost()
        {
            
            Boost = 1;
            var login = User.Identity.Name;
            var user = await _db.DataBase.FindAsync(login);
            var Rebirths = await _db.UserUpgrades.Where(u => u.UserLogin == login).Select(u => u.Rebirth).ToListAsync();
            var RebirthCount = Rebirths.Count > 0 ? Rebirths.Max() : 0;
            ShopUpgrades = _db.Upgrades.ToList();
            double basePrice = 15000;
            RebirthPrice = basePrice * Math.Pow(1.5, RebirthCount + 1);

            Rebirth = RebirthCount;

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
                    int rebirthMultiplier = (int)Math.Pow(2, RebirthCount);
                    var BaseClick = upgrader.Level * upgrader.Upgrade.Value;
                    Boost += (BaseClick * rebirthMultiplier);
                }
            }
            entry.ClickCount += Boost;
            await _db.SaveChangesAsync();
            Click = entry.ClickCount;
            return Page();

        }
        public async Task<IActionResult> OnPostRebirthAsync(int upgradeId)
        {
            ShopUpgrades = _db.Upgrades.ToList();
            var login = User.Identity.Name;
            var user = await _db.DataBase.FindAsync(login);
            var Rebirths = await _db.UserUpgrades.Where(u => u.UserLogin == login).Select(u => u.Rebirth).ToListAsync();
            var RebirthCount = Rebirths.Count > 0 ? Rebirths.Max() : 0;
            Rebirth = RebirthCount;
            var dbRebirthPrice = await _db.Upgrades.Where(u => u.EffectType == "Rebirth" && u.ItemId == upgradeId).Select(u => u.Price).FirstOrDefaultAsync();
            RebirthPrice = dbRebirthPrice * Math.Pow(1.5, RebirthCount + 1);

            

            if (user.ClickCount >= RebirthPrice)
            {
                var existingRebirth = await _db.UserUpgrades.FirstOrDefaultAsync(u => u.UserLogin == login && u.UpgradeId == upgradeId);

                if (existingRebirth != null)
                {
                    existingRebirth.Level += 1;
                }
                
                var upgrades = _db.UserUpgrades.Where(u => u.UserLogin == login);
                _db.UserUpgrades.RemoveRange(upgrades);
                user.ClickCount = 0;
                RebirthCount++;
                var newRebirthInfo = new UserInfo
                {
                    UserLogin = login,
                    UpgradeId = upgradeId,
                    Rebirth = RebirthCount,
                    Level = 1
                };
                await _db.UserUpgrades.AddAsync(newRebirthInfo);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/Index");
            }



        }
        public string GetUserName()
        {
            return User.Identity?.Name ?? "anon";
        }

    }
}
