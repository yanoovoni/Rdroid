using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Makes a connection with the table
/// </summary>
public class Connect
{
    const string FILE_NAME = "TalyanGame.mdb";

    public static string getConnectionString()
    {
        string location = HttpContext.Current.Server.MapPath("~/App_Data/" + FILE_NAME);
        string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; data source=" + location; ;
        return ConnectionString; 
    }

	public Connect()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}