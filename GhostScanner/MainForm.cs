using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace GhostScanner
{
    public partial class MainForm : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private bool isMonitoring = false;
        private Config config;
        private System.Threading.Timer monitoringTimer;
        private const string ConfigPath = "config.json";

        public MainForm()
        {
            InitializeComponent();
            LoadConfig();
            InitializeTrayIcon();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Ghost Scanner - Ultimate AI Helper";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.ResumeLayout(false);
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    string json = File.ReadAllText(ConfigPath);
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
                catch
                {
                    config = new Config();
                }
            }
            else
            {
                config = new Config();
            }
        }

        private void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }

        private void InitializeTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("👻 Show Ghost Scanner", null, ShowMainWindow);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Ask Ghost", null, AskGhost);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Start Monitoring", null, StartMonitoring);
            trayMenu.Items.Add("Stop Monitoring", null, StopMonitoring);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Configure", null, ShowConfig);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Exit", null, Exit);

            // Create ghost icon
            Bitmap iconBitmap = CreateGhostIcon();
            IntPtr hIcon = iconBitmap.GetHicon();
            Icon ghostIcon = Icon.FromHandle(hIcon);

            trayIcon = new NotifyIcon
            {
                Icon = ghostIcon,
                ContextMenuStrip = trayMenu,
                Text = "Ghost Scanner - Ultimate AI Helper\nMade by Soul and Lapex",
                Visible = true
            };

            trayIcon.DoubleClick += (s, e) => ShowMainWindow(s, e);
            UpdateTrayMenu();
        }

        private Bitmap CreateGhostIcon()
        {
            Bitmap bmp = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Ghost body (white)
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, 10, 10, 44, 44);
                }

                // Eyes (black)
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillEllipse(brush, 22, 22, 8, 8);
                    g.FillEllipse(brush, 34, 22, 8, 8);
                }

                // Mouth (smile)
                using (Pen pen = new Pen(Color.Black, 3))
                {
                    g.DrawArc(pen, 22, 32, 20, 12, 0, 180);
                }
            }
            return bmp;
        }

        private void UpdateTrayMenu()
        {
            trayMenu.Items[3].Enabled = !isMonitoring;
            trayMenu.Items[4].Enabled = isMonitoring;
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Show();
            this.BringToFront();
            this.Activate();
        }

        private void StartMonitoring(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                MessageBox.Show("Please configure your API key first!", "Ghost Scanner", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ShowConfig(sender, e);
                return;
            }

            isMonitoring = true;
            monitoringTimer = new System.Threading.Timer(MonitorScreen, null, 0, (int)(config.MonitoringInterval * 1000));
            trayIcon.ShowBalloonTip(2000, "Ghost Scanner", "Monitoring started! 👻", ToolTipIcon.Info);
            UpdateTrayMenu();
            UpdateStatus();
        }

        private void StopMonitoring(object sender, EventArgs e)
        {
            isMonitoring = false;
            monitoringTimer?.Dispose();
            trayIcon.ShowBalloonTip(2000, "Ghost Scanner", "Monitoring stopped", ToolTipIcon.Info);
            UpdateTrayMenu();
            UpdateStatus();
        }

        private async void MonitorScreen(object state)
        {
            if (!isMonitoring) return;

            try
            {
                Bitmap screenshot = CaptureScreen();
                if (screenshot != null)
                {
                    string response = await AskAI("You are Ghost, an ultimate AI helper watching the user's screen. Analyze what they're doing and provide helpful, concise suggestions (2-3 sentences max). Only respond if you have something genuinely helpful to say.", screenshot);
                    if (!string.IsNullOrEmpty(response) && !response.StartsWith("Error:"))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            trayIcon.ShowBalloonTip(5000, "👻 Ghost Helper", response.Length > 200 ? response.Substring(0, 200) + "..." : response, ToolTipIcon.Info);
                        });
                    }
                    screenshot.Dispose();
                }
            }
            catch { }
        }

        private Bitmap CaptureScreen()
        {
            try
            {
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                // Resize if too large
                if (bitmap.Width > 1920 || bitmap.Height > 1920)
                {
                    double ratio = Math.Min(1920.0 / bitmap.Width, 1920.0 / bitmap.Height);
                    int newWidth = (int)(bitmap.Width * ratio);
                    int newHeight = (int)(bitmap.Height * ratio);
                    Bitmap resized = new Bitmap(bitmap, newWidth, newHeight);
                    bitmap.Dispose();
                    return resized;
                }

                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private async void AskGhost(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                MessageBox.Show("Please configure your API key first!", "Ghost Scanner", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ShowConfig(sender, e);
                return;
            }

            string question = ShowInputDialog("What would you like to know about your screen?", "👻 Ask Ghost");

            if (string.IsNullOrWhiteSpace(question)) return;

            Bitmap screenshot = CaptureScreen();
            if (screenshot == null)
            {
                MessageBox.Show("Could not capture screen.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show loading
            Form loadingForm = new Form
            {
                Text = "Ghost Scanner",
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label loadingLabel = new Label
            {
                Text = "👻 Ghost is thinking...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12)
            };
            loadingForm.Controls.Add(loadingLabel);
            loadingForm.Show();
            loadingForm.Refresh();

            string response = await AskAI(question, screenshot);
            screenshot.Dispose();
            loadingForm.Close();

            if (!string.IsNullOrEmpty(response))
            {
                ShowResponseDialog(response);
            }
            else
            {
                MessageBox.Show("Could not get response from AI.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> AskAI(string prompt, Bitmap image)
        {
            if (string.IsNullOrWhiteSpace(config.ApiKey)) return null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60);

                    string imageBase64 = ImageToBase64(image);
                    string url = config.Provider == "openrouter" 
                        ? "https://openrouter.ai/api/v1/chat/completions"
                        : "https://api.openai.com/v1/chat/completions";

                    var request = new
                    {
                        model = config.Model,
                        messages = new[]
                        {
                            new
                            {
                                role = "user",
                                content = new object[]
                                {
                                    new { type = "text", text = prompt },
                                    new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{imageBase64}" } }
                                }
                            }
                        },
                        temperature = 0.7,
                        max_tokens = 1500
                    };

                    var json = JsonConvert.SerializeObject(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    if (config.Provider == "openrouter")
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");
                        client.DefaultRequestHeaders.Add("HTTP-Referer", "https://github.com/simplifaisoul/GhostScanner");
                        client.DefaultRequestHeaders.Add("X-Title", "Ghost Scanner");
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");
                    }

                    var response = await client.PostAsync(url, content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<dynamic>(responseText);
                        return result.choices[0].message.content.ToString();
                    }
                    else
                    {
                        return $"Error: {response.StatusCode} - {responseText}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string ImageToBase64(Bitmap image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void ShowResponseDialog(string response)
        {
            Form responseForm = new Form
            {
                Text = "👻 Ghost's Response",
                Size = new Size(700, 500),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(30, 30, 40)
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(52, 73, 94)
            };
            Label title = new Label
            {
                Text = "👻 Ghost's Response",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            header.Controls.Add(title);
            responseForm.Controls.Add(header);

            TextBox textBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Text = response,
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(236, 240, 241),
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(10)
            };
            responseForm.Controls.Add(textBox);

            Button closeBtn = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(responseForm.Width - 120, responseForm.Height - 50)
            };
            closeBtn.Click += (s, e) => responseForm.Close();
            responseForm.Controls.Add(closeBtn);

            responseForm.ShowDialog();
        }

        private void ShowConfig(object sender, EventArgs e)
        {
            using (ConfigForm configForm = new ConfigForm(config))
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    config = configForm.Config;
                    SaveConfig();
                }
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit Ghost Scanner?", "Ghost Scanner", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                isMonitoring = false;
                monitoringTimer?.Dispose();
                trayIcon.Visible = false;
                Application.Exit();
            }
        }

        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label { Left = 20, Top = 20, Text = text, Width = 440 };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 440 };
            Button confirmation = new Button
            {
                Text = "OK",
                Left = 350,
                Width = 100,
                Top = 80,
                DialogResult = DialogResult.OK
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void UpdateStatus()
        {
            // Status update for main window if needed
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnFormClosing(e);
        }
    }

    public class Config
    {
        public string Provider { get; set; } = "openrouter";
        public string ApiKey { get; set; } = "";
        public string Model { get; set; } = "openai/gpt-4o-mini";
        public double MonitoringInterval { get; set; } = 10.0;
    }
}
