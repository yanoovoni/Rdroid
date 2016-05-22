using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

public class ProductService
{
    public ProductService()
    {

    }
    public DataSet GetAllProducts()
    {
        OleDbConnection myConn = new OleDbConnection(Connstring.getConnectionString());
        OleDbCommand myCmd;
        OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();

        try
        {
            myCmd = new OleDbCommand("GetAllProducts", myConn);
            myCmd.CommandType = CommandType.StoredProcedure;
            myDataAdapter.SelectCommand = myCmd;
            DataSet ds = new DataSet();
            myDataAdapter.Fill(ds);
            return ds;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void updateProductPrice(ProductInBag Product)
    {
        OleDbConnection myConn = new OleDbConnection(Connstring.getConnectionString());
        OleDbCommand myCmd;
        OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
        try
        {
            myCmd = new OleDbCommand("UpdateUnitPrice", myConn);
            myCmd.CommandType = CommandType.StoredProcedure;
            OleDbParameter objParam;

            objParam = myCmd.Parameters.Add("@UnitPrice", OleDbType.Decimal);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = Product.Price;

            objParam = myCmd.Parameters.Add("@ProductID", OleDbType.Integer);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = Product.ProdID;

            myConn.Open();
            myCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConn.Close();
        }
    }

    //public int GetProductStock(int productID)
    //{
    //    OleDbConnection myConn = new OleDbConnection(Connstring.getConnectionString());
    //    OleDbCommand myCmd;
    //    OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();

    //    try
    //    {
    //        myCmd = new OleDbCommand("GetAllProducts", myConn);
    //        myCmd.CommandType = CommandType.StoredProcedure;
    //        myDataAdapter.SelectCommand = myCmd;
    //    }

    //}
}