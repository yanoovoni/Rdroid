using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components
        //InitializeComponent();
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public ContactDetails GetContactsByID(int IDmy)//הפעולה מחזירה פרטי איש קשר על פי מספרו בטבלה
    {
        UserService service = new UserService();
        return service.GetContactsByID(IDmy);
    }

    [WebMethod]
    public void InsertUser(UserDetails userDetails)//הפעולה מקבלת פרמטרים ומכניסה אותם לתוך טבלת משתמשים (בעצם יוצרים משתמש חדש)ו  
    {
        //FirstName, LastName, [Password], Email
        UserService service = new UserService();
        service.InsertUser(userDetails);
    }

    [WebMethod]
    public int EnterToSite(UserDetails userDetails)//נותן למשתשמש להתחבר לאתר על פי האימייל והסיסמא
    {
        UserService service = new UserService();
        return service.EnterToSite(userDetails);
    }

    //[WebMethod]
    //public DataSet GetFriends(UserDetails user)
    //{
    //    UserService service = new UserService();
    //    return service.GetFriends(user);
    //}

    [WebMethod]
    public DataSet GetContacts(UserDetails user)
    {
        UserService service = new UserService();
        return service.GetContacts(user);
    }

    [WebMethod]
    public DataSet GetFriendsAndContacts(UserDetails user)// הפעולה מחזירה "דטה סט" בו נמצאים גם החברים וגם אנשי הקשר
    {
        UserService service = new UserService();
        return service.GetFriendsAndContacts(user);
    }

    [WebMethod]
    public bool IfContactExist(ContactDetails contactDetais)//הפעולה בודקת האם איש הקשר כבר קיים
    {
        UserService service = new UserService();
        return service.IfContactExist(contactDetais);
    }

    [WebMethod]
    public void InsertContact(ContactDetails contactDetais)//הפעולה מאפשרת להוסיף מידע לתוך טבלת אנשי הקשר
    {
        UserService service = new UserService();
        service.IfContactExist(contactDetais);
    }


    [WebMethod]
    public DataSet FindFriends(string firstName, string lastName)//הפעולה מחזירה משתמש על פי שם מלא
    {
        UserService service = new UserService();
        return service.FindFriends(firstName, lastName);
    }

    [WebMethod]
    public void InsertFriend(FriendDetails friendDetais)//הפעולה מאפשרת להוסיף מידע לתוך טבלת חברים
    {
        UserService service = new UserService();
        service.InsertFriend(friendDetais);
    }




}
