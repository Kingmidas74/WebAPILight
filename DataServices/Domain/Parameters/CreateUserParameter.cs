namespace Domain.Parameters {
    public class CreateUserParameter : UserParameter {
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
    }
}