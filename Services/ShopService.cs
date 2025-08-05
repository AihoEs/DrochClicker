/* public int ItemId { get; set; }
        public string Name { get; set; }
        public string EffectType { get; set; } //boost or autoclick
        public int EffectValue { get; set; }
        public int Price { get; set; } */

using Drochclicker.Pages.Database_info;
using System.Threading.Tasks;

namespace DrochClicker.Services
{
    public class ShopService
    {
        private readonly DbContextInfo _db;

        public ShopService(DbContextInfo db)
        {
            _db = db;
        }
        public ShopInfo shop { get; set; }
        
        public UserInfo Upgrade { get; set; }
        public async Task AddDefaultUpgrade()
        {
            
            var newUpgrade = new List<ShopInfo>()
            {
                new ShopInfo
                {
                ItemId = 1,
                Name = "Накачать руку",
                EffectType = "boost",
                Value = 1,
                Price = 60
                },
                new ShopInfo
                {
                ItemId = 2,
                Name = "Смазка",
                EffectType = "boost",
                Value = 2,
                Price = 110
                },
                new ShopInfo
                {
                ItemId = 3,
                Name = "Бомж",
                EffectType = "boost",
                Value = 4,
                Price = 250
                },
                new ShopInfo
                {
                ItemId = 4,
                Name = "Обмануть друга",
                EffectType = "boost",
                Value = 7,
                Price = 600
                },
                new ShopInfo
                {
                ItemId = 5,
                Name = "Снять проститутку",
                EffectType = "boost",
                Value = 10,
                Price = 1500
                },
                new ShopInfo
                {
                ItemId = 6,
                Name = "Кончить",
                EffectType = "Rebirth",
                Value = 0,
                Price = 15000
                }



            };
            var existingIds = _db.Upgrades.Select(u => u.ItemId).ToHashSet();
            var NewUpgrades = newUpgrade.Where(x => !existingIds.Contains(x.ItemId)).ToList();

            if (NewUpgrades.Any())
            {
                _db.Upgrades.AddRange(NewUpgrades);
                await _db.SaveChangesAsync();

            }
        }
        public List<ShopInfo> GetAllUpgrades()
        {
            return _db.Upgrades.ToList();
        }




    }
}
