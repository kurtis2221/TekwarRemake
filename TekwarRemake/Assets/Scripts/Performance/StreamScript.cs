using UnityEngine;
using System.Collections;

public class StreamScript : MonoBehaviour
{
	public ZBound[] zones;
	public ZBound[] mirrors;
	public WarpScript[] warps;
	public Teleport[] teleports;
	
	void FixedUpdate()
	{
		Vector3 pos = GameBase.inst.player_tr.position;
		foreach(ZBound z in zones)
			z.EnableObjs(z.zone.Contains(pos));
		foreach(ZBound z in mirrors)
			z.EnableObjs(z.zone.Contains(pos));
		foreach(WarpScript w in warps)
			if(Vector3.Distance(w.transform.position,pos) < w.radius)
				w.Trigger();
		//Dynamic checking
		foreach(Teleport t in teleports)
			if(t.Trigger(t.zone.Contains(pos))) break;
	}
}