using Baccarat.BaccaratSimulatorUI;
using CalculationLogic.BaccaratSimulator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baccarat
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            this.KeyPreview = true;
            InitializeComponent();

           
        }

        private void SubForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        private void btnCombination_Click(object sender, EventArgs e)
        {
            this.Hide();
            BaccaratCombination combination = new BaccaratCombination();
            combination.FormClosing += SubForm_FormClosing;
            combination.ShowDialog();
        }

        private void btnBankerOnly2_Click(object sender, EventArgs e)
        {
            this.Hide();
            BaccaratBankerOnly combination = new BaccaratBankerOnly();
            combination.FormClosing += SubForm_FormClosing;
            combination.ShowDialog();
        }

        private void btnQuadruple_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSimilator_Click(object sender, EventArgs e)
        {
            //var test = new BaccaratSimulatorGame(6);
            //test.Shuffle();
            BacSimulator bacSimulator = new BacSimulator();
            bacSimulator.ShowDialog();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSimilator.Visible = !btnSimilator.Visible;
            }
        }
    }
}
