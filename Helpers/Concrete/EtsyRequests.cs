using dotnetEtsyApp.Models;
using dotnetEtsyApp.Models.RequestModels;

namespace dotnetEtsyApp.Helpers.Concrete
{
    public class EtsyRequests : IDisposable
    {
        
        private string _token;
        private readonly ApiEndPointsModel _apiEndPointsModel;
        public EtsyRequests(string token, ApiEndPointsModel apiEndPointsModel)
        {
            _token = token;
            _apiEndPointsModel = apiEndPointsModel;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
        public void GetShopId()
        {
            // get eyts shop id v3 
            
            
        }

       
    }
}