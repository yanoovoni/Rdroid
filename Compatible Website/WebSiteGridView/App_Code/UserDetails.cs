using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserDetails
/// </summary>
public class UserDetails
{
    private string UserID;
    private string FirstName;
    private string LastName;
    private string Phone;
    private string Address;
    private int CityID;
    private string State;
    private string ZipCode;


    public UserDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string userID
    {
        get { return UserID; }
        set { UserID = value; }
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

    public string phone
    {
        get { return Phone; }
        set { Phone = value; }
    }

    public string address
    {
        get { return Address; }
        set { Address = value; }
    }

    public int cityID
    {
        get { return CityID; }
        set { CityID = value; }
    }

    public string state
    {
        get { return State; }
        set { State = value; }
    }

    public string zipCode
    {
        get { return ZipCode; }
        set { ZipCode = value; }
    }

}