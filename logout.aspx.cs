using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Session ko clear aur abandon karo
        Session.Clear();
        Session.Abandon();
        // User ko login page par redirect karo
        Response.Redirect("adminlogin.aspx");
    }
}