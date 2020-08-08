using UnityEngine;
using System.Collections;

public class WeaponSway : MonoBehaviour
{
	float sway_x, sway_y;
	Vector3 default_pos;
	Vector3 move_pos;
	
	void Awake()
	{
		default_pos = transform.localPosition;
	}

	void Update()
	{
		sway_x = Mathf.Clamp(Input.GetAxis("Mouse X") * 0.2f, -0.2f, 0.2f);
		sway_y = Mathf.Clamp(Input.GetAxis("Mouse Y") * 0.2f, -0.2f, 0.2f);
		move_pos = new Vector3(default_pos.x+sway_x,default_pos.y+sway_y,default_pos.z);
		transform.localPosition = Vector3.Lerp(transform.localPosition,move_pos,2.0f*Time.deltaTime);
	}
}