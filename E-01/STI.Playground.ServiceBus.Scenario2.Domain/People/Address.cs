namespace STI.Playground.ServiceBus.Scenario2.Domain.People
{
    public struct Address
    {
        public Address(string street1, string street2, string city, string state, string zipCode)
        {
            Street1 = street1;
            Street2 = street2;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Street1 { get; private set; }
        public string Street2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        public static Address New(string street1, string street2, string city, string state, string zipCode)
            => new Address(street1, street2, city, state, zipCode);

        internal bool IsValid()
        => string.IsNullOrEmpty($"{Street1}{Street2}{City}{State}{ZipCode}");
    }
}
