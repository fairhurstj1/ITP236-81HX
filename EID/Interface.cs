using System;
using System.Collections.Generic;
namespace EID
{
    public interface IPerson
    {
        string FirstName {get; set;}
        string LastName {get; set;}
        int Age {get; set;}
        int BirthDate {get; set;}

        
    }
}