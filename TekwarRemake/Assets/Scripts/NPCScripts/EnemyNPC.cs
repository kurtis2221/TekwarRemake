using UnityEngine;
using System.Collections;

public class EnemyNPC : NPCBase
{
	protected override void SetupNPC()
	{
		hweapon = true;
		can_flee = false;
		health = 50;
		cosinous = 50;
		score = 600;
		st_score = 1200;
	}
}