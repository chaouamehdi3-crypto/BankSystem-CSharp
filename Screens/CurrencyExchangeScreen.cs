using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class CurrencyExchangeScreen
    {
        public static void Show()
        {
            bool back = false;
            while (!back)
            {
                ConsoleHelper.PrintHeader("CURRENCY EXCHANGE");

                // Show available rates
                var rates = CurrencyService.GetAllRates();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  Exchange Rates (Base: MAD):\n");
                Console.ResetColor();

                ConsoleHelper.PrintTableHeader(
                    ("Currency", 12),
                    ("Rate to MAD", 15)
                );

                foreach (var rate in rates)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"  | {rate.Key,-12} | {rate.Value,-15:N4} |");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  +--------------+-----------------+");
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [1] Convert Currency");
                Console.WriteLine("  [2] Back to Main Menu");
                Console.ResetColor();
                Console.WriteLine();

                int choice = ConsoleHelper.ReadMenuChoice("Choose an option", 1, 2);

                switch (choice)
                {
                    case 1: ShowConvert(); break;
                    case 2: back = true; break;
                }
            }
        }

        private static void ShowConvert()
        {
            Console.WriteLine();

            string[] codes = CurrencyService.GetCurrencyCodes();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  Available: {string.Join(", ", codes)}");
            Console.ResetColor();
            Console.WriteLine();

            string from = ConsoleHelper.ReadInput("From Currency (e.g., USD)").ToUpper();
            string to = ConsoleHelper.ReadInput("To Currency (e.g., EUR)").ToUpper();

            if (!codes.Contains(from) || !codes.Contains(to))
            {
                ConsoleHelper.PrintError("Invalid currency code. Please use one of the listed codes.");
                ConsoleHelper.WaitForKey();
                return;
            }

            decimal amount = ConsoleHelper.ReadDecimal("Amount to convert");

            try
            {
                decimal result = CurrencyService.Convert(from, to, amount);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ╔══════════════════════════════════════════╗");
                Console.WriteLine($"  ║  {amount:N2} {from}  =  {result:N4} {to,-8}       ║");
                Console.WriteLine("  ╚══════════════════════════════════════════╝");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.WaitForKey();
        }
    }
}
