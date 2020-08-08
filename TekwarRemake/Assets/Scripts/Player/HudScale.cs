using UnityEngine;
using System.Collections;

public class HudScale : MonoBehaviour
{
	void Awake()
	{
		int height = Screen.height;
		foreach(GUIText g in GetComponentsInChildren<GUIText>())
		{
			g.fontSize = (int)(g.fontSize/768f*height);
			g.pixelOffset = new Vector2(g.pixelOffset.x/768f*height,g.pixelOffset.y/768f*height);
		}
		foreach(GUITexture g in GetComponentsInChildren<GUITexture>())
			g.pixelInset = new Rect(
				g.pixelInset.x/768f*height,
				g.pixelInset.y/768f*height,
				g.pixelInset.width/768f*height,
				g.pixelInset.height/768f*height);
	}
}