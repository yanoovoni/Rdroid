using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.Services;

/// <summary>
/// Summary description for YuvalWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class YuvalWebService : System.Web.Services.WebService {

    public YuvalWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public DataSet GetAllProducts()
    {
        ProductService product = new ProductService();
        DataSet ds = product.GetAllProducts();
        return ds;
    }

    [WebMethod]
    public void updateProductPrice(ProductInBag Product)
    {
        ProductService product = new ProductService();
        product.updateProductPrice(Product);
        
    }

}
