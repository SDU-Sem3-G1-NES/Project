using DotNetEnv;
using System.Text;
using DataAccess;

namespace Models.Services;
public class UserCredentialsService
{
    private string fixedSalt { get; set; }

    private readonly UserRepository userRepository = new UserRepository();
    public UserCredentialsService()
    {
        string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envPath);

        string base64Salt = Env.GetString("BCRYPT_SALT");
        fixedSalt = Encoding.UTF8.GetString(Convert.FromBase64String(base64Salt));
    }

    public bool ValidateCredentials(string emailHash, string passwordHash)
    {

        string dbPasswordHash = userRepository.GetHashedPassword(emailHash) ?? string.Empty;

        if (dbPasswordHash != null && dbPasswordHash == passwordHash)
        {
            return true;
        }
        return false;
    }

    public string GetFixedSalt()
    {
        return fixedSalt;
    }
}