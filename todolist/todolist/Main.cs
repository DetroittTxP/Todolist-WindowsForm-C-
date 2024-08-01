using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace todolist
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            todogrid.CellClick += new DataGridViewCellEventHandler(todogrid_CellContentClick);


        }

        private void UserActionLabel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        public void LoadTask()
        {
            string iduser = LoginForm.loggedId.ToString();
            string sql = $"SELECT idtask,iduser,name,status FROM tasks WHERE iduser = {LoginForm.loggedId}";

            try
            {
                using (SqlConnection conn = new SqlConnection(LoginForm.conn_str))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        todogrid.DataSource = ds.Tables[0].DefaultView;

                        todogrid.Columns["idtask"].Visible = false;
                        todogrid.Columns["iduser"].Visible = false;

                        conn.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannt get your list");
                return;
            }


        }

        private void Main_Load(object sender, EventArgs e)
        {
            UserActionLabel.Text = LoginForm.loggedUsername.ToString();
            toolStripStatusLabel1.Text = $"UserID : {LoginForm.loggedId.ToString()}";

            LoadTask();

               
        }

        
        

        private void btnAdd_Click(object sender, EventArgs e)
        {

            string iduser = LoginForm.loggedId.ToString();
            
            string todo = txtAdd.Text;

            if(!String.IsNullOrEmpty(todo))
            {
                string sql = $"INSERT INTO tasks (iduser,name,status) VALUES ('{iduser}','{todo}','unfinish')";

                try
                {
                    using (SqlConnection conn = new SqlConnection(LoginForm.conn_str))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                           

                          
                        }
                    }
                    txtAdd.Clear();

                    LoadTask();
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"error : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please fill data in box");
                return;
            }
        }

        private void todogrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
               DataGridViewRow row = todogrid.Rows[e.RowIndex];

                string idtask = row.Cells["idtask"].Value.ToString();
                string status = row.Cells["status"].Value.ToString();
                string name = row.Cells["name"].Value.ToString();

                txtDelete.Text = idtask;
                txtUpdateName.Text = name;
                txtUpdateStatus.Text = status;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string idtask  = txtDelete.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(LoginForm.conn_str))
                {
                    string sql = $"DELETE FROM tasks WHERE idtask = {idtask}";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                         conn.Open();
                       cmd.ExecuteNonQuery();
                        conn.Close();

                        LoadTask();
                      
                    }
                }
                txtDelete.Clear();
                LoadTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
                return;
            }
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = txtUpdateName.Text; 
            string status = txtUpdateStatus.Text;
            string idtask = txtDelete.Text;


            try
            {
                using (SqlConnection conn = new SqlConnection(LoginForm.conn_str))
                {
                    string sql = $"UPDATE tasks SET name = '{name}' , status='{status}' WHERE idtask = {idtask} ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open(); cmd.ExecuteNonQuery(); conn.Close();
                    }
                }

                txtUpdateName.Clear();
                txtUpdateStatus.Clear();
                LoadTask();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}", $"ERROR", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

           
        }
    }
}
