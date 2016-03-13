using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserDetails
/// </summary>
public class UserDetails
{
    private string PhoneNumber;
    private string FirstName;
    private string LastName;
    private string Password;
    private string eMail;
    
    


    public UserDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string phoneNumber
    {
        get { return PhoneNumber; }
        set { PhoneNumber = value; }
    }

    public string firstName
    {
        get { return FirstName; }
        set { FirstName = value; }
    }

    public string lastName
    {
        get { return LastName; }
        set { LastName = value; }
    }

    public string password
    {
        get { return Password; }
        set { Password = value; }
    }

    public string email
    {
        get { return eMail; }
        set { eMail = value; }
    }

    

   

    

}