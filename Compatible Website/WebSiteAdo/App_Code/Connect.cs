using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Connect
/// </summary>
public class Connect
{
    const string FILE_NAME = "TalyanGame.mdb";
    public static string getConnectionString()
    {
        string location = HttpContext.Current.Server.MapPath("~/App_Data/" + FILE_NAME);
        string ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; data source = " + location;
        return ConnectionString;
    }
	public Connect()
	{
		
	}
}