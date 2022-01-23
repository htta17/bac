using Midas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas
{
    public partial class SetConnection : Form
    {
        public SetConnection()
        {
            InitializeComponent();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var result = DatabaseUtil.TestConnect(txtConnectionString.Text);
            MessageBox.Show(result ? "Kết nối cơ sở dữ liệu thành công." : "Kiểm tra kết nối CSDL.");
        }

        private void btnSaveConnection_Click(object sender, EventArgs e)
        {
            if (DatabaseUtil.TestConnect(txtConnectionString.Text))
            {
                StartApp.SaveConnection(txtConnectionString.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Kiểm tra kết nối CSDL.");
            }            
        }
    }
}
