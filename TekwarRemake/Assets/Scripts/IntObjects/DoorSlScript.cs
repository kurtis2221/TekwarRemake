using UnityEngine;
using System.Collections;

public class DoorSlScript : DoorScriptBase
{	
	Transform door;
	public Transform door2;
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
		movement = door.forward * speed;
		closed_pos = door.localPosition;
		opened_pos = closed_pos + door.forward * move_height;
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
			if(Vector3.Distance(door.transform.localPosition,closed_pos) > 0.1f)
			{
				door.transform.localPosition -= movement;
				if(door2 != null)
					door2.transform.localPosition += movement;
			}
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
			if(Vector3.Distance(door.transform.localPosition,opened_pos) > 0.1f)
			{
				door.transform.localPosition += movement;
				if(door2 != null)
					door2.transform.localPosition -= movement;
			}
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