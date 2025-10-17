
using System;

namespace Logger;


public record Employee : PersonEntity
{
    
      public Employee(Guid id, FullName fullName, string email, string V) : base(id, fullName, email)
    {
    }
}