using Drochclicker.Pages.Database_info;
using DrochClicker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DrochClicker.Pages
{
    [Authorize]
    public class ShopModel : PageModel
    {
        
        private readonly ShopService _shopService;
        private readonly DbContextInfo _db;
        

        public List<ShopInfo> ShopUpgrades { get; set; } = new();
        public List<UserInfo> UserUpgrades { get; set; } = new();
        public List<ShopInfo> VisibleUpgrades { get; set; }



        

        public List<ShopInfo> Upgrades { get; set; }
        public ShopModel(DbContextInfo db, ShopService shopService)
        {
            _db = db;
            _shopService = shopService;
            
           
            
        }
        
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToPage("/Login");
            }
            var login = User.Identity.Name;
            var allUpgrades = await _db.Upgrades.ToListAsync();
            var userUpgrades = await _db.UserUpgrades.Where(u => u.UserLogin == login).ToListAsync();
            VisibleUpgrades = new List<ShopInfo>();
            foreach (var upgrade in allUpgrades)
            {

                if (upgrade.PreviousUpgradeId == null)
                {
                    VisibleUpgrades.Add(upgrade);
                    continue;

                }
                var prevUserUpgrade = userUpgrades.FirstOrDefault(u => u.UpgradeId == upgrade.PreviousUpgradeId);

                if (prevUserUpgrade != null && prevUserUpgrade.Level > 0)
                {
                    VisibleUpgrades.Add(upgrade);

                }
            }
            _shopService.AddDefaultUpgrade();


            ShopUpgrades = _db.Upgrades.ToList();
            UserUpgrades = _db.UserUpgrades.Where(x => x.UserLogin == login).ToList();
            return Page();




        }
        public async Task<IActionResult> OnPostBuyAsync(int upgradeId)
        {
            
            var login = User.Identity.Name;
            var upgrades = await _db.UserUpgrades.FirstOrDefaultAsync(x => x.UserLogin == login && x.UpgradeId == upgradeId);
            ShopUpgrades = _db.Upgrades.ToList();
            var shopItem = ShopUpgrades.FirstOrDefault(x => x.ItemId == upgradeId);
            var user = await _db.DataBase.FindAsync(login);
            


            if (shopItem == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                
                int level = upgrades?.Level ?? 0;
                double finalPrice = shopItem.Price * Math.Pow(1.15, level);
                if (user.ClickCount >= (int)finalPrice)
                {


                    if (upgrades != null)
                    {
                        upgrades.Level++;
                        user.ClickCount -= (int)finalPrice;
                    }
                    else
                    {
                        
                        var newUpgrade = new UserInfo
                        {
                            UserLogin = login,
                            UpgradeId = upgradeId,
                            Level = 1,
                            
                        };
                        _db.UserUpgrades.Add(newUpgrade);
                        user.ClickCount -= (int)finalPrice;
                    }
                }
                else
                {
                    return Page();
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/System/Click");


        }
        public async Task<IActionResult> OnPostBuy5Async(int upgradeId)
        {

            var login = User.Identity.Name;
            var upgrades = await _db.UserUpgrades.FirstOrDefaultAsync(x => x.UserLogin == login && x.UpgradeId == upgradeId);
            ShopUpgrades = _db.Upgrades.ToList();
            var shopItem = ShopUpgrades.FirstOrDefault(x => x.ItemId == upgradeId);
            var user = await _db.DataBase.FindAsync(login);



            if (shopItem == null)
            {
                return RedirectToPage("/Index");
            }
            else
            {

                int level = upgrades?.Level ?? 0;
                double basePrice = shopItem.Price * Math.Pow(1.15, level); // цена следующего апгрейда
                int n = 5; 

                double rate = 1.15;

                double finalPrice = basePrice * (Math.Pow(rate, n) - 1) / (rate - 1);
                if (user.ClickCount >= (int)finalPrice)
                {


                    if (upgrades != null)
                    {
                        upgrades.Level += 5;
                        user.ClickCount -= (int)finalPrice;
                    }
                    else
                    {

                        var newUpgrade = new UserInfo
                        {
                            UserLogin = login,
                            UpgradeId = upgradeId,
                            Level = 5,

                        };
                        _db.UserUpgrades.Add(newUpgrade);
                        user.ClickCount -= (int)finalPrice;
                    }
                }
                else
                {
                    return Page();
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/System/Click");


        }
    }
}
