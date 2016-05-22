using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;

/// <summary>
/// Summary description for BuildTables
/// </summary>
public class BuildTables
{

	public BuildTables()
	{
      

	}

    protected DataTable createCityTbl()
    {
        DataTable tblCities = new DataTable("TblCity");//מגדיר טבלה

        DataColumn col1 = new DataColumn();
        col1.ColumnName = "cityname";
        col1.DataType = typeof(System.String);
        col1.Unique = false;
        col1.MaxLength = 25;

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "CityID";
        col2.DataType = typeof(System.Int32);
        col2.Unique = true;

        tblCities.Columns.Add(col1);//מוסיף עמודה עם שם עיר
        tblCities.Columns.Add(col2);//מוסיף עמודה עם קוד עיר

        DataColumn[] arr = new DataColumn[1];
        arr[0] = tblCities.Columns["cityID"];
        tblCities.PrimaryKey = arr;

        return tblCities;
    }
    private void CreateRelation(DataSet ds)
    {
        DataColumn Parentcol;
        DataColumn childcol;

        Parentcol = ds.Tables["TblCity"].Columns["CityID"];
        childcol = ds.Tables["TblPeople"].Columns["CityID"];

        DataRelation relcitypeople;

        relcitypeople = new DataRelation("City_People", Parentcol, childcol);
        ds.Relations.Add(relcitypeople);
    }

    protected void AddCity(DataTable tblCity, int cityId, string cityName)
    {
        DataRow dr;
        dr = tblCity.NewRow();
        dr["cityname"] = cityName;
        dr["CityID"] = cityId;
        tblCity.Rows.Add(dr);
    }

    protected void FillDataTable(DataTable tblCity)
    {
        AddCity(tblCity, 1, "Tel-Aviv");
        AddCity(tblCity, 2, "Netanya");
        AddCity(tblCity, 3, "Tiberis");

    }
    public DataSet BuildDataSet()
    {
        DataSet ds = new DataSet();


        DataTable dataTbl = this.createCityTbl();
        ds.Tables.Add(dataTbl);
        this.FillDataTable(dataTbl);

        DataTable data2 = CreatePeopleTbl();
        ds.Tables.Add(data2);
        this.FillDataTablePeople(data2);

       CreateRelation(ds);

        return ds;
    }

    protected DataTable CreatePeopleTbl()
    {
        //קוד מזהה,שם פרטי\ שם משפחה, כתובת\קוד עיר
        DataTable tblPeople = new DataTable("TblPeople");//מגדיר טבלה

        DataColumn col1 = new DataColumn();
        col1.ColumnName = "IDcode";
        col1.DataType = typeof(System.Int32);
        col1.Unique = true;

        DataColumn col2 = new DataColumn();
        col2.ColumnName = "FName";
        col2.DataType = typeof(System.String);
        col2.Unique = false;
        col2.MaxLength = 25;

        DataColumn col3 = new DataColumn();
        col3.ColumnName = "LName";
        col3.DataType = typeof(System.String);
        col3.Unique = false;
        col3.MaxLength = 25;

        DataColumn col4 = new DataColumn();
        col4.ColumnName = "Adress";
        col4.DataType = typeof(System.String);
        col4.Unique = false;
        col4.MaxLength = 50;

        DataColumn col5 = new DataColumn();
        col5.ColumnName = "CityID";
        col5.DataType = typeof(System.Int32);
        col5.Unique = false;

        tblPeople.Columns.Add(col1);
        tblPeople.Columns.Add(col2);
        tblPeople.Columns.Add(col3);
        tblPeople.Columns.Add(col4);
        tblPeople.Columns.Add(col5);

        DataColumn[] arr = new DataColumn[1];
        arr[0] = tblPeople.Columns["IDcode"];
        tblPeople.PrimaryKey = arr;

        return tblPeople;
    }

    protected void AddPeople(DataTable tblPeople, int Idcode, string Fname,string LName, string Adress, int CityID)
    {
        DataRow dr;
        dr = tblPeople.NewRow();
        dr["FName"] = Fname;
        dr["IDcode"] = Idcode;
        dr["LName"] = LName;
        dr["Adress"] = Adress;
        dr["CityId"] = CityID;
        tblPeople.Rows.Add(dr);
        
    }
    protected void FillDataTablePeople(DataTable tblPeople)
    {
            AddPeople(tblPeople,123456789, "Ariela","Goncherook","Raziel" ,1);
            AddPeople(tblPeople, 100000000, "Liel", "hkhfskf", "fkhl", 2);
            AddPeople(tblPeople, 111111111, "Ariela", "ouhyeof", "Rfeiel", 3);
        
    }
}