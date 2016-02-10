using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class UserManagment13 : System.Web.UI.Page
{
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

    
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        UserService userService = new UserService();
        string userID = GridView1.Rows[e.RowIndex].Cells[0].Text;
        userService.DeleteUser(userID);
        populateGrid();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            Button deleteButton = (Button)e.Row.Cells[8].FindControl("Button1");
            deleteButton.Attributes["onclick"] = "javascript:return confirm('are you sure you want to delete UserID #" + DataBinder.Eval(e.Row.DataItem, "userID") + "?')";
        }

        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    Label label1 = (Label)e.Row.Cells[6].FindControl("LabelSum");
        //    label1.Text= 
        //}
    }



    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        populateGrid();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow item = (GridViewRow)ddl.NamingContainer;

        string id = item.Cells[0].Text;

        UserService userService = new UserService();
        userService.UpdateCityName(int.Parse(ddl.SelectedValue), id);


        GridView1.EditIndex = -1;
        populateGrid();
    }
}