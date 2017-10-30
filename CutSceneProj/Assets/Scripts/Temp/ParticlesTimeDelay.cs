using UnityEngine;
using System.Collections;

public class ParticlesTimeDelay : MonoBehaviour 
{
	public float time;
	public bool delayEnable = false;
	public Transform trans = null;

	// Use this for initialization
	void Start () 
	{
		//gameObject.SetActive(!delayEnable);
		Invoke ("Delay",time);
	}


	void Delay()
	{
		if(trans)
			trans.gameObject.SetActive(delayEnable);
	}
}
