using UnityEngine;
using System.Collections;

public class CivilNPC : NPCBase
{
	protected override void SetupNPC()
	{
		hweapon = false;
		can_flee = true;
		health = 10;
		cosinous = 10;
		score = -500;
		st_score = 0;
	}
	
	public override void Death()
	{
		GameBase.inst.civ_cas = true;
		base.Death();
	}
}