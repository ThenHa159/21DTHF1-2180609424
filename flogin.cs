using QuanLyNhaHangVKAT.DAO;
using QuanLyNhaHangVKAT.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHangVKAT
{
    public partial class flogin : Form
    {
        public flogin()
        {
            InitializeComponent();
        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            string userName = txtusername.Text;
            string passWord = txtpassword.Text;
            if (login(userName, passWord))
            {
                Account loginAccount = AccountProvider.Instance.GetAccountByUserName(userName);
                fTableManager f = new fTableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();         // chỉ hiện 1 giao diện sử dụng
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu! ", "Thông báo");
            }
        }
        bool login ( string userName, string passWord)
        {
            return AccountProvider.Instance.login(userName, passWord);  
        }

        private void flogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
