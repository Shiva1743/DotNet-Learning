namespace CoreEmptyProject1.ViewModels
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaims> Claims { get; set; } = new List<UserClaims>();
    }
}
