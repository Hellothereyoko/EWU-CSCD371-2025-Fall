namespace Assignment;

public class Person : IPerson
{
    // Constructor updated to match IPerson property order usually expected
    public Person(string firstName, string lastName, IAddress address, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        EmailAddress = email;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public IAddress Address { get; }
    public string EmailAddress { get; }
}