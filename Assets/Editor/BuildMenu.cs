using Debug = UnityEngine.Debug;

using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.Diagnostics;
using System.IO;

public class BuildMenu : MonoBehaviour
{
	[MenuItem("Build/Open Android Build Folder")]
	public static void OpenAndroidBuildFolder()
	{
		// Due to an oddity in RevealInFinder we need to put a containing folder address in the folder we want to navigate
		// to otherwise it will just take us to the Build folder
		EditorUtility.RevealInFinder(@"\\192.168.1.250\ServerDisk1\_Build\Android\");
	}

	// The APK is faster to build than the AAB so it should be used for testing, and the AAB for publishing
	[MenuItem("Build/APK (ServerDisk1\\_Build)")]
	public static void AndroidBuildAPK()
	{
		// First we check for any existing .apk file and delete it (you can run into problems with the build not overwriting
		// the older file). So if directory exists then we need to delete out the .apk
		var previousAPKFileLocation = new DirectoryInfo(@"Build\Android\");
		if (previousAPKFileLocation.Exists)
		{
			var filePaths = previousAPKFileLocation.GetFiles("*.apk");

			foreach (var file in filePaths)
			{
				file.Delete();
			}
		}

		// Tweaks to speed up APK build

		//Set appBundle to false
		EditorUserBuildSettings.buildAppBundle = false;
		// Set Scripting to Mono
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
		// Set build to x86 (works with Genymotion, ARM does not)
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86;

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = FindActiveScenesInBuildSettings();
		buildPlayerOptions.locationPathName = @"Build\Android\" + Application.productName + ".apk";
		buildPlayerOptions.target = BuildTarget.Android;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			// We must use UnityEngine.Debug as we are using System.Diagnositics in order to run an external .exe file
			UnityEngine.Debug.Log("Build succeeded!");
			// Copy the file to the server (this is easier for the Android device to download from)
			File.Copy(@"Build\Android\" + Application.productName + ".apk", @"\\192.168.1.250\ServerDisk1\_Build\Android\"
				+ Application.productName + ".apk", true);
		}

		if (summary.result == BuildResult.Failed)
			UnityEngine.Debug.Log("***ANDROID BUILD FAILED!***");
	}

	// Google Play requires AAB files instead of APK files *** You must enable Build App Bundle in the Build Settings ***
	[MenuItem("Build/AAB (ServerDisk1\\_Build)")]
	public static void AndroidBuildAAB()
	{
		// First we check for any existing .aab file and delete it (you can run into problems with the build not overwriting
		// the older file). So if directory exists then we need to delete out the .aab
		var previousAABFileLocation = new DirectoryInfo(@"Build\Android\");
		if (previousAABFileLocation.Exists)
		{
			var filePaths = previousAABFileLocation.GetFiles("*.aab");

			foreach (var file in filePaths)
			{
				file.Delete();
			}
		}

		// Build Options for AAB (Google Play)

		// Add as appBundle
		EditorUserBuildSettings.buildAppBundle = true;
		// Set Scripting to Mono
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
		// Set architecture to ARM64 or ARM7 for deployment to Google Play
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = FindActiveScenesInBuildSettings();
		buildPlayerOptions.locationPathName = @"Build\Android\" + Application.productName + ".aab";
		buildPlayerOptions.target = BuildTarget.Android;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			// We must use UnityEngine.Debug as we are using System.Diagnositics in order to run an external .exe file
			Debug.Log("Build succeeded!");
			// Copy the file to the server (this is easier for the Android device to download from)
			File.Copy(@"Build\Android\" + Application.productName + ".aab", @"\\192.168.1.250\ServerDisk1\_Build\Android\"
				+ Application.productName + ".aab", true);
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("***ANDROID BUILD FAILED!***");
		}
	}

	// The APK is faster to build than the AAB so it should be used for testing, and the AAB for publishing
	[MenuItem("Build/Build APK and Launch Emulator")]
	public static void BuildAndRunAPK()
	{
		// Check if the 'player.exe' process is already running, if not then launch Genymoton
		// When using GetProcessByName we don't inlcude the .exe and it is case sensitive (find the process with TaskManager first)
		if (Process.GetProcessesByName("player").Length == 0)
		{
			// Immediately launch the Android Emulator (it should finish booting before the Build is finsihed
			var launchAndroidEmulator = new Process();
			launchAndroidEmulator.StartInfo.FileName = @"C:\Program Files\Genymobile\Genymotion\player";
			launchAndroidEmulator.StartInfo.Arguments = " --vm-name \"Google Nexus - Landscape\"";

			launchAndroidEmulator.Start();
		}
		// First we check for any existing .apk file and delete it (you can run into problems with the build not overwriting
		// the older file). So if directory exists then we need to delete out the .apk
		var previousAPKFileLocation = new DirectoryInfo(@"Build\Android\");
		if (previousAPKFileLocation.Exists)
		{
			var filePaths = previousAPKFileLocation.GetFiles("*.apk");

			foreach (var file in filePaths)
			{
				file.Delete();
			}
		}

		// Tweaks to speed up APK build

		//Set appBundle to false
		EditorUserBuildSettings.buildAppBundle = false;
		// Set Scripting to Mono
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
		// Set build to x86 (works with Genymotion, ARM does not)
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86;

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = FindActiveScenesInBuildSettings();
		buildPlayerOptions.locationPathName = @"Build\Android\" + Application.productName + ".apk";
		buildPlayerOptions.target = BuildTarget.Android;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			// We must use UnityEngine.Debug as we are using System.Diagnositics in order to run an external .exe file
			UnityEngine.Debug.Log("Build succeeded!");
			// Copy the file to the server (this is easier for the Android device to download from)
			File.Copy(@"Build\Android\" + Application.productName + ".apk", @"\\192.168.1.250\ServerDisk1\_Build\Android\"
				+ Application.productName + ".apk", true);
		}

		if (summary.result == BuildResult.Failed)
		{
			UnityEngine.Debug.Log("***ANDROID BUILD FAILED!***");
		}

		// If the build succeeds run the Android Emulator
		else
			InstallAPKOnEmulator();
	}

	[MenuItem("Build/Send APK to Emulator")]
	public static void InstallAPKOnEmulator()
	{
		var previousAPKFileLocation = new DirectoryInfo(@"Build\Android\");
		// Install the APK via ADB (use the same version of ADB that Genymotion [emulator] is using)
		var installAPK = new ProcessStartInfo(@"C:\Program Files\Genymobile\Genymotion\tools\adb.exe");
		// 'install -r' will overwrite any previous installation on Android
		installAPK.Arguments = " install -r " + previousAPKFileLocation + Application.productName + ".apk";

		// Wait for the APK to install
		var installing = Process.Start(installAPK);
		installing.WaitForExit();

		// Run the APK in the Android Emulator (use the same version of ADB that Genymotion [emulator] is using)
		var runAPK = new ProcessStartInfo(@"C:\Program Files\Genymobile\Genymotion\tools\adb.exe");
		// The company name must be lowercase and have no underscores (the product name case stays the same [minus the '_'])
		var packageName = "com." + Application.companyName.ToLower().Replace("_", "") + "." + Application.productName.Replace("_", "");
		runAPK.Arguments = " shell monkey -p " + packageName + " -c android.intent.category.LAUNCHER 1";

		Process.Start(runAPK);
		// It is too difficult to give the window focus via the Windows API, just click the window instead
		// If you REALLY need the window to 'pop up' then just build a Windows version instead
	}

	// If this folder defaults to the 'user' folder then *** You haven't built your game yet ***
	[MenuItem("Build/Open Windows Build Folder")]
	public static void OpenWindowsBuildFolder()
	{
		// Due to an oddity in RevealInFinder we need to put a containing folder address in the folder we want to navigate
		// to otherwise it will just take us to the Build folder
		EditorUtility.RevealInFinder(@"Build\Windows\MonoBleedingEdge");
	}

	[MenuItem("Build/Windows")]
	public static void WindowsBuild()
	{
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = FindActiveScenesInBuildSettings();
		buildPlayerOptions.locationPathName = @"Build\Windows\" + Application.productName + ".exe";
		buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
		buildPlayerOptions.options = BuildOptions.None;

		// Resolution & Monitor Build
		// ==========================
		// Unfortunately you cannot seem to build your game to the left monitor without first setting it up within the Build Settings first
		// 
		// Change Resolution Settings then go to Edit -> Project Settings -> Player Settings -> Resolution & Presentation
		// Set to Windowed, set Fullscreen: Windowed, this will give you access to the Resolution settings
		// Build the game once, this will allow you to select which display you want it built on
		// You can then disable the Resolution Dialog at the start of the game select 'Display Resolution Dialog' in the Player Settings
		// It will remember your settings for your next Build via this script

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded!");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("*** WINDOWS BUILD FAILED! ***");
		}
	}

	[MenuItem("Build/Build and Run _F5")]
	public static void WindowsBuildAndRun()
	{
		// Save any changes made to the Scene before Building
		EditorSceneManager.MarkAllScenesDirty();
		EditorSceneManager.SaveOpenScenes();

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = FindActiveScenesInBuildSettings();
		buildPlayerOptions.locationPathName = @"Build\Windows\" + Application.productName + ".exe";
		buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			UnityEngine.Debug.Log("Build succeeded!");
			// *** Option to launch Playmode in the Unity Editor after a successful build
			//EditorApplication.ExecuteMenuItem("Edit/Play");

			Process.Start(@"Build\Windows\" + Application.productName + ".exe");
			// *** FOR MULTIPLAYER TESTING *** Uncomment these lines if you need to test a multiplayer game
			//RunMultipleInstances();
		}

		if (summary.result == BuildResult.Failed)
		{
			UnityEngine.Debug.Log("*** WINDOWS BUILD FAILED! ***");
		}
	}

	[MenuItem("Build/Run Multiple Instances _F12")]
	public static void RunMultipleInstances()
	{
		Process.Start(@"Build\Windows\" + Application.productName + ".exe");
		Process.Start(@"Build\Windows\" + Application.productName + ".exe");
	}

	public static string[] FindActiveScenesInBuildSettings()
	{
		List<string> buildSceneList = new List<string>();

		foreach (var scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled)
				buildSceneList.Add(scene.path);
		}

		return buildSceneList.ToArray();
	}
}