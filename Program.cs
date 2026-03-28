using GestionCompteBancaire.Models;
using GestionCompteBancaire.Services;
using GestionCompteBancaire.Screens;

namespace GestionCompteBancaire
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bank Management System - Gestion du Compte Bancaire";

            // Ensure default admin user exists
            DataService.LoadUsers();

            // ─── Authentication Loop ────────────────────────────────
            while (true)
            {
                while (!LoginScreen.Show())
                {
                    // Keep showing login screen until successful
                }

                // ─── Main Application Loop ──────────────────────────
                ShowMainMenu();
            }
        }

        static void ShowMainMenu()
        {
            bool running = true;
            while (running)
            {
                ConsoleHelper.PrintHeader("MAIN SCREEN");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [1]  Clients List");
                Console.WriteLine("  [2]  Add New Client");
                Console.WriteLine("  [3]  Delete Client");
                Console.WriteLine("  [4]  Update Client");
                Console.WriteLine("  [5]  Find Client");
                Console.WriteLine("  [6]  Transactions");
                Console.WriteLine("  [7]  Manage Users");
                Console.WriteLine("  [8]  Login Register");
                Console.WriteLine("  [9]  Currency Exchange");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  [10] Logout");
                Console.ResetColor();
                Console.WriteLine();

                int choice;
                try
                {
                    choice = ConsoleHelper.ReadMenuChoice("Choose an option", 1, 10);
                }
                catch
                {
                    ConsoleHelper.PrintError("Invalid input. Please enter a number between 1 and 10.");
                    continue;
                }

                // ─── Permission Check & Routing ─────────────────────
                switch (choice)
                {
                    case 1:
                        if (CheckPermission(Permission.ClientsList))
                            ClientScreens.ShowClientsList();
                        break;

                    case 2:
                        if (CheckPermission(Permission.AddClient))
                            ClientScreens.ShowAddClient();
                        break;

                    case 3:
                        if (CheckPermission(Permission.DeleteClient))
                            ClientScreens.ShowDeleteClient();
                        break;

                    case 4:
                        if (CheckPermission(Permission.UpdateClient))
                            ClientScreens.ShowUpdateClient();
                        break;

                    case 5:
                        if (CheckPermission(Permission.FindClient))
                            ClientScreens.ShowFindClient();
                        break;

                    case 6:
                        if (CheckPermission(Permission.Transactions))
                            TransactionScreens.Show();
                        break;

                    case 7:
                        if (CheckPermission(Permission.ManageUsers))
                            UserScreens.Show();
                        break;

                    case 8:
                        if (CheckPermission(Permission.LoginRegister))
                            LoginRegisterScreen.Show();
                        break;

                    case 9:
                        if (CheckPermission(Permission.CurrencyExchange))
                            CurrencyExchangeScreen.Show();
                        break;

                    case 10:
                        if (ConsoleHelper.Confirm("Are you sure you want to logout?"))
                        {
                            AuthService.Logout();
                            running = false;
                        }
                        break;
                }
            }
        }

        static bool CheckPermission(Permission permission)
        {
            if (AuthService.HasPermission(permission))
                return true;

            ConsoleHelper.PrintAccessDenied();
            return false;
        }
    }
}
