namespace dotnetEtsyApp.Models.Cache
{
    public class StoreTokenData
    {
        public int ID { get; set; }
        public string StoreName { get; set; }
        public string StoreApi { get; set; }
        public string SecretKey { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime FinishDate { get; set; }
        public string DeactivationMessage { get; set; }
        private bool _isActive = false;
        public bool UserHaveActivateAccess { get; set; }
        public string reconnectUrl { get; set; } = "";
        public string Token { get; set; }
        public bool isActive
        {
            get
            {
                if(_isActive)
                    return true;

                
                if (FinishDate > DateTime.Now)
                {
                    _isActive = true;
                    return true;
                }
                else
                {
                    DeactivateToken("Token has expired");
                    return false;
                }
               
            }
        }
        public void DeactivateToken(string Message)
        {
            _isActive = false;
        }

    }
}