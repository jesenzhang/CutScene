using UnityEngine;
using System.Collections;
using System;

public enum EffectType{
	SCENE,
	CHARACTOR,
	UI
}

[ExecuteInEditMode]
public class EffectSetting : MonoBehaviour {

	public EffectType effectType = EffectType.CHARACTOR;
    
    //#if UNITY_EDITOR
    public Transform rootNode;
	public Transform mountNode;
	//#endif 

	[HideInInspector]
	[SerializeField]
	public string rootPath;

	[HideInInspector]
	[SerializeField]
	public string mountPath; 

	public string relativePath;

	public Vector3 offset;
    public Vector3 rotation;

    void Update () {
        #if UNITY_EDITOR
		if (rootNode!=null) {
			rootPath = GetFullPath (rootNode);  
		}
		if (mountNode!=null) {
			mountPath = GetFullPath (mountNode);
			if(!string.IsNullOrEmpty(rootPath)){
				relativePath = mountPath.Replace(rootPath+"/","");
			}
		}
		if(rootNode ==null && !string.IsNullOrEmpty(rootPath)){
			var go = GameObject.Find(rootPath);
			if(go!=null)
				rootNode = go.transform;
		}
		if(mountNode ==null && !string.IsNullOrEmpty(mountPath)){
			var go = GameObject.Find(mountPath);
			if(go!=null)
				rootNode = go.transform;
		}
		#endif
	}

	public bool LoadEffect(string rootName = null){
		string fullpath = string.IsNullOrEmpty(rootName)?rootPath:rootName +"/" + relativePath;
		var node = GameObject.Find (fullpath); 
		if (node == null) {
			Debug.LogWarningFormat ("EffectSetting root path of {0} not found", relativePath);
			return false;
		}
		transform.parent = node.transform;
		transform.localScale = Vector3.one;
		transform.localPosition = offset;
		transform.localRotation = Quaternion.Euler(rotation);
		return true;
	}

	public bool LoadEffect(Transform root){ 
		var node = root.Find (relativePath); 
		if (node == null) {
			Debug.LogWarningFormat ("EffectSetting root path of {0} not found", relativePath);
			return false;
		}
		transform.parent = node.transform;
		transform.localScale = Vector3.one;
		transform.localPosition = offset;
		transform.localRotation = Quaternion.Euler(rotation); 
		return true;
	}
	 

	private static string GetFullPath(Transform node){ 
		string path = node.name;
		while (node.parent != null) {
			node = node.parent;
			path = node.name +"/"+path;
		} 
		return path;
	}
}
