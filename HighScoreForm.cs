using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilestoneGUI250
{
    public partial class HighScoreForm : Form
    {
        private double _difficulty;
        public HighScoreForm(double difficulty)
        {
            InitializeComponent();
            _difficulty = difficulty;
        }


        private void HighScoreForm_Load(object sender, EventArgs e)
        {
            var topScores = LoadTopScores(_difficulty);

            lstHighScores.Items.Clear();
            foreach (var score in topScores)
            {
                // Formatting
                string formattedTime = $"{score.TimeElapsed.Minutes}:{score.TimeElapsed.Seconds:00}";

                // Comma separated score
                string formattedScore = score.Score.ToString("N0");

                lstHighScores.Items.Add($"{score.PlayerInitials} - Time: {formattedTime} - Score: {formattedScore} pts");
            }
        }
        public static List<PlayerStats> LoadTopScores(double difficulty)
        {
            var allScores = ScoreManager.LoadScores();
            return allScores.Where(s => s.Difficulty == difficulty)
                            .OrderByDescending(s => s.Score)
                            .Take(5)
                            .ToList();
        }
    }
}
