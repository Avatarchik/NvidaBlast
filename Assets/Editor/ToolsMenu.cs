using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

public class ToolsMenu
{
	// Examples on how to use MenuItem: https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items?playlist=17090
	// Modifier Keys Reference: https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items?playlist=17090

	// *** Modifier Keys ***
	// [CTRL]	%
	// [Shift]	#
	// [Alt]	&
	// [Arrow keys] LEFT/RIGHT/UP/DOWN 
	//
	// F1…F2 – F keys
	// HOME, END, PGUP, PGDN
	// _ (no key combination, just use the key itself)
	// Just hit F5 => _F5
	//
	// [Space] is represented by 'SPACE' (previously it was '_ ')
	//
	// [MenuItem("Tools/Open Persistent Data Path")]
	// This creates a new Menu Item called 'Tools' with 'Open Persistent Data Path' as an option

	[MenuItem("Tools/Open Persistent Data Path")]
	private static void OpenPersistentDataPath()
	{
		// Due to an oddity in RevealInFinder we need to put a containing folder address in the folder we want to navigate
		// to otherwise it will just take us to the Company Name folder
		EditorUtility.RevealInFinder(@"C:\Users\MSI\AppData\LocalLow\" + Application.companyName + @"\" 
			+ Application.productName + @"\Unity");

		// Persistant Data Path Locations
		//
		// Windows:
		// %userprofile%\AppData\LocalLow\<companyName>\<packageName>
		//
		// Android:
		// /storage/emulated/0/Android/data/<packageName>/files
	}

	[MenuItem("Tools/Clear PlayerPrefs + Save Slots")]
	static void ClearPlayerPrefsFile()
	{
		PlayerPrefs.DeleteAll();

		var saveSlots = new DirectoryInfo(@"C:\Users\MSI\AppData\LocalLow\" + Application.companyName + @"\"
			+ Application.productName);

		if (saveSlots.Exists)
		{
			var filePaths = saveSlots.GetFiles("*.*");

			foreach (var file in filePaths)
			{
				file.Delete();
			}
		}
	}

	[MenuItem("Tools/Pause Editor _PGUP")]
	private static void PauseEditor()
	{
		// Toggle between pause & play
		EditorApplication.isPaused = !EditorApplication.isPaused;
	}

	[MenuItem("Tools/Toggle Grid %G")]
	private static void ToggleGrid()
	{
		// Toggle the Grid
		ShowGrid = !ShowGrid;
	}

	// %PGUP => Hit [Ctrl + PageUp]
	[MenuItem("Tools/Toggle Auto Refresh %PGUP")]
	private static void ToggleAutoRefresh()
	{
		// Toggle between AutoRefresh & Manual Refresh
		if (EditorPrefs.GetBool("kAutoRefresh") == true)
			EditorPrefs.SetBool("kAutoRefresh", false);

		else
			EditorPrefs.SetBool("kAutoRefresh", true);
	}

	// taken from: http://answers.unity3d.com/questions/282959/set-inspector-lock-by-code.html
	[MenuItem("Tools/Toggle Inspector Lock %SPACE")] 
	static void ToggleInspectorLock()
	{
		// This code only works for one Inspector Window, if you have multiple it will only lock one
		// If you close the 'wrong' Inspector Window it will appear that this code no longer works
		// but if you open another Inspector Window you will see that it should work for one of them
		ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
		ActiveEditorTracker.sharedTracker.ForceRebuild();
	}

	[MenuItem("Tools/Toggle Always Show Colliders 2D %HOME")]
	static void ToggleAlwaysShowColliders2D()
	{
		Physics2D.alwaysShowColliders = !Physics2D.alwaysShowColliders;
	}

	[MenuItem("Tools/Play Audio Clip %#SPACE")]
	//[MenuItem("Tools/Play Audio Clip _SPACE")]	// You can uncomment this when flipping through audio files but you 
													// shouldn't keep 'space' as a hotkey as you will need space to name things
	public static void PlayClip()
	{
		System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
		System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
		System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
			"PlayClip",
			System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
			null,
			new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
			null
		);
		method.Invoke(
			null,
			new object[] { Selection.activeObject, 0, false }
		);
	}

	[MenuItem("Tools/Clear Parent %#Z")]
	static void ClearParent()
	{
		if (Selection.activeTransform.parent != null)
			Selection.activeTransform.transform.parent = null;
	}

	// Show Grid Codes

	private static Type m_annotationUtility;
	private static PropertyInfo m_showGridProperty;

	private static PropertyInfo ShowGridProperty
	{
		get
		{
			if (m_showGridProperty == null)
			{
				m_annotationUtility = Type.GetType("UnityEditor.AnnotationUtility,UnityEditor.dll");
				m_showGridProperty = m_annotationUtility.GetProperty("showGrid", BindingFlags.Static | BindingFlags.NonPublic);
			}
			return m_showGridProperty;
		}
	}

	public static bool ShowGrid
	{
		get
		{
			return (bool)ShowGridProperty.GetValue(null, null);
		}

		set
		{
			ShowGridProperty.SetValue(null, value, null);
		}
	}
}