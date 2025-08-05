using DrochClicker.Services;
using System.ComponentModel.DataAnnotations;

namespace Drochclicker.Pages.Database_info
{
    public class UserContext
    {
        [Key]
        public string Login { get; set; }
        public string Password { get; set; }
        public int ClickCount { get; set; }

        public List <UserInfo> UserUpgrades { get; set; } = new List<UserInfo>();
    }
}
