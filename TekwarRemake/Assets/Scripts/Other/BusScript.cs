using UnityEngine;
using System.Collections;
using System;

public class BusScript : MonoBehaviour
{
	public Transform[] path;
	int curr;
	
	void FixedUpdate ()
	{
		transform.position = Vector3.MoveTowards(transform.position,path[curr].position,0.2f);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,path[curr].rotation,1f);
		if(Vector3.Distance(transform.position,path[curr].position) < 0.1f)
		{
			if(curr == path.Length-1)
				curr = 0;
			else
				curr++;
		}
	}
}