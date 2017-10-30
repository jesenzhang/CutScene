using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class OBJExportor
{
	[MenuItem("Export/Export OBJ")]
		public static void ExportOBJ()
		{
			//Object[] objs = Selection.objects;
			Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			if(objs == null || objs.Length == 0)
			{
				Debug.LogError("there is no object selected!");
				return;
			}

			ExportOBJ (EditorUserBuildSettings.activeBuildTarget,objs); 
		}

	[MenuItem("Export/Export OBJ ShaderKeeper")]
		public static void ExportOBJShaderKeeper()
		{
		Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		if(objs == null || objs.Length == 0)
		{
			Debug.LogError("there is no object selected!");
			return;
		}

		ExportOBJShaderKeeper (EditorUserBuildSettings.activeBuildTarget,objs); 
		}

	private static void ExportOBJShaderKeeper(BuildTarget buildTarget, Object[] objs)
	{
		string rootPath = EditorUtils.PlatformPath(buildTarget);
		if(rootPath == null)
			return;

		foreach(Object obj in objs)
		{
			string objPath = AssetDatabase.GetAssetPath(obj);
			objPath = objPath.Substring(objPath.IndexOf("/") + 1);
			//objPath = "Export/EOS/" + objPath;
			objPath = rootPath + objPath;
			string file = objPath.Substring(0, objPath.LastIndexOf("/") + 1);
			if(!Directory.Exists(file))
				Directory.CreateDirectory(file);

			objPath = Path.ChangeExtension(objPath, "");

			GameObject go = (GameObject)Object.Instantiate(obj);

			string name = go.name;
			int index = name.IndexOf("(");
			if(index >= 0)
				name = name.Substring(0, index);

			Object final = obj;

			Renderer[] rendererArr = go.GetComponentsInChildren<Renderer>(true);
			if (rendererArr.Length > 0) 
			{
				Renderer renderer;
				ShaderKeeper keeper = go.GetComponent<ShaderKeeper> ();
				if (keeper == null) 
				{
					keeper = go.AddComponent<ShaderKeeper> ();
					keeper.renderers = new List<Renderer> ();
					keeper.shaderNames = new List<string> ();
				}

				for (int i = 0, count = rendererArr.Length; i < count; i++) 
				{
					renderer = rendererArr [i];
					keeper.renderers.Add (renderer);
					keeper.shaderNames.Add (renderer.sharedMaterial.shader.name);
				}

				final = EditorUtils.GetPrefab(go, name);
			}

			Object.DestroyImmediate(go);

			BuildPipeline.BuildAssetBundle(final, null, objPath, BuildAssetBundleOptions.CollectDependencies, buildTarget);
		}
		Debug.Log("Export over");
	}

	public static void ExportOBJ(BuildTarget buildTarget,Object[] objs)
	{
		string rootPath = EditorUtils.PlatformPath(buildTarget);
		if(rootPath == null)
			return;

		foreach(Object obj in objs)
		{
			string objPath = AssetDatabase.GetAssetPath(obj);
			objPath = objPath.Substring(objPath.IndexOf("/") + 1);
			//objPath = "Export/EOS/" + objPath;
			objPath = rootPath + objPath;
			string file = objPath.Substring(0, objPath.LastIndexOf("/") + 1);
			if(!Directory.Exists(file))
				Directory.CreateDirectory(file);

			objPath = Path.ChangeExtension(objPath, "");

			BuildPipeline.BuildAssetBundle(obj, null, objPath, BuildAssetBundleOptions.CollectDependencies, buildTarget);
		}
		Debug.Log("Export over");
	}
}
