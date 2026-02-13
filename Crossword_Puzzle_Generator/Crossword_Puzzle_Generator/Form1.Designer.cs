using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Crossword_Puzzle_Generator
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
            mainPanel = new Panel();
            rightPanel = new Panel();
            crosswordPanel = new Panel();
            crosswordCanvas = new Panel();
            leftPanel = new Panel();
            verticalWords = new TextBox();
            verticalLabel = new Label();
            horizontalWords = new TextBox();
            horizontalLabel = new Label();
            buttonPanel = new Panel();
            clearButton = new Button();
            generateButton = new Button();
            wordInput = new TextBox();
            inputLabel = new Label();
            mainPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            crosswordPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(rightPanel);
            mainPanel.Controls.Add(leftPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(5);
            mainPanel.Size = new Size(1370, 654);
            mainPanel.TabIndex = 0;
            // 
            // rightPanel
            // 
            rightPanel.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(crosswordPanel);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(605, 5);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(760, 644);
            rightPanel.TabIndex = 1;
            // 
            // crosswordPanel
            // 
            crosswordPanel.BackColor = Color.White;
            crosswordPanel.Controls.Add(crosswordCanvas);
            crosswordPanel.Dock = DockStyle.Fill;
            crosswordPanel.Location = new Point(0, 0);
            crosswordPanel.Name = "crosswordPanel";
            crosswordPanel.Size = new Size(758, 642);
            crosswordPanel.TabIndex = 0;
            // 
            // crosswordCanvas
            // 
            crosswordCanvas.Dock = DockStyle.Fill;
            crosswordCanvas.Location = new Point(0, 0);
            crosswordCanvas.Name = "crosswordCanvas";
            crosswordCanvas.Size = new Size(758, 642);
            crosswordCanvas.TabIndex = 0;
            // 
            // leftPanel
            // 
            leftPanel.BorderStyle = BorderStyle.FixedSingle;
            leftPanel.Controls.Add(verticalWords);
            leftPanel.Controls.Add(verticalLabel);
            leftPanel.Controls.Add(horizontalWords);
            leftPanel.Controls.Add(horizontalLabel);
            leftPanel.Controls.Add(buttonPanel);
            leftPanel.Controls.Add(wordInput);
            leftPanel.Controls.Add(inputLabel);
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Location = new Point(5, 5);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(600, 644);
            leftPanel.TabIndex = 0;
            // 
            // verticalWords
            // 
            verticalWords.Location = new Point(10, 488);
            verticalWords.Multiline = true;
            verticalWords.Name = "verticalWords";
            verticalWords.ReadOnly = true;
            verticalWords.ScrollBars = ScrollBars.Vertical;
            verticalWords.Size = new Size(580, 150);
            verticalWords.TabIndex = 6;
            // 
            // verticalLabel
            // 
            verticalLabel.AutoSize = true;
            verticalLabel.Location = new Point(10, 469);
            verticalLabel.Name = "verticalLabel";
            verticalLabel.Size = new Size(100, 16);
            verticalLabel.TabIndex = 5;
            verticalLabel.Text = "По вертикали:";
            // 
            // horizontalWords
            // 
            horizontalWords.Location = new Point(10, 316);
            horizontalWords.Multiline = true;
            horizontalWords.Name = "horizontalWords";
            horizontalWords.ReadOnly = true;
            horizontalWords.ScrollBars = ScrollBars.Vertical;
            horizontalWords.Size = new Size(580, 150);
            horizontalWords.TabIndex = 4;
            // 
            // horizontalLabel
            // 
            horizontalLabel.AutoSize = true;
            horizontalLabel.Location = new Point(10, 297);
            horizontalLabel.Name = "horizontalLabel";
            horizontalLabel.Size = new Size(114, 16);
            horizontalLabel.TabIndex = 3;
            horizontalLabel.Text = "По горизонтали:";
            // 
            // buttonPanel
            // 
            buttonPanel.BorderStyle = BorderStyle.FixedSingle;
            buttonPanel.Controls.Add(clearButton);
            buttonPanel.Controls.Add(generateButton);
            buttonPanel.Location = new Point(10, 250);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(580, 44);
            buttonPanel.TabIndex = 2;
            // 
            // clearButton
            // 
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.Location = new Point(159, 4);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(150, 35);
            clearButton.TabIndex = 1;
            clearButton.Text = "Очистить";
            clearButton.UseVisualStyleBackColor = true;
            // 
            // generateButton
            // 
            generateButton.FlatStyle = FlatStyle.Flat;
            generateButton.Location = new Point(3, 4);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(150, 35);
            generateButton.TabIndex = 0;
            generateButton.Text = "Сгенерировать";
            generateButton.UseVisualStyleBackColor = true;
            // 
            // wordInput
            // 
            wordInput.Location = new Point(10, 40);
            wordInput.Multiline = true;
            wordInput.Name = "wordInput";
            wordInput.ScrollBars = ScrollBars.Vertical;
            wordInput.Size = new Size(580, 200);
            wordInput.TabIndex = 1;
            // 
            // inputLabel
            // 
            inputLabel.AutoSize = true;
            inputLabel.Location = new Point(10, 12);
            inputLabel.Name = "inputLabel";
            inputLabel.Size = new Size(280, 16);
            inputLabel.TabIndex = 0;
            inputLabel.Text = "Введите слова (Каждое с новой строчки):";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 654);
            Controls.Add(mainPanel);
            DoubleBuffered = true;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Генератор кроссвордов";
            mainPanel.ResumeLayout(false);
            rightPanel.ResumeLayout(false);
            crosswordPanel.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            leftPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Label inputLabel;
        private TextBox wordInput;
        private Panel buttonPanel;
        private Button generateButton;
        private Button clearButton;
        private Label horizontalLabel;
        private TextBox horizontalWords;
        private Label verticalLabel;
        private Panel crosswordPanel;
        private Panel crosswordCanvas;
        private TextBox verticalWords;
    }
}
