namespace PMFWikipedia.ImplementationsBL.Helpers
{
    public class PasswordService
    {
        public static string HashPass(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public static bool VerifyPassword(string password, string attempt)
        {
            return BCrypt.Net.BCrypt.Verify(attempt, password);
        }
    }
}