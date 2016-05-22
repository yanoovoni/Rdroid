using System;
using System.Web;

public class Connstring
{
    const string FILENAME = "SqlPracticeStudentV2013.mdb";
    public static string getConnectionString()
    {
        string location = HttpContext.Current.Server.MapPath("~/App_Data/" + FILENAME);
        string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; data source=" + location;
        return ConnectionString;
    }
}