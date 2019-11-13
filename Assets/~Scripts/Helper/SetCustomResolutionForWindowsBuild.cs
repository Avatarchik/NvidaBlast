using UnityEngine;

public class SetCustomResolutionForWindowsBuild : MonoBehaviour 
{
	private void Awake()
	{
		// Set screen size for Standalone Resolution must be set in the Player Settings for PC. You cannot code the resolution
		// here from PlayerSettings unfortunately as it will give an error on build, so it must be 'hardcoded' in
#if UNITY_STANDALONE
		Screen.SetResolution(765, 1080, false);
		Screen.fullScreen = false;
#endif
	}
}