using GestionCompteBancaire.Models;
using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class ClientScreens
    {
        // ─── [1] Clients List ───────────────────────────────────────

        public static void ShowClientsList()
        {
            ConsoleHelper.PrintHeader("CLIENTS LIST");

            var clients = DataService.LoadClients();

            if (clients.Count == 0)
            {
                ConsoleHelper.PrintWarning("No clients found in the system.");
                ConsoleHelper.WaitForKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Total Clients: {clients.Count}\n");
            Console.ResetColor();

            ConsoleHelper.PrintTableHeader(
                ("Account #", 14),
                ("Full Name", 22),
                ("Phone", 15),
                ("Email", 25),
                ("Balance", 12)
            );

            foreach (var client in clients)
            {
                Console.WriteLine($"  {client}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  +----------------+------------------------+-----------------+---------------------------+--------------+");
            Console.ResetColor();

            ConsoleHelper.WaitForKey();
        }

        // ─── [2] Add New Client ─────────────────────────────────────

        public static void ShowAddClient()
        {
            ConsoleHelper.PrintHeader("ADD NEW CLIENT");

            var clients = DataService.LoadClients();

            string accountNumber;
            while (true)
            {
                accountNumber = ConsoleHelper.ReadInput("Account Number (unique)");
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    ConsoleHelper.PrintError("Account number cannot be empty.");
                    continue;
                }
                if (clients.Any(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase)))
                {
                    ConsoleHelper.PrintError($"Account number '{accountNumber}' already exists! Please enter a unique one.");
                    continue;
                }
                break;
            }

            string firstName = ConsoleHelper.ReadInput("First Name");
            string lastName = ConsoleHelper.ReadInput("Last Name");
            string phone = ConsoleHelper.ReadInput("Phone");
            string email = ConsoleHelper.ReadInput("Email");
            decimal balance = ConsoleHelper.ReadDecimal("Initial Balance");

            var newClient = new Client
            {
                AccountNumber = accountNumber,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email,
                Balance = balance
            };

            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Account #:  {newClient.AccountNumber}");
            ConsoleHelper.PrintInfo($"Name:       {newClient.FullName}");
            ConsoleHelper.PrintInfo($"Phone:      {newClient.Phone}");
            ConsoleHelper.PrintInfo($"Email:      {newClient.Email}");
            ConsoleHelper.PrintInfo($"Balance:    {newClient.Balance:N2}");

            if (ConsoleHelper.Confirm("Save this client?"))
            {
                clients.Add(newClient);
                DataService.SaveClients(clients);
                ConsoleHelper.PrintSuccess("Client added successfully!");
            }
            else
            {
                ConsoleHelper.PrintWarning("Operation cancelled.");
            }

            ConsoleHelper.WaitForKey();
        }

        // ─── [3] Delete Client ──────────────────────────────────────

        public static void ShowDeleteClient()
        {
            ConsoleHelper.PrintHeader("DELETE CLIENT");

            var clients = DataService.LoadClients();
            string accountNumber = ConsoleHelper.ReadInput("Enter Account Number to delete");

            var client = clients.Find(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));

            if (client == null)
            {
                ConsoleHelper.PrintError($"Client with account '{accountNumber}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            ConsoleHelper.PrintInfo($"Client Found: {client.FullName} (Balance: {client.Balance:N2})");

            if (ConsoleHelper.Confirm($"Are you sure you want to delete '{client.FullName}'?"))
            {
                clients.Remove(client);
                DataService.SaveClients(clients);
                ConsoleHelper.PrintSuccess("Client deleted successfully!");
            }
            else
            {
                ConsoleHelper.PrintWarning("Deletion cancelled.");
            }

            ConsoleHelper.WaitForKey();
        }

        // ─── [4] Update Client ──────────────────────────────────────

        public static void ShowUpdateClient()
        {
            ConsoleHelper.PrintHeader("UPDATE CLIENT");

            var clients = DataService.LoadClients();
            string accountNumber = ConsoleHelper.ReadInput("Enter Account Number to update");

            var client = clients.Find(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));

            if (client == null)
            {
                ConsoleHelper.PrintError($"Client with account '{accountNumber}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            Console.WriteLine();
            ConsoleHelper.PrintInfo("Current Information:");
            ConsoleHelper.PrintInfo($"  First Name: {client.FirstName}");
            ConsoleHelper.PrintInfo($"  Last Name:  {client.LastName}");
            ConsoleHelper.PrintInfo($"  Phone:      {client.Phone}");
            ConsoleHelper.PrintInfo($"  Email:      {client.Email}");
            Console.WriteLine();

            ConsoleHelper.PrintInfo("Enter new values (leave blank to keep current):\n");

            string firstName = ConsoleHelper.ReadInput($"First Name [{client.FirstName}]");
            string lastName = ConsoleHelper.ReadInput($"Last Name [{client.LastName}]");
            string phone = ConsoleHelper.ReadInput($"Phone [{client.Phone}]");
            string email = ConsoleHelper.ReadInput($"Email [{client.Email}]");

            if (!string.IsNullOrWhiteSpace(firstName)) client.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) client.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phone)) client.Phone = phone;
            if (!string.IsNullOrWhiteSpace(email)) client.Email = email;

            DataService.SaveClients(clients);
            ConsoleHelper.PrintSuccess("Client updated successfully!");

            ConsoleHelper.WaitForKey();
        }

        // ─── [5] Find Client ────────────────────────────────────────

        public static void ShowFindClient()
        {
            ConsoleHelper.PrintHeader("FIND CLIENT");

            string accountNumber = ConsoleHelper.ReadInput("Enter Account Number to search");

            var clients = DataService.LoadClients();
            var client = clients.Find(c => c.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));

            if (client == null)
            {
                ConsoleHelper.PrintError($"Client with account '{accountNumber}' not found.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  ╔═══════════════════════════════════════╗");
                Console.WriteLine("  ║         CLIENT DETAILS                ║");
                Console.WriteLine("  ╠═══════════════════════════════════════╣");
                Console.ResetColor();
                ConsoleHelper.PrintInfo($"  ║ Account #:  {client.AccountNumber,-24} ║");
                ConsoleHelper.PrintInfo($"  ║ Name:       {client.FullName,-24} ║");
                ConsoleHelper.PrintInfo($"  ║ Phone:      {client.Phone,-24} ║");
                ConsoleHelper.PrintInfo($"  ║ Email:      {client.Email,-24} ║");
                ConsoleHelper.PrintInfo($"  ║ Balance:    {client.Balance,-24:N2} ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ╚═══════════════════════════════════════╝");
                Console.ResetColor();
            }

            ConsoleHelper.WaitForKey();
        }
    }
}
