using UnityEngine;
using System.Collections;

public class PoliceNPC : NPCBase
{
	protected override void SetupNPC()
	{
		hweapon = true;
		can_flee = false;
		health = 50;
		cosinous = 50;
		score = -500;
		st_score = 0;
	}
	
	public override void Death()
	{
		GameBase.inst.civ_cas = true;
		base.Death();
	}
}