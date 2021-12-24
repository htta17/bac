using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baccarat.BaccaratSimulatorUI
{
    public partial class BacSimulator : Form
    {
        public BacSimulator()
        {
            InitializeComponent();

            panel1.BackgroundImage = Image.FromFile(@"Cards\CardBackRed.jpg");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Image.FromFile(@"Cards\5D.jpg");
        }
    }
}
