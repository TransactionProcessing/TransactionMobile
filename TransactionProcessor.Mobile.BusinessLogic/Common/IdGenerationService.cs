namespace TransactionProcessor.Mobile.BusinessLogic.Common
{
    public static class CorrelationIdProvider
    {
        private static readonly AsyncLocal<string?> _correlationId = new();

        public static string CorrelationId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_correlationId.Value))
                {
                    _correlationId.Value = Guid.NewGuid().ToString();
                }

                return _correlationId.Value;
            }
            set => _correlationId.Value = value;
        }

        public static void NewId() => CorrelationId = Guid.NewGuid().ToString();
    }
}
