using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace todolist
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

       public static string HashPassword(string password)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string Hashedpassword = HashPassword(txtRegisterPassword.Text);

            using (SqlConnection conn = new SqlConnection(LoginForm.conn_str))
            {
                //checkdup
                string CheckSql = $"SELECT COUNT(1) FROM users WHERE username=@username";

                using (SqlCommand cmd = new SqlCommand(CheckSql,conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@username", username);

                    int userCount = (int)cmd.ExecuteScalar();

                    if(userCount > 0)
                    {
                        MessageBox.Show("Username duplicate");
                            return;
                    }
                    else
                    {
                        //insert
                        string Insertsql = "INSERT INTO users (username,password) VALUES(@username,@password)";

                        using(SqlCommand cmdInsert = new SqlCommand(Insertsql,conn))
                        {
                            cmdInsert.Parameters.AddWithValue("@username", username);
                            cmdInsert.Parameters.AddWithValue("@password", Hashedpassword);
                            cmdInsert.ExecuteNonQuery();
                            MessageBox.Show("Registration successful!");
                        }
                    }
                }
            }


        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
