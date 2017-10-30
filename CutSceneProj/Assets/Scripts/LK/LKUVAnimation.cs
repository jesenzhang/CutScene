using System;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class LKUVAnimation : MonoBehaviour
{
	public float scrollSpeedX = 1f;
	public float scrollSpeedY = 0f;
	public float tilingX = 1f;
	public float tilingY = 1f;
	public float offsetX = 0f;
	public float offsetY = 0f;
	public bool mbUseSmoothDeltaTime = false;
	public bool mbFixedTileSize = false;
	public bool mbRepeat = true;
	public bool mbAutoDestruct = false;
	protected Vector3 mOriginalScale = default(Vector3);
	protected Vector2 mOriginalTiling = default(Vector2);
	protected Vector2 mEndOffset = default(Vector2);
	protected Vector2 mRepeatOffset = default(Vector2);
	protected Renderer mRenderer;
	public void SetFixedTileSize(bool bFixedTileSize)
	{
		this.mbFixedTileSize = bFixedTileSize;
	}
	public void ResetAnimation()
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
		this.Start();
	}
	private void Start()
	{
		this.mRenderer = base.GetComponent<Renderer>();
		if (this.mRenderer == null || this.mRenderer.sharedMaterial == null || this.mRenderer.sharedMaterial.mainTexture == null)
		{
			base.enabled = false;
		}
		else
		{
			this.mRenderer.material.mainTextureScale = new Vector2(this.tilingX, this.tilingY);
			float offset = this.offsetX + this.tilingX;
			this.mRepeatOffset.x = offset - (float)((int)offset);
			if (this.mRepeatOffset.x < 0f)
			{
				this.mRepeatOffset.x = this.mRepeatOffset.x + 1f;
			}
			offset = this.offsetY + this.tilingY;
			this.mRepeatOffset.y = offset - (float)((int)offset);
			if (this.mRepeatOffset.y < 0f)
			{
				this.mRepeatOffset.y = this.mRepeatOffset.y + 1f;
			}
			this.mEndOffset.x = 1f - (this.tilingX - (float)((int)this.tilingX) + (float)((this.tilingX - (float)((int)this.tilingX) < 0f) ? 1 : 0));
			this.mEndOffset.y = 1f - (this.tilingY - (float)((int)this.tilingY) + (float)((this.tilingY - (float)((int)this.tilingY) < 0f) ? 1 : 0));
		}
	}
	private void Update()
	{
		if (!(this.mRenderer == null) && !(this.mRenderer.sharedMaterial == null) && !(this.mRenderer.sharedMaterial.mainTexture == null))
		{
			if (this.mbFixedTileSize)
			{
				if (this.scrollSpeedX != 0f && this.mOriginalScale.x != 0f)
				{
					this.tilingX = this.mOriginalTiling.x * (base.transform.lossyScale.x / this.mOriginalScale.x);
				}
				if (this.scrollSpeedY != 0f && this.mOriginalScale.y != 0f)
				{
					this.tilingY = this.mOriginalTiling.y * (base.transform.lossyScale.y / this.mOriginalScale.y);
				}
				base.GetComponent<Renderer>().material.mainTextureScale = new Vector2(this.tilingX, this.tilingY);
			}
			if (this.mbUseSmoothDeltaTime)
			{
				this.offsetX += Time.smoothDeltaTime * this.scrollSpeedX;
				this.offsetY += Time.smoothDeltaTime * this.scrollSpeedY;
			}
			else
			{
				this.offsetX += Time.deltaTime * this.scrollSpeedX;
				this.offsetY += Time.deltaTime * this.scrollSpeedY;
			}
			bool bCallEndAni = false;
			if (!this.mbRepeat)
			{
				this.mRepeatOffset.x = this.mRepeatOffset.x + Time.deltaTime * this.scrollSpeedX;
				if (this.mRepeatOffset.x < 0f || 1f < this.mRepeatOffset.x)
				{
					this.offsetX = this.mEndOffset.x;
					base.enabled = false;
					bCallEndAni = true;
				}
				this.mRepeatOffset.y = this.mRepeatOffset.y + Time.deltaTime * this.scrollSpeedY;
				if (this.mRepeatOffset.y < 0f || 1f < this.mRepeatOffset.y)
				{
					this.offsetY = this.mEndOffset.y;
					base.enabled = false;
					bCallEndAni = true;
				}
			}
			this.offsetX = Mathf.Clamp01(this.offsetX);
			this.offsetY = Mathf.Clamp01(this.offsetY);
			this.offsetX %= 1f;
			this.offsetY %= 1f;
			this.mRenderer.material.mainTextureOffset = new Vector2(this.offsetX, this.offsetY);
			if (bCallEndAni)
			{
				if (this.mbAutoDestruct)
				{
					UnityEngine.Object.DestroyObject(gameObject);
				}
			}
		}
	}
}
