using GestionCompteBancaire.Models;
using GestionCompteBancaire.Services;

namespace GestionCompteBancaire.Screens
{
    public static class UserScreens
    {
        public static void Show()
        {
            bool back = false;
            while (!back)
            {
                ConsoleHelper.PrintHeader("MANAGE USERS");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [1] Users List");
                Console.WriteLine("  [2] Add New User");
                Console.WriteLine("  [3] Delete User");
                Console.WriteLine("  [4] Update User");
                Console.WriteLine("  [5] Assign Permissions");
                Console.WriteLine("  [6] Back to Main Menu");
                Console.ResetColor();
                Console.WriteLine();

                int choice = ConsoleHelper.ReadMenuChoice("Choose an option", 1, 6);

                switch (choice)
                {
                    case 1: ShowUsersList(); break;
                    case 2: ShowAddUser(); break;
                    case 3: ShowDeleteUser(); break;
                    case 4: ShowUpdateUser(); break;
                    case 5: ShowAssignPermissions(); break;
                    case 6: back = true; break;
                }
            }
        }

        private static void ShowUsersList()
        {
            ConsoleHelper.PrintHeader("USERS LIST");

            var users = DataService.LoadUsers();

            ConsoleHelper.PrintTableHeader(
                ("Username", 20),
                ("Permissions", 45)
            );

            foreach (var user in users)
            {
                Console.WriteLine($"  {user}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  +----------------------+-----------------------------------------------+");
            Console.ResetColor();

            ConsoleHelper.WaitForKey();
        }

        private static void ShowAddUser()
        {
            ConsoleHelper.PrintHeader("ADD NEW USER");

            var users = DataService.LoadUsers();

            string username;
            while (true)
            {
                username = ConsoleHelper.ReadInput("Username");
                if (string.IsNullOrWhiteSpace(username))
                {
                    ConsoleHelper.PrintError("Username cannot be empty.");
                    continue;
                }
                if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    ConsoleHelper.PrintError($"Username '{username}' already exists!");
                    continue;
                }
                break;
            }

            string password = ConsoleHelper.ReadInput("Password");

            var newUser = new User
            {
                Username = username,
                Password = password,
                Permissions = Permission.None
            };

            // Assign initial permissions
            Console.WriteLine();
            ConsoleHelper.PrintInfo("Assign permissions for this user:");
            newUser.Permissions = PromptPermissions();

            users.Add(newUser);
            DataService.SaveUsers(users);
            ConsoleHelper.PrintSuccess($"User '{username}' added successfully!");

            ConsoleHelper.WaitForKey();
        }

        private static void ShowDeleteUser()
        {
            ConsoleHelper.PrintHeader("DELETE USER");

            var users = DataService.LoadUsers();
            string username = ConsoleHelper.ReadInput("Enter Username to delete");

            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleHelper.PrintError("Cannot delete the admin user!");
                ConsoleHelper.WaitForKey();
                return;
            }

            var user = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                ConsoleHelper.PrintError($"User '{username}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            if (ConsoleHelper.Confirm($"Are you sure you want to delete user '{user.Username}'?"))
            {
                users.Remove(user);
                DataService.SaveUsers(users);
                ConsoleHelper.PrintSuccess("User deleted successfully!");
            }
            else
            {
                ConsoleHelper.PrintWarning("Deletion cancelled.");
            }

            ConsoleHelper.WaitForKey();
        }

        private static void ShowUpdateUser()
        {
            ConsoleHelper.PrintHeader("UPDATE USER");

            var users = DataService.LoadUsers();
            string username = ConsoleHelper.ReadInput("Enter Username to update");

            var user = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                ConsoleHelper.PrintError($"User '{username}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            ConsoleHelper.PrintInfo($"Current Username: {user.Username}");
            Console.WriteLine();

            string newPassword = ConsoleHelper.ReadInput($"New Password (leave blank to keep current)");

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                user.Password = newPassword;
            }

            DataService.SaveUsers(users);
            ConsoleHelper.PrintSuccess("User updated successfully!");

            ConsoleHelper.WaitForKey();
        }

        private static void ShowAssignPermissions()
        {
            ConsoleHelper.PrintHeader("ASSIGN PERMISSIONS");

            var users = DataService.LoadUsers();
            string username = ConsoleHelper.ReadInput("Enter Username");

            var user = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                ConsoleHelper.PrintError($"User '{username}' not found.");
                ConsoleHelper.WaitForKey();
                return;
            }

            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Current Permissions: {user.Permissions}");
            Console.WriteLine();

            ConsoleHelper.PrintInfo("Set new permissions:");
            user.Permissions = PromptPermissions();

            DataService.SaveUsers(users);
            ConsoleHelper.PrintSuccess($"Permissions for '{user.Username}' updated to: {user.Permissions}");

            ConsoleHelper.WaitForKey();
        }

        private static Permission PromptPermissions()
        {
            Permission result = Permission.None;

            var permissionList = new (Permission Flag, string Label)[]
            {
                (Permission.ClientsList,      "[1] Clients List"),
                (Permission.AddClient,        "[2] Add New Client"),
                (Permission.DeleteClient,     "[3] Delete Client"),
                (Permission.UpdateClient,     "[4] Update Client"),
                (Permission.FindClient,       "[5] Find Client"),
                (Permission.Transactions,     "[6] Transactions"),
                (Permission.ManageUsers,      "[7] Manage Users"),
                (Permission.LoginRegister,    "[8] Login Register"),
                (Permission.CurrencyExchange, "[9] Currency Exchange"),
            };

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n  For each permission, enter 'y' to grant or 'n' to deny:\n");
            Console.ResetColor();

            foreach (var (flag, label) in permissionList)
            {
                Console.Write($"  {label} (y/n): ");
                var key = Console.ReadKey(false);
                Console.WriteLine();

                if (key.KeyChar == 'y' || key.KeyChar == 'Y')
                    result |= flag;
            }

            return result;
        }
    }
}
