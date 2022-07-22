using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public static class Rules
    {
        // https://conwaylife.com/wiki/List_of_Life-like_cellular_automata
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
            new NamedRule("Pedestrian Life", "B38/S23"),
            new NamedRule("Serviettes", "B234/S"),
            new NamedRule("Iceballs", "B25678/S5678"),
            new NamedRule("Flock", "B3/S12"),
            new NamedRule("Coral", "B3/S45678"),
            new NamedRule("Long Life", "B345/S5"),
            new NamedRule("Mazectric with Mice", "B37/S1234"),
            new NamedRule("Maze with Mice", "B37/S12345"),
            new NamedRule("Vote 4/5", "B4678/S35678"),
            new NamedRule("H-trees", "B1/S012345678"),
            new NamedRule("Gnarl", "B1/S1"),
            new NamedRule("DotLife", "B3/S023"),
            new NamedRule("Bacteria", "B34/S456"),
            new NamedRule("Gems", "B3457/S4568"),
            new NamedRule("Stains", "B3678/S235678"),
            new NamedRule("HoneyLife", "B38/S238"),
            new NamedRule("Squiggles", "B124567/S123456"),
            new NamedRule("Tiny Machines", "B2/S2345"),
            new NamedRule("Tiny Machines 2", "B012378/S1234578"),
            new NamedRule("Recycling Life", "B3/S1256"),
            new NamedRule("Complex Oscillators", "B467/S123468"),
            new NamedRule("Trees", "B3/S0134567"),
            new NamedRule("Crystals", "B01238/S01235678"),
            new NamedRule("Arcing Crystals", "B0348/S01234678"),
            new NamedRule("Star Wars", "B2/S345/C4"),
            new NamedRule("Fireworks", "B13/S2/C21"),

        };
    }
}
