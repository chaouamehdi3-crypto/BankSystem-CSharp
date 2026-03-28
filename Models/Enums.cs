namespace GestionCompteBancaire.Models
{
    [Flags]
    public enum Permission
    {
        None             = 0,
        ClientsList      = 1 << 0,   // 1
        AddClient        = 1 << 1,   // 2
        DeleteClient     = 1 << 2,   // 4
        UpdateClient     = 1 << 3,   // 8
        FindClient       = 1 << 4,   // 16
        Transactions     = 1 << 5,   // 32
        ManageUsers      = 1 << 6,   // 64
        LoginRegister    = 1 << 7,   // 128
        CurrencyExchange = 1 << 8,   // 256
        All = ClientsList | AddClient | DeleteClient | UpdateClient |
              FindClient | Transactions | ManageUsers | LoginRegister | CurrencyExchange
    }
}
