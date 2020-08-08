using UnityEngine;
using System.Collections;

public class BossNPC : EnemyNPC
{
	protected override void SetupNPC()
	{
		hweapon = true;
		can_flee = false;
		health = 50;
		cosinous = 50;
		score = 1000;
		st_score = 2000;
	}
	
	public override void Death()
	{
		MainScript.inst.MissionComplete();
		base.Death();
	}
	
	public override void Stunned()
	{
		MainScript.inst.MissionComplete();
		base.Stunned();
	}
}