using MilestoneCST_250;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilestoneGUI250
{
    public partial class GridForm : Form
    {
        private int _gridSize;
        private Board _board;
        private Stopwatch stopwatch = new Stopwatch();


        public GridForm(int size)
        {
            InitializeComponent();
            _gridSize = size;
            _board = new Board(size);
            _board.SetupLiveNeighbors();
            _board.CalculateLiveNeighbors();
            stopwatch.Start();
        }
        public void SetDifficulty(double difficulty)
        {
            _board.Difficulty = difficulty;
            _board.SetupLiveNeighbors();  // live neighbors oin difficulty
            _board.CalculateLiveNeighbors();
        }

        private void GridForm_Load(object sender, EventArgs e)
        {
            GenerateButtons();
        }
        private void GenerateButtons()
        {
            // generate all buttons
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    Button btn = new Button
                    {
                        Width = 30,
                        Height = 30,
                        Left = i * 30,
                        Top = j * 30
                    };
                    btn.MouseUp += Btn_MouseUp;
                    this.Controls.Add(btn);
                }
            }
        }
        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int col = clickedButton.Left / 30;
            int row = clickedButton.Top / 30;
            var cell = _board.Grid[row, col];

            // checks if right mouse button was clicked to place a flag
            if (e.Button == MouseButtons.Right)
            {
                // cheecks if the button is already flagged
                if (clickedButton.BackgroundImage == Properties.Resources.minesweeperFlag)
                {
                    // if already flagged, unflag it
                    clickedButton.BackgroundImage = null;
                }
                else
                {
                    clickedButton.BackgroundImage = Properties.Resources.minesweeperFlag;
                    clickedButton.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            // For left click, reveal the cell as before
            else if (e.Button == MouseButtons.Left)
            {
                if (!cell.Visited)  // Only reveal if the cell hasn't been visited before
                {
                    if (cell.Live)
                    {
                        ShowGameOver();
                    }
                    else
                    {
                        _board.FloodFill(row, col);
                        UpdateBoardUI();
                    }
                    CheckForWin();
                }
            }
        }
        private void ShowGameOver()
        {
            foreach (Button btn in this.Controls)
            {
                int col = btn.Left / 30;
                int row = btn.Top / 30;
                var cell = _board.Grid[row, col];
                if (cell.Live)
                {
                    btn.BackgroundImage = Properties.Resources.minesweeperBomb;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.BackColor = Color.Red;
                }
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            MessageBox.Show($"Game Over! Time: {elapsedTime}");
        }
        private void UpdateBoardUI()
        {
            // reflects the state of _board on the UI
            foreach (Button btn in this.Controls)
            {
                if (btn is Button)
                {
                    int col = btn.Left / 30;
                    int row = btn.Top / 30;

                    var cell = _board.Grid[row, col];

                    if (cell.Visited)
                    {
                        if (cell.LiveNeighbors == 0)
                        {
                            btn.Text = ""; // Empty for cells without neighboring mines
                            btn.Enabled = false; // Disable button after clicked
                            btn.BackColor = Color.LightGray;
                        }
                        else if (cell.LiveNeighbors < 9) // Exclude mines themselves
                        {
                            btn.Text = cell.LiveNeighbors.ToString(); // display number of neighboring mines
                            btn.Enabled = false; // disable button after it's clicked
                            btn.BackColor = Color.LightGray;
                        }
                    }

                }
            }
        }
        private void CheckForWin()
        {
            // check if the game is won
            bool won = true;

            // Iterate through all cells
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    var cell = _board.Grid[i, j];
                    // If a cell that's not a mine hasn't been visited the game hasn't been won yet
                    if (!cell.Live && !cell.Visited)
                    {
                        won = false;
                        break;
                    }
                }

                if (!won) // Break outer loop if game isn't won yet
                    break;
            }

            if (won)
            {
                // stop the stopwatch and display the time
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                UpdateHighScores(stopwatch.Elapsed);

                foreach (Button btn in this.Controls)
                {
                    if (btn is Button)
                    {
                        int col = btn.Left / 30;
                        int row = btn.Top / 30;

                        var cell = _board.Grid[row, col];
                        if (cell.Live) // This will put flag images on all mines when won
                        {
                            btn.BackgroundImage = Properties.Resources.minesweeperFlag;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                }

                MessageBox.Show($"You Win! Time: {elapsedTime}");
            }
        }
        private void UpdateHighScores(TimeSpan elapsedTime)
        {
            string initials = GetUserInitials();
            if (string.IsNullOrEmpty(initials)) // If the user cancels the input or enters nothing
            {
                initials = "AAA"; // Default to AAA or handle it differently based on your preference
            }

            var scores = ScoreManager.LoadScores();
            scores.Add(new PlayerStats
            {
                PlayerInitials = initials,
                Difficulty = _board.Difficulty,
                TimeElapsed = elapsedTime
            });
            scores.Sort();
            if (scores.Count > 5)
                scores.RemoveAt(5);
            ScoreManager.SaveScores(scores);

            // show the HighScoreForm:
            HighScoreForm highScoreForm = new HighScoreForm(_board.Difficulty);
            highScoreForm.ShowDialog();
        }
        private string GetUserInitials()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Enter your initials:", "High Score!", "", -1, -1);
        }
    }
}
