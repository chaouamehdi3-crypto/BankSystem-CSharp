using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class TransactionScreens
    {
        public static void Show()
        {
            bool back = false;
            while (!back)
            {
                ConsoleHelper.PrintHeader("TRANSACTIONS");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [1] Deposit");
                Console.WriteLine("  [2] Withdraw");
                Console.WriteLine("  [3] Back to Main Menu");
                Console.ResetColor();
                Console.WriteLine();

                int choice = ConsoleHelper.ReadMenuChoice("Choose an option", 1, 3);

                switch (choice)
                {
                    case 1: ShowDeposit(); break;
                    case 2: ShowWithdraw(); break;
                    case 3: back = true; break;
                }
            }
        }

        private static void ShowDeposit()
        {
            ConsoleHelper.PrintHeader("DEPOSIT");

            var clients = DataService.LoadClients();
            string accountNumber = ConsoleHelper.ReadInput("Enter Account Number");

            var client = clients.Find(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));

            if (client == null)
            {
                ConsoleHelper.PrintError($"Client with account '{accountNumber}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            ConsoleHelper.PrintInfo($"Client: {client.FullName}  |  Current Balance: {client.Balance:N2}");

            decimal amount = ConsoleHelper.ReadDecimal("Enter Deposit Amount");

            if (amount <= 0)
            {
                ConsoleHelper.PrintError("Deposit amount must be greater than zero.");
                ConsoleHelper.WaitForKey();
                return;
            }

            if (ConsoleHelper.Confirm($"Deposit {amount:N2} into {client.FullName}'s account?"))
            {
                client.Balance += amount;
                DataService.SaveClients(clients);
                ConsoleHelper.PrintSuccess($"Deposited {amount:N2}. New Balance: {client.Balance:N2}");
            }
            else
            {
                ConsoleHelper.PrintWarning("Deposit cancelled.");
            }

            ConsoleHelper.WaitForKey();
        }

        private static void ShowWithdraw()
        {
            ConsoleHelper.PrintHeader("WITHDRAW");

            var clients = DataService.LoadClients();
            string accountNumber = ConsoleHelper.ReadInput("Enter Account Number");

            var client = clients.Find(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));

            if (client == null)
            {
                ConsoleHelper.PrintError($"Client with account '{accountNumber}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            ConsoleHelper.PrintInfo($"Client: {client.FullName}  |  Current Balance: {client.Balance:N2}");

            decimal amount = ConsoleHelper.ReadDecimal("Enter Withdrawal Amount");

            if (amount <= 0)
            {
                ConsoleHelper.PrintError("Withdrawal amount must be greater than zero.");
                ConsoleHelper.WaitForKey();
                return;
            }

            if (amount > client.Balance)
            {
                ConsoleHelper.PrintError($"Insufficient balance! Available: {client.Balance:N2}, Requested: {amount:N2}");
                ConsoleHelper.WaitForKey();
                return;
            }

            if (ConsoleHelper.Confirm($"Withdraw {amount:N2} from {client.FullName}'s account?"))
            {
                client.Balance -= amount;
                DataService.SaveClients(clients);
                ConsoleHelper.PrintSuccess($"Withdrawn {amount:N2}. New Balance: {client.Balance:N2}");
            }
            else
            {
                ConsoleHelper.PrintWarning("Withdrawal cancelled.");
            }

            ConsoleHelper.WaitForKey();
        }
    }
}
