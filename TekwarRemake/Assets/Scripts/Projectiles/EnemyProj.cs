using UnityEngine;
using System.Collections;

public class EnemyProj : ProjObj
{
	RaycastHit hit;
	
	protected override void ProjFixedUpdate()
	{
		if(Physics.SphereCast(transform.position,0.2f,transform.forward,out hit,2.0f))
		{
			if(hit.transform == GameBase.inst.player_tr)
			{
				MainScript.inst.Damage(Random.Range(2, 8));
			}
			Destroy(gameObject);
		}
	}
}