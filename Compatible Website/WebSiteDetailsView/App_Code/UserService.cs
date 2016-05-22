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
    public DataSet getUsers()
    {
        OleDbCommand myCmd = new OleDbCommand("allFromUser", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);        
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "UserTb");
            //dataset.Tables["Category"].PrimaryKey = new DataColumn[] { dataset.Tables["Category"].Columns["KindID"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }

    public DataSet getUsersAndCities()
    {
        OleDbCommand myCmd = new OleDbCommand("allFromUser&Cities", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "UserTb");
            //dataset.Tables["Category"].PrimaryKey = new DataColumn[] { dataset.Tables["Category"].Columns["KindID"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }

    public void UpdateUserDetails(UserDetails userDetails)
    {
        //UPDATE UserTb SET FirstName = [@userF], LastName = [@userL], Phone = [@phone], Addres = [@addres],
        //CityID = [@cityID], State = [@state], ZipCode = [@zipcode] WHERE UserID=[@userId];

        OleDbCommand myCmd = new OleDbCommand("updateUsers", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbParameter objParam;
        objParam = myCmd.Parameters.Add("@userF", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.firstName;

        objParam = myCmd.Parameters.Add("@userL", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.lastName;

        objParam = myCmd.Parameters.Add("@phone", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.phone;

        objParam = myCmd.Parameters.Add("@addres", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.address;

        objParam = myCmd.Parameters.Add("@cityID", OleDbType.Integer);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.cityID;

        objParam = myCmd.Parameters.Add("@state", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.state;

        objParam = myCmd.Parameters.Add("@zipcode", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.zipCode;
        objParam = myCmd.Parameters.Add("@userId", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.userID;
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

    public void DeleteUser(string userDetails)
    {
        OleDbCommand myCmd = new OleDbCommand("DeletUserByID", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@userId", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails;

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

    public DataSet getUsersAndCity()
    {
        OleDbCommand myCmd = new OleDbCommand("allFromUserCities", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "UserTb");
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }

    public DataSet getCity()
    {
        OleDbCommand myCmd = new OleDbCommand("ShowCityName", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "CityTbl");

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }

    public void UpdateCityName(int CityID,string userID)
    {
        OleDbCommand myCmd = new OleDbCommand("ShowCT", myConnection);
        //UPDATE UserTb SET CityID = [@CityID]
        //WHERE UserID=[@UserID];

        myCmd.CommandType = CommandType.StoredProcedure;

        OleDbParameter objParam;
        objParam = myCmd.Parameters.Add("@CityID", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = CityID;

        objParam = myCmd.Parameters.Add("@UserID", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userID;

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

    //public DataSet Count()
    //{
    //    OleDbCommand myCmd = new OleDbCommand("COUNT", myConnection);
    //    myCmd.CommandType = CommandType.StoredProcedure;
    //    DataSet data = new DataSet();

      

    //    string nnn = "";

    //    try
    //    {
    //        myConnection.Open();
           

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        myConnection.Close();
    //    }
    //    return data;
    //}
}