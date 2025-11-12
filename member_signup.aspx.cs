using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

public partial class member_signup : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            generateMemberID();
        }
    }
    private void generateMemberID()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(CAST(SUBSTRING(member_id, 3, LEN(member_id)-2) AS INT)), 0) FROM member_master_tbl", con);
            int lastID = Convert.ToInt32(cmd.ExecuteScalar());
            int newID = lastID + 1;
            string newMemberID = "M-" + newID.ToString("D4");

            txtMemberID.Text = newMemberID;

            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error generating Member ID: " + ex.Message;
        }
    }

    // --- NEW: Password Validation Function ---
    private bool IsPasswordValid(string password)
    {
        // Minimum 8 characters
        if (password.Length < 8) return false;
        // At least one uppercase letter (A-Z)
        if (!Regex.IsMatch(password, "[A-Z]")) return false;
        // At least one lowercase letter (a-z)
        if (!Regex.IsMatch(password, "[a-z]")) return false;
        // At least one digit (0-9)
        if (!Regex.IsMatch(password, "[0-9]")) return false;
        // At least one special character
        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]")) return false;

        return true;
    }

    private void clearFields()
    {
        txtAddress.Text = "";
        txtContact.Text = "";
        txtEmail.Text = "";
        txtFullName.Text = "";
        txtPassword.Text = "";
        txtPincode.Text = "";
        
    }

    protected void btnSignUp_Click(object sender, EventArgs e)
    {
        // Check Password Strength
        if (!IsPasswordValid(txtPassword.Text.Trim()))
        {
            lblMessage.Text = "Password must be at least 8 characters long and contain uppercase, lowercase, number, and a special character.";
            return;
        }

        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM member_master_tbl WHERE member_id = @member_id", con);
            checkCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            int memberCount = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (memberCount > 0)
            {
                lblMessage.Text = "Member ID already exists. Please try again or contact support.";
                con.Close();
                return;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl(member_id, full_name, contact_no, email, password, pincode, full_address, account_status) VALUES(@member_id, @full_name, @contact_no, @email, @password, @pincode, @full_address, 'active')", con);

            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@full_name", txtFullName.Text.Trim());
            cmd.Parameters.AddWithValue("@contact_no", txtContact.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@pincode", txtPincode.Text.Trim());
            cmd.Parameters.AddWithValue("@full_address", txtAddress.Text.Trim());

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Sign up successful! Your Member ID is: " + txtMemberID.Text.Trim() + ". You can now login.";
            clearFields();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error during sign up: " + ex.Message;
        }
    }
}