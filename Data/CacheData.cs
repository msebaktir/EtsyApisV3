using System.Net;
using System.Text.Json;
using dotnetEtsyApp.Helpers.Abstract;
using dotnetEtsyApp.Models.Cache;
using dotnetEtsyApp.Models.RequestModels;
using RestSharp;
using static dotnetEtsyApp.Models.Enums;

namespace dotnetEtsyApp.Data
{
    public class CacheData
    {
        
        private readonly ApplicationDbContext _context;
        private readonly ICodeGenerator _codeGenerator;
        private int _activationStore = 0;
        private List<StoreTokenData> _storeTokens; 
        public List<StoreTokenData> StoreTokens
        {
            get
            {
                if (_storeTokens == null || _storeTokens.Count == 0)
                {
                    _storeTokens = new List<StoreTokenData>();
                    ReConnectToActiveStores();
                }
                return _storeTokens;
            }
        }

        public CacheData(string connectionString, ICodeGenerator codeGenerator)
        {
            _storeTokens = new List<StoreTokenData>();
            _context = ApplicationDbContext.Create(connectionString);
            _codeGenerator = codeGenerator;
        }
        
        public void ReConnectToActiveStores()
        {

            foreach (var store in _context.Stores.Where(x => !x.IsDeleted))
            {
                if (_storeTokens.FirstOrDefault(x => x.StoreName == store.Name) == null)
                {
                    StoreTokenData storeToken = new StoreTokenData();
                    storeToken.StoreName = store.Name;
                    storeToken.StoreApi = store.SotreApi;
                    storeToken.SecretKey = store.StoreApiSecret;
                    storeToken.ID = store.Id;
                    _storeTokens.Add(storeToken);
                }
            }
        }
        public void AddStore(string StoreName, string StoreApi, string SecretKey, DateTime FinishDate)
        {
            StoreTokenData store = new StoreTokenData();
            store.StoreName = StoreName;
            store.StoreApi = StoreApi;
            store.SecretKey = SecretKey;
            store.FinishDate = FinishDate;
            _storeTokens.Add(store);
        }
        public List<StoreTokenData> GetStores()
        {
            return StoreTokens.Where(x => x.isActive).ToList();
        }
        public void AddStore(StoreTokenData store)
        {
            _storeTokens.Add(store);
        }
        public List<StoreTokenData> GetStoresWithUserUserPermission(string UserId)
        {
            var stores = StoreTokens.ToList();
            if (string.IsNullOrEmpty(UserId))
                return stores;
            else
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == UserId);
                if (user == null)
                    return stores;
                else
                {
                    var userStores = _context.UserStoreAuthorities.Where(x => x.User.Id == UserId).ToList();
                    foreach (var store in stores)
                    {
                        if (userStores.Any(x => x.Store.Name == store.StoreName && (x.Authority == UserAuthority.ReadWrite || x.Authority == UserAuthority.Read))){

                            store.UserHaveActivateAccess = true;
                            store.reconnectUrl = _codeGenerator.GenerateUrl(store.StoreApi).Url;
                        }
                    }
                    return stores;
                }
            }

        }
        public Task ChangeActiveStore(int StoreId)
        {
            _activationStore = StoreId;
            return Task.CompletedTask;
        }

        public TokenResponse GetToken(string Code){
            
            var TokenUrl = "https://api.etsy.com/v3/public/oauth/token";
            var dtt = JsonSerializer.Serialize(new TokenRequest(){
                GrantType = "authorization_code",
                ClientId = _context.Stores.FirstOrDefault(x=>x.Id == _activationStore).SotreApi,
                RedirectUri = Global.DomainName + "/EtsyAccess",
                Code = Code,
                CodeVerifier = _codeGenerator.codeVerifierModel.CodeVerifier
            });
            var client = new RestClient(TokenUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(dtt);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content);
                
                this._storeTokens.FirstOrDefault(x=>x.ID == _activationStore).Token = tokenResponse.AccessToken;
                this._storeTokens.FirstOrDefault(x=>x.ID == _activationStore).CreatedTime = DateTime.Now;
                this._storeTokens.FirstOrDefault(x=>x.ID == _activationStore).FinishDate = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

                return tokenResponse;
            }
            else
            {
                return null;
            }
        }
    }
}