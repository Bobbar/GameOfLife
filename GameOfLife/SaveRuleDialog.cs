using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameOfLife.Rules;

namespace GameOfLife
{
    public partial class SaveRuleDialog : Form
    {
        public string RuleName { get; set; }

        private NamedRule rule;

        public SaveRuleDialog()
        {
            InitializeComponent();
        }

        public SaveRuleDialog(NamedRule rule)
        {
            InitializeComponent();
            this.rule = rule;
            ruleValueTextBox.Text = this.rule.RuleVal;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            RuleName = ruleNameTextBox.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
