using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var homeLink = (HtmlAnchor)FindControl("homeLink");

        if (Session["role"] != null)
        {
            // User is logged in
            if (Session["role"].ToString() == "admin")
            {
                // Redirect to Admin Dashboard on Home click
                homeLink.HRef = "admindashboard.aspx";
                liAdminLogin.Visible = false;
                liMemberLogin.Visible = false;
                liHelloUser.Visible = true;
                liLogout.Visible = true;
                lblUsername.Text = "Hello, Admin!";
                liAdminDashboard.Visible = true;
            }
            else if (Session["role"].ToString() == "member")
            {
                // Redirect to Member Dashboard on Home click
                homeLink.HRef = "member_dashboard.aspx";
                liAdminLogin.Visible = false;
                liMemberLogin.Visible = false;
                liHelloUser.Visible = true;
                liLogout.Visible = true;
                lblUsername.Text = "Hello, " + Session["full_name"].ToString() + "!";
                liAdminDashboard.Visible = false;
            }
        }
        else
        {
            // User is not logged in, Home link goes to the default homepage
            homeLink.HRef = "homepage.aspx";
            liAdminLogin.Visible = true;
            liMemberLogin.Visible = true;
            liHelloUser.Visible = false;
            liLogout.Visible = false;
            liAdminDashboard.Visible = false;
        }
    }
}
