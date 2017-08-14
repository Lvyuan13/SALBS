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
        EnvironmentPanel envPanel;

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
            envPanel = new EnvironmentPanel();
            envPanel.Dock = DockStyle.Fill;
            GraphTabs.TabPages[0].Controls.Add(envPanel);

            GraphTabs.TabPages.Add("Atmosphere");

        }

        protected void updateList()
        {
            ModuleList.Items.Clear();
            ModuleList.Items.AddRange(moduleList.ToArray());
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            SimulationProgressReport report = (SimulationProgressReport)e.UserState;
            CurrentTimeLabel.Text = "Current Time: " + report.GlobalState.clock;
            Chart environmentChart = (Chart)envPanel.GetChildAtPoint(new Point(5, 5));
            environmentChart.Series["Pressure"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.TotalPressure);

            environmentChart.Series["Temperature"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.Temperature);

            environmentChart.Series["Enthalpy"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.TotalEnthalpy);

            var co2 = (from element in report.GlobalState.Atmospheric
                       where element.Resource == Resources.CO2
                       select element.Quantity).FirstOrDefault();
            environmentChart.Series["CO2 Quantity"].Points.AddXY(report.GlobalState.clock, co2);
            var ch4 = (from element in report.GlobalState.Atmospheric
                       where element.Resource == Resources.CH4
                       select element.Quantity).FirstOrDefault();
            environmentChart.Series["CH4 Quantity"].Points.AddXY(report.GlobalState.clock, ch4);
            var O = (from element in report.GlobalState.Atmospheric
                       where element.Resource == Resources.O
                       select element.Quantity).FirstOrDefault();
            environmentChart.Series["O Quantity"].Points.AddXY(report.GlobalState.clock, O);
            var N = (from element in report.GlobalState.Atmospheric
                       where element.Resource == Resources.N
                       select element.Quantity).FirstOrDefault();
            environmentChart.Series["N Quantity"].Points.AddXY(report.GlobalState.clock, N);

        }

        protected void runSimulation(object sender, DoWorkEventArgs args)
        {
            lock (simulation)
            {
                for (int i = 0; i < 5000; i++)
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
            worker.RunWorkerAsync();
        }

        private void CurrentTimeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
