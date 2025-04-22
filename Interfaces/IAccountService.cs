using SoftwareProject.Data;

namespace SoftwareProject.Interfaces;

public interface IAccountService
{
    Task <LoginStatus> LoginAccount(string email, string password);
    Task CreateAccount(Account account);
}

public enum LoginStatus
{
    Success,
    Failure
}

public enum RegisterStatus
{
    Success,
    Failure
}