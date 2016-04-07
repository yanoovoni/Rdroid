﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// The Phone object represents a phone that is connected to the server.
/// </summary>
public class Phone
{
    private string Id;
    private string User_Email = "";
    private int Task_Id_Generator_Number = 0;

    public Phone(string Id)
    {
        this.Id = Id;
    }

    public bool Logged_in()
    {
        return User_Email != "";
    }

    public string Get_Id()
    {
        return Id;
    }

    public string Get_Email()
    {
        return User_Email;
    }

    public void Set_Email(string email)
    {
        User_Email = email;
    }

    public void Send(string message)
    {
        ProxySocketInterface.Get_Instance().Send(message);
    }

    public string Generate_Task_Id()
    {
        Task_Id_Generator_Number++;
        return Task_Id_Generator_Number.ToString();
    }
}