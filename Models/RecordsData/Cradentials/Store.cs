namespace dotnetEtsyApp.Models.RecordsData.Cradentials
{
    public class Store:BaseEntity
    {
        public string Name { get; set; }
        public string StoreId { get; set; }
        public string StoreGrantedUserName { get; set; }
        public string StoreGrantedPassword { get; set; }
        public string SotreApi { get; set; }
        public string StoreApiSecret { get; set; }
        
    }
}