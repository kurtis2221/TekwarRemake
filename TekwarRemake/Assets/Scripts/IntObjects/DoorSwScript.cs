using UnityEngine;
using System.Collections;

public class DoorSwScript : DoorScriptBase
{
	public DoorSwScript door2;
	
	public Vector3 move_rot;
	public float speed = 1.0f;
	Quaternion close_state;
	Quaternion open_state;
	
	public AudioClip open_snd;
	public AudioClip close_snd;
	
	void Awake()
	{
		close_state = transform.localRotation;
		open_state = close_state * Quaternion.Euler(move_rot);
	}
	
	public override void Action()
	{
		if(!enabled)
		{
			if(state)
				GetComponent<AudioSource>().PlayOneShot(close_snd);
			else
				GetComponent<AudioSource>().PlayOneShot(open_snd);
			enabled = true;
			if(door2 != null)
				door2.enabled = true;
		}
	}
	
	void FixedUpdate()
	{
		if(state)
		{
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, close_state, speed);
			if(transform.localRotation == close_state)
			{
				state = false;
				enabled = false;
			}
		}
		else
		{
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, open_state, speed);
			if(transform.localRotation == open_state)
			{
				state = true;
				enabled = false;
			}
		}
	}
}