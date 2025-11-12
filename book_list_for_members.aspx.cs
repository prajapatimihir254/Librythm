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
        // Security check remains
        if (Session["role"] == null || Session["role"].ToString() != "member")
        {
            Response.Redirect("member_login.aspx");
        }

        if (!IsPostBack)
        {
            bindBooksGridView();
        }
        // Server-side search button click event hata diya gaya hai.
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // When the Search button is clicked, filter the data based on the input text.
        bindBooksGridView(txtSearchBook.Text.Trim());
    }
    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    string searchQuery = txtSearchBook.Text.Trim();
    //    if (!string.IsNullOrEmpty(searchQuery))
    //    {
    //        bindBooksGridView(searchQuery);
    //    }
    //    else
    //    {
    //        bindBooksGridView();
    //    }
    //}

    // Helper function to bind data to GridView
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

            // Base query selects all required fields including description
            string baseQuery = "SELECT book_name, author_name, publisher_name, language, current_stock, book_description FROM book_master_tbl";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // If a search term is provided, modify the query to filter results
                string filterQuery = baseQuery +
                    " WHERE book_name LIKE @SearchTerm OR " +
                    " author_name LIKE @SearchTerm OR " +
                    " publisher_name LIKE @SearchTerm OR " +
                    " book_description LIKE @SearchTerm";

                cmd = new SqlCommand(filterQuery, con);
                // Use % signs to find the search term anywhere in the specified columns
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
            }
            else
            {
                // If no search term, use the base query to show all books
                cmd = new SqlCommand(baseQuery, con);
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
            // You may want to display a generic error message here
        }
    }
}