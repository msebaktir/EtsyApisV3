using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace dotnetEtsyApp.Models.RecordsData.Cradentials
{
    public class User: IdentityUser
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        


    }
}