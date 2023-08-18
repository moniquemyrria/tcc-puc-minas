namespace Kodigos.API.ModelView.ServicesUser
{
    public class UserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? lastAccessDate { get; set; }
        public string? language { get; set; }


    }

    public class UserNewPassModel
    {
        public string userName { get; set; }
        public string oldPass { get; set; }
        public string newPass { get; set; }
    }

    public class RefreshPassModel
    {
        public string userNameOrEmail { get; set; }
    }

    public class LoginModel
    {
        public string user { get; set; }
        public string pass { get; set; }
    }
}
