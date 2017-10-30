using UnityEngine;
using System.Collections;

public class JumpNodeInfo : MonoBehaviour 
{
	public int id = -1;
	public int next = -1;
	public bool uniformSpeed = false;
	public int time = 0;
	public int delay = 0;
	public int cameraDistance = 0;
	public string animBegin = "";
	public string animMove = "";
	public string animOver = "";
	public int animBeginTime = 0;
}
