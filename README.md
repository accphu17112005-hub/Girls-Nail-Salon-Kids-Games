# Kids Nail Salon – Beauty Studio 💅✨

A complete, cross-platform mobile nail salon simulation game built for **Android** and **iOS**, inspired by casual kids beauty studio games (like Pazu's "Girls Nail Salon").

---

## 📱 Android vs iOS: Platform Comparison & Architecture

This project is built from a single unified codebase targeting both platforms with specific configurations:

| Feature / Aspect | Android (Google Play) | iOS (Apple App Store) |
| :--- | :--- | :--- |
| **Target Devices** | Android phones, tablets, foldable devices | iPhone, iPad |
| **OS Requirements** | Android 7.0+ (Min SDK 24, Target SDK 34) | iOS 13.0+ |
| **Orientation** | Portrait (locked) | Portrait (locked) |
| **Output Testing Artifact** | `.apk` (Direct side-loading on device/emulator) | Xcode Project exported, run on Simulator/device via Xcode |
| **Store Release Artifact** | `.aab` (Android App Bundle for Google Play) | Signed `.ipa` via Xcode Archive / TestFlight |
| **Rendering Backend** | Vulkan / OpenGL ES 3.0 | Metal |
| **Child Safety Compliance** | Google Play Families Policy (Parental Gate, no ID tracking) | Apple Kids Category & Guideline 1.3 / 5.1.4 (Parental Gate) |
| **App Identifier** | `com.kidsplay.nailsalon.beautystudio` | `com.kidsplay.nailsalon.beautystudio` |
| **Build Machine Requirement** | Any OS (Linux, macOS, Windows) with Android SDK | macOS with Xcode (command line or IDE) |

---

## 🎨 Game Features & Systems

1. **Hand Models & Skin Tones**:
   - 5 diverse skin tones: Fair Porcelain, Light Warm, Tan Golden, Rich Caramel, Deep Espresso.
2. **Individual Nail Selection & Clipping Mask**:
   - 5 individual fingernails (Thumb, Index, Middle, Ring, Pinky).
   - Precise Sprite Mask prevents paint, gradients, textures, or stickers from overflowing outside nail contours.
3. **6 Nail Shapes**:
   - Round, Square, Oval, Almond, Coffin, and Stiletto with individual or "Apply to All" option.
4. **55+ Polish Colors & Finishes**:
   - Solid, Pastel, Neon, Metallic, Glitter, Pearl, and Rainbow ombre.
   - 2-color and 3-color gradients with adjustable angles.
   - Textures: Glossy, Matte, Glitter particles, Metallic foil, and Pearl shimmer.
5. **110 Kawaii Stickers (11 Categories)**:
   - Hearts, Stars, Flowers, Animals, Cute Faces, Bows, Butterflies, Fruits, Food, Fantasy, and Seasonal.
   - Interactive placement, scaling, rotation, and re-layering.
6. **Gems & Jewelry**:
   - Diamonds, Rubies, Emeralds, Pearls, Star crystals, Rings, and Bracelets.
7. **Hand Care Spa Mini-Game**:
   - Nail filing, smoothing berry hand cream, and fluffy towel drying.
8. **Undo / Redo & Local Save System**:
   - Command pattern history for all editing steps.
   - Local JSON save file with thumbnail generation and My Gallery screen.
9. **Child Safety & Offline Play**:
   - 100% playable offline without account creation.
   - Multiplication math Parental Gate before opening any external link or settings.

---

## 🛠️ Build & Deployment Instructions

### 1. Building for Android

#### Testing Build (APK):
In Unity Editor or via Batchmode:
- Menu: `Build > Android > Build APK (Testing)`
- Output: `Builds/Android/KidsNailSalon-BeautyStudio.apk`
- Install on device:
  ```bash
  adb install -r Builds/Android/KidsNailSalon-BeautyStudio.apk
  ```

#### Google Play Release Build (AAB):
- Menu: `Build > Android > Build AAB (Google Play)`
- Output: `Builds/Android/KidsNailSalon-BeautyStudio.aab`
- Upload the `.aab` directly to Google Play Console under **Production** or **Internal Testing**.

### 2. Building for iOS

#### Export Xcode Project:
- Menu: `Build > iOS > Export Xcode Project`
- Output: `Builds/iOS/Xcode/`
- Open the generated Xcode project on a Mac:
  ```bash
  open Builds/iOS/Xcode/Unity-iPhone.xcodeproj
  ```
- Select your Development Team under **Signing & Capabilities**.
- Select target device (iPhone or iPad) and click **Product > Archive** to upload to App Store Connect / TestFlight.

---

## 🌐 Instant Playable Web Studio

You can test and play the complete simulation directly in your browser:
```bash
python3 -m http.server 8085
```
Open: `http://localhost:8085/PlayableStudio/index.html`
