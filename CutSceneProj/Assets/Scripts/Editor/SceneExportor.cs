using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public static partial class SceneExportor
{
	[MenuItem("Export/Export Scene")]
		static void _ExportScene()
		{
			Debug.Log("export cur Scene->" + EditorApplication.currentScene);

			if(EditorApplication.currentScene == string.Empty)
			{
				Debug.LogError("select a scene first");
				return;
			}

			_ExportScene(EditorApplication.currentScene, EditorUserBuildSettings.activeBuildTarget);
		}

	//public static void ExportScene(string scenePath, string platform)
	public static void ExportScene()
	{
		string[] arr = System.Environment.GetCommandLineArgs();
		if(arr == null || arr.Length == 0)
			return;
		if(arr.Length < 2)
			return;

		string scenePath = arr[7];
		string platform = arr[8];

		if(scenePath == null || scenePath == "")
			return;

		BuildTarget buildTarget = BuildTarget.StandaloneWindows;
		switch(platform)
		{
			case "ios":
				{
					buildTarget = BuildTarget.iOS;
				}
				break;
			case "android":
				{
					buildTarget = BuildTarget.Android;
				}
				break;
		}

		_ExportScene(scenePath, buildTarget);
	}

	private static void _ExportScene(string scenePath, BuildTarget buildTarget)
	{
		/*
		if(EditorApplication.currentScene == string.Empty)
		{
			Debug.LogError("select a scene first");
			return;
		}
		*/

		//string path = "Export/Scene/";
		string path = EditorUtils.PlatformPath(buildTarget);
		if(path == null)
			return;

		path += "Scene/";

		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);

		//string fileName = "Scene-" + Path.GetFileNameWithoutExtension(EditorApplication.currentScene) + "";
		//string fileName = "Scene-" + Path.GetFileNameWithoutExtension(scenePath) + "";
		string fileName = Path.GetFileNameWithoutExtension(scenePath);

		fileName = path + fileName;

		string res = BuildPipeline.BuildStreamedSceneAssetBundle(new string[]{scenePath}, fileName, buildTarget);
		if(res != string.Empty)
		{
			Debug.LogError(res);
			return;
		}

		Debug.Log("Export scene over");
	}
}
