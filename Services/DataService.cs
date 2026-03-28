using System.Text.Json;
using GestionCompteBancaire.Models;

namespace GestionCompteBancaire.Services
{
    public static class DataService
    {
        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string ClientsFile = Path.Combine(DataDirectory, "clients.json");
        private static readonly string UsersFile = Path.Combine(DataDirectory, "users.json");
        private static readonly string LoginRecordsFile = Path.Combine(DataDirectory, "login_records.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        static DataService()
        {
            if (!Directory.Exists(DataDirectory))
                Directory.CreateDirectory(DataDirectory);
        }

        // ─── Clients ────────────────────────────────────────────────

        public static List<Client> LoadClients()
        {
            return LoadFromFile<Client>(ClientsFile);
        }

        public static void SaveClients(List<Client> clients)
        {
            SaveToFile(ClientsFile, clients);
        }

        // ─── Users ──────────────────────────────────────────────────

        public static List<User> LoadUsers()
        {
            var users = LoadFromFile<User>(UsersFile);

            // Seed a default admin user if no users exist
            if (users.Count == 0)
            {
                users.Add(new User
                {
                    Username = "admin",
                    Password = "admin",
                    Permissions = Permission.All
                });
                SaveToFile(UsersFile, users);
            }

            return users;
        }

        public static void SaveUsers(List<User> users)
        {
            SaveToFile(UsersFile, users);
        }

        // ─── Login Records ──────────────────────────────────────────

        public static List<LoginRecord> LoadLoginRecords()
        {
            return LoadFromFile<LoginRecord>(LoginRecordsFile);
        }

        public static void SaveLoginRecords(List<LoginRecord> records)
        {
            SaveToFile(LoginRecordsFile, records);
        }

        public static void AddLoginRecord(string username)
        {
            var records = LoadLoginRecords();
            records.Add(new LoginRecord
            {
                Username = username,
                LoginTime = DateTime.Now
            });
            SaveLoginRecords(records);
        }

        // ─── Generic Helpers ────────────────────────────────────────

        private static List<T> LoadFromFile<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<T>();

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        private static void SaveToFile<T>(string filePath, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, JsonOptions);
            File.WriteAllText(filePath, json);
        }
    }
}
