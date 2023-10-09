using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySach.Models;

namespace QuanLySach
{
    public partial class Form1 : Form
    {
        ModelQLS model = new ModelQLS();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<LoaiSach> listLoaiSach = model.LoaiSach.ToList();  
            comboBoxTheLoai.DataSource = listLoaiSach;
            comboBoxTheLoai.DisplayMember = "TenLoai";
            comboBoxTheLoai.ValueMember = "MaLoai";

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dataGridViewMain.Rows.Clear();
                List<Sach> listSach = model.Sach.ToList();
                foreach (var item in listSach)
                {
                    int index = dataGridViewMain.Rows.Add();
                    dataGridViewMain.Rows[index].Cells[0].Value = item.MaSach;
                    dataGridViewMain.Rows[index].Cells[1].Value = item.TenSach;
                    dataGridViewMain.Rows[index].Cells[2].Value = item.NamXB;
                    dataGridViewMain.Rows[index].Cells[3].Value = item.LoaiSach.TenLoai;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi trong quá trình load data");
            }
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using(var transaction = model.Database.BeginTransaction())
            {
                try
                {
                    //kiểm tra sách đã tồn tại chưa
                    var ktSach = model.Sach.FirstOrDefault(p => p.MaSach == textBoxIDSach.Text);
                    if (ktSach != null)
                    {
                        MessageBox.Show("ID sách đã tồn tại!");
                        return;
                    }
                    else
                    {
                        Sach addSach = new Sach()
                        {
                            MaSach = textBoxIDSach.Text,
                            TenSach = textBoxTenSach.Text,
                            NamXB = int.Parse(textBoxNamXB.Text),
                            MaLoai = int.Parse(comboBoxTheLoai.SelectedValue.ToString())
                        };
                        model.Sach.Add(addSach);
                        model.SaveChanges();
                        MessageBox.Show("Thêm thành công!");
                        LoadData();
                        transaction.Commit();
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi trong quá trình thêm!");
                    transaction.Rollback();
                }
            }
        }

        private void dataGridViewMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridViewMain.Rows[e.RowIndex];
                    textBoxIDSach.Text = row.Cells[0].Value.ToString();
                    textBoxTenSach.Text = row.Cells[1].Value.ToString();
                    textBoxNamXB.Text = row.Cells[2].Value.ToString();
                    comboBoxTheLoai.Text = row.Cells[3].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private bool KiemTraSach(string IdSach)
        {
            var ktSach = model.Sach.FirstOrDefault(p => p.MaSach == IdSach);
            return ktSach != null;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            using(var transaction = model.Database.BeginTransaction())
            {
                try
                {
                    Sach updateSach = model.Sach.FirstOrDefault(p => p.MaSach == textBoxIDSach.Text);
                    updateSach.TenSach = textBoxTenSach.Text;
                    updateSach.NamXB = int.Parse(textBoxNamXB.Text);
                    updateSach.MaLoai = int.Parse(comboBoxTheLoai.SelectedValue.ToString());

                    model.SaveChanges();
                    LoadData();
                    MessageBox.Show("Sửa thành công!");
                    transaction.Commit();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi trong quá trình sửa!");
                    transaction.Rollback();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            using(var transaction = model.Database.BeginTransaction())
            {
                try
                {
                    Sach deleteSach = model.Sach.FirstOrDefault(p => p.MaSach == textBoxIDSach.Text);
                    if(deleteSach != null)
                    {
                        model.Sach.Remove(deleteSach);
                        model.SaveChanges();
                        MessageBox.Show("Xoá thành công!");
                        LoadData();
                        transaction.Commit();
                    }  
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi trong quá trình xoá!");
                    transaction.Rollback();
                }
            }
        }
    }
}
