using LunarNumericSimulator;
using LunarNumericSimulator.Reporting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using Module = LunarNumericSimulator.Module;
using static LunarNumericSimulator.Module;

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

            GraphTabs.TabPages.Add("Module Overview");
            var moPanel = new ModuleOverviewPanel();
            moPanel.Dock = DockStyle.Fill;
            GraphTabs.TabPages[1].Controls.Add(moPanel);


            var loadedModulesNames = simulation.getLoadedModuleNames();
            var autocomplete = new AutoCompleteStringCollection();
            autocomplete.AddRange(loadedModulesNames.ToArray());
            ModuleNameBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            ModuleNameBox.AutoCompleteCustomSource = autocomplete;
            ModuleNameBox.AutoCompleteMode = AutoCompleteMode.Append;

            this.AcceptButton = AddButton;

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
                BaseLoad.Text = "Base Load: " + Math.Round(report.PowerLoad) + " kW";
                updateEnvironmentTab(report);
                updateModuleOverviewTab(report);
                foreach (Module m in simulation.getModules())
                {
                    var state = (from element in report.ModuleStates
                                 where element.getID() == m.ModuleID
                                 select element).First();
                    updateTab(report.GlobalState.clock, m.ModuleID.ToString(), state);
                }
                updateTimer++;
            }
        }
        
        protected void updateEnvironmentTab(SimulationProgressReport report)
        {
            Chart environmentChart = (Chart)GraphTabs.TabPages[0].Controls[0].Controls[0];
            environmentChart.Series["Pressure"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.TotalPressure);
            environmentChart.Series["Pressure"].BorderWidth = 4;

            environmentChart.Series["Temperature"].Points.AddXY(report.GlobalState.clock, report.GlobalState.Atmospheric.Temperature);
            environmentChart.Series["Temperature"].BorderWidth = 4;

            environmentChart.Series["BaseLoad"].Points.AddXY(report.GlobalState.clock, report.PowerLoad);
            environmentChart.Series["BaseLoad"].BorderWidth = 4;

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
            }
            
        }

        protected void updateModuleOverviewTab(SimulationProgressReport report)
        {
            Chart moduleOverviewChart = (Chart)GraphTabs.TabPages[1].Controls[0].Controls[0];
            if (updateTimer % 20 == 0)
            {
                moduleOverviewChart.Series["Tanks"].Points.Clear();
                foreach (var tank in report.TankStates)
                {
                    if (tank.Value == 0)
                        continue;
                    moduleOverviewChart.Series["Tanks"].Points.AddXY(tank.Key, tank.Value);
                }

                moduleOverviewChart.Series["Module Power Usage"].Points.Clear();
                var modules = from element in report.ModuleStates
                              select new { Name = element.getName(), PowerUsage = -element.getResourceLevel(Resources.ElecticalEnergy) };

                foreach (var module in modules)
                {
                    if (module.PowerUsage == 0)
                        continue;
                    moduleOverviewChart.Series["Module Power Usage"].Points.AddXY(module.Name, module.PowerUsage);
                }
                
            }
        }

        protected void setupTab(string tabid)
        {
            var panel = new AtmospherePanel();
            panel.Dock = DockStyle.Fill;

            GraphTabs.TabPages[tabid].Controls.Add(panel);
            Module m = (from element in simulation.getModules()
                        where element.ModuleID == Convert.ToInt32(tabid)
                        select element).First();
            var configProperties = (from element in m.GetType().GetProperties()
                                   where element.GetCustomAttribute<NumericConfigurationParameter>() != null
                                   select new { prop= element, attr=  element.GetCustomAttribute<NumericConfigurationParameter>() }).ToList();
            if (configProperties.Count > 0)
            {
                TableLayoutPanel parameterTable = (TableLayoutPanel)panel.Controls["ConfigPanel"].Controls["ParameterPanel"];
                int row = 0, col = 0;
                parameterTable.RowCount = 2;
                parameterTable.ColumnCount = 3;
                foreach (var param in configProperties)
                {
                    Label lab = new Label();
                    lab.Font = new Font(lab.Font.Name, 12, FontStyle.Bold);
                    lab.Dock = DockStyle.Fill;
                    lab.Text = param.attr.friendlyName + ": " + param.prop.GetValue(m);
                    parameterTable.Controls.Add(lab, col, row);
                    if (row > 1)
                    {
                        row = 0;
                        col++;
                    }
                    else
                    {
                        row++;
                    }
                }
            } else
            {
                Panel parameterTable = (Panel)panel.Controls["ConfigPanel"];
                panel.Controls.Remove(parameterTable);
                panel.Controls["AtmosphereChart"].Dock = DockStyle.Fill;

            }
            Chart chart = (Chart)panel.Controls["AtmosphereChart"];
            foreach (var gas in m.getRegisteredResources()) {
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
                    chart.Series[res.ToString()].ChartArea = "Bottom";
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
            
            Chart chart = (Chart)GraphTabs.TabPages[tabid].Controls[0].Controls["AtmosphereChart"];

            foreach(var gas in report.getRegisteredResources())
            {
                chart.Series[gas.ToString()].Points.AddXY(clock, report.getResourceLevel(gas));
            }
        }

        protected void runSimulation(object sender, DoWorkEventArgs args)
        {
            lock (simulation)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    simulation.step();
                    worker.ReportProgress(0, simulation.getSimulationState());
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string moduleName = ModuleNameBox.Text;
            var res = simulation.getModuleConfiguration(moduleName);
            if (res == null)
                return;

            Form popup = new Form();
            popup.Text = "Add new Module";
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.ColumnCount = 1;
            layout.RowCount = res.Count + 1;
            for (int i = 0; i < res.Count; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                var control = createFromConfig(res[i]);
                control.Dock = DockStyle.Fill;
                layout.Controls.Add(control, 0, i);
            }

            Panel buttonPanel = new Panel();
            TableLayoutPanel buttonLayout = new TableLayoutPanel();
            Button cancelButton = new Button();
            cancelButton.Text = "Cancel";
            Button addButton = new Button();
            addButton.Text = "Add";
            addButton.Click += (obj, eventargs) =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                for(int j = 0; j < layout.Controls.Count; j++)
                {
                    if (layout.Controls[j].GetType().Name != "AttributePanel")
                        continue;
                    var propName = ((AttributePanel)layout.Controls[j]).propertyName;
                    var slider = ((NumericUpDown)layout.Controls[j].Controls[0].Controls[1]);
                    var propValue = slider.Value;
                    if (slider.Increment == 1)
                    {
                        result.Add(propName, Convert.ToInt32(propValue));
                    } else
                    {
                        result.Add(propName, Convert.ToDouble(propValue));
                    }
                }
                simulation.registerModule(moduleName, result);
                moduleList = simulation.getModules();
                updateList();
                ModuleNameBox.Clear();
                popup.Close();
            };

            cancelButton.Click += (obj, eventargs) =>
            {
                popup.Close();
            };

            buttonLayout.Controls.Add(cancelButton, 0, 0);
            buttonLayout.Controls.Add(addButton, 1, 0);
            buttonPanel.Controls.Add(buttonLayout);
            buttonPanel.Dock = DockStyle.Fill;
            layout.Controls.Add(buttonPanel, 0, layout.RowCount);

            layout.Dock = DockStyle.Fill;
            layout.Refresh();

            popup.Controls.Add(layout);
            popup.AutoSize = true;
            popup.Height = (layout.RowCount + 1) * 35;
            popup.AutoSizeMode = AutoSizeMode.GrowOnly;
            popup.FormBorderStyle = FormBorderStyle.FixedDialog;
            popup.ControlBox = false;
            popup.Show();
        }

        private Panel createFromConfig(NumericConfigurationParameter attr)
        {
            AttributePanel pan = new AttributePanel();
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.RowCount = 1;
            layout.ColumnCount = 2;
            Label textLabel = new Label();
            NumericUpDown entryField = new NumericUpDown();
            entryField.Maximum = 1e10M;
            switch (attr.ParameterType.Name)
            {
                case "Int32":
                    textLabel.Text = attr.friendlyName;
                    if (!attr.AllowNegative)
                        entryField.Minimum = 0;
                    entryField.Increment = 1M;
                    entryField.Value = Convert.ToInt32(attr.defaultValue);
                    pan.propertyName = attr.propertyName;
                    layout.Controls.Add(textLabel,0,0);
                    layout.Controls.Add(entryField, 1, 0);
                    layout.Dock = DockStyle.Fill;
                    textLabel.Dock = DockStyle.Fill;
                    entryField.Dock = DockStyle.Fill;
                    pan.Controls.Add(layout);
                    return pan;
                case "Double":
                    textLabel.Text = attr.friendlyName;
                    if (!attr.AllowNegative)
                        entryField.Minimum = 0;
                    entryField.Increment = 0.2M;
                    entryField.DecimalPlaces = 4;
                    entryField.Value = Convert.ToDecimal(attr.defaultValue);
                    pan.propertyName = attr.propertyName;
                    layout.Controls.Add(textLabel, 0, 0);
                    layout.Controls.Add(entryField, 1, 0);
                    layout.Dock = DockStyle.Fill;
                    textLabel.Dock = DockStyle.Fill;
                    entryField.Dock = DockStyle.Fill;
                    pan.Controls.Add(layout);
                    return pan;
                default:
                    throw new Exception("Unknown Parameter Type!");
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int id = ((Module)ModuleList.SelectedItem).ModuleID;
            simulation.deregisterModule(id);
            moduleList = simulation.getModules();
            updateList();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            AddButton.Enabled = false;
            RemoveButton.Enabled = false;
            StartButton.Enabled = false;
            openButton.Enabled = false;
            SaveButton.Enabled = false;

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

        public class AttributePanel: Panel
        {
            public string propertyName { get; set; }

            public AttributePanel(): base()
            {
                
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (moduleList.Count > 0)
                    simulation.deregisterAllModules();
                var serializedState = File.ReadAllText(dialog.FileName);
                var state = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(serializedState);
                foreach(var obj in state)
                {
                    object moduleNameObj;
                    obj.TryGetValue("moduleName", out moduleNameObj);
                    string moduleName = (string)moduleNameObj;
                    var configurationParameters = simulation.getModuleConfiguration(moduleName);
                    Dictionary<string, object> newConfig = new Dictionary<string, object>();
                    foreach(var param in configurationParameters)
                    {
                        object value;
                        obj.TryGetValue(param.propertyName, out value);
                        if (value == null)
                            throw new Exception("Could not load saved file!");
                        newConfig.Add(param.propertyName, value);
                    }
                    simulation.registerModule(moduleName, newConfig);
                }
                updateList();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var state = simulation.getModules();
            var serializedState = JsonConvert.SerializeObject(state);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "json files (*.json)|*.json";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, serializedState);
            }
        }
    }
}
