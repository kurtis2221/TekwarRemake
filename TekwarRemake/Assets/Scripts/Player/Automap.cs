using UnityEngine;
using System.Collections;

public class Automap : MonoBehaviour
{
	public Camera cam;
	public Transform arrow;
	bool map = false;
	
	void FixedUpdate()
	{
		Vector3 pos = GameBase.inst.player_tr.position;
		Vector3 rot = GameBase.inst.player_tr.rotation.eulerAngles;
		pos.y = 201f;
		arrow.position = pos;
		arrow.rotation = Quaternion.Euler(270f,rot.y,0f);
	}
	
	public void ActivateAutomap()
	{
		map = !map;
		enabled = map;
		cam.enabled = map;
	}
	
	public void Zoom(float input)
	{
		if(map)
		{
			if(input < 0)
			{
				if(cam.orthographicSize > 20)
				{
					cam.orthographicSize += input;
				}
			}
			else if(cam.orthographicSize < 100)
			{
				cam.orthographicSize += input;
			}
		}
	}
}