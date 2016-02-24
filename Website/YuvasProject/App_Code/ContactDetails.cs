using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ContactService
/// </summary>
public class ContactDetails
{
    private int ID;
    private int UserIDBelong;
    private string PhoneNumber;
    private string FirstName;
    private string LastName;
    private string Email; 

	public ContactDetails()
	{
		
	}

    public int id
    {
        get { return ID; }
        set { ID = value; }
    }

    public int userIDbelong
    {
        get { return UserIDBelong; }
        set { UserIDBelong = value; }
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

    public string email
    {
        get { return Email; }
        set { Email = value; }
    }
}