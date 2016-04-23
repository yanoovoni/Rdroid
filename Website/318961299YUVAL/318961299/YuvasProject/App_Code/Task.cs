using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;



/// <summary>
/// Summary description for Task
/// </summary>
public static class Task
{
    public static Proxy proxy = Proxy.Get_Instance();

    public static string[] GetFilesInFolder(string email, string folder_location)
    {
        string[] input = new string[1];
        input[0] = folder_location; 
        string output = proxy.HandleTask(email, "GET_FILES_IN_FOLDER", input);
        return output.Split(',');
    }

    public static string GetFile(string email, string file_location)
    {
        string[] input = new string[1];
        input[0] = file_location;
        return proxy.HandleTask(email, "GET_FILE", input);
    }

    public static bool SaveFile(string email, string location, string file_data)
    {
        string[] input = new string[2];
        input[0] = location;
        input[1] = file_data;
        string output = proxy.HandleTask(email, "SAVE_FILE", input);
        if (output == "success")
        {
            return true;
        }
        if (output == "failure")
        {
            return false;
        }
        throw new Exception();
    }

    public static string[,] GetContacts()
    {


    }

    public static bool SaveContact(string email, string DisplayName, string MobileNumber, string emailID)
    {
        string[] input = new string[3] {DisplayName, MobileNumber, emailID};
        string output = proxy.HandleTask(email, "SAVE_CONTACT", input);
        if (output == "success")
        {
            return true;
        }
        if (output == "failure")
        {
            return false;
        }
        throw new Exception();
    }
}