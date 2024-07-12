namespace ADS
{
    partial class readXml
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
            selectionButton = new Button();
            processButton = new Button();
            selectionText = new Label();
            processText = new Label();
            labelHost = new Label();
            textBoxHost = new TextBox();
            buttonSave = new Button();
            label4 = new Label();
            labelBanco = new Label();
            textBoxBanco = new TextBox();
            labelUser = new Label();
            textBoxUser = new TextBox();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            labelTest = new Label();
            buttonTest = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // selectionButton
            // 
            selectionButton.Location = new Point(201, 27);
            selectionButton.Name = "selectionButton";
            selectionButton.Size = new Size(109, 27);
            selectionButton.TabIndex = 0;
            selectionButton.Text = "Selecionar";
            selectionButton.UseVisualStyleBackColor = true;
            selectionButton.Click += selectionButton_Click;
            // 
            // processButton
            // 
            processButton.Location = new Point(201, 75);
            processButton.Name = "processButton";
            processButton.Size = new Size(109, 27);
            processButton.TabIndex = 1;
            processButton.Text = "Processar";
            processButton.UseVisualStyleBackColor = true;
            processButton.Click += processButton_Click;
            // 
            // selectionText
            // 
            selectionText.AutoSize = true;
            selectionText.Location = new Point(201, 9);
            selectionText.Name = "selectionText";
            selectionText.Size = new Size(97, 15);
            selectionText.TabIndex = 2;
            selectionText.Text = "Selecione a pasta";
            // 
            // processText
            // 
            processText.AutoSize = true;
            processText.Location = new Point(201, 57);
            processText.Name = "processText";
            processText.Size = new Size(143, 15);
            processText.TabIndex = 3;
            processText.Text = "Processe os arquivos XML";
            // 
            // labelHost
            // 
            labelHost.AutoSize = true;
            labelHost.Location = new Point(22, 9);
            labelHost.Name = "labelHost";
            labelHost.Size = new Size(32, 15);
            labelHost.TabIndex = 6;
            labelHost.Text = "Host";
            // 
            // textBoxHost
            // 
            textBoxHost.Location = new Point(22, 27);
            textBoxHost.Name = "textBoxHost";
            textBoxHost.Size = new Size(109, 23);
            textBoxHost.TabIndex = 7;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(201, 166);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(109, 23);
            buttonSave.TabIndex = 10;
            buttonSave.Text = "Salvar";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(201, 148);
            label4.Name = "label4";
            label4.Size = new Size(116, 15);
            label4.TabIndex = 11;
            label4.Text = "Salve a configuração";
            // 
            // labelBanco
            // 
            labelBanco.AutoSize = true;
            labelBanco.Location = new Point(22, 141);
            labelBanco.Name = "labelBanco";
            labelBanco.Size = new Size(40, 15);
            labelBanco.TabIndex = 12;
            labelBanco.Text = "Banco";
            // 
            // textBoxBanco
            // 
            textBoxBanco.Location = new Point(22, 159);
            textBoxBanco.Name = "textBoxBanco";
            textBoxBanco.Size = new Size(109, 23);
            textBoxBanco.TabIndex = 13;
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Location = new Point(22, 53);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(30, 15);
            labelUser.TabIndex = 14;
            labelUser.Text = "User";
            // 
            // textBoxUser
            // 
            textBoxUser.Location = new Point(22, 71);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new Size(109, 23);
            textBoxUser.TabIndex = 15;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(22, 97);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(39, 15);
            labelPassword.TabIndex = 16;
            labelPassword.Text = "Senha";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(22, 115);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(109, 23);
            textBoxPassword.TabIndex = 17;
            // 
            // labelTest
            // 
            labelTest.AutoSize = true;
            labelTest.Location = new Point(201, 105);
            labelTest.Name = "labelTest";
            labelTest.Size = new Size(92, 15);
            labelTest.TabIndex = 18;
            labelTest.Text = "Teste a Conexão";
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(201, 123);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(109, 22);
            buttonTest.TabIndex = 19;
            buttonTest.Text = "Testar";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += buttonTest_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(22, 210);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(288, 23);
            textBox1.TabIndex = 20;
            // 
            // readXml
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(348, 245);
            Controls.Add(textBox1);
            Controls.Add(buttonTest);
            Controls.Add(labelTest);
            Controls.Add(textBoxPassword);
            Controls.Add(labelPassword);
            Controls.Add(textBoxUser);
            Controls.Add(labelUser);
            Controls.Add(textBoxBanco);
            Controls.Add(labelBanco);
            Controls.Add(label4);
            Controls.Add(buttonSave);
            Controls.Add(textBoxHost);
            Controls.Add(labelHost);
            Controls.Add(processText);
            Controls.Add(selectionText);
            Controls.Add(processButton);
            Controls.Add(selectionButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "readXml";
            Text = "Read Xml";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button selectionButton;
        private Button processButton;
        private Label selectionText;
        private Label processText;
        private Label labelHost;
        private TextBox textBoxHost;
        private Button buttonSave;
        private Label label4;
        private Label labelBanco;
        private TextBox textBoxBanco;
        private Label labelUser;
        private TextBox textBoxUser;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Label labelTest;
        private Button buttonTest;
        private TextBox textBox1;
    }
}
