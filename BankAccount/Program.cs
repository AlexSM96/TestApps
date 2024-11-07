using BankAccount;

var bank = new Bank();
while (true)
{
    Console.Clear();
    ShowBaseMenuOperations();
    if (int.TryParse(Console.ReadLine(), out int operation))
    {
        switch (operation)
        {
            case 1:
                CreateNewAccount(bank);
                break;
            case 2:
                ShowAllAccountsInfo(bank);
                if (!bank.GetAccounts().Any())
                {
                    ShowFooter("Счета не найдены", ConsoleColor.Red);
                    continue;
                }

                Console.WriteLine("Введите номер счёта: ");
                if (!TryFindAccount(bank, out var account) || account is null)
                {
                    continue;
                }

                ShowMenuForWorkWithAccount(bank, account);
                break;
            case 3:
                ShowAllAccountsInfoWithFooter(bank);
                break;
            default:
                ShowFooter("Введена не корректная команда", ConsoleColor.Red);
                break;
        }
    }
}

#region WorkWithAccount
static void CreateNewAccount(Bank bank)
{
    while (true)
    {
        Console.WriteLine("Введите ФИО владельца счета:");
        string? owner = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(owner))
        {
            Console.Clear();
            continue;
        }

        Console.WriteLine("Введите стартовый баланс:");
        decimal startBalance = decimal.TryParse(Console.ReadLine(), out var result) ? result : 0;
        bank.CreateAccount(Random.Shared.Next(100000), owner, startBalance);
        ShowAllAccountsInfo(bank);
        ShowFooter("Создан новый счет");
        break;
    }
}

static void ShowMenuForWorkWithAccount(Bank bank, Account account)
{
    bool notNeedReturnBack = true;
    while (notNeedReturnBack)
    {
        ShowAccountMenuOperations(account);
        if (int.TryParse(Console.ReadLine(), out int opertation))
        {
            switch (opertation)
            {
                case 1: Transfer(bank, account); break;
                case 2: Deposit(account); break;
                case 3: Withdraw(account); break;
                case 4: ShowTransactionHistory(account); break;
                case 5:
                    notNeedReturnBack = false;
                    break;
                default:
                    ShowFooter("Введена не корректная команда", ConsoleColor.Red);
                    break;
            }
        }
    }
}

static void ShowTransactionHistory(Account account)
{
    var transactionHistory = account.GetTransactionHistory();
    if (transactionHistory is null || !transactionHistory.Any())
    {
        ShowFooter("История пуста", ConsoleColor.Red);
        return;
    }

    Console.WriteLine(string.Join("\n", transactionHistory));
    ShowFooter();
}

static void Withdraw(Account account)
{
    while (true)
    {
        if (account is null)
        {
            Console.WriteLine("Аккаунт не найден", ConsoleColor.Red);
            continue;
        }

        Console.WriteLine("Введите сумму которую необходимо снять: ");
        decimal amount = decimal.TryParse(Console.ReadLine(), out var result) ? result : -1;
        if (!account.TryWithdraw(amount))
        {
            ShowFooter("Сумма должна быть больше 0 и меньше текущего баланса", ConsoleColor.Red);
            continue;
        }

        ShowFooter($"Деньги сняты - текущий баланс {account.Balance} руб.");
        break;
    }
}

static void Deposit(Account account)
{
    while (true)
    {
        if (account is null)
        {
            ShowFooter("Аккаунт не найден", ConsoleColor.Red);
            continue;
        }

        Console.WriteLine("Введите сумму которую необходимо положить: ");
        decimal amount = decimal.TryParse(Console.ReadLine(), out var result) ? result : -1;
        if (amount <= 0)
        {
            ShowFooter("Сумма должна быть больше 0 и меньше текущего баланса", ConsoleColor.Red);
            continue;
        }

        account.Deposite(amount);
        ShowFooter($"Деньги зачислены - текущий баланс {account.Balance} руб.");
        break;
    }
}

static void Transfer(Bank bank, Account sourceAccount)
{
    while (true)
    {
        var accounts = bank.GetAccounts();
        if (accounts is null || accounts.Count() < 2)
        {
            ShowFooter("Отсутсвует счёт на который можно сделать превод", ConsoleColor.Red);
            break;
        }

        if(sourceAccount.Balance <= 0)
        {
            ShowFooter($"Выполнить перевод невозможно балланс - {sourceAccount.Balance} руб.", ConsoleColor.Red);
            break;
        }

        Console.WriteLine("Счета: ");
        Console.WriteLine(bank.GetStringAccounts());
        Console.WriteLine();
        Console.WriteLine("Выберете счет на который необходимо сделать превод: ");
        if(!TryFindAccount(bank, out var targetAccount) || targetAccount is null)
        {
            continue;
        }

        if(sourceAccount.AccountId == targetAccount.AccountId)
        {
            ShowFooter("Не возможно сделать превод на тотже счёт", ConsoleColor.Red);
            continue;
        }

        Console.WriteLine("Введите сумму перевода: ");
        decimal amount = decimal.TryParse(Console.ReadLine(), out decimal value) ? value : -1;
        if (amount <= 0 || amount > sourceAccount.Balance)
        {
            ShowFooter("Сумма должна быть больше 0 и меньше либо равна текущему балансу", ConsoleColor.Red);
            continue;
        }

        sourceAccount.Transfer(targetAccount, amount);
        ShowFooter("Перевод  выполнен");
        break;
    }
}

static void ShowAllAccountsInfo(Bank bank)
{
    var accounts = bank.GetAccounts();
    if (accounts is null || !accounts.Any())
    {
        ShowFooter("Нет счетов", ConsoleColor.Red);
        return;
    }

    Console.WriteLine("Счета: ");
    Console.WriteLine(bank.GetStringAccounts());   
}

static void ShowAllAccountsInfoWithFooter(Bank bank)
{
    ShowAllAccountsInfo(bank);
    ShowFooter();
}

static bool TryFindAccount(Bank bank, out Account? account)
{
    long accountId = long.TryParse(Console.ReadLine(), out long id) ? id : -1;
    account = bank.GetAccount(accountId);
    if (account is null)
    {
        ShowFooter("Счёт не найден", ConsoleColor.Red);
        return false;
    }

    return true;
}

#endregion

#region ViewMenu
static void ShowBaseMenuOperations()
{
    string operations = "1. Создать счёт\n2. Выбрать счёт\n3. Информация по счетам";
    Console.Clear();
    Console.WriteLine("Добро пожаловать в личный кабинет!");
    Console.WriteLine(); 
    Console.WriteLine(operations);
    Console.WriteLine("Выберете опрецию: ");
}

static void ShowAccountMenuOperations(Account account)
{
    Console.Clear();
    Console.WriteLine($"Текущий счет: {account.ToString()}");
    Console.WriteLine();
    string operations = "1. Сделать перевод\n2. Положить на счет\n3. Снять со счета\n4. История транзакций\n5. Вернуться в начальное меню";
    Console.WriteLine(operations);
    Console.WriteLine("Выберете опрецию: ");
}

static void ShowFooter(string operationResult = "", ConsoleColor consoleColor = ConsoleColor.Green)
{
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(operationResult);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Нажмите любую клавишу для выхода в начальное меню");
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
}
#endregion






