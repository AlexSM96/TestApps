namespace BankAccount;

internal class Bank
{
    private IList<Account> _accounts;

    public Bank()
    {
        _accounts = [];    
    }

    public void CreateAccount(long accountId, string owner, decimal ballance = 0)
    {
        if(_accounts is null)
        {
            throw new ArgumentNullException("Не инициализированы счета пользователей");
        }

        if(_accounts.Any(x => x.AccountId == accountId))
        {
            throw new InvalidOperationException($"Счет с ID = {accountId} уже существует");
        }

        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new ArgumentNullException("Счёт должен иметь владельца");
        }

        _accounts.Add(new Account(accountId, owner, ballance));
    }

    public Account? GetAccount(long accountId)
    {
        if (_accounts is null)
        {
            throw new ArgumentNullException("Не инициализированы счета пользователей");
        }

        return _accounts.FirstOrDefault(x => x.AccountId == accountId);
    }

    public IEnumerable<Account> GetAccounts()
    {
        return new List<Account>(_accounts);
    }

    public string GetStringAccounts()
    {
        return string.Join("\n", GetAccounts());
    }
}
