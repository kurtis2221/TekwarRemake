using UnityEngine;
using System.Collections;

public class MetroWpGizmo : MonoBehaviour
{
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position+new Vector3(4,0,0),new Vector3(37,4,8));
	}
}