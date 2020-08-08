using UnityEngine;
using System.Collections;

public class CharWp : MonoBehaviour
{
	public enum PathType : int
	{
		Civil,
		Police,
		Enemy,
		Combined
	}
	
	static Color[] cols =
	{
		new Color(0.0f,1.0f,0.0f),
		new Color(0.0f,0.0f,1.0f),
		new Color(1.0f,0.5f,0.0f),
		new Color(1.0f,1.0f,0.0f)
	};
	
	static Vector3 charsize = new Vector3(1,2,1);
	
	public PathType path_type;
	
	void OnDrawGizmos()
	{
		Gizmos.color = cols[(int)path_type];
		Gizmos.DrawCube(transform.position,charsize);
	}
}