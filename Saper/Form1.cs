using System;
using System.Windows.Forms;

namespace Saper
{
    public partial class Form1 : Form
    {
        private MinesweeperGame game;

        public Form1()
        {
            InitializeComponent();
            game = new MinesweeperGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}