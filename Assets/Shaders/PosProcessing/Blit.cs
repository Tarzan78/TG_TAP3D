using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blit : MonoBehaviour
{
    //colocar na main camera

    // A Material with the Unity shader you want to process the image with
    public Material matCircle;
    public Material matStandart;
    private Material mat;

	[Range(0.0f, 1.0f)] public float radius;

	public bool showCircle = true;

	private void Update()
	{


		if (showCircle)
		{
			//send info for Shader 
			matCircle.SetFloat("_Radius", radius);

			mat = matCircle;
		}
		else
		{
			mat = matStandart;
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Read pixels from the source RenderTexture, apply the material, copy the updated results to the destination RenderTexture
        Graphics.Blit(src, dest, mat);

    }
}
