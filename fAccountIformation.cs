﻿using QuanLyNhaHangVKAT.DAO;
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
    public partial class fAccountIformation : Form
    {
        private Account loginAccount; // các constructor

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; AccountProfile(LoginAccount);  } 
        }
        public fAccountIformation(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
        }
        void AccountProfile(Account acc) // hien thong tin
        {
            txtusername.Text = loginAccount.UserName;
            txtdisplayName.Text = loginAccount.DisplayName;
        }
        void updateAccount()
        {
            string userName = txtusername.Text;
            string displayName = txtdisplayName.Text;
            string passWord = txtPassword.Text;
            string newPass = txtNewPassword.Text;
            string reenterPass = txtReenterPass.Text;
            if (!newPass.Equals(reenterPass)) // so sánh chuỗi 2 cái có giống nhau hay không
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu giống với mật khẩu mới!", "Thông báp");
            }
            else
            {
                if(AccountProvider.Instance.UpdateAccountInformation(userName , displayName , passWord , newPass))
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu!, ko dien dung thi cut!", "Thông báo");
                }
            }

        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            updateAccount();
        }
    }
}
