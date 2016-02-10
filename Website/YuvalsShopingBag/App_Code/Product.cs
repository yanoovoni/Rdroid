using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class Product
{
    protected int mProdID;  //קוד מוצר
    protected string mProdName;  // שם מוצר
    public Product()
    {
        this.mProdID = -1;
        this.mProdName = null;
    }

    public Product(Product p)
    {
        this.mProdID = p.mProdID;
        this.mProdName = p.mProdName;
    }
    public Product(int inProdID, string inProdName)
    {
        this.mProdID = inProdID;
        this.mProdName = inProdName;
    }
    public Product(int inProdID)
    {
        this.mProdID = inProdID;
    }
    public int ProdID
    {
        get
        {
            return mProdID;
        }
        set
        {
            mProdID = value;
        }
    }

    public string ProdName
    {
        get
        {
            return mProdName;
        }
        set
        {
            mProdName = value;
        }
    }
}