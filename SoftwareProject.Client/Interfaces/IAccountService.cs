using SoftwareProject.Data;

namespace SoftwareProject.Interfaces;

public interface IAccountService
{
    Task<Account?> LoginAccount(string email, string password);
    Task<CreateAccountResultDto> CreateAccount(Account account);
}

public struct CreateAccountResultDto
{
    public CreateAccountResultDto(RegisterStatus pStatus, int pAccountId)
    {
        status = pStatus;
        accountId = pAccountId;
    }
    
    public RegisterStatus status;
    public int accountId;
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