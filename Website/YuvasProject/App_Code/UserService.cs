using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// Summary description for UserService
/// </summary>
public class UserService
{
    protected OleDbConnection myConnection;
    public UserService()
    {
        string connectionString = Connect.getConnectionString();
        myConnection = new OleDbConnection(connectionString);
    }
    public void InsertUser(UserDetails userDetails)
    {
        OleDbCommand myCmd = new OleDbCommand("InsertIntoUsers", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;

        OleDbParameter objParam;
        

        objParam = myCmd.Parameters.Add("@FirstName", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.firstName;

        objParam = myCmd.Parameters.Add("@LastName", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.lastName;

        objParam = myCmd.Parameters.Add("@Password", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.password;

        objParam = myCmd.Parameters.Add("@Email", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.email;



        try
        {
            myConnection.Open();
            myCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
    }

    public int EnterToSite(UserDetails userDetails)
    {
        OleDbCommand myCmd = new OleDbCommand("CheckIfUserExist", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;

        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@Email", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.email;

        objParam = myCmd.Parameters.Add("@Password", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.password;

        int x = 0;
        try
        {
            myConnection.Open();
            x = (int)myCmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        return x;

    }

    public DataSet GetFriends(UserDetails user)
    {
        OleDbCommand myCmd = new OleDbCommand("ShowFriends", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        DataSet dataset = new DataSet();

        OleDbParameter parameter;
        parameter = myCmd.Parameters.Add("@UserID", OleDbType.BSTR);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = user.phoneNumber;

        try
        {
            adapter.Fill(dataset, "Users");
            dataset.Tables["Users"].PrimaryKey = new DataColumn[] { dataset.Tables["Users"].Columns["UserID"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;

    }

    public DataSet GetContacts(UserDetails user)
    {
        OleDbCommand myCmd = new OleDbCommand("ShowContacts", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        DataSet dataset = new DataSet();

        OleDbParameter parameter;
        parameter = myCmd.Parameters.Add("@UserPhone", OleDbType.BSTR);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = user.phoneNumber;

        try
        {
            adapter.Fill(dataset, "Contacts");
            dataset.Tables["Contacts"].PrimaryKey = new DataColumn[] { dataset.Tables["Contacts"].Columns["UserIDBelong"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }
    public DataSet GetFriendsAndContacts(UserDetails user)// הפעולה מחזירה "דטה סט" בו נמצאים גם החברים וגם אנשי הקשר
    {
        OleDbParameter parameter;
        OleDbCommand myCmd = new OleDbCommand("ShowFriends", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapterFriends = new OleDbDataAdapter(myCmd);


        parameter = myCmd.Parameters.Add("@PhoneID", OleDbType.BSTR);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = user.phoneNumber;

        myCmd = new OleDbCommand("GetContacts", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapterContacts = new OleDbDataAdapter(myCmd);
        parameter = myCmd.Parameters.Add("@PhoneID", OleDbType.BSTR);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = user.phoneNumber;
        
        
        DataSet dataSet = new DataSet();

        try
        {
            myConnection.Open();
            adapterFriends.Fill(dataSet, "Friends");
            dataSet.Tables["Friends"].PrimaryKey = new DataColumn[] { dataSet.Tables["Friends"].Columns["UserID"] };
            adapterContacts.Fill(dataSet, "Contacts");
            dataSet.Tables["Contacts"].PrimaryKey = new DataColumn[] { dataSet.Tables["Contacts"].Columns["ID"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        return dataSet;
    }

    public bool IfContactExist(ContactDetails contactDetais)//הפעולה בודקת האם איש הקשר כבר קיים
    {
        bool find = false;
        object obj = null;
        OleDbCommand myCmd = new OleDbCommand("checkIfContactExistsByUserPhoneBelong", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@UserPhoneBelong", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.userPhoneBelong;

        objParam = myCmd.Parameters.Add("@PhoneNumber", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.phoneNumber;

        try
        {
            myConnection.Open();
            obj = myCmd.ExecuteScalar();
            if (obj == null)
                return find;
            find = true;
            return find;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }

       
    }

    public void InsertContact( ContactDetails contactDetais)//הפעולה מאפשרת להוסיף מידע לתוך טבלת אנשי הקשר
    {
        OleDbCommand myCmd = new OleDbCommand("InsertIntoContacts", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        //INSERT INTO Contacts ( UserIDBelong, PhoneNumber, FirstName, LastName, Email )
       // VALUES(@UserIDBelong, @PhoneNumber, @FirstName, @LastName, @Email);

        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@UserPhoneBelong", OleDbType.Integer);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.userPhoneBelong;

        objParam = myCmd.Parameters.Add("@PhoneNumber", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.phoneNumber;

        objParam = myCmd.Parameters.Add("@FirstName", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.firstName;

        objParam = myCmd.Parameters.Add("@LastName", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.lastName;

        objParam = myCmd.Parameters.Add("@Email", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.email;
        objParam = myCmd.Parameters.Add("@Status", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = contactDetais.status;
        try
        {
            myConnection.Open();
            myCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
    }

    public ContactDetails GetContactsByID(int IDmy)//הפעולה מחזירה פרטי איש קשר על פי מספרו בטבלה
    {
        OleDbCommand myCmd = new OleDbCommand("ReturnContactByID", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataReader reader;
        DataSet dataset = new DataSet();
        ContactDetails contact = new ContactDetails();
        
        OleDbParameter parameter;
        parameter = myCmd.Parameters.Add("@ID", OleDbType.BSTR);
        parameter.Direction = ParameterDirection.Input;
        parameter.Value = IDmy;

        try
        {
            myConnection.Open();
            reader = myCmd.ExecuteReader();
            while (reader.Read())
            {
                contact.id = IDmy;
                contact.phoneNumber = reader["PhoneNumber"].ToString();
                contact.firstName = reader["FirstName"].ToString();
                contact.lastName = reader["LastName"].ToString();
                contact.email = reader["Email"].ToString();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        return contact;
    }

    //public DataSet getUsersAndCities()
    //{
    //    OleDbCommand myCmd = new OleDbCommand("allFromUser&Cities", myConnection);
    //    myCmd.CommandType = CommandType.StoredProcedure;
    //    OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
    //    DataSet dataset = new DataSet();

    //    try
    //    {
    //        adapter.Fill(dataset, "UserTb");
    //        //dataset.Tables["Category"].PrimaryKey = new DataColumn[] { dataset.Tables["Category"].Columns["KindID"] };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return dataset;
    //}

    //public void UpdateUserDetails(UserDetails userDetails)
    //{
    //    //UPDATE UserTb SET FirstName = [@userF], LastName = [@userL], Phone = [@phone], Addres = [@addres],
    //    //CityID = [@cityID], State = [@state], ZipCode = [@zipcode] WHERE UserID=[@userId];

    //    OleDbCommand myCmd = new OleDbCommand("updateUsers", myConnection);
    //    myCmd.CommandType = CommandType.StoredProcedure;
    //    OleDbParameter objParam;
    //    objParam = myCmd.Parameters.Add("@userF", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.firstName;

    //    objParam = myCmd.Parameters.Add("@userL", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.lastName;

    //    objParam = myCmd.Parameters.Add("@phone", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.phone;

    //    objParam = myCmd.Parameters.Add("@addres", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.address;

    //    objParam = myCmd.Parameters.Add("@cityID", OleDbType.Integer);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.cityID;

    //    objParam = myCmd.Parameters.Add("@state", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.state;

    //    objParam = myCmd.Parameters.Add("@zipcode", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.zipCode;
    //    objParam = myCmd.Parameters.Add("@userId", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails.userID;
    //    try
    //    {
    //        myConnection.Open();
    //        myCmd.ExecuteNonQuery();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        myConnection.Close();
    //    }
    //}

    //public void DeleteUser(string userDetails)
    //{
    //    OleDbCommand myCmd = new OleDbCommand("DeletUserByID", myConnection);
    //    myCmd.CommandType = CommandType.StoredProcedure;
    //    OleDbParameter objParam;

    //    objParam = myCmd.Parameters.Add("@userId", OleDbType.BSTR);
    //    objParam.Direction = ParameterDirection.Input;
    //    objParam.Value = userDetails;

    //    try
    //    {
    //        myConnection.Open();
    //        myCmd.ExecuteNonQuery();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        myConnection.Close();
    //    }
    //}

    //public DataSet getUsersAndCity()
    //{
    //    OleDbCommand myCmd = new OleDbCommand("allFromUserCities", myConnection);
    //    myCmd.CommandType = CommandType.StoredProcedure;
    //    OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
    //    DataSet dataset = new DataSet();

    //    try
    //    {
    //        adapter.Fill(dataset, "UserTb");
            
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return dataset;
    //}


}