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
    private string UserPhoneBelong;
    private string PhoneNumber;
    private string FirstName;
    private string LastName;
    private string Email;
    private bool Status;
	public ContactDetails()
	{
		
	}

    public int id
    {
        get { return ID; }
        set { ID = value; }
    }

    public string  userPhoneBelong
    {
        get { return UserPhoneBelong; }
        set { UserPhoneBelong = value; }
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
    public bool status
    {
        get { return Status; }
        set { Status = value; }
    }
}