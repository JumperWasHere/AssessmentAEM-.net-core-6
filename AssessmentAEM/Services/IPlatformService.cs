using AssessmentAEM.Models;

namespace AssessmentAEM.Services
{
    public interface IPlatformService
    {
        public Platform Create(Platform platform);
        public Platform Get(int id);
        public List<Platform> List();
        public Platform Update(Platform platform);
        public bool Delete(int id);
    }
}
