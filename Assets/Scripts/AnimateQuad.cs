using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateQuad : MonoBehaviour
{
	[SerializeField]
	private Renderer renderer;

	private float offset = 0f;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update()
	{
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
		offset += 0.15f;
	}
}
