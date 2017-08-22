using LunarNumericSimulator;
using LunarNumericSimulator.Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SimulatorGUI
{
    public partial class SimulationGUI : Form
    {
        BackgroundWorker worker;
        Simulation simulation;
        List<Module> moduleList;
        protected float updateTimer = 0;
        public object sync = 0;

        public SimulationGUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            simulation = new Simulation();
            simulation.initiate();

            moduleList = simulation.getModules();
            ModuleList.DisplayMember = "moduleFriendlyName";
            ModuleList.ValueMember = "ModuleID";
            updateList();

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(runSimulation);
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;

            GraphTabs.TabPages.Clear();
            GraphTabs.TabPages.Add("Environmental Summary");
            var envPanel = new EnvironmentPanel();
            envPanel.Dock = DockStyle.Fill;
            GraphTabs.TabPages[0].Controls.Add(envPanel);

        }

        protected void updateList()
        {
            ModuleList.Items.Clear();
            ModuleList.Items.AddRange(moduleList.ToArray());
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lock (sync)
            {
                SimulationProgressReport report = (SimulationProgressReport)e.UserState;
                CurrentTimeLabel.Text = "Current Time: " + report.GlobalState.clock;

                updateEnvironmentTab(report);
                foreach (Module m in simulation.getModules())
                {

                    var state = (from element in report.ModuleStates
                                 where element.getID() == m.ModuleID
                                 select element).First();
                    updateTab(report.GlobalState.clock, m.ModuleID.ToString(), state);
                }
            }
        }
        
        protected void updateEnvironmentTab(SimulationProgressReport report)
        {
            Chart environmentChart = (Chart)GraphTabs.TabPages[0].Controls[0].Controls[0];
            environmentChart.Series["Pressure"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.TotalPressure);
            environmentChart.Series["Pressure"].BorderWidth = 4;

            environmentChart.Series["Temperature"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.Temperature);
            environmentChart.Series["Temperature"].BorderWidth = 4;

            environmentChart.Series["Enthalpy"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.TotalEnthalpy);
            environmentChart.Series["Enthalpy"].BorderWidth = 4;

            if (updateTimer % 50 == 0)
            {
                environmentChart.Series["Gas Distribution"].Points.Clear();
                var co2 = (from element in report.GlobalState.Atmospheric
                           where element.Resource == Resources.CO2
                           select element.Quantity).FirstOrDefault();
                environmentChart.Series["Gas Distribution"].Points.AddXY(Resources.CO2.ToString(), co2);
                var ch4 = (from element in report.GlobalState.Atmospheric
                           where element.Resource == Resources.CH4
                           select element.Quantity).FirstOrDefault();
                environmentChart.Series["Gas Distribution"].Points.AddXY(Resources.CH4.ToString(), ch4);
                var O = (from element in report.GlobalState.Atmospheric
                         where element.Resource == Resources.O
                         select element.Quantity).FirstOrDefault();
                environmentChart.Series["Gas Distribution"].Points.AddXY(Resources.O.ToString(), O);
                var N = (from element in report.GlobalState.Atmospheric
                         where element.Resource == Resources.N
                         select element.Quantity).FirstOrDefault();
                environmentChart.Series["Gas Distribution"].Points.AddXY(Resources.N.ToString(), N);
                updateTimer = -1;
            }
            updateTimer++;
        }

        protected void setupTab(string tabid)
        {
            var panel = new AtmospherePanel();
            panel.Dock = DockStyle.Fill;
            GraphTabs.TabPages[tabid].Controls.Add(panel);
            Module m = (from element in simulation.getModules()
                        where element.ModuleID == Convert.ToInt32(tabid)
                        select element).First();
            Chart chart = (Chart)panel.Controls[0];
            foreach(var gas in m.getRegisteredResources()) {
                setupSeries(gas, chart);
            }
        }

        protected void setupSeries(Resources res, Chart chart)
        {
            switch (res)
            {
                case Resources.CH4:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.GreenYellow;
                    break;
                case Resources.O:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.Azure;
                    break;
                case Resources.N:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.Black;
                    break;
                case Resources.CO2:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.RosyBrown;
                    break;
                case Resources.H:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.Gold;
                    break;
                case Resources.H2O:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.DarkBlue;
                    break;
                case Resources.ElecticalEnergy:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Bottom";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.Yellow;
                    break;
                case Resources.Heat:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.DarkRed;
                    break;
                case Resources.Food:
                    chart.Series.Add(new Series(res.ToString()));
                    chart.Series[res.ToString()].ChartArea = "Top";
                    chart.Series[res.ToString()].ChartType = SeriesChartType.FastLine;
                    chart.Series[res.ToString()].BorderWidth = 4;
                    chart.Series[res.ToString()].BorderColor = Color.Green;
                    break;
            }
        }

        protected void updateTab(UInt64 clock, string tabid, ModuleResourceLevels report)
        {
            
            Chart chart = (Chart)GraphTabs.TabPages[tabid].Controls[0].Controls[0];

            foreach(var gas in report.getRegisteredResources())
            {
                chart.Series[gas.ToString()].Points.AddXY(clock, report.getResourceLevel(gas));
            }
        }

        protected void runSimulation(object sender, DoWorkEventArgs args)
        {
            lock (simulation)
            {
                for (int i = 0; i < 3000000000; i++)
                {
                    simulation.step();
                    worker.ReportProgress(0, simulation.getSimulationState());
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string moduleName = ModuleNameBox.Text;
            simulation.registerModule(moduleName);
            moduleList = simulation.getModules();
            updateList();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int id = (int)ModuleList.SelectedValue;
            simulation.deregisterModule(id);
            moduleList = simulation.getModules();
            updateList();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            AddButton.Enabled = false;
            RemoveButton.Enabled = false;

            foreach (Module m in simulation.getModules())
            {
                GraphTabs.TabPages.Add(m.ModuleID.ToString(), m.moduleFriendlyName);
                setupTab(m.ModuleID.ToString());
            }

            worker.RunWorkerAsync();
        }

        private void CurrentTimeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
