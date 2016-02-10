using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// Summary description for DataService
/// </summary>
public class DataService
{
    protected OleDbConnection myConnection;
    protected OleDbDataAdapter adapter;

    public DataService()
    {
        string connectionString = Connect.getConnectionString();
        myConnection = new OleDbConnection(connectionString);
    }

    public string GetKind(int num)//מקבל מספר קוד במידה קיים מחזיר את הקטגוריה
    {
        string ssql = "select NameKind from Category where KindID= " + num;
        OleDbCommand mycmd = new OleDbCommand(ssql, myConnection);
        object obj = null;
        try
        {
            myConnection.Open();//פותח קשר
            obj = mycmd.ExecuteScalar();//מפעיל פעולה ומחזיר ערך
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        if (obj == null)//אם ריק מחזיר שאין ערך אחרת מחזיר קטגוריה
            return string.Empty;
        else return obj.ToString();
    }


    public bool AddKind(string kind)//מוסיפה את המילה לממסד נתונים במידה והמילה לא קיימת שם.
    {
        if (!Check(kind))
        {
            string stquary = "INSERT INTO Category (NameKind) VALUES ('" + kind + "')";
            OleDbCommand objcmd = new OleDbCommand(stquary, myConnection);
            try
            {
                this.myConnection.Open();
                objcmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConnection.Close();
            }
            return true;
        }
        else return false;
    }
    public bool Check(string description)//בודק אם קטגוריה מופיעה בטבלה
    {
        string sql = "SELECT * FROM Category WHERE NameKind='" + description + "'";
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);
        Object found = null;
        try
        {
            myConnection.Open();
            found = cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        return (found != null);
    }

    public bool Exist(int kod)//בודק אם קיים קטגוריה באותו קוד
    {
        string sql = "Select * From Category WHERE KindID=" + kod;
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);
        object obj = null;

        try
        {
            myConnection.Open();
            obj = cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        if (obj != null)
            return true;
        return false;

    }

    public bool Deleteornot(int kod)//למחוק או לא
    {
        bool exist = Exist(kod);
        if (exist)
        {
            string sql = "DELETE FROM WordsTbl WHERE kodWord=" + kod;
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);
            try
            {
                myConnection.Open();
               cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConnection.Close();
            }
            return true;
        }
        else return false;

    }
    public bool DeleteKind (int Kind)//מוחק קטגוריה
    {

        if (Deleteornot(Kind))
        {
            string stquary = "Delete FROM Category  where KindID=" + Kind ;
            OleDbCommand objcmd = new OleDbCommand(stquary, myConnection);
            try
            {
                this.myConnection.Open();
                objcmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConnection.Close();
            }
            return true;
        }
        else return false;
    }

    public OleDbDataReader getallsubject()//texebox-multiline/listsubjects
    {
        OleDbDataReader dr;
        OleDbCommand connect = new OleDbCommand("getallcat", myConnection);
        connect.CommandType = CommandType.StoredProcedure;

        try
        {
            myConnection.Open();
            dr = connect.ExecuteReader();

        }
        catch (OleDbException x)
        {
            throw x;
        }
        return dr;
}
    public DataSet GetallsubjectsSP()//מחזיר הכל לתוך dataset
    {
        //getallcat= select NameKind from Category;

        string connection = Connect.getConnectionString();
        OleDbCommand myCmd = new OleDbCommand("getallcat", myConnection);
        //myCmd.CommandType = CommandType.StoredProcedure;
        //OleDbDataAdapter adapter = new OleDbDataAdapter();
        //adapter.SelectCommand = myCmd;
        myCmd.ExecuteReader();
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "Category");
            dataset.Tables["Category"].PrimaryKey = new DataColumn[] { dataset.Tables["Category"].Columns["KindID"] };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataset;
    }

    public string Getallcat()//חזיר קטגוריות לstring
    {
        DataService dataservice = new DataService();
        OleDbDataReader datareader = dataservice.getallsubject();
        string output = null;
        try
        {
            while (datareader.Read())
            {
                output += datareader["NameKind"].ToString() + "\n";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return output;
    }

    public DataSet subjectsIntoDataSet()
    {
        string sql = "Select * FROM Category";
        OleDbDataAdapter adapter = new OleDbDataAdapter(sql, myConnection);
        DataSet dataset = new DataSet();

        try
        {
            adapter.Fill(dataset, "Category");
            dataset.Tables["Category"].PrimaryKey = new DataColumn[] { dataset.Tables["Category"].Columns["KindID"] };
        }
        catch(Exception ex)
        {
            throw ex;
        }
        return dataset;
        
    }
    public string GetCatById(int x)
    {
        OleDbCommand objmcd = new OleDbCommand("ReturnC", myConnection);
        objmcd.CommandType = CommandType.StoredProcedure;
      
       string t = ""+x;
       try
       {
           OleDbParameter p;

           myConnection.Open();
           
           p = objmcd.Parameters.Add("@x", OleDbType.Double);
           p.Direction = ParameterDirection.Input;
           p.Value = double.Parse(t);  
           t = (string)objmcd.ExecuteScalar();
       }
       catch (Exception ex)
       {
           throw ex;
       }
       finally
       {
           myConnection.Close();
       }
        return t;
       
    }
    private void CreateRelation(DataSet ds)
    {
        DataColumn Parentcol;
        DataColumn childcol;

        Parentcol = ds.Tables["TblCity"].Columns["CityID"];
        childcol = ds.Tables["TblPeople"].Columns["CityID"];

        DataRelation relcitypeople;

        relcitypeople = new DataRelation("City_People", Parentcol, childcol);
        ds.Relations.Add(relcitypeople);
    }
}
