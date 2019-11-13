using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Helper
{
	public static int GetCurrentLevelNumber()
	{
		var levelName = SceneManager.GetActiveScene().name;
		int.TryParse(new string(levelName.Where(char.IsDigit).ToArray()), out int currentLevelNumber);

		return currentLevelNumber;
	}

	public static int GetNextLevelNumber()
	{
		var levelName = SceneManager.GetActiveScene().name;
		int.TryParse(new string(levelName.Where(char.IsDigit).ToArray()), out int currentLevelNumber);

		return currentLevelNumber + 1;
	}

	// Return the number of scenes in the Build Settings with the name 'Level'
	public static int GetLevelsFromSceneFolder()
	{
		int totalLevels = 0;

		DirectoryInfo pathToSceneFiles = new DirectoryInfo(Application.dataPath + @"/_Scenes");
		var sceneFiles = pathToSceneFiles.GetFiles("*.unity");

		foreach (var scene in sceneFiles)
		{
			// Only count the scenes that have 'Level' in their name
			if (scene.Name.Contains("Level"))
				totalLevels++;
		}

		return totalLevels;
	}
}