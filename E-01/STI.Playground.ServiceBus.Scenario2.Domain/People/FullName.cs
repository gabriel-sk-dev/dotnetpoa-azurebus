namespace STI.Playground.ServiceBus.Scenario2.Domain.People
{
    public struct FullName
    {
        public FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public static FullName New(string firstName, string lastName)
            => new FullName(firstName, lastName);

        public override string ToString()
            => $"{LastName}, {FirstName}";
    }
}
