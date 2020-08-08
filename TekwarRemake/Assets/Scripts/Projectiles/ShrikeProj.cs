using UnityEngine;
using System.Collections;

public class ShrikeProj : ProjObj
{
	static Vector3 grow = new Vector3(4f,4f,4f);
	
	bool dest = false;
	RaycastHit hit;
	
	protected override void ProjFixedUpdate()
	{
		if(dest)
		{
			transform.localScale = Vector3.MoveTowards(transform.localScale,grow,0.5f);
			if(transform.localScale == grow)
				Destroy(gameObject);
			return;
		}
		if(Physics.SphereCast(transform.position,0.3f,transform.forward,out hit,2.0f,GameBase.WEAPON_RAY))
		{
			WeaponScript.inst.CheckHit(ref hit, false, 2);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			dest = true;
		}
	}
}