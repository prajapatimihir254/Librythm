using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class member_dashboard : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["role"] == null || Session["role"].ToString() != "member")
        {
            Response.Redirect("member_login.aspx");
        }
        else
        {
            lblMemberName.Text = Session["full_name"].ToString();
            if (!IsPostBack)
            {
                bindIssuedBooksGridView();
            }
        }
    }
    private void bindIssuedBooksGridView()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT book_name, issue_date, due_date FROM book_issue_tbl WHERE member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", Session["member_id"].ToString());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridViewIssuedBooks.DataSource = dt;
            GridViewIssuedBooks.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            // In a production environment, you would log this error.
        }
    }
}