
using System;
using System.Security.Principal;

namespace Logger;


public record Employee : PersonEntity
{   
    public string Role { get; init; }
    
      public Employee(Guid id, FullName fullName, string email, string role) : base(id, fullName, email)
    {
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }
}