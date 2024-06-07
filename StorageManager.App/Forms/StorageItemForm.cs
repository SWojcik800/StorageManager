using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StorageManager.App.Features.Storage;

namespace StorageManager.App
{
    public partial class StorageItemForm : Form
    {
        List<StorageItem> Items = new List<StorageItem>();
        StorageItem EditedItem = new StorageItem();
        public StorageItemForm()
        {
            InitializeComponent();
            Text = "Storage manager";

            this.storageItemBindingSource.DataSource = Items;
            this.editedItemBindingSource.DataSource = EditedItem;
            this.dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            RefreshItems();
        }

        private void DataGridView1_SelectionChanged(object? sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                var item = (StorageItem)this.dataGridView1.SelectedRows[0].DataBoundItem;
                EditedItem.Read(item.Id);
                this.editedItemBindingSource.ResetCurrentItem();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                Cursor.Current = Cursors.WaitCursor;
                EditedItem.Save();
                EditedItem.Read(EditedItem.Id);
                var savedItemId = EditedItem.Id;
                this.editedItemBindingSource.ResetCurrentItem();
                RefreshItems();

                Cursor.Current = Cursors.WaitCursor;

                dataGridView1.ClearSelection();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var item = (StorageItem)row.DataBoundItem;
                    if (item.Id == savedItemId)
                        row.Selected = true;
                }
            }

        }

        private void RefreshItems()
        {
            Items = StorageItem.ReadItems();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Items;
            //this.dataGridView1.Refresh();
            //Data.Clear();
            //foreach (var item in StorageItem.ReadItems())
            //{
            //    Data.Add(item);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditedItem.ResetToDefault();
            this.editedItemBindingSource.ResetCurrentItem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Wybór zdjęcia";
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    var image = new Bitmap(dlg.FileName);
                    var content = File.ReadAllBytes(dlg.FileName);
                    EditedItem.Image = Convert.ToBase64String(content);
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                e.Cancel = true;
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "To pole jest wymagane");
                return;
            }

            if (textBox1.Text.Length > 100)
            {
                e.Cancel = true;
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Przekroczono maksymalną ilość znaków dla tego pola");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(textBox1, "");

        }

        private void richTextBox1_Validating(object sender, CancelEventArgs e)
        {
            if (richTextBox1.Text.Length > 520)
            {
                e.Cancel = true;
                richTextBox1.Focus();
                errorProvider1.SetError(richTextBox1, "Przekroczono maksymalną ilość znaków dla tego pola");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(richTextBox1, "");
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (richTextBox1.Text.Length > 100)
            {
                e.Cancel = true;
                textBox2.Focus();
                errorProvider1.SetError(textBox2, "Przekroczono maksymalną ilość znaków dla tego pola");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(textBox2, "");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshItems();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditedItem.Image))
                return;

            ShowImageForm.Show(EditedItem.DisplayImage);

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            //DbConnectionForm.InitDbConnection();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DbConnectionForm.InitDbConnection();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            var result = MessageBox.Show("Czy napewno chcesz usunąć zaznaczony wpis?", "Usuwanie danych", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                var row = (StorageItem)dataGridView1.SelectedRows[0].DataBoundItem;
                row.Delete();
                RefreshItems();
                Cursor.Current = Cursors.Default;

            }
        }
    }
}
