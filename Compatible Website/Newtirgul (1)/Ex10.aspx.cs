using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Ex10 : System.Web.UI.Page
{
    public DataSet ds;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Load_Data();
        }
        else { this.ds = (DataSet)Session["DataSet"]; }
    }
 
    private void ShowDataInGridView()
    {


        this.CityTable.DataSource = ds.Tables["TblCity"];
        this.CityTable.DataBind();

        this.People.DataSource = ds.Tables["TblPeople"];
        this.People.DataBind();

        DropDownListCity.DataSource = ds.Tables["TblCity"];
        DropDownListCity.DataTextField = "cityname";
        DropDownListCity.DataValueField = "CityId";
        DropDownListCity.DataBind();

    }
    private void Load_Data()
    {
        try
        {
            BuildTables bt = new BuildTables();
            this.ds = bt.BuildDataSet();
            Page.Session["DataSet"] = ds;
            ShowDataInGridView();
            ShowinTextBox();
            ShowinGVCityPerson();
            BuildCityPerson(DropDownListCity.SelectedValue);
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }
       

    }

    protected void TextBoxShow_Click(object sender, EventArgs e)
    {
        ShowinTextBox();
    }
    private void ShowinTextBox()
    {
        DataRow dr;
        string output = "";
        for (int i = 0; i < ds.Tables["TblPeople"].Rows.Count; i++)
        {
            dr = ds.Tables["TblPeople"].Rows[i];

            output += dr["IDcode"] + "\t" +
                dr["FName"] + "\t" +
                dr["LName"] + "\t" +
                dr["Adress"] + "\t" +
                (dr.GetParentRow("City_People")["CityName"]).ToString() + "\n";
        }

        ShowData.Text = output;
    }

    private void ShowinGVCityPerson()
    {
        DataTable cityPerson = new DataTable();
        try
        {
            cityPerson.Columns.Add(new DataColumn("Id"));
            cityPerson.Columns.Add(new DataColumn("Fname"));
            cityPerson.Columns.Add(new DataColumn("CityName"));

            for (int i = 0; i < ds.Tables["TblPeople"].Rows.Count; i++)
            {
                DataRow currectRow = ds.Tables["TblPeople"].Rows[i];
                DataRow NewRow = cityPerson.NewRow();

                NewRow["Id"] = currectRow["IDcode"];
                NewRow["Fname"] = currectRow["FName"];
                NewRow["CityName"] = currectRow.GetParentRow("City_People")["CityName"];

                cityPerson.Rows.Add(NewRow);

            }
            this.CityPersonGriedview.DataSource = cityPerson;
            this.CityPersonGriedview.DataBind();
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
        }
    }
    protected void cityPersonGV_Click(object sender, EventArgs e)
    {
        ShowinGVCityPerson(); 
    }
    private void BuildCityPerson(string cityId)
    {
         DataTable cityPerson = new DataTable();
         DataRow city = ds.Tables["TblCity"].Rows.Find(cityId);
         try
         {

             cityPerson.Columns.Add(new DataColumn("Id"));
             cityPerson.Columns.Add(new DataColumn("Fname"));
             cityPerson.Columns.Add(new DataColumn("CityName"));

             DataRow[] personDe = city.GetChildRows("City_People");

             for (int i = 0; i < personDe.Length; i++)
             {
                 DataRow currect = personDe[i];
                 DataRow NewRow = cityPerson.NewRow();

                 NewRow["Id"] = currect["IDcode"];
                 NewRow["Fname"] = currect["FName"];
                 NewRow["CityName"] = currect.GetParentRow("City_People")["CityName"];

                 cityPerson.Rows.Add(NewRow);
             }

             this.CityPersonGriedview.DataSource = cityPerson;
             this.CityPersonGriedview.DataBind();
         }
         catch (Exception ex)
         {
             error.Text = ex.Message;
         }
    }

    protected void DropDownListCity_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BuildCityPerson(DropDownListCity.SelectedValue);
    }
}