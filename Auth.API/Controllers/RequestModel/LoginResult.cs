using System.Collections.Generic;

namespace Auth.Controllers.RequestModel
{
    public class LoginResult
    {
        public string Login { get; set; }

        public string AccessToken { get; set; }

        public IList<string>? Roles { get; set; }
    }
}