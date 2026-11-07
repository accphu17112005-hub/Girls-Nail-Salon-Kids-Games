# Kids Nail Salon – iOS Project 💅🍎

A complete native iOS app wrapper utilizing high-performance Apple WebKit (`WKWebView`) to run the **Kids Nail Salon & Beauty Studio** game smoothly on iPhones and iPads.

---

## 📁 Project Structure

```
ios/
├── KidsNailSalon.xcodeproj/      # Complete Xcode Project file
│   ├── project.pbxproj
│   └── project.xcworkspace
├── KidsNailSalon/
│   ├── AppDelegate.swift         # App lifecycle & entrypoint
│   ├── SceneDelegate.swift       # Modern UIWindowScene management
│   ├── ViewController.swift      # WKWebView container with fullscreen immersive configuration
│   ├── Info.plist                # App bundle metadata, portrait lock, capabilities
│   ├── Base.lproj/
│   │   └── LaunchScreen.storyboard # Native launch splash screen
│   ├── Assets.xcassets/          # App icon catalog (iPhone, iPad, App Store 1024x1024)
│   └── WebApp/                   # Bundled offline game assets & engine
│       ├── Assets/               # Hand art, polish, gems, spa tools, audio
│       └── PlayableStudio/       # Studio HTML5 game engine
└── ExportOptions.plist           # App Store archive export configuration
```

---

## 🚀 How to Run & Build

### 1. Open in Xcode (on macOS)
Double click or run:
```bash
open ios/KidsNailSalon.xcodeproj
```

### 2. Configure Signing
1. Click on the root **KidsNailSalon** project in the Project Navigator.
2. Select the **KidsNailSalon** target.
3. Open the **Signing & Capabilities** tab.
4. Select your Apple Developer Team.

### 3. Run on Simulator or Physical Device
- Select an **iPhone** or **iPad** target from the device menu (e.g., iPhone 15 Pro, iPad Air).
- Click **Play (Cmd + R)** to build and run.

### 4. Build IPA via Command Line (Terminal)
```bash
cd ios
# Build Archive
xcodebuild -project KidsNailSalon.xcodeproj -scheme KidsNailSalon -configuration Release -archivePath build/KidsNailSalon.xcarchive archive

# Export IPA
xcodebuild -exportArchive -archivePath build/KidsNailSalon.xcarchive -exportPath build/ -exportOptionsPlist ExportOptions.plist
```
The resulting `KidsNailSalon.ipa` will be ready for TestFlight or App Store Connect submission.

