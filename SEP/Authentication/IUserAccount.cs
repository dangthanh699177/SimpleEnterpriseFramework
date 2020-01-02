namespace SEP.Authentication
{
    public interface IUserAccount
    {
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string Username { get; set; }
    }
}