using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;


namespace todolist
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public static string loggedUsername = "";
        public static int loggedId = -1;
        public static string conn_str = "Server=DESKTOP-E8F1SPO\\SQLEXPRESS;Database=Todolist;Trusted_Connection=True;TrustServerCertificate=True";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string hashed = RegisterForm.HashPassword(password);

            try
            {
                using (SqlConnection conn = new SqlConnection(conn_str))
                {
                    string query = "SELECT * FROM users WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashed);
                       
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable dtable = new DataTable();

                        da.Fill(dtable);

                        if (dtable.Rows.Count > 0)
                        {
                            loggedUsername = username;
                            loggedId = Convert.ToInt32(dtable.Rows[0]["iduser"]);
                            Main mainForm = new Main();
                            mainForm.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Login failed. Please check your username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtUsername.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm rForm = new RegisterForm();
            rForm.Show();
            this.Hide();
        }
    }
}
