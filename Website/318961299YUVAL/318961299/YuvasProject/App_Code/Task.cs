using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Task
/// </summary>
public static class Task
{
    public static Proxy proxy = Proxy.Get_Instance();
    public static string GetFilesInFolder(String folder_location)
    {
        return proxy.HandleTask();
    }
}