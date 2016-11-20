namespace MainExample
{
    partial class RegulatorySheduleForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listRegSh = new System.Windows.Forms.ListView();
            this.NumberOfTrain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouteName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ArrivalTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DepartureTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DispatchStation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StationOfDestination = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DaysFollowing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listRegSh
            // 
            this.listRegSh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NumberOfTrain,
            this.RouteName,
            this.ArrivalTime,
            this.DepartureTime,
            this.DispatchStation,
            this.StationOfDestination,
            this.DaysFollowing});
            this.listRegSh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRegSh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listRegSh.Location = new System.Drawing.Point(0, 0);
            this.listRegSh.Margin = new System.Windows.Forms.Padding(4);
            this.listRegSh.Name = "listRegSh";
            this.listRegSh.Size = new System.Drawing.Size(1643, 696);
            this.listRegSh.TabIndex = 1;
            this.listRegSh.UseCompatibleStateImageBehavior = false;
            this.listRegSh.View = System.Windows.Forms.View.Details;
            // 
            // NumberOfTrain
            // 
            this.NumberOfTrain.Text = "Номер поезда";
            this.NumberOfTrain.Width = 157;
            // 
            // RouteName
            // 
            this.RouteName.Text = "Маршрут";
            this.RouteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouteName.Width = 125;
            // 
            // ArrivalTime
            // 
            this.ArrivalTime.Text = "Время отбытия";
            this.ArrivalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ArrivalTime.Width = 205;
            // 
            // DepartureTime
            // 
            this.DepartureTime.Text = "Время прибытия";
            this.DepartureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DepartureTime.Width = 177;
            // 
            // DispatchStation
            // 
            this.DispatchStation.Text = "Станция отбытия";
            this.DispatchStation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DispatchStation.Width = 303;
            // 
            // StationOfDestination
            // 
            this.StationOfDestination.Text = "Станция прибытия";
            this.StationOfDestination.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StationOfDestination.Width = 241;
            // 
            // DaysFollowing
            // 
            this.DaysFollowing.Text = "Дни следования";
            this.DaysFollowing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DaysFollowing.Width = 430;
            // 
            // RegulatorySheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1643, 696);
            this.Controls.Add(this.listRegSh);
            this.Name = "RegulatorySheduleForm";
            this.Text = "RegulatorySheduleForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listRegSh;
        private System.Windows.Forms.ColumnHeader NumberOfTrain;
        private System.Windows.Forms.ColumnHeader RouteName;
        private System.Windows.Forms.ColumnHeader ArrivalTime;
        private System.Windows.Forms.ColumnHeader DepartureTime;
        private System.Windows.Forms.ColumnHeader DispatchStation;
        private System.Windows.Forms.ColumnHeader StationOfDestination;
        private System.Windows.Forms.ColumnHeader DaysFollowing;
    }
}