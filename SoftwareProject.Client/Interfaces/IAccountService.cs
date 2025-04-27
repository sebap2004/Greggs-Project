using SoftwareProject.Data;

namespace SoftwareProject.Interfaces;

/// <summary>
/// Account service interface.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Logs into an account.
    /// </summary>
    /// <param name="email">Email to log in with</param>
    /// <param name="password">Password to log in with</param>
    /// <returns>Account that you logged in to</returns>
    Task<Account?> LoginAccount(string email, string password);
    
    /// <summary>
    /// Creates an account.
    /// </summary>
    /// <param name="account">Account to create</param>
    /// <returns>Created account data transfer object containing the account's data.</returns>
    Task<CreateAccountResultDto> CreateAccount(Account account);
}

/// <summary>
/// Struct that represents result of account creation.
/// </summary>
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

/// <summary>
/// Enum representing the status of a login attempt.
/// </summary>
public enum LoginStatus
{
    Success,
    Failure
}

/// <summary>
/// Enum representing status of a register attempt.
/// </summary>
public enum RegisterStatus
{
    Success,
    FailureAccountExists,
    FailureToCreateAccount,
    FailureDatabaseError,
    FailurePasswordsDontMatch
}