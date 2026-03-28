using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class LoginScreen
    {
        public static bool Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ConsoleHelper.Divider);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
       ╔══════════════════════════════════════════╗
       ║     BANK MANAGEMENT SYSTEM               ║
       ║     Système de Gestion Bancaire           ║
       ╚══════════════════════════════════════════╝
            ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ConsoleHelper.Divider);
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  Please login to continue:\n");
            Console.ResetColor();

            string username = ConsoleHelper.ReadInput("Username");
            string password = ConsoleHelper.ReadInput("Password");

            if (AuthService.Login(username, password))
            {
                ConsoleHelper.PrintSuccess($"Welcome, {AuthService.CurrentUser!.Username}! Login successful.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey(true);
                return true;
            }
            else
            {
                ConsoleHelper.PrintError("Invalid username or password!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Press any key to try again...");
                Console.ResetColor();
                Console.ReadKey(true);
                return false;
            }
        }
    }
}
