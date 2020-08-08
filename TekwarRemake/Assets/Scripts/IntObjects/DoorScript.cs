using UnityEngine;
using System.Collections;

public class DoorScript : DoorScriptBase
{	
	Transform door;
	public float move_height = 5;
	public float speed = 0.05f;
	
	Vector3 movement;
	Vector3 opened_pos, closed_pos;
	
	public AudioClip open_snd;
	public AudioClip close_snd;
	public AudioClip extra_snd;
		
	void Awake()
	{
		door = transform.GetChild(0);
		movement = new Vector3(0,speed,0);
		closed_pos = door.transform.localPosition;
		opened_pos = closed_pos + new Vector3(0,move_height,0);
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
		}
	}
	
	void FixedUpdate()
	{
		if(state)
		{
			if(door.transform.localPosition.y >= closed_pos.y)
				door.transform.localPosition -= movement;
			else
			{
				if(extra_snd != null)
				{
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().PlayOneShot(extra_snd);
				}
				door.transform.localPosition = closed_pos;
				state = false;
				enabled = false;
			}
		}
		else
		{
			if(door.transform.localPosition.y <= opened_pos.y)
				door.transform.localPosition += movement;
			else
			{
				if(extra_snd != null)
				{
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().PlayOneShot(extra_snd);
				}
				door.transform.localPosition = opened_pos;
				state = true;
				enabled = false;
			}
		}
	}
}