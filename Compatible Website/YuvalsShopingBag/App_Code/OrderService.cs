using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

public class OrderService
{
    OleDbConnection myConnection;
    OleDbCommand myCmd;
    OleDbTransaction transaction;

    public OrderService()
    {
    }
    public void InserOrderDetail(Order order)
    {
        DataSet ds=convertToTable(order.OrderProductds, order.OrderID);

        myCmd = new OleDbCommand("Select * From OrderDetails", myConnection);
        myCmd.Transaction = transaction;

        OleDbDataAdapter adapter = new OleDbDataAdapter(myCmd);
        OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);


        adapter.InsertCommand = builder.GetInsertCommand();

        try
        {
            adapter.Update(ds, "ShoppingBasketDs");

        }
        catch (Exception ex)
        {
            throw ex;

        }   
    }
    public int InserNewOrder(Order order)
    {
        int orderID = 0;

        OleDbCommand myCmd = new OleDbCommand("InsertOrder", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;

        myCmd.Transaction=transaction;

        //INSERT INTO Users ( CustomerID, OrderDate )
        //VALUES ( @CustomerID, @OrderDate);

        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@CustomerID", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = order.CustomerID;

        objParam = myCmd.Parameters.Add("@OrderDate", OleDbType.Date);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = order.OrderDate;

        try
        {
            myCmd.ExecuteNonQuery();

            myCmd = new OleDbCommand("GetLastOrderID", myConnection);
            //SELECT Max(Orders.OrderID) AS Expr1
            //FROM orders;

            myCmd.CommandType = CommandType.StoredProcedure;
            myCmd.Transaction = transaction;

            object obj = myCmd.ExecuteScalar();
            if (obj != null)
            {
                orderID = (int)obj;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return orderID;
    }
    public int CreateOrder(Order order)
    {
        string connectionString = Connstring.getConnectionString();
        myConnection = new OleDbConnection(connectionString);

        try
        {
            myConnection.Open();
            // התחלת הטרנזקציה
            transaction = myConnection.BeginTransaction();
            //הכנסת ההזמנה לטבלת הזמנות ושליפת מספר ההזמנה שנוצרה  
            int orderID = InserNewOrder(order);
            order.OrderID = orderID;
            //System.Single discount=0;

            //  הכנסת המוצרים בהזמנה
            InserOrderDetail(order);
            // אישור הטרנזקציה
            transaction.Commit();
            return orderID;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw ex;
        }
    }

    public DataSet convertToTable(ShoppingBag shoppingBag, int orderID)
    {
        DataTable dtShopingBag = new DataTable();

        DataColumn[] dtColumd = new DataColumn[]
        { 
            new DataColumn("OrderID"), 
            new DataColumn("ProductID"), 
            new DataColumn("UnitPrice"),
            new DataColumn("Quantity"),
             new DataColumn("Discount"),
        };

        dtShopingBag.Columns.AddRange(dtColumd);

        for (int i = 0; i < shoppingBag.Products.Count; i++)
        {
            DataRow currRow = dtShopingBag.NewRow();
            currRow["OrderID"] = orderID;
            currRow["ProductID"] = ((ProductInBag)shoppingBag.Products[i]).ProdID;
            currRow["UnitPrice"] = ((ProductInBag)shoppingBag.Products[i]).Price;
            currRow["Quantity"] = ((ProductInBag)shoppingBag.Products[i]).Quantity;
            currRow["Discount"] = 0;
            dtShopingBag.Rows.Add(currRow);
        }

        DataSet ds = new DataSet();
        ds.Tables.Add(dtShopingBag);
        ds.Tables[0].TableName = "ShoppingBasketDs";
        return ds;
    }
}