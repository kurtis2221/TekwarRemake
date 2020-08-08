using UnityEngine;
using System.Collections.Generic;

public abstract class IntObj : MonoBehaviour
{
	public abstract void Action();
}

public class DoorScriptBase : IntObj
{
	[HideInInspector]
	public bool state = false;
	
	public override void Action() { }
}

public abstract class ShootObj : MonoBehaviour
{
	[HideInInspector]
	public int health = 1;
	
	public void Damage(int input)
	{
		health -= input;
		if(health <= 0)
		{
			health = 0;
			Action();
		}
	}
	
	protected abstract void Action();
}

public abstract class ExplObj : ShootObj
{
	public bool marked = false;
}

public abstract class ProjObj : MonoBehaviour
{
	const int WAIT_TIME = 300;
	int waittime;
	
	void Awake()
	{
		waittime = WAIT_TIME;
	}
	
	void FixedUpdate()
	{
		ProjFixedUpdate();
		waittime--;
		if(waittime <= 0) Destroy(gameObject);
	}
	
	protected abstract void ProjFixedUpdate();
}

public abstract class PlayerObj : MonoBehaviour
{
	public enum Anims
	{
		IDLE,
		WALK,
		RUN,
		FIRE,
		HIT,
		DEATH,
		DEATH2,
		SPEC,
		INVALID = -1
	}
	
	public const int MAX_HEALTH = 100;
	
	[HideInInspector]
	public int health;
	[HideInInspector]
	public int cosinous;
	[HideInInspector]
	public int score;
	[HideInInspector]
	public int st_score;
	
	public string walk_anim;
	public string run_anim;
	public string fire_anim;
	public string spec_anim;
	public Animation anim_obj;
	
	Anims curr_anim;
	string[] anim_arr =
	{
		"idle",
		"walk",
		"run",
		"fire",
		"hit",
		"death",
		"death2",
		"cower"
	};
	
	public virtual bool Damage(int input)
	{
		DoAnim(PlayerObj.Anims.HIT, true);
		health -= input;
		if(health <= 0)
		{
			health = 0;
			MainScript.inst.AddScore(score);
			Death();
		}
		else Damage2(input);
		return true;
	}
	
	public void Stun(int input)
	{
		DoAnim(PlayerObj.Anims.HIT, true);
		cosinous -= input;
		if(cosinous <= 0)
		{
			cosinous = 0;
			MainScript.inst.AddScore(st_score);
			Stunned();
		}
		else Stun2(input);
	}
	
	public abstract void Damage2(int input);
	public abstract void Stun2(int input);
	public abstract void Death();
	public abstract void Stunned();
	
	protected void SetupAnimation()
	{
		if(walk_anim != string.Empty) anim_arr[(int)Anims.WALK] = walk_anim;
		if(run_anim != string.Empty) anim_arr[(int)Anims.RUN] = run_anim;
		if(fire_anim != string.Empty) anim_arr[(int)Anims.FIRE] = fire_anim;
		if(spec_anim != string.Empty) anim_arr[(int)Anims.SPEC] = spec_anim;
		anim_obj[anim_arr[(int)Anims.FIRE]].layer = 1;
		anim_obj[anim_arr[(int)Anims.HIT]].layer = 1;
	}
	
	protected void ResetAnim()
	{
		curr_anim = Anims.INVALID;
	}
	
	public void DoAnim(Anims anim, bool ignore = false, bool inst = false)
	{
		if(anim == curr_anim) return;
		if(!ignore) curr_anim = anim;
		if(inst) anim_obj.Play(anim_arr[(int)anim]);
		else anim_obj.CrossFade(anim_arr[(int)anim]);
	}
}

[System.Serializable]
public class LodObject
{
	public GameObject model;
	public GameObject lod;
	public bool stat;
	
	public void Action()
	{
		model.SetActiveRecursively(!stat);
		lod.SetActiveRecursively(stat);
	}
}