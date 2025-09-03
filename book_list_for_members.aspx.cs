using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class book_list_for_members : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["role"] == null || Session["role"].ToString() != "member")
        {
            Response.Redirect("member_login.aspx");
        }

        if (!IsPostBack)
        {
            bindBooksGridView();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string searchQuery = txtSearchBook.Text.Trim();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            bindBooksGridView(searchQuery);
        }
        else
        {
            bindBooksGridView();
        }
    }

    private void bindBooksGridView(string searchTerm = null)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string query = "SELECT book_name, author_name, publisher_name, language, current_stock FROM book_master_tbl WHERE book_name LIKE @searchTerm OR author_name LIKE @searchTerm OR genre LIKE @searchTerm";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
            }
            else
            {
                cmd = new SqlCommand("SELECT book_name, author_name, publisher_name, language, current_stock FROM book_master_tbl", con);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridViewBooks.DataSource = dt;
            GridViewBooks.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            // Log or display error
        }
    }
}