using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class AddFriend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonFindFriends_Click(object sender, EventArgs e)
    {
        UserService userService = new UserService();
        DataSet dataset = new DataSet();
        string FullName = TextBoxSearchFriend.Text;
        int x = 0;
        string LastName = "";

        for (int i = 0; i < FullName.Length; i++)//הפעולה מחלקת את הסטרינג השלם לשתי מילים: שם פרטי ושם משפחה
        {
            if (FullName[i].ToString() == " ")
            {
                x = i + 1;
            }        
        }
        LastName = FullName.Substring(x, FullName.Length-x);
        
        FullName = FullName.Substring(0, x-1);

        dataset = userService.FindFriends(FullName, LastName);

        GridViewFriends.DataSource = dataset;
        GridViewFriends.DataBind(); 
    }
    protected void GridViewFriends_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "chck")
        {

        }

    }


}