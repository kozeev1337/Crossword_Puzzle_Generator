using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Crossword_Puzzle_Generator
{
    public partial class Form1 : Form
    {
        private CrosswordGenerator crosswordGenerator;
        private List<CrosswordGenerator.CrosswordWord> currentCrossword;
        private char[,] currentGrid;
        private const int CellSize = 35;
        private Font cellFont = new Font("Arial", 14);
        private Font numberFont = new Font("Arial", 8);
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            crosswordGenerator = new CrosswordGenerator();

            // ����������� ����������� �������
            generateButton.Click += GenerateButton_Click;
            clearButton.Click += ClearButton_Click;

            // ��������� Paint ������� ��� ��������� ����������
            crosswordCanvas.Paint += CrosswordCanvas_Paint;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            GenerateCrossword();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearCrossword();
        }

        private void GenerateCrossword()
        {
            try
            {
                string input = wordInput.Text.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("������� ����� ��� ��������� ����������!", "������",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // �������� ����� �� �����
                var inputWords = GetWordsFromInput(input);

                if (inputWords.Count == 0)
                {
                    MessageBox.Show("������� ���� �� ���� �����!", "������",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ���������� ���������
                currentCrossword = crosswordGenerator.GenerateCrossword(inputWords);
                currentGrid = crosswordGenerator.GetGrid();

                // ���������, ������� ���� ������� ����������
                if (currentCrossword.Count == 0)
                {
                    MessageBox.Show("�� ������� ������������� ���������. ���������� ������ �����.",
                        "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ���������� ����������
                MessageBox.Show($"������� ���������� {currentCrossword.Count} �� {inputWords.Count} ����",
                    "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ���������� ����� �� ����������� � ���������
                DisplayWordLists();

                // �������������� ���������
                crosswordCanvas.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ��������� ����������: {ex.Message}",
                    "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> GetWordsFromInput(string input)
        {
            var result = new List<string>();

            var inputLines = input.Split(new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in inputLines)
            {
                string word = line.Trim().ToUpper();

                if (!string.IsNullOrWhiteSpace(word))
                {
                    result.Add(word);
                }
            }

            return result;
        }

        private void DisplayWordLists()
        {
            if (currentCrossword == null) return;

            // �������� ������ ����
            var horizontalWordsList = crosswordGenerator.GetHorizontalWords();
            var verticalWordsList = crosswordGenerator.GetVerticalWords();

            // ���������� ����� �� �����������
            horizontalWords.Clear();
            foreach (var word in horizontalWordsList)
            {
                horizontalWords.AppendText($"{word.Number}. {word.Word}" + Environment.NewLine);
            }

            // ���������� ����� �� ���������
            verticalWords.Clear();
            if (verticalWordsList.Count > 0)
            {
                foreach (var word in verticalWordsList)
                {
                    verticalWords.AppendText($"{word.Number}. {word.Word}" + Environment.NewLine);
                }
            }
            else
            {
                verticalWords.AppendText("������������ ����� �� �������" + Environment.NewLine);
            }
        }

        private void CrosswordCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (currentGrid == null) return;

            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int gridSize = currentGrid.GetLength(0);
            int cellSize = CellSize;

            // ������� ������� ������� �������
            int minX = gridSize, maxX = 0;
            int minY = gridSize, maxY = 0;
            bool hasLetters = false;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (currentGrid[i, j] != ' ')
                    {
                        hasLetters = true;
                        if (j < minX) minX = j;
                        if (j > maxX) maxX = j;
                        if (i < minY) minY = i;
                        if (i > maxY) maxY = i;
                    }
                }
            }

            if (!hasLetters) return;

            // ��������� �������
            minX = Math.Max(0, minX - 1);
            maxX = Math.Min(gridSize - 1, maxX + 1);
            minY = Math.Max(0, minY - 1);
            maxY = Math.Min(gridSize - 1, maxY + 1);

            // ��������� ������� ��� �������������
            int width = (maxX - minX + 1) * cellSize;
            int height = (maxY - minY + 1) * cellSize;

            int offsetX = Math.Max(10, (crosswordCanvas.Width - width) / 2);
            int offsetY = Math.Max(10, (crosswordCanvas.Height - height) / 2);

            // ������ ����� � �����
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j <= maxX; j++)
                {
                    int x = offsetX + (j - minX) * cellSize;
                    int y = offsetY + (i - minY) * cellSize;

                    g.FillRectangle(Brushes.White, x, y, cellSize, cellSize);
                    g.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);

                    if (currentGrid[i, j] != ' ')
                    {
                        g.FillRectangle(Brushes.LightYellow, x + 1, y + 1, cellSize - 2, cellSize - 2);

                        string letter = currentGrid[i, j].ToString();
                        SizeF letterSize = g.MeasureString(letter, cellFont);
                        float letterX = x + (cellSize - letterSize.Width) / 2;
                        float letterY = y + (cellSize - letterSize.Height) / 2;
                        g.DrawString(letter, cellFont, Brushes.Black, letterX, letterY);
                    }
                }
            }

            // ������ ������ ����
            if (currentCrossword != null)
            {
                foreach (var word in currentCrossword)
                {
                    int x = offsetX + (word.X - minX) * cellSize;
                    int y = offsetY + (word.Y - minY) * cellSize;

                    string number = word.Number.ToString();
                    SizeF numberSize = g.MeasureString(number, numberFont);
                    float numberX = x + 2;
                    float numberY = y + 2;

                    g.FillRectangle(Brushes.White, numberX - 1, numberY - 1,
                        numberSize.Width + 2, numberSize.Height + 2);
                    g.DrawString(number, numberFont, Brushes.Red, numberX, numberY);
                }
            }
        }

        private void ClearCrossword()
        {
            wordInput.Clear();
            horizontalWords.Clear();
            verticalWords.Clear();
            currentCrossword = null;
            currentGrid = null;
            crosswordCanvas.Invalidate();
        }
    }
}