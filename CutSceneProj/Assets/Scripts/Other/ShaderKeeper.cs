using System.Collections.Generic;
using UnityEngine;

public class ShaderKeeper : MonoBehaviour
{
	public List<Renderer> renderers;

	public List<string> shaderNames;

	void Awake() 
	{
		if (renderers != null && shaderNames != null)
		{
			if (renderers.Count == shaderNames.Count) 
			{
				Renderer render = null;
				for (int i = 0, count = renderers.Count; i < count; i++)
				{
					render = renderers[i];
					if(render.sharedMaterial != null)
					{
						Shader shader = Shader.Find(shaderNames[i]);
						if(shader != null)
							render.sharedMaterial.shader = shader;
						else
							Debug.LogError("---------------------------------------------shader is not found->" + shaderNames[i]);
					}
				}
			}
		}
	}
}
