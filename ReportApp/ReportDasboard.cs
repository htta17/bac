using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Charts;
using LiveCharts.Wpf;

namespace ReportApp
{
    public partial class ReportDasboard : Form
    {
        public ReportDasboard()
        {
            InitializeComponent();

            cbxAllReports.SelectedIndex = 0;
            dataGridView1.ReadOnly = true;

            DatabaseUtil.ConnectionString = RegisterUtil.GetConnectionString();
        }
        string ConnectionStr { get; set; }

        private void btnDeleteScrapData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chứ", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            
            DatabaseUtil.ExecuteNonQuery(@"
                            DELETE  AutoResults
                            FROM    AutoSessions INNER JOIN 
                                    AutoResults ON AutoSessions.ID = AutoResults.AutoSessionID
                            WHERE   IsClosed = 1 AND (NoOfStepsRoot < 40 OR NoOfStepsRoot > 70)");

            DatabaseUtil.ExecuteNonQuery(@"
                            DELETE  AutoRoots
                            FROM    AutoSessions INNER JOIN 
                                    AutoRoots ON AutoSessions.ID = AutoRoots.AutoSessionID
                            WHERE   IsClosed = 1 AND (NoOfStepsRoot < 40 OR NoOfStepsRoot > 70)");

            DatabaseUtil.ExecuteNonQuery(@"DELETE FROM AutoSessions WHERE IsClosed = 1 AND (NoOfStepsRoot < 40 OR NoOfStepsRoot > 70)");
            
        }

        void DrawSessions_Root_LineChart()
        {
            var data = DatabaseUtil.SelectQueryCommand(@"
                    SELECT  ID, 
                            StartDateTime, NoOfStepsRoot AS NoSteps,  
                            MinRoot, MaxRoot, 
                            RootMainProfit As MainProfit, 
                            sum(RootMainProfit) OVER (Order by ID) As AccMainProfit, 
                            RootProfit0 AS Profit0, 
                            sum(RootProfit0) OVER (Order by ID) As AccProfit0, 
                            RootProfit1 AS Profit1, 
                            sum(RootProfit1) OVER (Order by ID) As AccProfit1, 
                            RootProfit2 AS Profit2, 
                            sum(RootProfit2) OVER (Order by ID) As AccProfit2, 
                            RootProfit3 AS Profit3 ,
                            sum(RootProfit3) OVER (Order by ID) As AccProfit3
                    FROM    AutoSessions
                    -- WHERE   TableNumber = 3
                    ORDER BY ID");
            dataGridView1.DataSource = data;

            var graphList = ConvertUtil.ConvertDataTable<RootReport>(data);

            cartesianChart1.Series.Clear();

            cartesianChart1.Series.Add(new LineSeries
            {
                Title = "Phiên chính",
                Values = new ChartValues<int>(graphList.Select(c => c.AccMainProfit)),
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = "Phiên số 1",
                Values = new ChartValues<int>(graphList.Select(c => c.AccProfit0)),
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = "Phiên số 2",
                Values = new ChartValues<int>(graphList.Select(c => c.AccProfit1)),
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = "Phiên số 3",
                Values = new ChartValues<int>(graphList.Select(c => c.AccProfit2)),
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = "Phiên số 4",
                Values = new ChartValues<int>(graphList.Select(c => c.AccProfit3)),
            });
        }

        private void btnSee_Click(object sender, EventArgs e)
        {
            if (cbxAllReports.SelectedIndex == 0)
                DrawSessions_Root_LineChart();

        }
    }
}
