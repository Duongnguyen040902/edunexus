using Microsoft.AspNetCore.Authorization;

namespace sep.backend.v1.Requests.Policy
{
    public class SchoolRequirement : IAuthorizationRequirement
    {
        public SchoolRequirement() { }
    }

    public class ClassRequirement : IAuthorizationRequirement
    {
        public ClassRequirement() { }
    }

    public class ClubRequirement : IAuthorizationRequirement 
    {

        public ClubRequirement() { }
    }

    public class BusRequirement : IAuthorizationRequirement 
    {
        public BusRequirement() { }
    }

    public class MultiRoleRequirement : IAuthorizationRequirement
    {
        public MultiRoleRequirement() { }
    }

    public class ClassOfTeacherAndPupilRequirement : IAuthorizationRequirement
    {
        public ClassOfTeacherAndPupilRequirement() { }
    }

    public class SchoolYearRequirement : IAuthorizationRequirement
    {
        public SchoolYearRequirement() { }
    }
}
