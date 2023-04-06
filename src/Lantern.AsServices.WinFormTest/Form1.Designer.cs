namespace Lantern.AsService.WinFormTest
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
            BtnCreateWindow = new Button();
            TbUrl = new TextBox();
            BtnGoto = new Button();
            TbScript = new TextBox();
            BtnEvaluate = new Button();
            TbClickSelector = new TextBox();
            BtnClick = new Button();
            BtnFill = new Button();
            TbFillSelector = new TextBox();
            TbFillContent = new TextBox();
            BtnHidden = new Button();
            BtnClose = new Button();
            TbWatForSelector = new TextBox();
            BtnWaitForSelector = new Button();
            BtnFacebookLogin = new Button();
            TbWaitForUrl = new TextBox();
            BtnWaitForUrl = new Button();
            LbConsoleOutput = new ListBox();
            BtnGoogleSearch = new Button();
            BtnWaitForLoad = new Button();
            BtnThrowException = new Button();
            SuspendLayout();
            // 
            // BtnCreateWindow
            // 
            BtnCreateWindow.Location = new Point(211, 25);
            BtnCreateWindow.Name = "BtnCreateWindow";
            BtnCreateWindow.Size = new Size(135, 29);
            BtnCreateWindow.TabIndex = 0;
            BtnCreateWindow.Text = "显示或创建窗口";
            BtnCreateWindow.UseVisualStyleBackColor = true;
            BtnCreateWindow.Click += BtnCreateWindow_Click;
            // 
            // TbUrl
            // 
            TbUrl.Location = new Point(22, 137);
            TbUrl.Name = "TbUrl";
            TbUrl.Size = new Size(594, 27);
            TbUrl.TabIndex = 1;
            TbUrl.Text = "https://bing.com";
            // 
            // BtnGoto
            // 
            BtnGoto.Location = new Point(640, 135);
            BtnGoto.Name = "BtnGoto";
            BtnGoto.Size = new Size(149, 29);
            BtnGoto.TabIndex = 2;
            BtnGoto.Text = "Goto";
            BtnGoto.UseVisualStyleBackColor = true;
            BtnGoto.Click += BtnGoto_Click;
            // 
            // TbScript
            // 
            TbScript.Location = new Point(22, 196);
            TbScript.Name = "TbScript";
            TbScript.Size = new Size(594, 27);
            TbScript.TabIndex = 1;
            TbScript.Text = "document.querySelector('#sb_form_q').value = 'aaa'";
            // 
            // BtnEvaluate
            // 
            BtnEvaluate.Location = new Point(640, 193);
            BtnEvaluate.Name = "BtnEvaluate";
            BtnEvaluate.Size = new Size(149, 29);
            BtnEvaluate.TabIndex = 2;
            BtnEvaluate.Text = "Evaluate";
            BtnEvaluate.UseVisualStyleBackColor = true;
            BtnEvaluate.Click += BtnEvaluate_Click;
            // 
            // TbClickSelector
            // 
            TbClickSelector.Location = new Point(22, 300);
            TbClickSelector.Name = "TbClickSelector";
            TbClickSelector.Size = new Size(594, 27);
            TbClickSelector.TabIndex = 3;
            TbClickSelector.Text = "#sb_form_go";
            // 
            // BtnClick
            // 
            BtnClick.Location = new Point(640, 297);
            BtnClick.Name = "BtnClick";
            BtnClick.Size = new Size(149, 29);
            BtnClick.TabIndex = 4;
            BtnClick.Text = "Click";
            BtnClick.UseVisualStyleBackColor = true;
            BtnClick.Click += BtnClick_Click;
            // 
            // BtnFill
            // 
            BtnFill.Location = new Point(640, 244);
            BtnFill.Name = "BtnFill";
            BtnFill.Size = new Size(149, 29);
            BtnFill.TabIndex = 5;
            BtnFill.Text = "Fill";
            BtnFill.UseVisualStyleBackColor = true;
            BtnFill.Click += BtnFill_Click;
            // 
            // TbFillSelector
            // 
            TbFillSelector.Location = new Point(22, 247);
            TbFillSelector.Name = "TbFillSelector";
            TbFillSelector.Size = new Size(151, 27);
            TbFillSelector.TabIndex = 6;
            TbFillSelector.Text = "#sb_form_q";
            // 
            // TbFillContent
            // 
            TbFillContent.Location = new Point(211, 247);
            TbFillContent.Name = "TbFillContent";
            TbFillContent.Size = new Size(405, 27);
            TbFillContent.TabIndex = 6;
            TbFillContent.Text = "这是内容";
            // 
            // BtnHidden
            // 
            BtnHidden.Location = new Point(111, 25);
            BtnHidden.Name = "BtnHidden";
            BtnHidden.Size = new Size(94, 29);
            BtnHidden.TabIndex = 0;
            BtnHidden.Text = "隐藏窗口";
            BtnHidden.UseVisualStyleBackColor = true;
            BtnHidden.Click += BtnHidden_Click;
            // 
            // BtnClose
            // 
            BtnClose.Location = new Point(11, 25);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(94, 29);
            BtnClose.TabIndex = 0;
            BtnClose.Text = "关闭窗口";
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += BtnClose_Click;
            // 
            // TbWatForSelector
            // 
            TbWatForSelector.Location = new Point(22, 353);
            TbWatForSelector.Name = "TbWatForSelector";
            TbWatForSelector.Size = new Size(594, 27);
            TbWatForSelector.TabIndex = 3;
            TbWatForSelector.Text = "#sb_form_go";
            // 
            // BtnWaitForSelector
            // 
            BtnWaitForSelector.Location = new Point(640, 351);
            BtnWaitForSelector.Name = "BtnWaitForSelector";
            BtnWaitForSelector.Size = new Size(149, 29);
            BtnWaitForSelector.TabIndex = 4;
            BtnWaitForSelector.Text = "WaitForSelector";
            BtnWaitForSelector.UseVisualStyleBackColor = true;
            BtnWaitForSelector.Click += BtnWaitForSelector_Click;
            // 
            // BtnFacebookLogin
            // 
            BtnFacebookLogin.Location = new Point(12, 79);
            BtnFacebookLogin.Name = "BtnFacebookLogin";
            BtnFacebookLogin.Size = new Size(146, 29);
            BtnFacebookLogin.TabIndex = 7;
            BtnFacebookLogin.Text = "Facebook Login";
            BtnFacebookLogin.UseVisualStyleBackColor = true;
            BtnFacebookLogin.Click += BtnFacebookLogin_Click;
            // 
            // TbWaitForUrl
            // 
            TbWaitForUrl.Location = new Point(22, 410);
            TbWaitForUrl.Name = "TbWaitForUrl";
            TbWaitForUrl.Size = new Size(594, 27);
            TbWaitForUrl.TabIndex = 3;
            // 
            // BtnWaitForUrl
            // 
            BtnWaitForUrl.Location = new Point(640, 408);
            BtnWaitForUrl.Name = "BtnWaitForUrl";
            BtnWaitForUrl.Size = new Size(149, 29);
            BtnWaitForUrl.TabIndex = 4;
            BtnWaitForUrl.Text = "WaitForUrl";
            BtnWaitForUrl.UseVisualStyleBackColor = true;
            BtnWaitForUrl.Click += BtnWaitForUrl_Click;
            // 
            // LbConsoleOutput
            // 
            LbConsoleOutput.FormattingEnabled = true;
            LbConsoleOutput.ItemHeight = 20;
            LbConsoleOutput.Location = new Point(11, 480);
            LbConsoleOutput.Name = "LbConsoleOutput";
            LbConsoleOutput.Size = new Size(1086, 244);
            LbConsoleOutput.TabIndex = 8;
            // 
            // BtnGoogleSearch
            // 
            BtnGoogleSearch.Location = new Point(188, 79);
            BtnGoogleSearch.Name = "BtnGoogleSearch";
            BtnGoogleSearch.Size = new Size(146, 29);
            BtnGoogleSearch.TabIndex = 7;
            BtnGoogleSearch.Text = "Google Search";
            BtnGoogleSearch.UseVisualStyleBackColor = true;
            BtnGoogleSearch.Click += BtnGoogleSearch_Click;
            // 
            // BtnWaitForLoad
            // 
            BtnWaitForLoad.Location = new Point(809, 410);
            BtnWaitForLoad.Name = "BtnWaitForLoad";
            BtnWaitForLoad.Size = new Size(149, 29);
            BtnWaitForLoad.TabIndex = 4;
            BtnWaitForLoad.Text = "WaitForLoad";
            BtnWaitForLoad.UseVisualStyleBackColor = true;
            BtnWaitForLoad.Click += BtnWaitForLoad_Click;
            // 
            // BtnThrowException
            // 
            BtnThrowException.Location = new Point(470, 25);
            BtnThrowException.Name = "BtnThrowException";
            BtnThrowException.Size = new Size(146, 29);
            BtnThrowException.TabIndex = 7;
            BtnThrowException.Text = "Throw Exception";
            BtnThrowException.UseVisualStyleBackColor = true;
            BtnThrowException.Click += BtnThrowException_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1109, 738);
            Controls.Add(LbConsoleOutput);
            Controls.Add(BtnThrowException);
            Controls.Add(BtnGoogleSearch);
            Controls.Add(BtnFacebookLogin);
            Controls.Add(TbFillContent);
            Controls.Add(TbFillSelector);
            Controls.Add(BtnFill);
            Controls.Add(BtnWaitForLoad);
            Controls.Add(BtnWaitForUrl);
            Controls.Add(BtnWaitForSelector);
            Controls.Add(TbWaitForUrl);
            Controls.Add(BtnClick);
            Controls.Add(TbWatForSelector);
            Controls.Add(TbClickSelector);
            Controls.Add(BtnEvaluate);
            Controls.Add(BtnGoto);
            Controls.Add(TbScript);
            Controls.Add(TbUrl);
            Controls.Add(BtnClose);
            Controls.Add(BtnHidden);
            Controls.Add(BtnCreateWindow);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnCreateWindow;
        private TextBox TbUrl;
        private Button BtnGoto;
        private TextBox TbScript;
        private Button BtnEvaluate;
        private TextBox TbClickSelector;
        private Button BtnClick;
        private Button BtnFill;
        private TextBox TbFillSelector;
        private TextBox TbFillContent;
        private Button BtnHidden;
        private Button BtnClose;
        private TextBox TbWatForSelector;
        private Button BtnWaitForSelector;
        private Button BtnFacebookLogin;
        private TextBox TbWaitForUrl;
        private Button BtnWaitForUrl;
        private ListBox LbConsoleOutput;
        private Button BtnGoogleSearch;
        private Button BtnWaitForLoad;
        private Button BtnThrowException;
    }
}