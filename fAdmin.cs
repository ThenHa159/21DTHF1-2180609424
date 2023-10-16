using QuanLyNhaHangVKAT.DAO;
using QuanLyNhaHangVKAT.DTO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLyNhaHangVKAT
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource categoryList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            dgvFood.DataSource = foodList; // binding food
            dgvAccount.DataSource = accountList;
            dgvCategory.DataSource = categoryList;  
            // chạy các methods
            loadListFood();
            FoodBinding();
            LoadAccount();
            AccountBinding();
            LoadCategory();
            CategoryBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
            

          
            
        }
        #region methods

        void loadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void FoodBinding() // hàm binding food , true là chấp nhận ép kiểu chuỗi cho nó , DataSourceUpdateMode.Never là không cho sửa đổi giá trị khi binding
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtFoodId.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void AccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayname.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            txtPassWord.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Password", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountProvider.Instance.GetListAccount();
        }
        
        void LoadCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void CategoryBinding()
        {
            txtCategoryName.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIdCategory.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
            cb.ValueMember = "id";
        }
        void loadListDoanhThu(DateTime checkIn , DateTime checkOut)
        {
            dgvThongKe.DataSource = BillDAO.Instance.GetListDoanhThu(checkIn , checkOut);
        }
        void clearFood()
        {
            txtFoodId.Clear();  
            txtFoodName.Clear();    

            nmFoodPrice.Accelerations.Clear();
        }
        void clearAccount()
        {
            txtUserName.Clear();
            txtDisplayname.Clear();
            txtPassWord.Clear();
            nmType.Accelerations.Clear();
        }
        void clearCategory()
        {
            txtCategoryName.Clear();    
            txtIdCategory.Clear();
        }


        #endregion

        #region events
        private void txtFoodName_TextChanged(object sender, EventArgs e)
        {

        }
        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }
        private void dgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btnFoodWatch_Click(object sender, EventArgs e)
        {
            loadListFood();
        }
        private void btnThongke_Click(object sender, EventArgs e)
        {
            loadListDoanhThu(dtIn.Value, dtOut.Value);
        }
        private void txtFoodId_TextChanged(object sender, EventArgs e) // Id food thay đổi sẽ thay đổi Category theo để binding thức ăn
        {
            try
            {
                if (dgvFood.SelectedCells.Count > 0) // nếu có dữ liệu sẽ truy cập vào dữ liệu đầu tiên [0] 
                {
                    DataGridViewCell selectedCell = dgvFood.SelectedCells[0];

                    if (selectedCell.OwningRow.Cells["idCategory"].Value != null) // ktra xem idCategory có khác null không , nếu không sẽ chọn dữ liệu để xuất ra
                    {
                        int categoryId = Convert.ToInt32(selectedCell.OwningRow.Cells["idCategory"].Value);
                        cbFoodCategory.SelectedValue = categoryId;
                    }
                }
            }
            catch
            {
                
            }    
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            clearFood();
            btnEditFood.Enabled = false;
            btnRemoveFood.Enabled = false;
            btnFoodWatch.Enabled = false;
            btnSearchFood.Enabled = false;
            IsAccessible = true; // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không
            MessageBox.Show("Vui lòng nhập thông tin món ăn", "Thông báo");
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            clearFood();
            btnAddFood.Enabled = false;
            btnRemoveFood.Enabled = false;
            btnFoodWatch.Enabled = false;
            btnSearchFood.Enabled = false;
            MessageBox.Show("Vui lòng chọn món ăn muốn cập nhật", "Thông báo");
        }

        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            if (txtFoodName.Text == "" || txtFoodId.Text == "")
            {
                MessageBox.Show("Vui lòng chọn món ăn muốn xóa!!!", "Thông báo");
            }
            else
            {
                int id = Convert.ToInt32(txtFoodId.Text);
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    FoodDAO.Instance.DeleteFood(id);
                    MessageBox.Show("Xóa món thành công");
                }
                loadListFood();
            }
          
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string name = txtSearchFood.Text;
            try
            {
                foodList.DataSource = FoodDAO.Instance.SearchFoodByName(name);
            }
            catch 
            {
                MessageBox.Show("Không tìm thấy tên món ăn");
            }
        }
        private void btnWatchAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            clearAccount();
            btnRemoveAccount.Enabled = false;
            btnEditAccount.Enabled = false;
            IsAccessible = true; // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không
            MessageBox.Show("Vui lòng nhập thông tin tài khoản","Thông báo");
            
            
        }

        private void btnRemoveAccount_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || txtDisplayname.Text == "" || txtPassWord.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản bạn muốn xóa!!!", "Thông báo");
            }
            else
            {
                string userName = txtUserName.Text;
                if (loginAccount.UserName.Equals(userName))
                {
                    MessageBox.Show("Tài khoản bạn đang sử dụng không thể xóa", "Cảnh báo");
                    return;
                }
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    AccountProvider.Instance.DeleteAccount(userName);
                    MessageBox.Show("Xóa tài khoản thành công");
                }

                LoadAccount();
            }
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            btnAddAccount.Enabled = false;
            btnRemoveAccount.Enabled = false;
            clearAccount();
            MessageBox.Show("Chọn tài khoản muốn cập nhật", "Thông báo");
        }
        
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            bntRemoveCategory.Enabled = false;
            btnEditCategory.Enabled = false;
            clearCategory();
            IsAccessible = true; // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không
            MessageBox.Show("Vui lòng nhập thông tin loại món ăn", "Thông báo");

        }

        private void bntRemoveCategory_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text == "" || txtIdCategory.Text == "")
            {
                MessageBox.Show("Vui lòng chọn loại món ăn bạn muốn xóa!!!", "Thông báo");
            }
            else
            {
                int id = int.Parse(txtIdCategory.Text);
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    CategoryDAO.Instance.DeleteCategory(id);
                    MessageBox.Show("Xóa loại món ăn thành công");
                }
                else
                {
                    MessageBox.Show("Xóa loại món ăn thất bại");
                }

                LoadCategory();
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            bntRemoveCategory.Enabled = false;
            btnAddCategory.Enabled = false;
            clearCategory();
            MessageBox.Show("Chọn loại món ăn muốn cập nhật", "Thông báo");
        }
        private void btnLuuAccount_Click(object sender, EventArgs e)
        {
            // Thêm tài khoản
            if (IsAccessible) // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không 
            {
                string userName = txtUserName.Text;
                string displayName = txtDisplayname.Text;
                string password = txtPassWord.Text;
                int type = (int)nmType.Value;
                try
                {

                    if (txtUserName.Text == "" || txtDisplayname.Text == "" || txtPassWord.Text == "")
                    {
                        MessageBox.Show("Thêm tài khoản thất bại , vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        if (AccountProvider.Instance.checkAccountExists(userName))
                        {
                            MessageBox.Show("Thêm tài khoản thất bại , đã tồn tại tài khoản này!!!", "Thông báo");
                            clearAccount();
                        }
                        else
                        {
                            AccountProvider.Instance.InsertAccount(userName, displayName, type, password);
                            MessageBox.Show("Thêm tài khoản thành công");
                            LoadAccount();
                            btnRemoveAccount.Enabled = true;
                            btnEditAccount.Enabled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Thêm tài khoản thất bại , đã tồn tại tài khoản này!!!", "Thông báo");
                    clearAccount();
                }

            }
            else // Update tài khoản
            {
                string userName = txtUserName.Text;
                string displayName = txtDisplayname.Text;
                string password = txtPassWord.Text;
                int type = (int)nmType.Value;
                try
                {
                    if (txtUserName.Text == "" || txtDisplayname.Text == "" || txtPassWord.Text == "")
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật tài khoản này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            AccountProvider.Instance.UpdateAccount(userName, displayName, type, password);
                            MessageBox.Show("Cập nhật tài khoản thành công");
                            LoadAccount();
                            btnAddAccount.Enabled = true;
                            btnRemoveAccount.Enabled = true;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xảy ra: " + ex.Message);
                }
            }
        } // lưu account

        private void btnKhongLuu_Click(object sender, EventArgs e) // account
        {
            clearAccount();
            btnRemoveAccount.Enabled = true;
            btnEditAccount.Enabled = true;
            btnAddAccount.Enabled= true;    
        }

        private void btnLuuFood_Click(object sender, EventArgs e)
        {
            // Thêm món ăn
            if (IsAccessible) // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không 
            {
                string name = txtFoodName.Text;
                int categoryID = (cbFoodCategory.SelectedItem as Category).Id;
                float price = (float)nmFoodPrice.Value;
                try
                {

                    if (txtFoodName.Text == "")
                    {
                        MessageBox.Show("Thêm món ăn thất bại , vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        if (FoodDAO.Instance.checkFoodExists(name))
                        {
                            MessageBox.Show("Thêm món thất bại , đã tồn tại món ăn này!!!", "Thông báo");
                            clearFood();
                        }
                        else
                        {
                            FoodDAO.Instance.InsertFood(name, categoryID, price);
                            MessageBox.Show("Thêm món ăn thành công");
                            loadListFood();
                            btnEditFood.Enabled = true;
                            btnRemoveFood.Enabled = true;
                            btnFoodWatch.Enabled = true;
                            btnSearchFood.Enabled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Thêm món ăn thất bại , đã tồn tại món ăn này!!!", "Thông báo");
                    clearFood();
                }

            }
            else // Update món ăn
            {
                string name = txtFoodName.Text;
                int categoryID = (cbFoodCategory.SelectedItem as Category).Id;
                float price = (float)nmFoodPrice.Value;
                try
                {
                    if (txtFoodName.Text == "" || txtFoodId.Text == "")
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật món ăn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            int id = Convert.ToInt32(txtFoodId.Text);
                            FoodDAO.Instance.UpdateFood(id, name, categoryID, price);
                            MessageBox.Show("Cập nhật món ăn thành công");
                            loadListFood();
                            btnRemoveFood.Enabled = true;
                            btnFoodWatch.Enabled = true;
                            btnSearchFood.Enabled = true;
                            btnAddFood.Enabled = true;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xảy ra: " + ex.Message);
                }
            }
        }//lưu food

        private void btnCancelFood_Click(object sender, EventArgs e)
        {
            clearFood();
            btnEditFood.Enabled = true;
            btnRemoveFood.Enabled = true;
            btnFoodWatch.Enabled = true;
            btnSearchFood.Enabled = true;
            btnAddFood.Enabled = true;
        }// 
        private void btnLuuCategory_Click(object sender, EventArgs e)
        {
            // Thêm loại món ăn
            if (IsAccessible) // Thuộc tính này được sử dụng để xác định xem một thành phần giao diện (control) có khả năng tương tác với người dùng hay không 
            {
                string name = txtCategoryName.Text;
                try
                {

                    if (txtCategoryName.Text == "")
                    {
                        MessageBox.Show("Thêm loại món ăn thất bại , vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        if (CategoryDAO.Instance.checkCategoryExists(name))
                        {
                            MessageBox.Show("Thêm loại món thất bại , đã tồn tại loại món ăn này!!!", "Thông báo");
                            clearCategory();
                        }
                        else
                        {
                            CategoryDAO.Instance.InsertCategory(name);
                            MessageBox.Show("Thêm loại món ăn thành công");
                            LoadCategory();
                            bntRemoveCategory.Enabled = true;
                            btnEditCategory.Enabled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Thêm loại món ăn thất bại , đã tồn tại loại món ăn này!!!", "Thông báo");
                    clearFood();
                }

            }
            else // Update món ăn
            {

                string name = txtCategoryName.Text;
                try
                {
                    if (txtCategoryName.Text == "" || txtIdCategory.Text == "")
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!!", "Thông báo");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật loại món ăn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            int id = int.Parse(txtIdCategory.Text);
                            CategoryDAO.Instance.UpdateCategory(name, id);
                            MessageBox.Show("Cập nhật loại món ăn thành công");
                            LoadCategory();
                            bntRemoveCategory.Enabled = true;
                            btnAddCategory.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xảy ra: " + ex.Message);
                }
            }
        } // lưu Category

        private void btnCancelCategory_Click(object sender, EventArgs e)
        {
            clearCategory();
            btnAddCategory.Enabled = true;
            bntRemoveCategory.Enabled = true;
            btnEditCategory.Enabled = true;
        }//
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();

        }
        private void btnThoatCate_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnThoatDoanhThu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bntThoatFood_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void fAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }






        #endregion

        
    }
}
