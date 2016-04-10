using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FriendDetails
/// </summary>
public class FriendDetails
{
    //        INSERT INTO Friends ( PhoneIDAsking, PhoneIDAccepting, DateOfFriendship, Status )
//VALUES (@PhoneIDAsking, @PhoneIDAccepting, @DateOfFriendship, @Status);


    string PhoneIDAsking;
    string PhoneIDAccepting;
    DateTime DateOfFriendship;
    bool Status;

	public FriendDetails()
	{


	}

    public string phoneIDAsking
    {
        get { return PhoneIDAsking; }
        set { PhoneIDAsking = value; }
    }

    public string phoneIDAccepting
    {
        get { return PhoneIDAccepting; }
        set { PhoneIDAccepting = value; }
    }

    public DateTime dateOfFriendship
    {
        get { return DateOfFriendship; }
        set { DateOfFriendship = value; }
    }

    public bool status
    {
        get { return Status; }
        set { Status = value; }
    }
}