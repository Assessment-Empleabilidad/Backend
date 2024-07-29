using BCryptNet = BCrypt.Net.BCrypt;

namespace Backend.Helper
{
    public class EncrypPass
    {
         public string HashPassword(string password)
        {
            //se genera el cifrado
            string hashedPassword = BCryptNet.HashPassword(password, BCryptNet.GenerateSalt());
            return hashedPassword;
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            //verificar si las contraseñas coinciden
            bool passwordMatch = BCryptNet.Verify(providedPassword, hashedPassword);
            return passwordMatch;
        }
    }
}