using GestionCompteBancaire.Models;
using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class ConsoleHelper
    {
        public const string Divider = "══════════════════════════════════════════════════════════════════════════";
        public const string ThinDivider = "──────────────────────────────────────────────────────────────────────────";

        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Divider);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ★  {title}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Divider);
            Console.ResetColor();

            if (AuthService.CurrentUser != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  User: {AuthService.CurrentUser.Username}  |  Date: {DateTime.Now:yyyy-MM-dd HH:mm}");
                Console.WriteLine(ThinDivider);
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✓ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ✗ {message}");
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n  ⚠ {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {message}");
            Console.ResetColor();
        }

        public static void PrintAccessDenied()
        {
            PrintHeader("ACCESS DENIED");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ╔═══════════════════════════════════════════════╗");
            Console.WriteLine("  ║   You do not have permission to access       ║");
            Console.WriteLine("  ║   this feature. Contact your administrator.  ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════╝");
            Console.ResetColor();
            WaitForKey();
        }

        public static void WaitForKey()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Press any key to go back to Main Menu...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static string ReadInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  {prompt}: ");
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                string input = ReadInput(prompt);
                if (decimal.TryParse(input, out decimal value) && value >= 0)
                    return value;
                PrintError("Invalid amount. Please enter a valid positive number.");
            }
        }

        public static int ReadMenuChoice(string prompt, int min, int max)
        {
            while (true)
            {
                string input = ReadInput(prompt);
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                    return choice;
                PrintError($"Invalid choice. Please enter a number between {min} and {max}.");
            }
        }

        public static bool Confirm(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n  {message} (y/n): ");
            Console.ResetColor();
            var key = Console.ReadKey(false);
            Console.WriteLine();
            return key.KeyChar == 'y' || key.KeyChar == 'Y';
        }

        public static void PrintTableHeader(params (string Title, int Width)[] columns)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string header = "| ";
            string separator = "+-";
            foreach (var col in columns)
            {
                header += col.Title.PadRight(col.Width) + " | ";
                separator += new string('-', col.Width) + "-+-";
            }
            Console.WriteLine("  " + separator.TrimEnd('+', '-') + "+");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + header.TrimEnd());
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  " + separator.TrimEnd('+', '-') + "+");
            Console.ResetColor();
        }
    }
}
