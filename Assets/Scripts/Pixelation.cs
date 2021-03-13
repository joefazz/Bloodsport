using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixelation : MonoBehaviour
{
	[SerializeField]
	private RenderTexture renderTexture;

	// Start is called before the first frame update
	void Start()
	{
		int realRatio = Mathf.Max(Mathf.RoundToInt(Screen.width / Screen.height), 1);
		renderTexture.width = NearestSuperiorPowerOf2(Mathf.RoundToInt(renderTexture.width * realRatio));
	}

	private void OnGUI()
	{
		GUI.depth = 20;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
	}

	private int NearestSuperiorPowerOf2(int n)
	{
		return (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(n) / Mathf.Log(2)));
	}
}
