using UnityEngine;
using System.Collections.Generic;

public class GrassStream : MonoBehaviour
{
	public Transform[] objs;
	List<Renderer> grass;
	
	void Awake()
	{
		grass = new List<Renderer>();
		foreach(Transform o in objs)
			grass.AddRange(o.GetComponentsInChildren<Renderer>());
	}
	
	void FixedUpdate()
	{
		Vector3 pos = GameBase.inst.player_tr.transform.position;
		foreach(Renderer r in grass)
			r.enabled = Vector3.Distance(r.transform.position,pos) < 48;
	}
}