using UnityEngine;
using System.Collections;

public class HudControl : MonoBehaviour
{
	public enum HudEnum
	{
		Health,
		Cosinousness,
		Ammo
	}
	
	public GUITexture[] hud_element;
	float max;
	
	void Start()
	{
		max = hud_element[0].pixelInset.width;
	}

	public void ChangeData(HudEnum id, float val)
	{
		int i = (int)id;
		hud_element[i].pixelInset = new Rect(
			hud_element[i].pixelInset.x,
			hud_element[i].pixelInset.y,
			max * val,
			hud_element[i].pixelInset.height);
	}
}