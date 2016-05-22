using System;
using System.Collections;
public class Order
{
    int orderID;
    string customerID;
    int employeeID;
    DateTime orderDate;
    DateTime requiredDate;
    private ShoppingBag orderProductds;

    public Order()
    {
        //OrderProductds.Products=new ArrayList();
    }

    public ShoppingBag OrderProductds
    {
        get { return this.orderProductds; }
        set { this.orderProductds = value; }
    }
    public int OrderID
    {
        get { return this.orderID; }
        set { this.orderID = value; }
    }
    public string CustomerID
    {
        get { return this.customerID; }
        set { this.customerID = value; }
    }
    public int EmployeeID
    {
        get { return this.employeeID; }
        set { this.employeeID = value; }
    }
    public DateTime OrderDate
    {
        get { return this.orderDate; }
        set { this.orderDate = value; }
    }
    public DateTime RequiredDate
    {
        get { return this.requiredDate; }
        set { this.requiredDate = value; }
    }
  
}