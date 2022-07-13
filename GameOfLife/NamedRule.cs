using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class NamedRule
    {
        public string Name;
        public string RuleVal;
        public Rule Rule;

        public NamedRule(string name, string rule)
        {
            Name = name;
            RuleVal = rule;

            ParseRule(rule);
        }

        private void ParseRule(string rule)
        {
            var bsRules = rule.Split('/');
            var bRule = bsRules[0].Remove(0, 1).ToArray();
            var sRule = bsRules[1].Remove(0, 1).ToArray();

            int bVal = 0;
            foreach (var v in bRule)
            {
                int b = int.Parse(v.ToString());
                bVal += 1 << b;
            }

            int sVal = 0;
            foreach (var v in sRule)
            {
                int s = int.Parse(v.ToString());
                sVal += 1 << s;
            }

            Rule.B = bVal;
            Rule.S = sVal;
        }
    }
}
