using UnityEngine;
using System.Collections;

public class HudFadeIO : MonoBehaviour
{
	Color col;
	bool fading;
	bool back;
	
	public void Awake()
	{
		col = GetComponent<GUITexture>().color;
		back = false;
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
		col.a += back ? -0.01f : 0.01f;
		GetComponent<GUITexture>().color = col;
		if(!back && col.a >= 1.0f)
		{
			back = true;
		}
		else if(back && col.a <= 0.0f)
		{
			enabled = false;
			back = false;
			fading = false;
			GetComponent<GUITexture>().enabled = false;
		}
	}
}