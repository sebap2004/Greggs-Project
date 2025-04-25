using SoftwareProject.Data;

namespace SoftwareProject.Interfaces;

public interface IAccountService
{
    Task<Account?> LoginAccount(string email, string password);
    Task<RegisterStatus> CreateAccount(Account account);
}

public enum LoginStatus
{
    Success,
    Failure
}

public enum RegisterStatus
{
    Success,
    FailureAccountExists,
    FailureToCreateAccount,
    FailureDatabaseError,
    FailurePasswordsDontMatch
}