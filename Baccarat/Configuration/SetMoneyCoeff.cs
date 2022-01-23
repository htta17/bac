using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas.Configuration
{
    public partial class SetMoneyCoeff : Form
    {
        public SetMoneyCoeff()
        {
            InitializeComponent();
            var _moneyCoeff = Registry.GetValue(REG_PATH, REG_MONEY_COEFF_KEY, string.Empty).ToString();
            var _displayType = Registry.GetValue(REG_PATH, REG_MONEY_DISPLAYTYPE_KEY, string.Empty).ToString();
            if (string.IsNullOrEmpty(_moneyCoeff))
            {
                numericUpDown1.Value = 50_000;
                cbxDisplayType.SelectedIndex = 0;
            }
            else
            {
                numericUpDown1.Value = int.Parse(_moneyCoeff);
                cbxDisplayType.SelectedIndex = int.Parse(_displayType);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cbxDisplayType.Items[0] = (numericUpDown1.Value / 1000).ToString() + "K";
            cbxDisplayType.Items[1] = string.Format("{0:n0}", numericUpDown1.Value);
        }
        
        const string REG_PATH = "HKEY_CURRENT_USER\\MidasSoft";
        const string REG_MONEY_COEFF_KEY = "MoneyCoeff";
        const string REG_MONEY_DISPLAYTYPE_KEY = "DisplayType";

        private void button1_Click(object sender, EventArgs e)
        {
            Registry.SetValue(REG_PATH, REG_MONEY_COEFF_KEY, numericUpDown1.Value);
            Registry.SetValue(REG_PATH, REG_MONEY_DISPLAYTYPE_KEY, cbxDisplayType.SelectedIndex);
            this.Close();
        }
    }
}
