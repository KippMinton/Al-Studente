using System.Threading.Tasks;
using AlStudente.Auth.Models;

namespace AlStudente.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
        Task<FirebaseUser> RegisterStudent(StudentRegistration studentRegistration);
    }
}