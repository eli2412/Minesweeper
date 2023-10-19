using MilestoneCST_250;

namespace MilestoneGUI250
{
    public partial class Form1 : Form
    {
        private List<PlayerStats> _allScores;
        public Form1()
        {
            InitializeComponent();
            _allScores = ScoreManager.LoadScores();
        }

        private void easyBtn_Click(object sender, EventArgs e)
        {
            OpenGridForm(5, 0.10); // 5x5 grid for easy with 10% bombs
        }


        private void OpenGridForm(int size, double difficulty)
        {
            GridForm gridForm = new GridForm(size);
            gridForm.SetDifficulty(difficulty);  // Set the difficulty before setting up the board
            gridForm.Show();
            this.Hide();
        }

        private void mediumBtn_Click(object sender, EventArgs e)
        {
            OpenGridForm(10, 0.20); // 10x10 grid for medium with 20% bombs
        }

        private void hardBtn_Click(object sender, EventArgs e)
        {
            OpenGridForm(15, 0.30); // 15x15 grid for hard with 30% bombs
        }
    }
}
