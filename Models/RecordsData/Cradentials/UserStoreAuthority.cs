using static dotnetEtsyApp.Models.Enums;

namespace dotnetEtsyApp.Models.RecordsData.Cradentials
{
   
    public class UserStoreAuthority:BaseEntity
    {
        public User User { get; set; }
        public Store Store { get; set; }
        public UserAuthority Authority { get; set; } = UserAuthority.None;
    }
}