using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneGUI250
{
    public class ScoreManager
    {

        private const string ScoreFileName = "highscores1.txt";

        public static List<PlayerStats> LoadScores()
        {
            var scores = new List<PlayerStats>();

            if (File.Exists(ScoreFileName))
            {
                var lines = File.ReadAllLines(ScoreFileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        scores.Add(new PlayerStats
                        {
                            PlayerInitials = parts[0],
                            Difficulty = double.Parse(parts[1]),
                            TimeElapsed = TimeSpan.Parse(parts[2])
                        });
                    }
                }
            }

            return scores;
        }

        public static void SaveScores(List<PlayerStats> scores)
        {
            var lines = scores.Select(s => $"{s.PlayerInitials},{s.Difficulty},{s.TimeElapsed}").ToArray();
            File.WriteAllLines(ScoreFileName, lines);
        }
    }
}
