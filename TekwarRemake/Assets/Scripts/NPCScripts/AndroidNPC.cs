using System;
using UnityEngine;

public class AndroidNPC : NPCBase
{
	public Renderer rend;
	
	const float radius = 5.0f;
	const int SND_WAIT = 50;
	const float EX_DIST = 5;
	
	int snd_wait;
	bool marked;
	
	protected override void SetupNPC()
	{
		hweapon = false;
		player_dist = EX_DIST;
		can_flee = false;
		health = 50;
		score = 1200;
		snd_wait = 0;
	}
	
	protected override void NPCFleeUpdate()
	{
		if(player_dist < EX_DIST) Death();
		if(snd_wait <= 0)
		{
			snd_wait = SND_WAIT;
			GetComponent<AudioSource>().PlayOneShot(shoot_snd);
		}
		snd_wait--;
	}
	
	public override void Death()
	{
		if(marked) return;
		enabled = false;
		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		GetComponent<Collider>().enabled = false;
		rend.enabled = false;
		marked = true;
		GameBase.inst.ShowMsg("Was an android");
		GameObject.Instantiate(GameBase.inst.pr_aexpl,transform.position,transform.rotation);
		foreach(Collider col in Physics.OverlapSphere(transform.position,radius))
		{
			PlayerObj tmp = col.GetComponent<PlayerObj>();
			if(tmp == this) continue;
			if(tmp != null) tmp.Damage(30);
		}
	}
}