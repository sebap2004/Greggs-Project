using SoftwareProject.Data;

namespace SoftwareProject.Interfaces;

public interface IAccountService
{
    Task <Account?> LoginAccount(string email, string password);
    Task CreateAccount(Account account);
}