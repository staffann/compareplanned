namespace CompareView
{
    partial class CompareViewControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.itemSelectorBase1 = new ZoneFiveSoftware.Common.Visuals.ItemSelectorBase();
            this.CompareTreeList = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.CompareTLMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TableSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExpandAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CollapseAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GoToMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GoToPlannedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GoToPerformedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToOverlayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendPlannedToOverlayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendPerformedToOverlayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendAllToOverlayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.CompareTLMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is the CompareView control";
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.itemSelectorBase1);
            this.MainSplitContainer.Panel1.Enabled = false;
            this.MainSplitContainer.Panel1Collapsed = true;
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.CompareTreeList);
            this.MainSplitContainer.Size = new System.Drawing.Size(767, 382);
            this.MainSplitContainer.SplitterDistance = 191;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // itemSelectorBase1
            // 
            this.itemSelectorBase1.BackColor = System.Drawing.Color.Transparent;
            this.itemSelectorBase1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.itemSelectorBase1.ButtonImage = null;
            this.itemSelectorBase1.Location = new System.Drawing.Point(4, 4);
            this.itemSelectorBase1.Name = "itemSelectorBase1";
            this.itemSelectorBase1.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.itemSelectorBase1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.itemSelectorBase1.Size = new System.Drawing.Size(100, 20);
            this.itemSelectorBase1.TabIndex = 0;
            // 
            // CompareTreeList
            // 
            this.CompareTreeList.BackColor = System.Drawing.Color.Transparent;
            this.CompareTreeList.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.CompareTreeList.CheckBoxes = false;
            this.CompareTreeList.ContextMenuStrip = this.CompareTLMenuStrip;
            this.CompareTreeList.DefaultIndent = 15;
            this.CompareTreeList.DefaultRowHeight = -1;
            this.CompareTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompareTreeList.HeaderRowHeight = 21;
            this.CompareTreeList.Location = new System.Drawing.Point(0, 0);
            this.CompareTreeList.MultiSelect = true;
            this.CompareTreeList.Name = "CompareTreeList";
            this.CompareTreeList.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.Auto;
            this.CompareTreeList.NumLockedColumns = 0;
            this.CompareTreeList.RowAlternatingColors = true;
            this.CompareTreeList.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.CompareTreeList.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.CompareTreeList.RowHotlightMouse = true;
            this.CompareTreeList.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.CompareTreeList.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.CompareTreeList.RowSeparatorLines = true;
            this.CompareTreeList.ShowLines = false;
            this.CompareTreeList.ShowPlusMinus = true;
            this.CompareTreeList.Size = new System.Drawing.Size(767, 382);
            this.CompareTreeList.TabIndex = 0;
            // 
            // CompareTLMenuStrip
            // 
            this.CompareTLMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TableSettingsMenuItem,
            this.ExpandAllMenuItem,
            this.CollapseAllMenuItem,
            this.GoToMenuItem,
            this.SendToOverlayMenuItem});
            this.CompareTLMenuStrip.Name = "CompareTLMenuStrip";
            this.CompareTLMenuStrip.Size = new System.Drawing.Size(158, 136);
            this.CompareTLMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.CompareTLMenuStrip_Opening);
            // 
            // TableSettingsMenuItem
            // 
            this.TableSettingsMenuItem.Name = "TableSettingsMenuItem";
            this.TableSettingsMenuItem.Size = new System.Drawing.Size(157, 22);
            this.TableSettingsMenuItem.Text = "Table settings";
            this.TableSettingsMenuItem.Click += new System.EventHandler(this.tableSettingsMenuItem_Click);
            // 
            // ExpandAllMenuItem
            // 
            this.ExpandAllMenuItem.Name = "ExpandAllMenuItem";
            this.ExpandAllMenuItem.Size = new System.Drawing.Size(157, 22);
            this.ExpandAllMenuItem.Text = "Expand all";
            this.ExpandAllMenuItem.Click += new System.EventHandler(this.ExpandAllMenuItem_Click);
            // 
            // CollapseAllMenuItem
            // 
            this.CollapseAllMenuItem.Name = "CollapseAllMenuItem";
            this.CollapseAllMenuItem.Size = new System.Drawing.Size(157, 22);
            this.CollapseAllMenuItem.Text = "Collapse all";
            this.CollapseAllMenuItem.Click += new System.EventHandler(this.CollapseAllMenuItem_Click);
            // 
            // GoToMenuItem
            // 
            this.GoToMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GoToPlannedMenuItem,
            this.GoToPerformedMenuItem});
            this.GoToMenuItem.Name = "GoToMenuItem";
            this.GoToMenuItem.Size = new System.Drawing.Size(157, 22);
            this.GoToMenuItem.Text = "Go to";
            // 
            // GoToPlannedMenuItem
            // 
            this.GoToPlannedMenuItem.Name = "GoToPlannedMenuItem";
            this.GoToPlannedMenuItem.Size = new System.Drawing.Size(130, 22);
            this.GoToPlannedMenuItem.Text = "Planned";
            this.GoToPlannedMenuItem.Click += new System.EventHandler(this.GoToPlannedMenuItem_Click);
            // 
            // GoToPerformedMenuItem
            // 
            this.GoToPerformedMenuItem.Name = "GoToPerformedMenuItem";
            this.GoToPerformedMenuItem.Size = new System.Drawing.Size(130, 22);
            this.GoToPerformedMenuItem.Text = "Performed";
            this.GoToPerformedMenuItem.Click += new System.EventHandler(this.GoToPerformedMenuItem_Click);
            // 
            // SendToOverlayMenuItem
            // 
            this.SendToOverlayMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendPlannedToOverlayMenuItem,
            this.SendPerformedToOverlayMenuItem,
            this.SendAllToOverlayMenuItem});
            this.SendToOverlayMenuItem.Name = "SendToOverlayMenuItem";
            this.SendToOverlayMenuItem.Size = new System.Drawing.Size(157, 22);
            this.SendToOverlayMenuItem.Text = "Send to Overlay";
            // 
            // SendPlannedToOverlayMenuItem
            // 
            this.SendPlannedToOverlayMenuItem.Name = "SendPlannedToOverlayMenuItem";
            this.SendPlannedToOverlayMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SendPlannedToOverlayMenuItem.Text = "Planned";
            this.SendPlannedToOverlayMenuItem.Click += new System.EventHandler(this.SendToOverlayMenuItem_Click);
            // 
            // SendPerformedToOverlayMenuItem
            // 
            this.SendPerformedToOverlayMenuItem.Name = "SendPerformedToOverlayMenuItem";
            this.SendPerformedToOverlayMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SendPerformedToOverlayMenuItem.Text = "Performed";
            this.SendPerformedToOverlayMenuItem.Click += new System.EventHandler(this.SendToOverlayMenuItem_Click);
            // 
            // SendAllToOverlayMenuItem
            // 
            this.SendAllToOverlayMenuItem.Name = "SendAllToOverlayMenuItem";
            this.SendAllToOverlayMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SendAllToOverlayMenuItem.Text = "All";
            this.SendAllToOverlayMenuItem.Click += new System.EventHandler(this.SendToOverlayMenuItem_Click);
            // 
            // CompareViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.label1);
            this.Name = "CompareViewControl";
            this.Size = new System.Drawing.Size(767, 382);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.ResumeLayout(false);
            this.CompareTLMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private ZoneFiveSoftware.Common.Visuals.ItemSelectorBase itemSelectorBase1;
        private ZoneFiveSoftware.Common.Visuals.TreeList CompareTreeList;
        private System.Windows.Forms.ContextMenuStrip CompareTLMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem TableSettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExpandAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CollapseAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GoToMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GoToPlannedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GoToPerformedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendToOverlayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendPlannedToOverlayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendPerformedToOverlayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendAllToOverlayMenuItem;
    }
}