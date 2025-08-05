using Drochclicker.Pages.Database_info;
using System.ComponentModel.DataAnnotations;

namespace DrochClicker.Services
{
    public class ShopInfo
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string EffectType { get; set; } //boost or autoclick
        public int Value { get; set; }
        public int Price { get; set; }
    }
    public class UserInfo
    {
        [Key]
        public int UpgradeId { get; set; }
        [Key]
        public string UserLogin { get; set; }
        public int Level { get; set; }
        public int ShopItemId { get; set; }

        public UserContext User { get; set; }
        public ShopInfo Upgrade { get; set; }
    }
}
