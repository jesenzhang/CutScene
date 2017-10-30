using UnityEngine;
using System.Collections;

public class FPSUtility : MonoBehaviour {
	public float updateInterval = 0.5F;
	private double lastInterval;
	private int frames = 0;
	private float fps;

	private GUIStyle fontStyle;

	void Awake(){
		fontStyle = new GUIStyle();  
		fontStyle.normal.background = null;    //设置背景填充  
		fontStyle.normal.textColor= new Color(1,0,0);   //设置字体颜色  
		fontStyle.fontSize = 30;       //字体大小  
	}
	void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	void OnGUI()
	{
		GUI.Label(new Rect(10,Screen.height - 100,200,200),"FPS:" + fps.ToString("f2"),fontStyle);
	}
	void Update()
	{
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow > lastInterval + updateInterval)
		{
			fps = (float)(frames / (timeNow - lastInterval));
			frames = 0;
			lastInterval = timeNow;
		}
	} 
}
