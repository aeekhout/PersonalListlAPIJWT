using PersonalListlAPIJWT.Models;

namespace PersonalListlAPIJWT.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { 
                Username = "alex", 
                Password = "1234", 
                Name = "Alexander Eekhout", 
                Email = "aeekhout@gmail.com", 
                Role = "Administrator" 
            },
            new()
            {
                Username = "alba",
                Password = "4321",
                Name = "Albanellys Pereira",
                Email = "albanellyspereira@gmail.com",
                Role = "Standard"
            }
        };
    }
}
