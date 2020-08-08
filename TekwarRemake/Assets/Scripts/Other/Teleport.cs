using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
	public bool inside;
	public Bounds zone;
	Transform teleport;
	
	void Awake()
	{
		inside = false;
		zone.center = transform.position;
		if(transform.childCount > 0)
			teleport = transform.GetChild(0);
		else
			teleport = transform.parent;
	}
	
	public bool Trigger(bool input)
	{
		if(inside == input) return false;
		inside = input;
		if(!input) return false;
		teleport.GetComponent<Teleport>().inside = input;
		GameBase.inst.player_tr.transform.position = teleport.position;
		GameBase.inst.player_tr.GetComponent<FPSWalkerEnhanced>().ResetFall();
		return true;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position,zone.size);
	}
}