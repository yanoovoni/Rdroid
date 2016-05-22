using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class UserManagmnent : System.Web.UI.Page
{
    int sum = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            populateGrid();
        }
    }
    private DataSet getData()
    {

        UserService userService = new UserService();
        return userService.getUsersAndCity();
    }

    private void populateGrid()
    {
        GridView1.DataSource = getData();
        GridView1.DataBind();
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet ds = getData();
        DataView dataView = new DataView(ds.Tables[0]);
        dataView.Sort = e.SortExpression;
        GridView1.DataSource = dataView;
        GridView1.DataBind();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShowRowNumber")
        {
            object row = e.CommandArgument;
            Label1.Text = row.ToString();
            int rowNumber = Convert.ToInt32(row);
            Label2.Text = ((GridView)sender).Rows[rowNumber].Cells[1].Text + " " + ((GridView)sender).Rows[rowNumber].Cells[2].Text;
        }

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        populateGrid();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        populateGrid();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            UserDetails userDetails = new UserDetails();

            userDetails.userID = GridView1.Rows[e.RowIndex].Cells[0].Text;
            userDetails.firstName = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text;
            userDetails.lastName = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            userDetails.phone = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[5].Controls[0])).Text;
            userDetails.cityID = int.Parse(((DropDownList)(GridView1.Rows[e.RowIndex].Cells[3].FindControl("DropDownList1"))).SelectedValue);
            userDetails.address = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].FindControl("TextBox1"))).Text;
            userDetails.state = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].FindControl("TextBox3"))).Text;
            userDetails.zipCode = ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].FindControl("TextBox2"))).Text;
            

            UserService userService = new UserService();
            userService.UpdateUserDetails(userDetails);

            GridView1.EditIndex = -1;
            populateGrid();
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        UserService userService = new UserService();
        string userID = GridView1.Rows[e.RowIndex].Cells[0].Text;
        userService.DeleteUser(userID);
        populateGrid();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        UserService userService=new UserService();
        //string x = userService.Count().ToString();

   

        if (e.Row.RowType != DataControlRowType.Header && 
            e.Row.RowType != DataControlRowType.Footer && 
            e.Row.RowType != DataControlRowType.Pager)
        {
            sum++; 
            Button deleteButton = (Button)e.Row.Cells[8].FindControl("Button1");
            deleteButton.Attributes["onclick"] = "javascript:return confirm('are you sure you want to delete UserID #" + DataBinder.Eval(e.Row.DataItem, "userID") + "?')";
        
            if (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate) ||
            e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Normal))
            {
                UserService user = new UserService();
                DropDownList ddl = (DropDownList)e.Row.Cells[4].FindControl("DropDownList1");
                DataSet dsCities = new DataSet();
                dsCities = user.getCity();
                ddl.DataSource = dsCities;
                ddl.DataTextField = dsCities.Tables[0].Columns[1].ToString();
                ddl.DataValueField = dsCities.Tables[0].Columns[0].ToString();
                ddl.DataBind();


                object cityname = DataBinder.Eval(e.Row.DataItem, "cityId");
                ddl.SelectedValue = cityname.ToString();
            }
        }


        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label label1 = (Label)e.Row.Cells[6].FindControl("LableSum");
            label1.Text = sum.ToString();
        }
    }



    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        populateGrid();
    }
}