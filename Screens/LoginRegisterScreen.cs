using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class LoginRegisterScreen
    {
        public static void Show()
        {
            ConsoleHelper.PrintHeader("LOGIN REGISTER (History)");

            var records = DataService.LoadLoginRecords();

            if (records.Count == 0)
            {
                ConsoleHelper.PrintWarning("No login records found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Total Login Records: {records.Count}\n");
            Console.ResetColor();

            ConsoleHelper.PrintTableHeader(
                ("Username", 20),
                ("Login Date & Time", 25)
            );

            // Show most recent first
            foreach (var record in records.OrderByDescending(r => r.LoginTime))
            {
                Console.WriteLine($"  {record}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  +----------------------+---------------------------+");
            Console.ResetColor();

            ConsoleHelper.WaitForKey();
        }
    }
}
