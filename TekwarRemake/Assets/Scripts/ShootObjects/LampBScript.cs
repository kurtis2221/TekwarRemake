using UnityEngine;
using System.Collections;

public class LampBScript : ShootObj
{
	protected override void Action()
	{
		GameObject.Instantiate(GameBase.inst.pr_lampb,transform.position,transform.rotation);
		Destroy(gameObject);
	}
}