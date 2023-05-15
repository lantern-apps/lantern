namespace WinFormsApp1
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
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            BtnNavigate = new Button();
            TbUrl = new TextBox();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(12, 94);
            webView21.Name = "webView21";
            webView21.Size = new Size(776, 344);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // BtnNavigate
            // 
            BtnNavigate.Location = new Point(694, 33);
            BtnNavigate.Name = "BtnNavigate";
            BtnNavigate.Size = new Size(94, 29);
            BtnNavigate.TabIndex = 1;
            BtnNavigate.Text = "导航";
            BtnNavigate.UseVisualStyleBackColor = true;
            BtnNavigate.Click += BtnNavigate_Click;
            // 
            // TbUrl
            // 
            TbUrl.Location = new Point(8, 32);
            TbUrl.Name = "TbUrl";
            TbUrl.Size = new Size(664, 27);
            TbUrl.TabIndex = 2;
            TbUrl.Text = "https://www.facebook.com/login";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TbUrl);
            Controls.Add(BtnNavigate);
            Controls.Add(webView21);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Button BtnNavigate;
        private TextBox TbUrl;
    }
}