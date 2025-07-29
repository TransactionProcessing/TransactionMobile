namespace TransactionProcessor.Mobile.BusinessLogic.Common
{
    /*public class IdGenerationService
    {
        internal delegate Guid GenerateUniqueIdFromObject(Object payload);

        internal delegate Guid GenerateUniqueIdFromString(String payload);


        private static readonly JsonSerialiser JsonSerialiser = new(() => new JsonSerializerSettings
                                                                          {
                                                                              Formatting = Formatting.None
                                                                          });

        private static readonly GenerateUniqueIdFromObject GenerateUniqueId =
            data => IdGenerationService.GenerateGuidFromString(IdGenerationService.JsonSerialiser.Serialise(data));

        private static readonly GenerateUniqueIdFromString GenerateGuidFromString = uniqueKey => {
                                                                                        using SHA256 sha256Hash = SHA256.Create();
                                                                                        //Generate hash from the key
                                                                                        Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(uniqueKey));

                                                                                        Byte[] j = bytes.Skip(Math.Max(0, bytes.Count() - 16)).ToArray(); //Take last 16

                                                                                        //Create our Guid.
                                                                                        return new Guid(j);
                                                                                    };

        public static Guid GenerateGuid(Object o) => IdGenerationService.GenerateUniqueId(o);
    }*/

    public interface ICorrelationIdProvider
    {
        string CorrelationId { get; }
        void SetCorrelationId(string correlationId);
        void ResetCorrelationId();
    }

    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private static readonly AsyncLocal<string?> _correlationId = new();

        public string CorrelationId => _correlationId.Value ??= Guid.NewGuid().ToString();

        public void SetCorrelationId(string correlationId)
        {
            _correlationId.Value = correlationId ?? Guid.NewGuid().ToString();
        }

        public void ResetCorrelationId()
        {
            _correlationId.Value = Guid.NewGuid().ToString();
        }
    }
}
