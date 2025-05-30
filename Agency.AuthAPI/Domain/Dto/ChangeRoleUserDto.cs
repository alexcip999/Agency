namespace Agency.AuthAPI.Domain.Dto
{
    public class ChangeRoleUserDto
    {
        public Guid UserId { get; set; }
        public string NewRole { get; set; }
    }
}
