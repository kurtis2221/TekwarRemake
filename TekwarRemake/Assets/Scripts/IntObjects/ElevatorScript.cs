using UnityEngine;
using System.Collections;

public class ElevatorScript : IntObj {
	
	public Transform door;
	public float move_height = 5;
	public float speed = 0.05f;
	
	Vector3 movement;
	Vector3 opened_pos, closed_pos;
	bool state = false;
	
	public AudioClip move_snd;
	public AudioClip end_snd;
		
	void Awake()
	{
		movement = new Vector3(0,0,speed);
		closed_pos = door.transform.localPosition;
		opened_pos = closed_pos + new Vector3(0,0,move_height);
	}
	
	public override void Action()
	{
		if(!enabled)
		{
			GetComponent<AudioSource>().PlayOneShot(move_snd);
			enabled = true;
		}
	}
	
	void FixedUpdate()
	{
		if(state)
		{
			if(door.transform.localPosition.z <= closed_pos.z)
				door.transform.localPosition -= movement;
			else
			{
				if(end_snd != null)
				{
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().PlayOneShot(end_snd);
				}
				door.transform.localPosition = closed_pos;
				state = false;
				enabled = false;
			}
		}
		else
		{
			if(door.transform.localPosition.z >= opened_pos.z)
				door.transform.localPosition += movement;
			else
			{
				if(end_snd != null)
				{
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().PlayOneShot(end_snd);
				}
				door.transform.localPosition = opened_pos;
				state = true;
				enabled = false;
			}
		}
	}
}