namespace OpsMgrAlertConsole
{
    partial class frmMainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainWindow));
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.lvComputers = new System.Windows.Forms.ListView();
            this.ssStatusBar = new System.Windows.Forms.StatusStrip();
            this.tssTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblAlert = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssAlert = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblAll = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssAll = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblCrit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssCrit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblWarn = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssWarn = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblOkey = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssOkey = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblNull = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssNull = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblAcs = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssAcs = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lvAlerts = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictCam1 = new System.Windows.Forms.PictureBox();
            this.txtAlertSplash = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.perfCntACS = new System.Diagnostics.PerformanceCounter();
            this.imgIcons2 = new System.Windows.Forms.ImageList(this.components);
            this.timer2Wnd = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer3Cam = new System.Windows.Forms.Timer(this.components);
            this.txtCamHost = new System.Windows.Forms.TextBox();
            this.ssStatusBar.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictCam1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perfCntACS)).BeginInit();
            this.SuspendLayout();
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "IcoNull.ico");
            this.imgIcons.Images.SetKeyName(1, "IcoOKey");
            this.imgIcons.Images.SetKeyName(2, "IcoWarn ");
            this.imgIcons.Images.SetKeyName(3, "IcoCrit");
            this.imgIcons.Images.SetKeyName(4, "IcoPause");
            this.imgIcons.Images.SetKeyName(5, "IcoExcl");
            this.imgIcons.Images.SetKeyName(6, "IcoInfo");
            this.imgIcons.Images.SetKeyName(7, "IcoMaint");
            // 
            // lvComputers
            // 
            this.lvComputers.AllowColumnReorder = true;
            this.lvComputers.BackColor = System.Drawing.Color.Azure;
            this.lvComputers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvComputers.Font = new System.Drawing.Font("Lucida Console", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvComputers.LargeImageList = this.imgIcons;
            this.lvComputers.Location = new System.Drawing.Point(0, 0);
            this.lvComputers.Name = "lvComputers";
            this.lvComputers.Size = new System.Drawing.Size(973, 129);
            this.lvComputers.SmallImageList = this.imgIcons;
            this.lvComputers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvComputers.TabIndex = 0;
            this.lvComputers.UseCompatibleStateImageBehavior = false;
            this.lvComputers.View = System.Windows.Forms.View.List;
            this.lvComputers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvComputers_KeyDown);
            // 
            // ssStatusBar
            // 
            this.ssStatusBar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ssStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssTime,
            this.tssLblAlert,
            this.tssAlert,
            this.tssLblAll,
            this.tssAll,
            this.tssLblCrit,
            this.tssCrit,
            this.tssLblWarn,
            this.tssWarn,
            this.tssLblOkey,
            this.tssOkey,
            this.tssLblNull,
            this.tssNull,
            this.tssLblAcs,
            this.tssAcs,
            this.tssInfo,
            this.tssProgBar});
            this.ssStatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ssStatusBar.Location = new System.Drawing.Point(0, 129);
            this.ssStatusBar.Name = "ssStatusBar";
            this.ssStatusBar.Size = new System.Drawing.Size(973, 29);
            this.ssStatusBar.SizingGrip = false;
            this.ssStatusBar.TabIndex = 1;
            this.ssStatusBar.Text = "statusBar";
            this.ssStatusBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ssStatusBar_KeyDown);
            // 
            // tssTime
            // 
            this.tssTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssTime.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tssTime.Margin = new System.Windows.Forms.Padding(3);
            this.tssTime.Name = "tssTime";
            this.tssTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tssTime.Size = new System.Drawing.Size(71, 23);
            this.tssTime.Text = "0:00:00";
            this.tssTime.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // tssLblAlert
            // 
            this.tssLblAlert.BackColor = System.Drawing.Color.White;
            this.tssLblAlert.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblAlert.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblAlert.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblAlert.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpExcl;
            this.tssLblAlert.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblAlert.Name = "tssLblAlert";
            this.tssLblAlert.Size = new System.Drawing.Size(61, 23);
            this.tssLblAlert.Text = "Alerts";
            this.tssLblAlert.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssAlert
            // 
            this.tssAlert.BackColor = System.Drawing.Color.White;
            this.tssAlert.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssAlert.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssAlert.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssAlert.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tssAlert.Name = "tssAlert";
            this.tssAlert.Size = new System.Drawing.Size(23, 23);
            this.tssAlert.Text = "0";
            // 
            // tssLblAll
            // 
            this.tssLblAll.BackColor = System.Drawing.Color.White;
            this.tssLblAll.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblAll.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblAll.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblAll.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblAll.Name = "tssLblAll";
            this.tssLblAll.Size = new System.Drawing.Size(74, 23);
            this.tssLblAll.Text = "Computers";
            // 
            // tssAll
            // 
            this.tssAll.BackColor = System.Drawing.Color.White;
            this.tssAll.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssAll.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssAll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssAll.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tssAll.Name = "tssAll";
            this.tssAll.Size = new System.Drawing.Size(23, 23);
            this.tssAll.Text = "0";
            // 
            // tssLblCrit
            // 
            this.tssLblCrit.BackColor = System.Drawing.Color.White;
            this.tssLblCrit.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblCrit.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblCrit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tssLblCrit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblCrit.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpCrit;
            this.tssLblCrit.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblCrit.Name = "tssLblCrit";
            this.tssLblCrit.Size = new System.Drawing.Size(20, 23);
            this.tssLblCrit.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssCrit
            // 
            this.tssCrit.BackColor = System.Drawing.Color.White;
            this.tssCrit.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssCrit.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssCrit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssCrit.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.tssCrit.Name = "tssCrit";
            this.tssCrit.Size = new System.Drawing.Size(23, 23);
            this.tssCrit.Text = "0";
            // 
            // tssLblWarn
            // 
            this.tssLblWarn.BackColor = System.Drawing.Color.White;
            this.tssLblWarn.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblWarn.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblWarn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblWarn.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpWarn;
            this.tssLblWarn.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblWarn.Name = "tssLblWarn";
            this.tssLblWarn.Size = new System.Drawing.Size(20, 23);
            this.tssLblWarn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssWarn
            // 
            this.tssWarn.BackColor = System.Drawing.Color.White;
            this.tssWarn.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssWarn.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssWarn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssWarn.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.tssWarn.Name = "tssWarn";
            this.tssWarn.Size = new System.Drawing.Size(23, 23);
            this.tssWarn.Text = "0";
            // 
            // tssLblOkey
            // 
            this.tssLblOkey.BackColor = System.Drawing.Color.White;
            this.tssLblOkey.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblOkey.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblOkey.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblOkey.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpOkey;
            this.tssLblOkey.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblOkey.Name = "tssLblOkey";
            this.tssLblOkey.Size = new System.Drawing.Size(20, 23);
            this.tssLblOkey.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssOkey
            // 
            this.tssOkey.BackColor = System.Drawing.Color.White;
            this.tssOkey.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssOkey.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssOkey.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssOkey.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.tssOkey.Name = "tssOkey";
            this.tssOkey.Size = new System.Drawing.Size(23, 23);
            this.tssOkey.Text = "0";
            // 
            // tssLblNull
            // 
            this.tssLblNull.BackColor = System.Drawing.Color.White;
            this.tssLblNull.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblNull.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblNull.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblNull.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpNull;
            this.tssLblNull.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblNull.Name = "tssLblNull";
            this.tssLblNull.Size = new System.Drawing.Size(42, 23);
            this.tssLblNull.Text = "??";
            this.tssLblNull.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssNull
            // 
            this.tssNull.BackColor = System.Drawing.Color.White;
            this.tssNull.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssNull.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssNull.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssNull.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tssNull.Name = "tssNull";
            this.tssNull.Size = new System.Drawing.Size(23, 23);
            this.tssNull.Text = "0";
            // 
            // tssLblAcs
            // 
            this.tssLblAcs.BackColor = System.Drawing.Color.White;
            this.tssLblAcs.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssLblAcs.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssLblAcs.Enabled = false;
            this.tssLblAcs.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssLblAcs.Image = global::OpsMgrAlertConsole.Properties.Resources.BmpAudit;
            this.tssLblAcs.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tssLblAcs.Name = "tssLblAcs";
            this.tssLblAcs.Size = new System.Drawing.Size(52, 23);
            this.tssLblAcs.Text = "ACS";
            this.tssLblAcs.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tssAcs
            // 
            this.tssAcs.BackColor = System.Drawing.Color.White;
            this.tssAcs.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssAcs.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssAcs.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tssAcs.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tssAcs.Name = "tssAcs";
            this.tssAcs.Size = new System.Drawing.Size(4, 23);
            // 
            // tssInfo
            // 
            this.tssInfo.Margin = new System.Windows.Forms.Padding(3);
            this.tssInfo.Name = "tssInfo";
            this.tssInfo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tssInfo.Size = new System.Drawing.Size(20, 23);
            this.tssInfo.Text = "...";
            // 
            // tssProgBar
            // 
            this.tssProgBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tssProgBar.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.tssProgBar.Name = "tssProgBar";
            this.tssProgBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tssProgBar.Size = new System.Drawing.Size(75, 23);
            this.tssProgBar.Step = 5;
            this.tssProgBar.Visible = false;
            // 
            // lvAlerts
            // 
            this.lvAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAlerts.Enabled = false;
            this.lvAlerts.FullRowSelect = true;
            this.lvAlerts.GridLines = true;
            this.lvAlerts.Location = new System.Drawing.Point(0, 0);
            this.lvAlerts.Name = "lvAlerts";
            this.lvAlerts.OwnerDraw = true;
            this.lvAlerts.Size = new System.Drawing.Size(973, 492);
            this.lvAlerts.SmallImageList = this.imgIcons;
            this.lvAlerts.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAlerts.StateImageList = this.imgIcons;
            this.lvAlerts.TabIndex = 0;
            this.lvAlerts.UseCompatibleStateImageBehavior = false;
            this.lvAlerts.View = System.Windows.Forms.View.Details;
            this.lvAlerts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvAlerts_MouseDoubleClick);
            this.lvAlerts.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvAlerts_DrawColumnHeader);
            this.lvAlerts.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvAlerts_DrawItem);
            this.lvAlerts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvAlerts_MouseUp);
            this.lvAlerts.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvAlerts_MouseMove);
            this.lvAlerts.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvAlerts_DrawSubItem);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtCamHost);
            this.splitContainer1.Panel1.Controls.Add(this.pictCam1);
            this.splitContainer1.Panel1.Controls.Add(this.lvAlerts);
            this.splitContainer1.Panel1.Controls.Add(this.txtAlertSplash);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvComputers);
            this.splitContainer1.Panel2.Controls.Add(this.ssStatusBar);
            this.splitContainer1.Size = new System.Drawing.Size(973, 654);
            this.splitContainer1.SplitterDistance = 492;
            this.splitContainer1.TabIndex = 0;
            // 
            // pictCam1
            // 
            this.pictCam1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictCam1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictCam1.Location = new System.Drawing.Point(490, 129);
            this.pictCam1.Name = "pictCam1";
            this.pictCam1.Size = new System.Drawing.Size(480, 360);
            this.pictCam1.TabIndex = 2;
            this.pictCam1.TabStop = false;
            // 
            // txtAlertSplash
            // 
            this.txtAlertSplash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtAlertSplash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlertSplash.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtAlertSplash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAlertSplash.Location = new System.Drawing.Point(0, 0);
            this.txtAlertSplash.Multiline = true;
            this.txtAlertSplash.Name = "txtAlertSplash";
            this.txtAlertSplash.ReadOnly = true;
            this.txtAlertSplash.Size = new System.Drawing.Size(973, 492);
            this.txtAlertSplash.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // perfCntACS
            // 
            this.perfCntACS.CategoryName = "Processor";
            this.perfCntACS.CounterName = "% Processor Time";
            // 
            // imgIcons2
            // 
            this.imgIcons2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons2.ImageStream")));
            this.imgIcons2.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons2.Images.SetKeyName(0, "IcoNull.ico");
            this.imgIcons2.Images.SetKeyName(1, "IcoOKey");
            this.imgIcons2.Images.SetKeyName(2, "IcoWarn ");
            this.imgIcons2.Images.SetKeyName(3, "IcoCrit");
            this.imgIcons2.Images.SetKeyName(4, "IcoPause");
            this.imgIcons2.Images.SetKeyName(5, "IcoExcl");
            this.imgIcons2.Images.SetKeyName(6, "IcoInfo");
            this.imgIcons2.Images.SetKeyName(7, "IcoMaint");
            // 
            // timer2Wnd
            // 
            this.timer2Wnd.Interval = 10000;
            this.timer2Wnd.Tick += new System.EventHandler(this.timer2Wnd_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer3Cam
            // 
            this.timer3Cam.Interval = 10000;
            this.timer3Cam.Tick += new System.EventHandler(this.timer3Cam_Tick);
            // 
            // txtCamHost
            // 
            this.txtCamHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCamHost.BackColor = System.Drawing.SystemColors.Control;
            this.txtCamHost.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCamHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCamHost.ForeColor = System.Drawing.Color.DarkRed;
            this.txtCamHost.Location = new System.Drawing.Point(490, 89);
            this.txtCamHost.Name = "txtCamHost";
            this.txtCamHost.ReadOnly = true;
            this.txtCamHost.Size = new System.Drawing.Size(480, 40);
            this.txtCamHost.TabIndex = 3;
            this.txtCamHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 654);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmMainWindow";
            this.ShowIcon = false;
            this.Text = "OpsMgr Console";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainWindow_Load);
            this.Shown += new System.EventHandler(this.frmMainWindow_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMainWindow_KeyDown);
            this.ssStatusBar.ResumeLayout(false);
            this.ssStatusBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictCam1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perfCntACS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imgIcons;
        private System.Windows.Forms.ListView lvComputers;
        private System.Windows.Forms.StatusStrip ssStatusBar;
        private System.Windows.Forms.ListView lvAlerts;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel tssAll;
        private System.Windows.Forms.ToolStripStatusLabel tssLblAcs;
        private System.Windows.Forms.ToolStripStatusLabel tssLblCrit;
        private System.Windows.Forms.ToolStripStatusLabel tssLblWarn;
        private System.Windows.Forms.ToolStripStatusLabel tssLblOkey;
        private System.Windows.Forms.ToolStripStatusLabel tssLblNull;
        private System.Windows.Forms.ToolStripProgressBar tssProgBar;
        private System.Windows.Forms.ToolStripStatusLabel tssInfo;
        private System.Windows.Forms.ToolStripStatusLabel tssLblAlert;
        private System.Windows.Forms.ToolStripStatusLabel tssLblAll;
        private System.Windows.Forms.ToolStripStatusLabel tssAlert;
        private System.Windows.Forms.ToolStripStatusLabel tssCrit;
        private System.Windows.Forms.ToolStripStatusLabel tssWarn;
        private System.Windows.Forms.ToolStripStatusLabel tssOkey;
        private System.Windows.Forms.ToolStripStatusLabel tssNull;
        private System.Windows.Forms.ToolStripStatusLabel tssAcs;
        private System.Windows.Forms.ToolStripStatusLabel tssTime;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Diagnostics.PerformanceCounter perfCntACS;
        private System.Windows.Forms.TextBox txtAlertSplash;
        private System.Windows.Forms.ImageList imgIcons2;
        private System.Windows.Forms.Timer timer2Wnd;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictCam1;
        private System.Windows.Forms.Timer timer3Cam;
        private System.Windows.Forms.TextBox txtCamHost;
    }
}

