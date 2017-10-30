using System;
using System.Collections.Generic;
using UnityEngine;
public class LKCurveAnimation : MonoBehaviour
{
	[SerializeField]
	private bool mEnable;
	[SerializeField]
	private float mDuration;
	[SerializeField]
	private float mDelayTime;
	[SerializeField]
	private int mnLoopCount;
	[SerializeField]
	private bool mbAutoDisable;
	[SerializeField]
	private List<LKCurveInfo> mLKCurveInfoList = new List<LKCurveInfo>();
	private float mDurationDelta;
	private float mDelayDelta;
	private ILKCurveAnimationImp[] mLKCurveAnimationImpArr = null;
	public GameObject eventReceiver;
	public string callWhenFinished;
	private int mnLoopDelta = 0;
	public bool enable
	{
		get
		{
			return this.mEnable;
		}
		set
		{
			this.mEnable = value;
		}
	}
	public float duration
	{
		get
		{
			return this.mDuration;
		}
		set
		{
			this.mDuration = value;
		}
	}
	public float delayTime
	{
		get
		{
			return this.mDelayTime;
		}
		set
		{
			this.mDelayTime = value;
		}
	}
	public int loopCount
	{
		get
		{
			return this.mnLoopCount;
		}
		set
		{
			this.mnLoopCount = value;
		}
	}
	public bool autoDisable
	{
		get
		{
			return this.mbAutoDisable;
		}
		set
		{
			this.mbAutoDisable = value;
		}
	}
	public void Reset()
	{
		this.mnLoopDelta = 0;
		this.mDurationDelta = 0f;
		this.mDelayDelta = 0f;
		base.gameObject.SetActive(true);
		if (null == this.mLKCurveAnimationImpArr)
		{
			this.mLKCurveAnimationImpArr = new ILKCurveAnimationImp[8];
		}
		int i = 0;
		int max = this.mLKCurveInfoList.Count;
		while (i < max)
		{
			int impIndex = (int)this.mLKCurveInfoList[i].curveType;
			if (null == this.mLKCurveAnimationImpArr[impIndex])
			{
				this.mLKCurveAnimationImpArr[impIndex] = this.CreateLKCurveAnimationImp(this.mLKCurveInfoList[i].curveType);
			}
			i++;
		}
		i = 0;
		max = this.mLKCurveInfoList.Count;
		while (i < max)
		{
			this.mLKCurveInfoList[i].Reset();
			i++;
		}
		i = 0;
		max = this.mLKCurveInfoList.Count;
		while (i < max)
		{
			LKCurveInfo lkCurveInfo = this.mLKCurveInfoList[i];
			if (lkCurveInfo.enable)
			{
				int impIndex = (int)lkCurveInfo.curveType;
				ILKCurveAnimationImp imp = this.mLKCurveAnimationImpArr[impIndex];
				if (null != imp)
				{
					imp.Init(lkCurveInfo);
					imp.Reset();
				}
			}
			i++;
		}
		i = 0;
		max = this.mLKCurveInfoList.Count;
		while (i < max)
		{
			int impIndex = (int)this.mLKCurveInfoList[i].curveType;
			ILKCurveAnimationImp imp = this.mLKCurveAnimationImpArr[impIndex];
			if (null != imp)
			{
				imp.ApplyCurve(this.mLKCurveInfoList[i], 0f);
			}
			i++;
		}
	}
	public void PlayCurveAnimation()
	{
		this.mEnable = true;
	}
	public void StopCurveAnimation()
	{
		this.mEnable = false;
		if (this.autoDisable)
		{
			base.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(this.callWhenFinished))
		{
			GameObject go = this.eventReceiver;
			if (go == null)
			{
                go = base.gameObject;
			}
			//LuaMessage msg = new LuaMessage(base.get_gameObject(), this, "TweenEnd");
			//go.SendMessage(this.callWhenFinished, msg, 1);
		}
	}
	public int GetCurveInfoCount()
	{
		return this.mLKCurveInfoList.Count;
	}
	public LKCurveInfo GetCurveInfo(int lkCurveInfoIndex)
	{
		LKCurveInfo result;
		if (lkCurveInfoIndex < this.mLKCurveInfoList.Count)
		{
			result = this.mLKCurveInfoList[lkCurveInfoIndex];
		}
		else
		{
			result = null;
		}
		return result;
	}
	public void AddCurveInfo(LKCurveInfo lkCurveInfo)
	{
		this.mLKCurveInfoList.Add(lkCurveInfo);
	}
	public void RemoveCurveInfo(LKCurveInfo lkCurveInfo)
	{
		this.mLKCurveInfoList.Remove(lkCurveInfo);
	}
	public void RemoveCurveInfo(int lkCurveInfoIndex)
	{
		this.mLKCurveInfoList.RemoveAt(lkCurveInfoIndex);
	}
	public void RemoveAllCurveInfo()
	{
		this.mLKCurveInfoList.Clear();
	}
	private void OnEnable()
	{
		this.Reset();
		this.PlayCurveAnimation();
	}
	private void Start()
	{
		this.Reset();
	}
	private void Update()
	{
		if (this.mEnable)
		{
			if (this.mDelayDelta > this.mDelayTime)
			{
				this.mDurationDelta += Time.deltaTime;
				if (this.mDurationDelta >= this.mDuration)
				{
					if (this.mnLoopCount >= 0 && this.mnLoopDelta >= this.mnLoopCount - 1)
					{
						this.StopCurveAnimation();
					}
					else
					{
						this.mnLoopDelta++;
						this.mDurationDelta -= this.mDuration;
						this.Reset();
					}
				}
				this.Evaluate(this.mDurationDelta);
			}
			else
			{
                this.mDelayDelta += Time.deltaTime;
			}
		}
	}
	private void Evaluate(float time)
	{
		if (this.mLKCurveInfoList != null)
		{
			float normalizedTime = time / this.mDuration;
			int i = 0;
			int max = this.mLKCurveInfoList.Count;
			while (i < max)
			{
				LKCurveInfo lkCurveInfo = this.mLKCurveInfoList[i];
				if (lkCurveInfo.enable)
				{
					int impIndex = (int)lkCurveInfo.curveType;
					ILKCurveAnimationImp imp = this.mLKCurveAnimationImpArr[impIndex];
					if (null != imp)
					{
						imp.ApplyCurve(lkCurveInfo, normalizedTime);
					}
				}
				i++;
			}
		}
	}
	private ILKCurveAnimationImp CreateLKCurveAnimationImp(LKCurveInfo.LKCurveType curveType)
	{
		ILKCurveAnimationImp result;
		switch (curveType)
		{
		case LKCurveInfo.LKCurveType.none:
			result = null;
			break;
		case LKCurveInfo.LKCurveType.position:
			result = new LKPositionCurveAnimationImp(this);
			break;
		case LKCurveInfo.LKCurveType.rotation:
			result = new LKRotationCurveAnimationImp(this);
			break;
		case LKCurveInfo.LKCurveType.scale:
			result = new LKScaleCurveAnimation(this);
			break;
		case LKCurveInfo.LKCurveType.material_color1:
			result = new LKMaterialColor1CurveAnimation(this);
			break;
		case LKCurveInfo.LKCurveType.texture_uv:
			result = new LKTextureUVCurveAnimationImp(this);
			break;
		case LKCurveInfo.LKCurveType.material_color2:
			result = new LKMaterialColor2CurveAnimationImp(this);
			break;
		case LKCurveInfo.LKCurveType.UIWidgetColor:
			result = new LKColorCurveAnimation(this);
			break;
		default:
			result = null;
			break;
		}
		return result;
	}
}
