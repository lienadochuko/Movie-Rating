namespace Movie_Rating.Models.DTO
{
	public class UserDTO
	{
		public string Name { get; set; }
		public string Gender { get; set; }
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string NormalizedUserName { get; set; }
		public string Email { get; set; }
		public string NormalizedEmail { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public bool TwoFactorEnabled { get; set; }
	}
}
