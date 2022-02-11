using PersonalListlAPIJWT.Models;

namespace PersonalListlAPIJWT.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
