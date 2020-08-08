using UnityEngine;
using System.Collections.Generic;

public class ExplObjScript : ExplObj
{	
	protected float radius;
	protected float radius2;
	protected int dam_min;
	protected int dam_max;
	RaycastHit hit;
	
	void Awake()
	{
		radius = 2.0f;
		radius2 = 5.0f;
		dam_min = 5;
		dam_max = 10;
	}
	
	protected override void Action()
	{
		DoExpl(this);
		GameObject.Instantiate(GameBase.inst.pr_expl_snd,transform.position,transform.rotation);
	}
	
	void DoExpl(ExplObj obj)
	{
		obj.marked = true;
		Vector3 pos = obj.transform.position;
		foreach(Collider col in Physics.OverlapSphere(pos,radius2))
		{
			PlayerObj tmp = col.GetComponent<PlayerObj>();
			if(tmp != null)
			{
				Transform pl_tr = tmp.transform;
				Vector3 pl_pos = tmp.transform.position;
				if(Physics.Linecast(pos,pl_pos,out hit,GameBase.EXPL_RAY))
				{
					if(hit.transform == pl_tr)
					{
						tmp.Damage(Vector3.Distance(pos,pl_pos) < radius ? dam_max : dam_min);	
					}
				}
			}
			ExplObjScript tmp2 = col.GetComponent<ExplObjScript>();
			if(tmp2 != null && !tmp2.marked)
			{
				Transform pl_tr = tmp2.transform;
				Vector3 pl_pos = tmp2.transform.position;
				if(Physics.Linecast(pos,pl_pos,out hit))
				{
					if(hit.transform == pl_tr)
					{
						DoExpl(tmp2);
					}
				}
			}
		}
		GameObject.Instantiate(GameBase.inst.pr_expl,pos,obj.transform.rotation);
		Destroy(obj.gameObject);
	}
}