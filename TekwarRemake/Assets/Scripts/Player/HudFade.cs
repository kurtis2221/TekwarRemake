using UnityEngine;
using System.Collections;

public class HudFade : MonoBehaviour
{
	System.Action hndl;
	Color col;
	bool fading;
	
	public void Init(System.Action hndl)
	{
		this.hndl = hndl;
		col = GetComponent<GUITexture>().color;
	}
	
	public void Fade()
	{
		if(fading) return;
		GetComponent<GUITexture>().enabled = true;
		fading = true;
		enabled = true;
	}
	
	public void Update()
	{
		col.a += 0.01f;
		GetComponent<GUITexture>().color = col;
		if(col.a >= 1.0f)
		{
			enabled = false;
			hndl();
		}
	}
}