using System;
using System.Drawing;
using System.Windows.Forms;

namespace GhostScanner
{
    public partial class ConfigForm : Form
    {
        public Config Config { get; private set; }

        private ComboBox providerCombo;
        private TextBox apiKeyBox;
        private TextBox modelBox;
        private NumericUpDown intervalBox;
        private CheckBox showKeyCheck;

        public ConfigForm(Config config)
        {
            Config = new Config
            {
                Provider = config.Provider,
                ApiKey = config.ApiKey,
                Model = config.Model,
                MonitoringInterval = config.MonitoringInterval
            };

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "👻 Ghost Scanner Configuration";
            this.Size = new Size(550, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(52, 73, 94)
            };
            Label title = new Label
            {
                Text = "⚙️ Configuration",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            header.Controls.Add(title);
            this.Controls.Add(header);

            // Content Panel
            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            int yPos = 20;

            // Provider
            Label providerLabel = new Label
            {
                Text = "AI Provider:",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            content.Controls.Add(providerLabel);

            providerCombo = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            providerCombo.Items.AddRange(new[] { "openrouter", "openai" });
            providerCombo.SelectedItem = Config.Provider;
            content.Controls.Add(providerCombo);

            yPos += 40;

            // API Key
            Label apiKeyLabel = new Label
            {
                Text = "API Key:",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            content.Controls.Add(apiKeyLabel);

            apiKeyBox = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(250, 25),
                UseSystemPasswordChar = true,
                Text = Config.ApiKey
            };
            content.Controls.Add(apiKeyBox);

            showKeyCheck = new CheckBox
            {
                Text = "Show",
                Location = new Point(440, yPos),
                Size = new Size(60, 25)
            };
            showKeyCheck.CheckedChanged += (s, e) => apiKeyBox.UseSystemPasswordChar = !showKeyCheck.Checked;
            content.Controls.Add(showKeyCheck);

            yPos += 40;

            // Model
            Label modelLabel = new Label
            {
                Text = "Model:",
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            content.Controls.Add(modelLabel);

            modelBox = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(300, 25),
                Text = Config.Model
            };
            content.Controls.Add(modelBox);

            yPos += 30;

            Label modelHint = new Label
            {
                Text = "Examples: openai/gpt-4o-mini, anthropic/claude-3-haiku",
                Location = new Point(180, yPos),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };
            content.Controls.Add(modelHint);

            yPos += 40;

            // Interval
            Label intervalLabel = new Label
            {
                Text = "Monitoring Interval (seconds):",
                Location = new Point(20, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            content.Controls.Add(intervalLabel);

            intervalBox = new NumericUpDown
            {
                Location = new Point(230, yPos),
                Size = new Size(100, 25),
                Minimum = 5,
                Maximum = 300,
                Increment = 5,
                Value = (decimal)Config.MonitoringInterval
            };
            content.Controls.Add(intervalBox);

            yPos += 50;

            // Help text
            Label helpLabel = new Label
            {
                Text = "Get API keys:\n• OpenRouter: https://openrouter.ai\n• OpenAI: https://platform.openai.com",
                Location = new Point(20, yPos),
                Size = new Size(480, 60),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };
            content.Controls.Add(helpLabel);

            // Buttons
            Button saveBtn = new Button
            {
                Text = "Save",
                Size = new Size(120, 35),
                Location = new Point(180, yPos + 80),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            saveBtn.FlatAppearance.BorderSize = 0;
            saveBtn.Click += SaveButton_Click;
            content.Controls.Add(saveBtn);

            Button cancelBtn = new Button
            {
                Text = "Cancel",
                Size = new Size(120, 35),
                Location = new Point(320, yPos + 80),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            cancelBtn.FlatAppearance.BorderSize = 0;
            cancelBtn.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            content.Controls.Add(cancelBtn);

            this.Controls.Add(content);
            this.ResumeLayout(false);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Config.Provider = providerCombo.SelectedItem.ToString();
            Config.ApiKey = apiKeyBox.Text;
            Config.Model = modelBox.Text;
            Config.MonitoringInterval = (double)intervalBox.Value;

            if (string.IsNullOrWhiteSpace(Config.ApiKey))
            {
                MessageBox.Show("Please enter an API key!", "Ghost Scanner", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}