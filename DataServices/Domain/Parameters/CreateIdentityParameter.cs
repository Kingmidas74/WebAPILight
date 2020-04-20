using Domain.Interfaces;

namespace Domain.Parameters {
    public class CreateIdentityParameter : ICommandParameter {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}