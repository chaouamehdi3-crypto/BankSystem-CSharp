using System.Text.Json.Serialization;

namespace GestionCompteBancaire.Models
{
    public class User
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("permissions")]
        public Permission Permissions { get; set; } = Permission.None;

        public bool HasPermission(Permission permission)
        {
            return (Permissions & permission) == permission;
        }

        public override string ToString()
        {
            return $"| {Username,-20} | {Permissions,-45} |";
        }
    }
}
