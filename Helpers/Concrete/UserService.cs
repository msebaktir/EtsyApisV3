using System.Security.Claims;
using System.Text;
using dotnetEtsyApp.Data;
using dotnetEtsyApp.Models;
using dotnetEtsyApp.Models.RecordsData.Cradentials;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace dotnetEtsyApp.Helpers.Concrete
{
    public class UserService
    {
        private readonly string HashSecret = "dotnetEtsyApp";
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string passwordToHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(HashSecret);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
            return hashed;
        }
        
        
        public async Task<ReturnData<bool>> Login(string username, string password)
        {
            
            var hashedPassword = passwordToHash(password);
            var userResult = isUserValid(username, hashedPassword);
            if (!userResult.Success)
            {
                return new ReturnData<bool>(false, userResult.Message);
            }
            await CreateUserClaims(userResult.Data);
            return new ReturnData<bool>(true);
        }
        public async Task CreateUserClaims(User user){
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
             var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true,
            };
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        public async Task Logout() => await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


        public ReturnData<User> isUserValid(string username, string hashedPassword)
        {

            var user = _context.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return new ReturnData<User>(false, "User not found");
            }
            if (user.Password != hashedPassword)
            {
                return new ReturnData<User>(false, "Password is not correct");
            }
            return new ReturnData<User>(user);
        }

        public ReturnData<bool> Register(string username, string password, string email = "" , string role = "")
        {
            var hashedPassword = passwordToHash(password);
            var user = new User
            {
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Password = hashedPassword,
                Email = email,
                Role = role
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            
            return new ReturnData<bool>(true);
        }
    }
}