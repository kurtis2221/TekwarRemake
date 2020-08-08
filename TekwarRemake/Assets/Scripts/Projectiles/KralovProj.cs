using UnityEngine;
using System.Collections;

public class KralovProj : ProjObj
{
	public Object obj;
	public Object obj2;
	
	RaycastHit hit;
	
	protected override void ProjFixedUpdate()
	{
		if(Physics.SphereCast(transform.position,0.2f,transform.forward,out hit,2.0f,GameBase.WEAPON_RAY))
		{
			bool pl = WeaponScript.inst.CheckHit(ref hit, false, 0);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GameObject.Instantiate(pl ? obj2 : obj, transform.position, default(Quaternion));
			Destroy(gameObject);
		}
	}
}