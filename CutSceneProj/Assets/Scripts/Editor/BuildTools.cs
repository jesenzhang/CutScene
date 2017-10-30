using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildTools  {

	[MenuItem("Build/Android")]
	public static void BuildAndroid(){
		PlayerSettings.colorSpace = ColorSpace.Linear;
		BuildPipeline.BuildPlayer (new string[0], "tt.apk", BuildTarget.Android,BuildOptions.None);
	}
}
