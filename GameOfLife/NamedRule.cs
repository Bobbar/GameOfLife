using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class NamedRule
    {
        public string Name { get; set; }

        public string RuleVal { get; set; }

        public int Born
        {
            get { return _rule.B; }
            set { _rule.B = value; }
        }

        public int Survive
        {
            get { return _rule.S; }
            set { _rule.S = value; }
        }

        public Rule Rule
        {
            get { return _rule; }
        }

        private Rule _rule;

        public NamedRule(string name, string rule)
        {
            Name = name;
            RuleVal = rule;

            ParseRule(rule);
        }

        public NamedRule() { }

        private void ParseRule(string rule)
        {
            try
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

                _rule.B = bVal;
                _rule.S = sVal;
            }
            catch
            {
                _rule.B = 0;
                _rule.S = 0;
                throw;
            }
        }
    }

    public class NameRuleComparer : IEqualityComparer<NamedRule>
    {
        public bool Equals(NamedRule? x, NamedRule? y)
        {
            if (x == null || y == null)
                return false;

            return x.Name == y.Name && x.RuleVal == y.RuleVal;
        }

        public int GetHashCode([DisallowNull] NamedRule obj)
        {
            return obj.Rule.B + obj.Rule.S;
        }
    }
}
