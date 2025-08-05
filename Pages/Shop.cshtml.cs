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



        

        public List<ShopInfo> Upgrades { get; set; }
        public ShopModel(DbContextInfo db, ShopService shopService)
        {
            _db = db;
            _shopService = shopService;
            
           
            
        }
        
        public void OnGet()
        {
            var login = User.Identity.Name;
            _shopService.AddDefaultUpgrade();


            ShopUpgrades = _db.Upgrades.ToList();
            UserUpgrades = _db.UserUpgrades.Where(x => x.UserLogin == login).ToList();





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
                var price = shopItem.Price;

                if (user.ClickCount >= price)
                {


                    if (upgrades != null)
                    {
                        upgrades.Level++;
                        user.ClickCount -= price;
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
                        user.ClickCount -= price;
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
