using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public static class Rules
    {
        public static List<NamedRule> LifeRules = new List<NamedRule>()
        {
            new NamedRule("Replicator", "B1357/S1357"),
            new NamedRule("Fredkin", "B1357/S02468"),
            new NamedRule("Seeds", "B2/S"),
            new NamedRule("Live Free or Die", "B2/S0"),
            new NamedRule("Life without death", "B3/S012345678"),
            new NamedRule("Flock", "B3/S12"),
            new NamedRule("Mazectric", "B3/S1234"),
            new NamedRule("Maze", "B3/S12345"),
            new NamedRule("Conway's Game of Life", "B3/S23"),
            new NamedRule("2×2", "B36/S125"),
            new NamedRule("HighLife", "B36/S23"),
            new NamedRule("Move", "B368/S245"),
            new NamedRule("Day & Night", "B3678/S34678"),
            new NamedRule("DryLife", "B37/S23"),
            new NamedRule("Pedestrian Life", "B38/S23")
        };
    }
}
