using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {
	
	Vector3 tmp;
	Transform mainobj;
	Vector3 def_pos;
	Vector3[] move_vector = new Vector3[2];
	public float limit = 0.1f;
	public float speed = 0.01f;
	public float speed2 = 0.02f;
	float distance = 0.0f;
	bool ismove = false;
	bool moved = false;
	bool isdown = false;
	float tmpy = 0.0f;
	float[] limits = new float[2];
	
	void Start()
	{
		mainobj = GameObject.Find("Player").transform;
		def_pos = transform.localPosition;
		move_vector[0] = new Vector3(0,speed,0);
		move_vector[1] = new Vector3(0,speed2,0);
		limits[0] = def_pos.y+limit;
		limits[1] = def_pos.y-limit;
	}
	
	void FixedUpdate()
	{
		ismove = Input.GetButton("Horizontal") || Input.GetButton("Vertical");
		distance = Vector3.Distance(tmp,mainobj.localPosition);
		if(ismove)
		{
			if(distance > 0.25f)
			{
				transform.localPosition += isdown ? -move_vector[1] : move_vector[1];
				DoBob();
			}
			else if(distance > 0.05f)
			{
				transform.localPosition += isdown ? -move_vector[0] : move_vector[0];
				DoBob();
			}
			moved = true;
		}
		else if(moved)
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition,def_pos,0.005f);
			if(transform.localPosition == def_pos)
				moved = false;
		}
		tmp = mainobj.localPosition;
	}
	
	void DoBob()
	{
		tmpy = transform.localPosition.y;
		if(tmpy > limits[0]) isdown = true;
		else if(tmpy < limits[1]) isdown = false;
	}
}