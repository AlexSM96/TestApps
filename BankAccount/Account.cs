namespace BankAccount;

internal class Account
{
    private readonly IList<string> _transactionHistory;
    public Account(long accountId, string accountOwner, decimal balance = 0)
    {
        _transactionHistory = [];
        AccountId = accountId;
        AccountOwner = accountOwner;
        Balance = balance;
        AddTransaction($"Создан аккаунт {accountId} с начальным балансом: {balance} руб.");
    }

    public long AccountId { get; }
    public string AccountOwner { get; private set; }
    public decimal Balance { get; private set; }


    public void Deposite(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма вносимая на счет должна быть больше 0");
        }

        Balance += amount;
        AddTransaction($"На счет внесена сумма: {amount} руб.");
    }

    public bool TryWithdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сумма снимаемая со счета должна быть больше 0");
        }

        if (amount > Balance)
        {
            return false;
        }

        Balance -= amount;
        AddTransaction($"Со счета {AccountId} снята сумма {amount}");
        return true;
    }

    public void Transfer(Account target, decimal amount)
    {
        if (!TryWithdraw(amount))
        {
            throw new InvalidOperationException("Не достаточно средст на счёте");
        }

        if (target is null)
        {
            throw new ArgumentNullException($"Не назначен счёт для отправки денежных средств");
        }

        if (target.AccountId == AccountId)
        {
            throw new InvalidOperationException("Не возможно сделать превод на тотже счёт");
        }

        target.Deposite(amount);
        AddTransaction($"Выполнен первод cо счета {AccountId} на счет {target.AccountId} на сумму {amount} руб.");
    }


    public IEnumerable<string> GetTransactionHistory()
    {
        var transactionHistory = new List<string>(_transactionHistory);
        return transactionHistory;
    }

    public override string ToString()
    {
        return $"[{AccountId}] {AccountOwner} - {Balance} руб.";
    }

    private void AddTransaction(string info)
    {
        if (_transactionHistory is null)
        {
            throw new Exception($"Не иницилизирована история транзакций для аккунта {AccountId}");
        }

        _transactionHistory.Add($"{DateTime.Now} : {info}");
    }
}
