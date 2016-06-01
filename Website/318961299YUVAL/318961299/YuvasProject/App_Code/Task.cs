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
        string output = new string(proxy.HandleTask(email, "GET_FILES_IN_FOLDER", input));
        return output.Split('/');
    }

    public static bool GetFile(string email, string file_location, out char[] Data)
    {
        Data = null;
        string[] input = new string[1];
        input[0] = file_location;
        char[] output = proxy.HandleTask(email, "GET_FILE", input);
        if (new string(output).StartsWith("success/"))
        {
            Data = new char[output.Length - "success/".Length];
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = output[i + "success/".Length];
            }
            return true;
        }
        else
        {
            if (new string(output).StartsWith("failure/"))
            {
                Data = new string(output).Substring("failure/".Length).ToCharArray();
                return false;
            }
        }
        return false;
        
    }

    public static bool SaveFile(string email, string location, string file_data)
    {
        string[] input = new string[2];
        input[0] = location;
        input[1] = file_data;
        string output = new string(proxy.HandleTask(email, "SAVE_FILE", input));
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


    public static bool SaveContact(string email, string DisplayName, string MobileNumber, string emailID)
    {
        string[] input = new string[3] {DisplayName, MobileNumber, emailID};
        string output = new string(proxy.HandleTask(email, "SAVE_CONTACT", input));
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