using UnityEngine;
using System.Collections;

public class VehWpGizmo : MonoBehaviour
{
	void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(transform.position,transform.right);
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position,new Vector3(1,1,1));
	}
}