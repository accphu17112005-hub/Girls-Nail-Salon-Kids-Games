#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace KidsNailSalon.Editor
{
    public static class BuildScripts
    {
        private static readonly string[] BuildScenes = {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/CharacterSelection.unity",
            "Assets/Scenes/HandCare.unity",
            "Assets/Scenes/NailStudio.unity",
            "Assets/Scenes/Result.unity",
            "Assets/Scenes/Gallery.unity",
            "Assets/Scenes/Settings.unity"
        };

        [MenuItem("Build/Android/Build APK (Testing)")]
        public static void BuildAndroidAPK()
        {
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Builds/Android");
            Directory.CreateDirectory(outputDir);
            string apkPath = Path.Combine(outputDir, "KidsNailSalon-BeautyStudio.apk");

            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = apkPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            Debug.Log($"[BuildScripts] Starting Android APK build to: {apkPath}");
            BuildReport report = BuildPipeline.BuildPlayer(options);
            PrintBuildReport(report);
        }

        [MenuItem("Build/Android/Build AAB (Google Play)")]
        public static void BuildAndroidAAB()
        {
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Builds/Android");
            Directory.CreateDirectory(outputDir);
            string aabPath = Path.Combine(outputDir, "KidsNailSalon-BeautyStudio.aab");

            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = aabPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            Debug.Log($"[BuildScripts] Starting Android AAB build to: {aabPath}");
            BuildReport report = BuildPipeline.BuildPlayer(options);
            PrintBuildReport(report);
        }

        [MenuItem("Build/iOS/Export Xcode Project")]
        public static void ExportXcodeProject()
        {
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Builds/iOS/Xcode");
            Directory.CreateDirectory(outputDir);

            PlayerSettings.iOS.targetOSVersionString = "13.0";
            PlayerSettings.iOS.cameraUsageDescription = "Kids Nail Salon allows taking photos to frame nail art.";

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = outputDir,
                target = BuildTarget.iOS,
                options = BuildOptions.None
            };

            Debug.Log($"[BuildScripts] Exporting iOS Xcode Project to: {outputDir}");
            BuildReport report = BuildPipeline.BuildPlayer(options);
            PrintBuildReport(report);
        }

        private static void PrintBuildReport(BuildReport report)
        {
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BuildScripts] SUCCESS: Build completed in {summary.totalTime.TotalSeconds:F1}s with size {summary.totalSize / (1024 * 1024)} MB.");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[BuildScripts] FAILED: Build failed with {summary.totalErrors} errors.");
            }
        }
    }
}
#endif
