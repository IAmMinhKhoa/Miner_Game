using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class customMaskUi : Image
{
	public override Material materialForRendering
	{
		get
		{
			Material material = new Material(base.materialForRendering);
			material.SetInt("_Stencil", 4);
			return material;
		}
	}
}
