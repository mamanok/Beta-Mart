using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSAccess_SMART_CRUD_
{
    public partial class Form1 : Form
    {

        private string id = "";
        private int row = 0; 

        public Form1()
        {
            InitializeComponent();
            resetMe();
        }

        private void resetMe()
        {
            this.id = "";
            namabarangtextBox.Text = "";
            kodebarangtextBox.Text = "";
            stokbarangtextBox.Text = "";
            if (satuancomboBox.Items.Count > 0)
            {
                satuancomboBox.SelectedIndex = 0;
            }
            hargabarangtextBox.Text = "";

            updatebutton.Text = "UPDATE ()";
            deletebutton.Text = "DELETE ()";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new OleDbCommand(mySQL, CRUD.con);
            AddParameters(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void AddParameters(string str)
        {
            CRUD.cmd.Parameters.Clear();

            if (str == "Delete" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue(id, this.id);
            }

            CRUD.cmd.Parameters.AddWithValue("Nama Barang", namabarangtextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("Kode Barang", kodebarangtextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("Stok Barang", stokbarangtextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("Satuan Barang", satuancomboBox.SelectedItem.ToString().Trim());
            CRUD.cmd.Parameters.AddWithValue("Harga Barang", hargabarangtextBox.Text.Trim());

            if (str == "Update" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue(id, this.id);
            }
        }

        private void insertbutton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(namabarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(kodebarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(stokbarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(hargabarangtextBox.Text.Trim()) )
            {
                MessageBox.Show("Please Insert The Data","Insert Data : Rahman Sucipto",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "INSERT INTO tb_smart_crud(nama_brg, kode_brg, stok, satuan, harga) VALUES(@namabarang, @kodebarang, @stokbarang, @satuanbarang, @hargabarang)";
            execute(CRUD.sql, "Insert");

            MessageBox.Show("The Record Has Been Saved","Insert Data : Rahman Sucipto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");
            resetMe();
        }

        private void loadData(string keyword)
        {
            CRUD.sql = "SELECT auto_id, nama_brg, kode_brg, stok, satuan, harga FROM tb_smart_crud ORDER BY auto_id ASC";
            string strKeyword = string.Format("%{0}%", keyword);

            CRUD.cmd = new OleDbCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("keyword1", strKeyword);
            CRUD.cmd.Parameters.AddWithValue("keyword2", keyword);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            if(dt.Rows.Count > 0)
            {
                row = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                row = 0;
            }

            toolStripStatusLabel1.Text = "Number of row(s): " + row.ToString();

            DataGridView dgv = dataGridView1;

            dgv.MultiSelect = false;
            dgv.AutoGenerateColumns = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.DataSource = dt;
            dgv.Columns[0].HeaderText = "ID";
            dgv.Columns[1].HeaderText = "Nama Barang";
            dgv.Columns[2].HeaderText = "Kode Barang";
            dgv.Columns[3].HeaderText = "Stok";
            dgv.Columns[4].HeaderText = "Satuan";
            dgv.Columns[5].HeaderText = "Harga";

            dgv.Columns[0].Width = 90;
            dgv.Columns[1].Width = 228;
            dgv.Columns[2].Width = 129;
            dgv.Columns[3].Width = 125;
            dgv.Columns[4].Width = 125;
            dgv.Columns[5].Width = 125;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridView dgv = dataGridView1;

                this.id = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                updatebutton.Text = "UPDATE (" + this.id + ")";
                deletebutton.Text = "DELETE (" + this.id + ")";

                namabarangtextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[1].Value).Trim();
                kodebarangtextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[2].Value).Trim();
                stokbarangtextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[3].Value).Trim();
                satuancomboBox.SelectedItem = Convert.ToString(dgv.CurrentRow.Cells[4].Value).Trim();
                hargabarangtextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[5].Value).Trim();
            }
        }

        private void updatebutton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Please Select Item From the List", "Update Data : Rahman Sucipto",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(namabarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(kodebarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(stokbarangtextBox.Text.Trim()) ||
                string.IsNullOrEmpty(hargabarangtextBox.Text.Trim()))
            {
                MessageBox.Show("Please Insert The Data", "Update Data : Rahman Sucipto",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CRUD.sql = "UPDATE tb_smart_crud SET nama_brg = @namabarang, kode_brg = @kodebarang, stok = @stokbarang, satuan = @satuanbarang, harga = @hargabarang WHERE auto_id = @id";
            execute(CRUD.sql, "Update");

            MessageBox.Show("The Record Has Been Updated", "Update Data : Rahman Sucipto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");
            resetMe();
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Please Select Item From the List", "Delete Data : Rahman Sucipto",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Do You Want To Delete The Selected Record?", "Delete Data : Rahman Sucipto",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CRUD.sql = "DELETE FROM tb_smart_crud WHERE auto_id = @id";
                execute(CRUD.sql, "Delete");

                MessageBox.Show("The Record Has Been Deleted", "Delete Data : Rahman Sucipto",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadData("");
                resetMe();
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            resetMe();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void satuancomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void hargabarangtextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void keywordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void namabarangtextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
