using System;
using System.Data;
using System.Data.SqlClient;

namespace PLS__C_sharp
{

    using System.Data.SqlClient;
    public partial class MainForm : Form
    {
        const int lastColumnNum = 9;

        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;


        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\LERN\C#\PLS  C-sharp\PLS  C-sharp\Database1.mdf"";Integrated Security=True");
            sqlConnection.Open();

            LoadData();
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadData();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == lastColumnNum)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[lastColumnNum].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Remove this string?","Removal",MessageBoxButtons.YesNo,MessageBoxIcon.Question) 
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Patients"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Patients");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Patients"].NewRow();

                        row["LastName"] = dataGridView1.Rows[rowIndex].Cells["LastName"].Value;
                        row["FirstName"] = dataGridView1.Rows[rowIndex].Cells["FirstName"].Value;
                        row["MiddleName"] = dataGridView1.Rows[rowIndex].Cells["MiddleName"].Value;
                        row["Birthday"] = dataGridView1.Rows[rowIndex].Cells["Birthday"].Value;
                        row["Height"] = dataGridView1.Rows[rowIndex].Cells["Height"].Value;
                        row["Weight"] = dataGridView1.Rows[rowIndex].Cells["Weight"].Value;
                        row["BloodGroup"] = dataGridView1.Rows[rowIndex].Cells["BloodGroup"].Value;
                        row["Diagnoses"] = dataGridView1.Rows[rowIndex].Cells["Diagnoses"].Value;

                        dataSet.Tables["Patients"].Rows.Add(row);

                        dataSet.Tables["Patients"].Rows.RemoveAt(dataSet.Tables["Patients"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count -2);

                        dataGridView1.Rows[e.RowIndex].Cells[lastColumnNum].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Patients");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Patients"].Rows[r]["LastName"] = dataGridView1.Rows[r].Cells["LastName"].Value;
                        dataSet.Tables["Patients"].Rows[r]["FirstName"] = dataGridView1.Rows[r].Cells["FirstName"].Value;
                        dataSet.Tables["Patients"].Rows[r]["MiddleName"] = dataGridView1.Rows[r].Cells["MiddleName"].Value;
                        dataSet.Tables["Patients"].Rows[r]["Birthday"] = dataGridView1.Rows[r].Cells["Birthday"].Value;
                        dataSet.Tables["Patients"].Rows[r]["Height"] = dataGridView1.Rows[r].Cells["Height"].Value;
                        dataSet.Tables["Patients"].Rows[r]["Weight"] = dataGridView1.Rows[r].Cells["Weight"].Value;
                        dataSet.Tables["Patients"].Rows[r]["BloodGroup"] = dataGridView1.Rows[r].Cells["BloodGroup"].Value;
                        dataSet.Tables["Patients"].Rows[r]["Diagnoses"] = dataGridView1.Rows[r].Cells["Diagnoses"].Value;

                        sqlDataAdapter.Update(dataSet, "Patients");

                        dataGridView1.Rows[e.RowIndex].Cells[lastColumnNum].Value = "Delete";
                    }
                    
                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[lastColumnNum, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[lastColumnNum, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("Select *, 'Delete' AS [Command] FROM Patients", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Patients");

                dataGridView1.DataSource = dataSet.Tables["Patients"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[lastColumnNum, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ReloadData()
        {
            try
            {
                dataSet.Tables["Patients"].Clear();

                sqlDataAdapter.Fill(dataSet, "Patients");

                dataGridView1.DataSource = dataSet.Tables["Patients"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[lastColumnNum, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 5 || dataGridView1.CurrentCell.ColumnIndex == 6)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PLS (Patients List Consructor) - program for work with lists of patients in Hospital. " +
                "Enjoy using!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int32.Parse(textBox1.Text);
                
                
            }
            catch (FormatException)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"{comboBox2.Text} LIKE '%{textBox1.Text}%'";
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"{comboBox2.Text} = {textBox1.Text}";
            }
        }
    }
}