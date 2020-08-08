using UnityEngine;
using System.Collections;

public class MetroScript : MonoBehaviour {
	
	public Transform left_doors;
	public Transform right_doors;
	public float speed;
	public float door_speed;
	public float wait;
	public float door_time;
	public Transform[] stations;
	
	bool iswaiting = false;
	bool door_wait = false;
	bool ddir = false;
	bool cdir = false;
	bool slowing = false;
	int current = 0;
	
	public AudioClip metro_start, metro_stop;
	public AudioClip door_snd;
	
	bool tmr_wait, tmr_wait2;
	float tmr_time;
	
	void FixedUpdate()
	{
		if(!iswaiting)
		{
			if(Vector3.Distance(transform.position,stations[current].position) > 0.5f)
			{
				transform.position += new Vector3((cdir) ? speed : -speed,0,0);
				if(!slowing && Vector3.Distance(transform.position,stations[current].position) < 16f)
				{
					slowing = true;
					GetComponent<AudioSource>().PlayOneShot(metro_stop);
				}
			}
			else
			{
				transform.position = stations[current].position;
				
				current += (cdir) ? -1 : 1;		
				if(current == stations.Length)
				{
					cdir = true;
					current = stations.Length-2;
				}
				else if(current == -1)
				{
					cdir = false;
					current = 1;
				}
				iswaiting = true;
				ddir = true;
				GetComponent<AudioSource>().PlayOneShot(door_snd);
			}
		}
		else if(!door_wait)
		{
			left_doors.localPosition += new Vector3(
					(ddir) ? -door_speed : door_speed,
					0,
					0);
			right_doors.localPosition += new Vector3(
					(ddir) ? door_speed : -door_speed,
					0,
					0);
			
			if(left_doors.localPosition.x < -1.5f)
			{
				door_wait = true;
				ddir = false;
				left_doors.localPosition = new Vector3(-1.5f,0,0);
				right_doors.localPosition = new Vector3(1.5f,0,0);
				tmr_wait2 = true;
			}
			
			if(ddir == false)
			{
				if(left_doors.localPosition.x > 0)
				{
					door_wait = true;
					left_doors.localPosition = new Vector3(0,0,0);
					right_doors.localPosition = new Vector3(0,0,0);
					slowing = false;
					tmr_wait = true;
				}
			}
		}
		if(tmr_wait)
		{
			tmr_time += Time.fixedDeltaTime;
			if(tmr_time >= wait)
			{
				iswaiting = false;
				door_wait = false;
				GetComponent<AudioSource>().PlayOneShot(metro_start);
				tmr_time = 0;
				tmr_wait = false;
			}
		}
		else if(tmr_wait2)
		{
			tmr_time += Time.fixedDeltaTime;
			if(tmr_time >= door_time)
			{
				door_wait = false;
				GetComponent<AudioSource>().PlayOneShot(door_snd);
				tmr_time = 0;
				tmr_wait2 = false;
			}
		}
	}
}