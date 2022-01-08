namespace Nzy3d.WinformsDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myRenderer3D = new Nzy3d.Winforms.Renderer3D();
            this.SuspendLayout();
            // 
            // myRenderer3D
            // 
            this.myRenderer3D.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.myRenderer3D.APIVersion = new System.Version(3, 3, 0, 0);
            this.myRenderer3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myRenderer3D.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.myRenderer3D.IsEventDriven = true;
            this.myRenderer3D.Location = new System.Drawing.Point(0, 0);
            this.myRenderer3D.Name = "myRenderer3D";
            this.myRenderer3D.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            this.myRenderer3D.Size = new System.Drawing.Size(800, 450);
            this.myRenderer3D.TabIndex = 0;
            this.myRenderer3D.Text = "renderer3d1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.myRenderer3D);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Winforms.Renderer3D myRenderer3D;
    }
}