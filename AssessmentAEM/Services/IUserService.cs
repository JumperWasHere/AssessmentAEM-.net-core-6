using AssessmentAEM.Models;

namespace AssessmentAEM.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
