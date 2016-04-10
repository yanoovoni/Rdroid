using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;

/// <summary>
/// Summary description for DataService
/// </summary>
public class DataService
{
    protected OleDbConnection myConnection;
    protected OleDbDataAdapter adapter;
    protected OleDbDataAdapter adapterKinds;
    protected OleDbDataAdapter adapterWords;
	public DataService()
	{
        string connectionString = Connect.getConnectionString();
        myConnection = new OleDbConnection(connectionString);
        string sSql = "select * from KindsTBL";
        adapterKinds = new OleDbDataAdapter(sSql, myConnection);
        string sql = "select * from wordsTBL";
        adapterWords = new OleDbDataAdapter(sql, myConnection);
        string sSql1 = "select * from KindsTBL";
        adapter = new OleDbDataAdapter(sql, myConnection);
        
	}

    public string GetCategoryName(int code){
        string str = "";
       object obj=null;
       string sql = "select typeName from KindsTbl where koditem=" + code;
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);

        try
        {
            myConnection.Open();
            obj = cmd.ExecuteScalar();
            if (obj != null)
                return obj.ToString();
            return str;
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
    public bool addcategory(string name)
    {
        if (!checkifexist(name))
        {
            string sql = "insert Into KindsTbl(typeName) values('" + name + "');";
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
        return false;
    }

    public bool checkifexist(string name)
    {
        object obj = null;
        string sql = "select typeName from KindsTbl where typeName='" + name + "'";
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);

        try
        {
            myConnection.Open();
            obj = cmd.ExecuteScalar();
            if (obj == null)
                return false;
            return true;

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

    public OleDbDataReader ShowAll()
    {
        OleDbDataReader dr = null;
        string sql = "SELECT * from KindsTbl";
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);

        try
        {
            myConnection.Open();
            dr = cmd.ExecuteReader();
            
            return dr;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string ToText()
    {
        OleDbDataReader dr = ShowAll();
        string output = "";

        try
        {
            while (dr.Read())
            {
                output += dr["typeName"].ToString() + "\n";
            }
            return output;
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

    public bool checkifexist2(int kod)
    {
        object obj = null;
        string sql = "select * from KindsTbl where kodItem=" + kod;
        OleDbCommand cmd = new OleDbCommand(sql, myConnection);

        try
        {
            myConnection.Open();
            obj = cmd.ExecuteScalar();
            if (obj == null)
                return false;
            return true;

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

    public bool deleteWords(int kod)
    {

        if (checkifexist2(kod)==false)
        {
            string sql = "delete from wordsTBL where kodItem=" + kod;
            OleDbCommand cmd = new OleDbCommand(sql, myConnection);

            try
            {
                myConnection.Open();
                cmd.ExecuteNonQuery();

            }
            catch(Exception ex)
            {
                throw ex;
            }
           
            finally
            {
                myConnection.Close();
            }
            return true;
        }
        return true;
    }


    public bool deleteCategory(int kod)
    {

        if (deleteWords(kod))
        {
            string sql = "delete from kindsTBL where kodItem=" + kod;
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
        return false;
    }


    public DataSet GetSubjects2()
    {
        string sSql = "select * from KindsTbl";
        OleDbCommand myCmd = new OleDbCommand(sSql, myConnection);
        OleDbDataAdapter adapter = new OleDbDataAdapter();
        adapter.SelectCommand = myCmd;
        DataSet dataSet = new DataSet();

        try
        {
            adapter.Fill(dataSet, "KindsTBL");
            dataSet.Tables["KindsTBL"].PrimaryKey = new DataColumn[] { dataSet.Tables["KindsTbl"].Columns["kodItem"] };
        }

        catch (Exception ex)
        {
            throw ex;
        }

        return dataSet;
    }

    public DataSet GetSubjectAndWords()
    {
        DataSet dataSet = new DataSet();

        try
        {
            myConnection.Open();
            adapterKinds.Fill(dataSet, "KindsTBL");
            adapterWords.Fill(dataSet, "wordsTBL");

            dataSet.Tables["KindsTBL"].PrimaryKey = new DataColumn[] { dataSet.Tables["KindsTBL"].Columns["kodItem"] };
            //dataSet.Tables["wordsTBL"].PrimaryKey = new DataColumn[] { dataSet.Tables["wordsTBL"].Columns["kod"] };

            CreateRelation(dataSet);

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

    private void CreateRelation(DataSet DS)
    {
        DataColumn parentCol;
        DataColumn childCol;

        parentCol = DS.Tables["KindsTBL"].Columns["kodItem"];
        childCol = DS.Tables["wordsTBL"].Columns["kodItem"];

        DataRelation relKindWords = new DataRelation("KindWord", parentCol, childCol);

        DS.Relations.Add(relKindWords);
    }

    public void ADDKindDS(string sug, DataSet Ds)
    {
        if (!CheckExistenceDS(sug,Ds))
        {
            DataRow dataRow = Ds.Tables["KindsTBL"].NewRow();
            dataRow["kodItem"] = (LastRow(Ds) + 1);
            dataRow["typeName"] = sug;
            Ds.Tables["KindsTBL"].Rows.Add(dataRow);
        }
        
    }

    public int LastRow(DataSet Ds)
    {
        
            int lastRow = Ds.Tables["KindsTBL"].Rows.Count;
            while (Ds.Tables["KindsTBL"].Rows[lastRow - 1].RowState == DataRowState.Deleted)
                lastRow--;
            int lastCode = (int)Ds.Tables["KindsTBL"].Rows[lastRow - 1]["kodItem"];
            return lastCode;
        
    }


    public void UpdateKindsDS(string kod, string sug, DataSet Ds)
    {
        if (!CheckExistenceDS(sug, Ds))
        {
            DataRow kinds = Ds.Tables["KindsTBL"].Rows.Find(kod);
            kinds["typename"] = sug;
        }
    }

    public void DeleteKindDS(string kod, DataSet Ds)
    {
         DataRow dr = Ds.Tables["KindsTBL"].Rows.Find(kod);
        DataRow[] words = dr.GetChildRows("kindWord");
        if (words.Length == 0)
        {
            dr.Delete();
        }
    }

    public bool CheckExistenceDS(string sug, DataSet Ds)
    {
        string criteria = "typeName='" + sug + "'";

        DataRow[] dr = Ds.Tables["KindsTBL"].Select(criteria);
        if (dr.Length !=0)
            return true;
        return false;
    }

    public void updateDbfromDS(DataSet ds)
    {
        OleDbCommandBuilder builder = new OleDbCommandBuilder(adapterKinds);
        adapterKinds.DeleteCommand = builder.GetDeleteCommand();
        adapterKinds.UpdateCommand = builder.GetUpdateCommand();
        adapterKinds.InsertCommand = builder.GetInsertCommand();
        adapterKinds.Update(ds, "KindsTBL");
    }

    public string sugMila(string kod)
    {
        try
        {
            myConnection.Open();
            string sSql = "select * from KindsTBL where kodItem=@kodItem";
            string t = "" + kod;
            OleDbCommand objCmd = new OleDbCommand(sSql, myConnection);
            OleDbParameter objParam;
            objParam = objCmd.Parameters.Add("@kod", OleDbType.BSTR);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = double.Parse(t);

            t = (string)objCmd.ExecuteScalar();

            return t;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        { 
            myConnection.Close();
        }
        return "";
    }

    

}
