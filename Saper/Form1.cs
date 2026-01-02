using Saper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Saper
{
    public partial class Form1 : Form
    {
        private MinesweeperGame game;
        private Button[,] cellButtons;
        private System.Windows.Forms.Timer gameTimer;
        private int cellSize = 30;

        private Label minesLabel;
        private Button smileButton;
        private Label timeLabel;
        private TableLayoutPanel gamePanel;
        private MenuStrip menuStrip;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
            InitializeGame();
        }

        private void CreateInterface()
        {
            Text = "Сапер";
            Size = new Size(16 * cellSize + 40, 16 * cellSize + 120);
            StartPosition = FormStartPosition.CenterScreen;

            menuStrip = new MenuStrip();
            ToolStripMenuItem gameMenu = new ToolStripMenuItem("Игра");
            ToolStripMenuItem newGameItem = new ToolStripMenuItem("Новая игра");
            newGameItem.Click += NewGameItem_Click;
            gameMenu.DropDownItems.Add(newGameItem);
            menuStrip.Items.Add(gameMenu);
            Controls.Add(menuStrip);

            Panel infoPanel = new Panel();
            infoPanel.Size = new Size(Width, 50);
            infoPanel.BackColor = Color.LightGray;
            infoPanel.Top = menuStrip.Height;

            minesLabel = new Label();
            minesLabel.Text = "040";
            minesLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            minesLabel.ForeColor = Color.Red;
            minesLabel.BackColor = Color.Black;
            minesLabel.Size = new Size(60, 30);
            minesLabel.TextAlign = ContentAlignment.MiddleCenter;
            minesLabel.Left = 20;
            minesLabel.Top = 10;
            infoPanel.Controls.Add(minesLabel);

            smileButton = new Button();
            smileButton.Text = ":)";
            smileButton.Font = new Font("Arial", 14, FontStyle.Bold);
            smileButton.Size = new Size(40, 40);
            smileButton.Left = (infoPanel.Width - smileButton.Width) / 2;
            smileButton.Top = 5;
            smileButton.Click += SmileButton_Click;
            infoPanel.Controls.Add(smileButton);

            timeLabel = new Label();
            timeLabel.Text = "000";
            timeLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            timeLabel.ForeColor = Color.Red;
            timeLabel.BackColor = Color.Black;
            timeLabel.Size = new Size(60, 30);
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            timeLabel.Left = infoPanel.Width - 80;
            timeLabel.Top = 10;
            infoPanel.Controls.Add(timeLabel);

            Controls.Add(infoPanel);

            gamePanel = new TableLayoutPanel();
            gamePanel.Top = infoPanel.Bottom;
            gamePanel.Left = 20;
            gamePanel.Size = new Size(16 * cellSize, 16 * cellSize);

            gamePanel.ColumnCount = 16;
            gamePanel.RowCount = 16;

            for (int i = 0; i < 16; i++)
            {
                gamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSize));
                gamePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSize));
            }

            Controls.Add(gamePanel);

            cellButtons = new Button[16, 16];

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    Button button = new Button();
                    button.Size = new Size(cellSize, cellSize);
                    button.Tag = new Point(x, y);
                    button.MouseDown += CellButton_MouseDown;
                    button.Font = new Font("Arial", 10, FontStyle.Bold);

                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.Gray;
                    button.FlatAppearance.MouseOverBackColor = button.BackColor;
                    button.FlatAppearance.MouseDownBackColor = button.BackColor;

                    button.UseVisualStyleBackColor = false;

                    cellButtons[x, y] = button;
                    gamePanel.Controls.Add(button, x, y);
                }
            }

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
        }

        private void InitializeGame()
        {
            game = new MinesweeperGame();
            UpdateInterface();
        }

        private void UpdateInterface()
        {
            minesLabel.Text = game.MinesLeft.ToString("000");

          
            timeLabel.Text = game.GameTime.ToString("000");

            if (game.GameStatus == 0) 
                smileButton.Text = ":)";
            else if (game.GameStatus == 1)
                smileButton.Text = ":)";
            else if (game.GameStatus == 2)
                smileButton.Text = ":(";
            else if (game.GameStatus == 3) 
                smileButton.Text = "B)";

            UpdateGameBoard();
        }

        private void UpdateGameBoard()
        {
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    Button button = cellButtons[x, y];
                    button.BackColor = SystemColors.Control;
                    button.ForeColor = Color.Black;
                    button.Text = "";
                    button.Enabled = true;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.Gray;
                    button.FlatAppearance.MouseOverBackColor = button.BackColor;
                    button.FlatAppearance.MouseDownBackColor = button.BackColor;
                }
            }

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    Button button = cellButtons[x, y];
                    GameCell cell = game.Board.GetCell(x, y);

                    if (cell == null) continue;

                    if (cell.IsOpened())
                    {
                        button.Enabled = false;
                        button.BackColor = Color.LightGray;

                        if (cell.HasMine)
                        {
                            button.Text = "💣";
                            button.ForeColor = Color.Red;
                        }
                        else if (cell.MinesAround > 0)
                        {
                            button.Text = cell.MinesAround.ToString();
                            if (cell.MinesAround == 1) button.ForeColor = Color.Blue;
                            else if (cell.MinesAround == 2) button.ForeColor = Color.Green;
                            else if (cell.MinesAround == 3) button.ForeColor = Color.Red;
                            else if (cell.MinesAround >= 4) button.ForeColor = Color.DarkBlue;
                        }
                    }
                    else if (cell.IsFlagged())
                    {
                        button.Text = "🚩";
                        button.ForeColor = Color.Red;
                    }
                    else if (cell.HasQuestion())
                    {
                        button.Text = "?";
                        button.ForeColor = Color.Blue;
                    }

                    if (game.GameStatus == 2 && cell.HasMine && !cell.IsFlagged())
                    {
                        button.Text = "*";
                        button.ForeColor = Color.Red;
                        button.BackColor = Color.LightGray;
                    }

                    if (game.GameStatus == 3 && cell.HasMine && cell.IsFlagged())
                    {
                        button.Text = "🚩";
                        button.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void CellButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (game.GameStatus == 2 || game.GameStatus == 3) return;

            Button button = (Button)sender;
            Point coordinates = (Point)button.Tag;
            int x = coordinates.X;
            int y = coordinates.Y;

            if (e.Button == MouseButtons.Left)
            {
                bool moved = game.MakeMove(x, y);
                if (moved && game.GameStatus == 1 && !gameTimer.Enabled)
                {
                    gameTimer.Start();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                game.ToggleFlag(x, y);
            }

            UpdateInterface();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            game.UpdateTimer();
            UpdateInterface();

            if (game.GameStatus == 2 || game.GameStatus == 3)
            {
                gameTimer.Stop();
            }
        }

        private void SmileButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void NewGameItem_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            gameTimer.Stop();
            game.NewGame();
            UpdateInterface();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}