# 👻 Ghost Scanner - Ultimate AI Helper

**Made by Soul and Lapex**

Ghost Scanner is a native Windows application that monitors your screen and provides AI-powered assistance to help you be as productive as possible.

## ✨ Features

- **👻 Cool Ghost Theme**: Beautiful, modern interface
- **Screen Monitoring**: Ghost watches your screen in the background
- **AI-Powered Assistance**: Uses advanced AI models to understand what you're doing
- **Native Windows App**: Built with C# - fast and professional
- **System Tray Integration**: Runs silently in the background
- **On-Demand Help**: Ask Ghost questions about your screen anytime
- **Automatic Assistance**: Optional proactive help based on screen activity

## 🚀 Quick Installation

### Step 1: Download
- Click "Code" → "Download ZIP" on GitHub
- Extract the ZIP file

### Step 2: Build (One-Time)
1. Open `GhostScanner.sln` in Visual Studio 2022 (free Community edition)
2. Press `F6` to build
3. Done! The .exe is in `GhostScanner\bin\Release\net6.0-windows\GhostScanner.exe`

### Step 3: Install
1. Right-click `INSTALL.bat` → "Run as administrator"
2. Follow the prompts
3. Ghost Scanner will be installed to `C:\Program Files\GhostScanner\`

### Step 4: Launch
- Double-click the desktop shortcut
- Or run `C:\Program Files\GhostScanner\GhostScanner.exe`
- The application window opens automatically

### Step 5: Configure
1. Right-click the system tray icon (👻) → "Configure"
2. Enter your API key:
   - **OpenRouter**: https://openrouter.ai (Recommended)
   - **OpenAI**: https://platform.openai.com
3. Choose your model (e.g., `openai/gpt-4o-mini`)
4. Click "Save"
5. Click "Start Monitoring" in the main window

## 📖 Usage

- **Start Monitoring**: Click "Start Monitoring" button
- **Ask Ghost**: Click "Ask Ghost" to ask questions about your screen
- **Configure**: Right-click system tray icon → "Configure"
- **Exit**: Right-click system tray icon → "Exit"

## 🎯 How It Works

1. Ghost captures screenshots of your screen
2. Sends them to an AI model with context
3. Analyzes what you're doing and provides helpful suggestions
4. Can answer questions about what's visible on your screen

## 🔒 Privacy

- Screen captures are processed locally
- Sent only to your configured AI provider
- No data stored permanently
- You control when monitoring is active

## 🛠️ Requirements

- **Windows 10/11**
- **Visual Studio 2022** (Community Edition - free) for building
- **.NET 6.0 SDK** (included with Visual Studio)
- **Internet connection** (for AI API access)
- **API key** from OpenRouter or OpenAI

## 📝 License

MIT License - See LICENSE file

## 🤝 Support

For issues: https://github.com/simplifaisoul/GhostScanner/issues

---

**👻 Made by Soul and Lapex - The Ultimate Screen Helper**
