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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.AtmosphereChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ParameterPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).BeginInit();
            this.SuspendLayout();
            // 
            // AtmosphereChart
            // 
            this.AtmosphereChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AtmosphereChart.BackColor = System.Drawing.Color.DimGray;
            chartArea5.AxisX.Title = "Time";
            chartArea5.AxisY.Title = "Mass Resource Consumption (kg)";
            chartArea5.Name = "Top";
            chartArea6.AxisX.Title = "Time";
            chartArea6.AxisY.Title = "Energy Resource Consumption (kJ)";
            chartArea6.Name = "Bottom";
            this.AtmosphereChart.ChartAreas.Add(chartArea5);
            this.AtmosphereChart.ChartAreas.Add(chartArea6);
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend3.Name = "EnvironmentLegend";
            this.AtmosphereChart.Legends.Add(legend3);
            this.AtmosphereChart.Location = new System.Drawing.Point(0, 79);
            this.AtmosphereChart.Name = "AtmosphereChart";
            series3.ChartArea = "Bottom";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "EnvironmentLegend";
            series3.Name = "Energy Usage";
            this.AtmosphereChart.Series.Add(series3);
            this.AtmosphereChart.Size = new System.Drawing.Size(786, 518);
            this.AtmosphereChart.TabIndex = 1;
            this.AtmosphereChart.Text = "chart1";
            this.AtmosphereChart.Click += new System.EventHandler(this.EnvironmentChart_Click);
            // 
            // ParameterPanel
            // 
            this.ParameterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParameterPanel.ColumnCount = 4;
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ParameterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ParameterPanel.Location = new System.Drawing.Point(0, 0);
            this.ParameterPanel.Name = "ParameterPanel";
            this.ParameterPanel.RowCount = 2;
            this.ParameterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ParameterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ParameterPanel.Size = new System.Drawing.Size(786, 73);
            this.ParameterPanel.TabIndex = 2;
            // 
            // AtmospherePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.ParameterPanel);
            this.Controls.Add(this.AtmosphereChart);
            this.Name = "AtmospherePanel";
            this.Size = new System.Drawing.Size(786, 597);
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart AtmosphereChart;
        private System.Windows.Forms.TableLayoutPanel ParameterPanel;
    }
}
