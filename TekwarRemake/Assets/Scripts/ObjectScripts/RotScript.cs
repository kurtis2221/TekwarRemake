using UnityEngine;
using System.Collections;

public class RotScript : MonoBehaviour
{
	public Vector3 axis = Vector3.up;
	public float speed = 1.0f;
	
	void FixedUpdate()
	{
		transform.RotateAroundLocal(axis,speed);
	}
}