using System.Text.Json.Serialization;

namespace GestionCompteBancaire.Models
{
    public class Client
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return $"| {AccountNumber,-14} | {FullName,-22} | {Phone,-15} | {Email,-25} | {Balance,12:N2} |";
        }
    }
}
