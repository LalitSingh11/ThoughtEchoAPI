namespace ThoughtEcho.Models.ResponseModels
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
