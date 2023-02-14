namespace Mango.Services.Helpers
{
    public static class Constants
    {
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public enum APIType
        {
            GET, POST, PUT, DELETE
        }
    }
}
