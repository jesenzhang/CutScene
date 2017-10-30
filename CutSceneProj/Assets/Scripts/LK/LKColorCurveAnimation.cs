using System;
using UnityEngine;
public class LKColorCurveAnimation : ILKCurveAnimationImp
{
	//private UIWidget mUIWidget;
	private Color mOriginalColor;
	private bool isEnable;
	public LKColorCurveAnimation(LKCurveAnimation lkCurveAnimation)
	{
        /*
		this.mUIWidget = lkCurveAnimation.transform.GetComponent<UIWidget>();
		if (this.mUIWidget != null)
		{
			this.isEnable = true;
			this.mOriginalColor = this.mUIWidget.color;
		}
		else
		{
			this.isEnable = false;
		}
        */
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
	}
	public void Reset()
	{
		if (this.isEnable)
		{
			//this.mUIWidget.color = this.mOriginalColor;
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		if (this.isEnable)
		{
			float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime);
			Color colorDelta = lkCurveInfo.toColor - this.mOriginalColor;
            /*
			Color currColor = this.mUIWidget.color;
			if (lkCurveInfo.rApply)
			{
				currColor.r += colorDelta.r * deltaCurveValue;
			}
			if (lkCurveInfo.gApply)
			{
				currColor.g += colorDelta.g * deltaCurveValue;
			}
			if (lkCurveInfo.bApply)
			{
				currColor.b += colorDelta.b * deltaCurveValue;
			}
			if (lkCurveInfo.aApply)
			{
				currColor.a += colorDelta.a * deltaCurveValue;
			}
			this.mUIWidget.color = currColor;
            */
		}
	}
}
