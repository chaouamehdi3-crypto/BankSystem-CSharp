using System.Text.Json.Serialization;

namespace GestionCompteBancaire.Models
{
    public class LoginRecord
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("loginTime")]
        public DateTime LoginTime { get; set; }

        public override string ToString()
        {
            return $"| {Username,-20} | {LoginTime,-25:yyyy-MM-dd HH:mm:ss} |";
        }
    }
}
