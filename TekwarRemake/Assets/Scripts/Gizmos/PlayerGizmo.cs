using UnityEngine;
using System.Collections;

public class PlayerGizmo : MonoBehaviour
{
	static Vector3 charsize = new Vector3(1,2,1);
	
	void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position,transform.forward);
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position,charsize);
	}
}