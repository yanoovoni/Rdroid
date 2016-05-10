using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using localhostWebService;

public partial class SignUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Finnish_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            UserDetails user = new UserDetails();
            user.firstName = TextBox1.Text;
            user.lastName = TextBox2.Text;
            user.password = TextBox3.Text;
            user.email = TextBox4.Text;


            UserService userService = new UserService();
            try
            {
                userService.InsertUser(user);
            }
            catch (Exception ex)
            {
                this.LabelMassege.Text = ex.Message;
            }
        }

    }

    private void addYearsToDropDownList()
    {
        int startYear = DateTime.Now.Year;
        int endYear = DateTime.Now.Year - 100;

        for (int year = startYear; year > endYear; year--)
        {
            DropDownListBYears.Items.Add(year.ToString());
        }
    }

    private void addDaysToDropDownList(int daysInMonth)
    {
        DropDownListBDay.Items.Clear();
        for (int day = 1; day <= daysInMonth; day++)
        {
            DropDownListBDay.Items.Add(day.ToString());
        }
    }


    protected void DropDownListBMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        int month = int.Parse(DropDownListBMonth.SelectedValue);
        int year = int.Parse(DropDownListBYears.SelectedValue);
        int daysInMonth = DateTime.DaysInMonth(year, month);
        addDaysToDropDownList(daysInMonth);
    }
}