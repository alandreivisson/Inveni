using System.Security.Claims;

namespace Inveni.ViewModel {
    public class AuthenticationResult {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
