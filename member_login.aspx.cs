using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class member_login : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        // You can add a session check here to prevent logged-in users from accessing this page
        if (Session["role"] != null && Session["role"].ToString() == "member")
        {
            Response.Redirect("member_dashboard.aspx");
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id=@member_id AND password=@password", con);
            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                // Set session variables for the logged-in member
                Session["member_id"] = txtMemberID.Text.Trim();
                Session["full_name"] = dt.Rows[0]["full_name"].ToString();
                Session["role"] = "member";

                // Redirect to the member dashboard
                Response.Redirect("member_dashboard.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid credentials!";
            }
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "An error occurred: " + ex.Message;
        }
    }
}