using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {
	
	public float horizontalSpeed =500;
	
	
	public float rotate = 90;
	private GameObject thePlayer;
	public Transform thePlayerTrans;
	
	public float distance = 7;
	public float height = 7;
	private float heightPare = 1;
	
	public float maxHeight = 8.5f;
	private float minHeight = 2f;
	
	public float maxDis = 8.5f;
	private float minDis = 2f;
	
	private float lightDis;
	
	
	private bool isShowChange = false;
	
	private float changeSpeed = 0.1f;
	
//	private float heightDamping = 2.0f;
	
//	private float rotationDamping = 3.0f;
	Quaternion currentRotation;
	float currentRotationAngle;
	
	Camera theCameral;
	
	
	Vector3 nor;
	RaycastHit[] hits;
	List<RaycastHit> hits1 = new List<RaycastHit>();
	List<RaycastHit> listHit = new List<RaycastHit>();
	
	
	//private Light theLight;
	// Use this for initialization
	void Start () {
		theCameral = GetComponent<Camera>();
	//	theLight = GameObject.Find("SpotlightTest").GetComponent<Light>();
	//	theLight.type = LightType.Spot;
	//	theLight.cullingMask = 1<<15;
		//thePlayer = GameObject.Find("Player"); 
	}
	
	// Update is called once per frame
	void Update () 
	{
        FollowIng();
        ScrollWheelControl();
        CarmeralRotation();
		AnglePreferences();
        if (thePlayer==null)
        {
            thePlayer = GameObject.Find("Player");
            
        }
        else
        {
            ParentCarmer();
        //    CameraFromAlpha();
        }
		
		
	}
	void ParentCarmer()
	{
		thePlayerTrans.position = thePlayer.transform.position+new Vector3(0,heightPare,0);
	}
	void FollowIng()
	{
		if(thePlayerTrans == null)
		{
			return;
		}
		
		float wantedRotationAngle = thePlayerTrans.eulerAngles.y;
		float wantedHeight = thePlayerTrans.position.y+height;
		 currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
	//	currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentRotationAngle = wantedRotationAngle;
	//	currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		currentHeight = wantedHeight;
	
		currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	

		transform.position = thePlayerTrans.position;
		transform.position -=  currentRotation * Vector3.forward * distance;

		transform.position = new Vector3(transform.position.x , currentHeight,transform.position.z) ;
	
		transform.LookAt (thePlayerTrans);
		
		transform.parent = thePlayerTrans;
		
		lightDis = 10+5*(distance-minDis);
		
	//	theLight.range = lightDis;
	}
	float firstVec = 0;
	
	/// <summary>
	/// 摄像机旋转
	/// </summary>
/*	public void CarmeralRotation()
	{
		float nextVec = 0;
		if(Input.GetMouseButtonDown(1))
		{
			firstVec = Input.mousePosition.x;
			
		}
		if(Input.GetMouseButton(1))
		{
			nextVec = Input.mousePosition.x;
			float v = nextVec-firstVec;
			rotate = v/3;
			thePlayerTrans.RotateAround(thePlayerTrans.position, Vector3.up, rotate * Time.deltaTime);
		}
	}*/
	public void CarmeralRotation()
	{
		float v;
		if(Input.GetMouseButton(1))
		{
			firstVec = Input.mousePosition.x;
			float h = horizontalSpeed * Input.GetAxis("Mouse X");
			thePlayerTrans.RotateAround(thePlayerTrans.position, Vector3.up, h * Time.deltaTime);
		}
	}
	/// <summary>
	/// 视角调节
	/// </summary>
	void  ScrollWheelControl()
	{
		float v = Input.GetAxisRaw("Mouse ScrollWheel");
		if(v != 0 )
		{
			isShowChange = true;
			StartCoroutine(CameraAngle(v));
			Invoke("StopCameraAngle",0.8f);
		}
	}
	/// <summary>
	/// 摄像机镜头近远变化 润滑处理
	/// </summary>
	/// <returns>
	/// The angle.
	/// </returns>
	IEnumerator CameraAngle(float v)
	{
		v = v/Mathf.Abs(v);
		while(isShowChange)
		{
			if(distance>maxDis)
			{
				distance=maxDis;
			}
			else if(distance<minDis)
			{
				distance=minDis;
			}
			
			if(height>maxHeight)
			{
				height=maxHeight;
			}
			else if(height<minHeight)
			{
				height=minHeight;
			}
			
		
			height = height-v*changeSpeed/3.0f;
			
	
			distance = distance-v*changeSpeed/3.0f;
		
			
			
			yield return null;
		}
	}
	void StopCameraAngle()
	{
		isShowChange = false;
		StopCoroutine("CameraAngle");
	}
	/// <summary>
	/// 参数取值范围
	/// </summary>
	void AnglePreferences()
	{
		
	}
	/// <summary>
	///人物与摄像机之间存在 列如墙壁阻挡时，修改墙壁阻挡Alpha
	/// </summary>
	void CameraFromAlpha()
	{
        
		nor = (thePlayer.transform.position-this.transform.position).normalized;
		
		hits1.Clear();
		hits = Physics.RaycastAll(this.transform.position, nor, 40.0F,1<<12);
		if(hits.Length ==0)
		{
			foreach(RaycastHit hitTemp in listHit)
			{
				hitTemp.transform.BroadcastMessage("ShaderChangeDiffuse",SendMessageOptions.DontRequireReceiver);
			}
			listHit.Clear();
		}
		else 
		{
			for(int j=0 ; j<hits.Length ; j++)
			{
				hits1.Add(hits[j]);
			}
		}
		
        int i = 0;
        while (i < hits1.Count) {
            RaycastHit hit = hits1[i];
			if(hit.transform.tag == "Alpha")
			{
				if(!listHit.Contains(hit))
				{
					listHit.Add(hit);
				}
				hit.transform.BroadcastMessage("ChangeTransparent",SendMessageOptions.DontRequireReceiver);
			}
			
            i++;
        }
	} 

}
