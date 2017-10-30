using System;
using UnityEngine;
[Serializable]
public class LKCurveInfo
{
	public enum LKCurveType
	{
		none,
		position,
		rotation,
		scale,
		material_color1,
		texture_uv,
		material_color2,
		UIWidgetColor,
		Max
	}
	public bool enable;
	public string curveName = string.Empty;
	public LKCurveInfo.LKCurveType curveType = LKCurveInfo.LKCurveType.none;
	public AnimationCurve animationCurve = new AnimationCurve();
	public float scaleValue;
	public bool xAxis;
	public bool yAxis;
	public bool zAxis;
	public bool isWorld;
	public bool bRecursive;
	public Color fromColor;
	public Color toColor;
	public bool rApply;
	public bool gApply;
	public bool bApply;
	public bool aApply;
	public bool xApply;
	public bool yApply;
	private float mLastCurveValue = 0f;
	public void Reset()
	{
		this.mLastCurveValue = 0f;
	}
	public float Evaluate(float time)
	{
		float curveValue = 0f;
		if (this.animationCurve != null)
		{
			curveValue = this.animationCurve.Evaluate(time);
		}
		return curveValue;
	}
	public float GetDeltaCurveValue(float time)
	{
		float currentCurveValue = this.Evaluate(time);
		float deltaCurveValue = currentCurveValue - this.mLastCurveValue;
		this.mLastCurveValue = currentCurveValue;
		return deltaCurveValue;
	}
}
