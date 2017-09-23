namespace SimulatorGUI
{
    partial class AtmospherePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.AtmosphereChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ParameterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ConfigPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).BeginInit();
            this.ConfigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AtmosphereChart
            // 
            this.AtmosphereChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AtmosphereChart.BackColor = System.Drawing.Color.DimGray;
            this.AtmosphereChart.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Scaled;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.Title = "Resource Consumption (kg)";
            chartArea1.Name = "Top";
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisY.Title = "Power Consumption (kJ)";
            chartArea2.Name = "Bottom";
            this.AtmosphereChart.ChartAreas.Add(chartArea1);
            this.AtmosphereChart.ChartAreas.Add(chartArea2);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "EnvironmentLegend";
            this.AtmosphereChart.Legends.Add(legend1);
            this.AtmosphereChart.Location = new System.Drawing.Point(0, 71);
            this.AtmosphereChart.Name = "AtmosphereChart";
            series1.ChartArea = "Bottom";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "EnvironmentLegend";
            series1.Name = "Energy Usage";
            this.AtmosphereChart.Series.Add(series1);
            this.AtmosphereChart.Size = new System.Drawing.Size(786, 526);
            this.AtmosphereChart.TabIndex = 1;
            this.AtmosphereChart.Text = "chart1";
            this.AtmosphereChart.Click += new System.EventHandler(this.EnvironmentChart_Click);
            // 
            // ParameterPanel
            // 
            this.ParameterPanel.ColumnCount = 3;
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ParameterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParameterPanel.Location = new System.Drawing.Point(0, 0);
            this.ParameterPanel.Name = "ParameterPanel";
            this.ParameterPanel.RowCount = 2;
            this.ParameterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ParameterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ParameterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ParameterPanel.Size = new System.Drawing.Size(786, 65);
            this.ParameterPanel.TabIndex = 2;
            this.ParameterPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // ConfigPanel
            // 
            this.ConfigPanel.Controls.Add(this.ParameterPanel);
            this.ConfigPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ConfigPanel.Location = new System.Drawing.Point(0, 0);
            this.ConfigPanel.Name = "ConfigPanel";
            this.ConfigPanel.Size = new System.Drawing.Size(786, 65);
            this.ConfigPanel.TabIndex = 3;
            // 
            // AtmospherePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ConfigPanel);
            this.Controls.Add(this.AtmosphereChart);
            this.Name = "AtmospherePanel";
            this.Size = new System.Drawing.Size(786, 597);
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).EndInit();
            this.ConfigPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart AtmosphereChart;
        private System.Windows.Forms.TableLayoutPanel ParameterPanel;
        private System.Windows.Forms.Panel ConfigPanel;
    }
}
