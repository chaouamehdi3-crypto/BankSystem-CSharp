using GestionCompteBancaire.Models;

namespace GestionCompteBancaire.Services
{
    public static class AuthService
    {
        public static User? CurrentUser { get; private set; }

        public static bool Login(string username, string password)
        {
            var users = DataService.LoadUsers();
            var user = users.Find(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
                DataService.AddLoginRecord(user.Username);
                return true;
            }

            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool HasPermission(Permission permission)
        {
            return CurrentUser?.HasPermission(permission) ?? false;
        }
    }
}
