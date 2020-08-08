using UnityEngine;
using System.Collections;

public class LODScript : MonoBehaviour
{
	public static LODScript inst;
	
	public LodObject[] objs;
	bool inrange;
	
	void Awake()
	{
		inst = this;
	}
	
	void FixedUpdate()
	{
		Vector3 pos = GameBase.inst.player_tr.position;
		foreach(LodObject obj in objs)
		{
			inrange = Vector3.Distance(pos,obj.model.transform.position) < 64.0f;
			if(obj.stat && inrange)
			{
				obj.stat = false;
				obj.Action();
			}
			else if(!obj.stat && !inrange)
			{
				obj.stat = true;
				obj.Action();
			}
		}
	}
	
	public void Recalc()
	{
		foreach(LodObject obj in objs)
		{
			obj.stat = false;
			obj.Action();
		}
	}
}