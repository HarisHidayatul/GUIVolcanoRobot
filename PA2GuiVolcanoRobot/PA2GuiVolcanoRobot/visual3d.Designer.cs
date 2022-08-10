
namespace PA2GuiVolcanoRobot
{
    partial class visual3d
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
            this.GL_Monitor = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GL_Monitor
            // 
            this.GL_Monitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GL_Monitor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GL_Monitor.Location = new System.Drawing.Point(0, 2);
            this.GL_Monitor.Name = "GL_Monitor";
            this.GL_Monitor.Size = new System.Drawing.Size(502, 288);
            this.GL_Monitor.TabIndex = 15;
            this.GL_Monitor.VSync = false;
            this.GL_Monitor.Load += new System.EventHandler(this.GL_Monitor_Load);
            this.GL_Monitor.Paint += new System.Windows.Forms.PaintEventHandler(this.GL_Monitor_Paint);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // visual3d
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(503, 292);
            this.Controls.Add(this.GL_Monitor);
            this.Name = "visual3d";
            this.Text = "visual3d";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.visual3d_FormClosing);
            this.Load += new System.EventHandler(this.visual3d_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl GL_Monitor;
        private System.Windows.Forms.Timer timer1;
    }
}