namespace GestionCompteBancaire.Services
{
    public static class CurrencyService
    {
        private static readonly Dictionary<string, decimal> RatesToMAD = new()
        {
            { "MAD", 1.00m },
            { "USD", 10.05m },   
            { "EUR", 10.85m },   
            { "GBP", 12.70m },   
            { "CAD", 7.35m },    
            { "CHF", 11.20m },   
            { "JPY", 0.067m },   
            { "TRY", 0.30m }    
        };

        public static IReadOnlyDictionary<string, decimal> GetAllRates() => RatesToMAD;

        public static decimal Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            fromCurrency = fromCurrency.ToUpper();
            toCurrency = toCurrency.ToUpper();

            if (!RatesToMAD.ContainsKey(fromCurrency))
                throw new ArgumentException($"Unknown currency: {fromCurrency}");
            if (!RatesToMAD.ContainsKey(toCurrency))
                throw new ArgumentException($"Unknown currency: {toCurrency}");

            decimal amountInMAD = amount * RatesToMAD[fromCurrency];
            decimal result = amountInMAD / RatesToMAD[toCurrency];

            return Math.Round(result, 4);
        }

        public static string[] GetCurrencyCodes() => RatesToMAD.Keys.ToArray();
    }
}
