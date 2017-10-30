using UnityEngine;
using System.Collections;
using GameBase.Controller;
using GameBase;
using Data;

namespace GameCore
{
	public class MovMovement : MonoBehaviour 
	{
		private Transform m_transform;

		private NavAgentExController control = null;
		private Vector3 moveDir = Vector3.zero;
		private MovementAtt movementAtt = null;

		private TouchBase touchBase;
		private VirtualScreen virtualScreen;
		private bool beginDrag = false;
		private bool beginSwipe = false;

		private float prevSend = 0;

		public bool moveByMouse = false;

		enum State
		{
			IDLE,
			FOLLOW,
		}

		private State state = State.IDLE;


		void Awake()
		{
			m_transform = transform;

			control = gameObject.GetComponent<NavAgentExController>();
			if(control == null)
				control = gameObject.AddComponent<NavAgentExController>();

			if(control != null)
			{
				float h = control.GetControllerHeight();
				control.SetControllerCenter(new Vector3(0, h / 2, 0));
				control.SetControllerRadius(0.2f);
			}

			//control.SetBaseSpeed(20);
			{
				InitTouch();
			}
		}

		private void InitTouch()
		{
			bool realTouch = false;
			if(Application.platform == RuntimePlatform.Android)
				realTouch = true;

			GameObject go = gameObject;
			touchBase = go.AddComponent<TouchBase>();
            virtualScreen = go.AddComponent<VirtualScreen>();

            TouchBase.SetUseRealTouch(realTouch);
            TouchBase.OnDragStart += OnDragStart;
            TouchBase.OnDrag += OnDrag;
            TouchBase.OnDragEnd += OnDragEnd;
            TouchBase.OnSwipeStart += OnSwipeStart;
            TouchBase.OnSwipe += OnSwipe;
            TouchBase.OnSwipeEnd += OnSwipeEnd;
			TouchBase.OnDoubleTap += OnDoubleTap;
		}

		private void OnDragStart(Gesture gesture)
        {
            if (!gesture.isHoverReservedArea)
            {
                beginDrag = true;
            }
        }

        private void OnDrag(Gesture gesture)
        {
            if (beginDrag)
            {
                //if (luaOnDrag != null)
                 //   LuaManager.CallFunc_VX(luaOnDrag, gesture.deltaPosition.x, gesture.deltaPosition.y);
            }
        }

        private void OnDragEnd(Gesture gesture)
        {
            //beginDrag = false;
        }

        private void OnSwipeStart(Gesture gesture)
        {
            if (!gesture.isHoverReservedArea)
            {
                beginSwipe = true;
            }
        }

        private void OnSwipe(Gesture gesture)
        {
            if (beginSwipe)
            {
				FreeCamera.AppendFreeCRXY(gesture.swipeVector.x, -gesture.swipeVector.y);
                //if (luaOnSwipe != null)
                 //   LuaManager.CallFunc_VX(luaOnSwipe, gesture.swipeVector.x, gesture.swipeVector.y);
            }
        }

        private void OnSwipeEnd(Gesture gesture)
        {
            beginSwipe = false;
        }

		private void OnDoubleTap(Gesture gesture)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(gesture.position.x, gesture.position.y, 0));
			int layer = 1 << LayerMask.NameToLayer("Surface");
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 1000, layer))
			{
				Debug.Log("hit point->" + hit.point);
				Move(hit.point);
			}
		}

		//---------------------------------------------------------------



		void Start () 
		{
			//MovCamera.movCam.StartNormal(m_transform);
			SetBaseSpeed(8);

			FreeCamera.SetTarget(transform);
			FreeCamera.SetWantedDistance(15);
		}

		public void SetBaseSpeed(float v)
		{
			if(control != null)
				control.SetRunSpeed(v);
		}

		public void SetSpeepPercent(float v)
		{
			/*
			   if(control != null)
			   control.SetSpeedPercent(v);
			   */
		}

		public float CalculateY(Vector3 pos)
		{
			if(control != null)
				return control.GetSurfaceY(pos);

			return 0;
		}

		public void Jump()
		{
			/*
			   if(control != null)
			   control.Jump();
			   */
		}

		public void Jump(Vector3 dir)
		{
			/*
			   if(control != null)
			   control.Jump(dir);
			   */
		}

		//public void Move(int ix, int iy)
		public void Move(float ix, float iy)
		{
			if(control == null)
				return;


			int re = control.Move(-ix, -iy);
			if(re >= 0)
			{
			}
		}

		public void Move(Vector3f position, Vector3f dir, long time)
		{
			/*
			if(control != null)
				control.MoveDir(Converter.Vector3fToVector3(position), Converter.Vector3fToVector3(dir), time);
				*/
		}

		public void Move(Vector3 dest)
		{
			if(control == null)
				return;
			{
				//control.Reset();
				control.MoveTo(dest);
			}
		}

		public void SetPosition(Vector3 pos)
		{
			m_transform.position = pos;
		}

		public void Stop()
		{
			state = State.IDLE;
			if(control != null)
				control.Stop();
		}

		void OnEnable()
		{
		}

		void Update () 
		{
			if(Application.platform == RuntimePlatform.Android)
			{
			}
			else
			{
				if(control == null)
					return;
				if(!moveByMouse)
				{
					int ix = 0, iy = 0;
					if(Input.GetKey(KeyCode.W))
						iy = 1;
					if(Input.GetKey(KeyCode.S))
						iy = -1;
					if(Input.GetKey(KeyCode.A))
						ix = -1;
					if(Input.GetKey(KeyCode.D))
						ix = 1;

					if(ix != 0 || iy != 0)
						Move(ix, iy);
					else
						Stop();
				}
				else
				{
					if(Input.GetMouseButtonDown(0))
					{
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

						if(!Physics.Raycast(ray, out hit, 100.0f, 1 << LayerMask.NameToLayer("Surface")))
							return;

						Move(hit.point);
					}
				}
			}
		}
	}
}
