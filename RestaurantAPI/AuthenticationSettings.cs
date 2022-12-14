namespace RestaurantAPI
{
    internal class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpiryDays { get; set; }
        public string JwtIssuer { get; set; }
    }
}