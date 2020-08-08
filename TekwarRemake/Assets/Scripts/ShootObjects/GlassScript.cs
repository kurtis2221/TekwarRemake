using UnityEngine;
using System.Collections;

public class GlassScript : ShootObj
{
	protected override void Action()
	{
		GameObject tmp = (GameObject)GameObject.Instantiate(GameBase.inst.pr_glass,transform.position,transform.rotation);
		tmp.transform.localScale = transform.localScale;
		tmp.transform.parent = transform.parent;
		Destroy(gameObject);
	}
}