using UnityEngine;
using System.Collections;

public class ZBound : MonoBehaviour
{
	bool inside;
	public Bounds zone;
	public GameObject[] objs;
	public bool subway;
	
	void Awake()
	{
		inside = true;
		zone.center = transform.position;
	}
	
	public void EnableObjs(bool input)
	{
		if(inside == input) return;
		inside = input;
		foreach(GameObject o in objs)
			o.SetActiveRecursively(input);
		if(inside && subway)
			LODScript.inst.Recalc();
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position,zone.size);
	}
}