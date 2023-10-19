using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneGUI250
{
    public class PlayerStats : IComparable<PlayerStats>
    {
        public string PlayerInitials { get; set; }
        public double Difficulty { get; set; }
        public TimeSpan TimeElapsed { get; set; }

        public int CompareTo(PlayerStats other)
        {
            return -1 * this.Score.CompareTo(other.Score); // Highest score first

        }
        public double Score
        {
            get
            {
                return (Difficulty + 1000) * 2 - TimeElapsed.TotalSeconds;
            }
        }

    }
}
