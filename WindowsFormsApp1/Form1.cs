using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;

namespace FinalProjesi
{
    public partial class frmKitaplik : Form
    {
        public frmKitaplik()
        {
            InitializeComponent();
        }

        string conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Vu4ll\\Desktop\\FinalProjesi-237017041\\WindowsFormsApp1\\Kutuphane.accdb";
        OleDbConnection con;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        private void frmKitaplik_Load(object sender, EventArgs e)
        {
            GetBooks();
            btnEkle.Enabled = false;
            btnIptal.Enabled = false;

            // ClearControls();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text;
            string yazar = txtYazar.Text;
            string yayinevi = txtYayinevi.Text;
            string tur = cboxTur.SelectedItem?.ToString();
            int yil = (int)numYil.Value;
            int sayfaSayisi = (int)numSayfaSayisi.Value;

            if (ad == string.Empty || yazar == string.Empty || yayinevi == string.Empty ||
                tur == null || yil <= 0 || sayfaSayisi <= 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con = new OleDbConnection(conString);
            string query = "INSERT INTO Kitaplar (adi, yazarAdi, yayinevi, yil, tur, sayfaSayisi) VALUES " +
                "(@ad, @yazar, @yayinevi, @yil, @tur, @sayfasayisi)";
            cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@ad", ad);
            cmd.Parameters.AddWithValue("@yazar", yazar);
            cmd.Parameters.AddWithValue("@yayinevi", yayinevi);
            cmd.Parameters.AddWithValue("@yil", yil);
            cmd.Parameters.AddWithValue("@tur", tur);
            cmd.Parameters.AddWithValue("@sayfasayisi", sayfaSayisi);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            GetBooks();
            ClearControls();

            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;
            btnYeni.Enabled = true;
            btnEkle.Enabled = false;

            MessageBox.Show("Yeni kayıt başarıyla eklendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text;
            string yazar = txtYazar.Text;
            string yayinevi = txtYayinevi.Text;
            string tur = cboxTur.SelectedItem?.ToString();
            int yil = (int)numYil.Value;
            int sayfaSayisi = (int)numSayfaSayisi.Value;

            if (ad == string.Empty || yazar == string.Empty || yayinevi == string.Empty ||
                tur == null || yil <= 0 || sayfaSayisi <= 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con = new OleDbConnection(conString);
            string query = "UPDATE Kitaplar SET " +
                "adi=@ad, yazarAdi=@yazar, yayinevi=@yayinevi, yil=@yil, tur=@tur, sayfaSayisi=@sayfasayisi " +
                "WHERE id=@id";
            cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@ad", ad);
            cmd.Parameters.AddWithValue("@yazar", yazar);
            cmd.Parameters.AddWithValue("@yayinevi", yayinevi);
            cmd.Parameters.AddWithValue("@yil", yil);
            cmd.Parameters.AddWithValue("@tur", tur);
            cmd.Parameters.AddWithValue("@sayfasayisi", sayfaSayisi);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            GetBooks();
            ClearControls();
            MessageBox.Show("Kayıt başarıyla güncellendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            con = new OleDbConnection(conString);
            string query = "DELETE FROM Kitaplar WHERE id=@id";
            cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            GetBooks();
            ClearControls();
            MessageBox.Show("Kayıt başarıyla silindi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            btnYeni.Enabled = false;
            btnEkle.Enabled = true;
            btnIptal.Enabled = true;
            ClearControls();
        }

        void ClearControls()
        {
            dgvKitaplar.ClearSelection();
            txtYayinevi.Clear();
            txtYazar.Clear();
            txtAd.Clear();
            numSayfaSayisi.Value = numSayfaSayisi.Minimum;
            cboxTur.SelectedIndex = -1;
            numYil.Value = numYil.Minimum;
        }

        void GetBooks()
        {
            con = new OleDbConnection(conString);
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Kitaplar ORDER BY id", con);
            con.Open();
            adapter.Fill(dt);
            dgvKitaplar.DataSource = dt;
            con.Close();
        }

        private void dgvKitaplar_SelectionChanged(object sender, EventArgs e)
        {
            
            txtID.Text = dgvKitaplar.CurrentRow.Cells[0].Value.ToString();
            txtAd.Text = dgvKitaplar.CurrentRow.Cells[1].Value.ToString();
            txtYazar.Text = dgvKitaplar.CurrentRow.Cells[2].Value.ToString();
            txtYayinevi.Text = dgvKitaplar.CurrentRow.Cells[3].Value.ToString();
            numYil.Value = Convert.ToInt32(dgvKitaplar.CurrentRow.Cells[4].Value);
            cboxTur.Text = dgvKitaplar.CurrentRow.Cells[5].Value.ToString();
            numSayfaSayisi.Value = Convert.ToInt32(dgvKitaplar.CurrentRow.Cells[6].Value);
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            btnEkle.Enabled = false;
            btnIptal.Enabled = false;
            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;
            btnYeni.Enabled = true;
        }
    }
}
