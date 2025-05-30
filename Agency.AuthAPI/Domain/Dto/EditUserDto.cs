namespace Agency.AuthAPI.Domain.Dto
{
    public class EditUserDto
    {
        public Guid UserId { get; set; }
        public string NewName { get; set; }
        public string NewEmail { get; set; }
        public string NewPhone { get; set; }
    }
}
