namespace StorageManager.App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DbConnectionForm.InitDbConnection();
        }

        private void zarzadzanieKolekcjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StorageItemForm childForm = new StorageItemForm();
            OpenForm(childForm);


        }

        private void OpenForm(Form childForm)
        {
            childForm.TopLevel = false;

            childForm.FormBorderStyle = FormBorderStyle.None;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(childForm);

            childForm.Dock = DockStyle.Fill;

            childForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StorageItemForm childForm = new StorageItemForm();
            OpenForm(childForm);
        }

    }
}
