using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NPCBase : PlayerObj
{
	public enum NpcAlert
	{
		CivilM,
		CivilW,
		BadM,
		BadW,
		CopM,
		CopM2,
		CivilM2,
		CivilW2,
		None = -1
	}
	
	public enum NpcDeath
	{
		Man,
		Woman,
		None = -1
	}
	
	static List<NPCBase> npc_list = new List<NPCBase>();
	static string[] alert_folders =
	{
		"ScreamM",
		"ScreamW",
		"ScreamBM",
		"ScreamBW",
		"ScreamCM",
		"ScreamCM2",
		"ScreamM2",
		"ScreamW2"
	};
	static AudioClip[][] alert_coll;
	static string[] death_folders =
	{
		"DeathM",
		"DeathW"
	};
	static AudioClip[][] death_coll;
	
	public bool hostile = false;
	public bool nochase = false;
	public bool stayput = false;
	public bool proj_shot = false;
	public Transform proj_point;
	public Transform normal_wp;
	public AudioClip shoot_snd;
	public Renderer muzzle;
	
	public NpcAlert alert_typ;
	public NpcDeath death_typ;
	
	protected bool can_flee;
	protected bool hweapon;
	protected float player_dist;
	
	const float SP_WALK = 2;
	const float SP_RUN = 5;
	const int WAIT_TIME_MIN = 500;
	const int WAIT_TIME_MAX = 1000;
	const int WAIT_SC_MIN = 500;
	const int WAIT_SC_MAX = 2000;
	const int WAIT_SH_MIN = 15;
	const int WAIT_SH_MAX = 30;
	const int MIN_ALERT = 1000;
	const int MAX_ALERT = 3000;
	const float LOS_SHOOT = 20;
	const float LOS_ALERT = 60;
	const float AL_DIST = 40;
	const float DS_DIST = 2;
	const float CH_DIST = 4;
	
	CharWp[] wp_normal;
	
	AudioClip[] alert_snds;
	AudioClip[] death_snds;

	UnityEngine.AI.NavMeshAgent nav_npc;
	bool flee;
	int alert;
	
	int waittime = 0;
	int waittime_sc = 0;
	int waittime_sh = 0;
	
	RaycastHit hit;
	
	void Awake()
	{
		if(alert_coll == null)
		{
			alert_coll = new AudioClip[alert_folders.Length][];
			for(int i = 0; i < alert_folders.Length; i++)
			{
				alert_coll[i] = System.Array.ConvertAll(Resources.LoadAll(alert_folders[i], typeof(AudioClip)),x => (AudioClip)x);
			}
			death_coll = new AudioClip[death_folders.Length][];
			for(int i = 0; i < death_folders.Length; i++)
			{
				death_coll[i] = System.Array.ConvertAll(Resources.LoadAll(death_folders[i], typeof(AudioClip)),x => (AudioClip)x);
			}
		}
		SetupNPC();
		SetupAnimation();
		flee = false;
		alert = 0;
		nav_npc = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if(alert_typ != NpcAlert.None) alert_snds = alert_coll[(int)alert_typ];
		if(death_typ != NpcDeath.None) death_snds = death_coll[(int)death_typ];
		if(!stayput) wp_normal = normal_wp.GetComponentsInChildren<CharWp>();
		MoveNextPosition();
	}
	
	void Start()
	{
		npc_list.Add(this);
	}
	
	void OnEnable()
	{
		if(hweapon)
		{
			GetComponent<Light>().enabled = false;
			muzzle.enabled = false;
		}
		ResetAnim();
	}
	
	void FixedUpdate()
	{
		Transform player = GameBase.inst.player_tr;
		Vector3 pos = transform.position;
		Vector3 proj_pos = proj_point.position;
		Vector3 player_pos = player.position;
		Vector3 player_pos2 = player_pos;
		player_pos2.y = pos.y;
		player_dist = Vector3.Distance(pos,player_pos);
		bool player_close;
		nav_npc.speed = flee ? SP_RUN : SP_WALK;
		
		//Alert decrease
		if(alert > 0)
		{
			alert--;
			if(alert <= 0) SetFlee(false);
		}
		
		//When hostile or weapon drawn check player is in sights
		if(!flee)
		{
			if(hostile || WeaponScript.inst.weapon_drawn)
			{
				if(player_dist < AL_DIST)
				{
					float angle = Vector3.Angle(player_pos2 - pos, transform.forward);
					if(angle < LOS_ALERT)
					{
						if(Physics.Linecast(proj_pos, player_pos, out hit))
						{
							if(hit.transform == player)
							{
								SetFlee(true);
							}
						}
					}
				}
			}
		}
		
		//Flee handling
		if(flee)
		{
			if(player_dist < AL_DIST)
			{
				if(Physics.Linecast(proj_pos, player_pos, out hit))
				{
					if(hit.transform == player)
					{
						SetFlee(true);
						if(!can_flee && !nochase)
						{
							nav_npc.destination = GameBase.inst.player_tr.position;
							player_close = player_dist < CH_DIST;
							if(player_close)
							{
								nav_npc.speed = 0;
								player_pos.y = pos.y;
								transform.LookAt(player_pos);
							}
							else
							{
								nav_npc.destination = player_pos;
							}
						}
						else if(nochase)
						{
							player_pos.y = pos.y;
							transform.LookAt(player_pos);
						}
						//Scream
						if(waittime_sc == 0)
						{
							waittime_sc = Random.Range(WAIT_SC_MIN, WAIT_SC_MAX);
							if(alert_typ != NpcAlert.None)
								GetComponent<AudioSource>().PlayOneShot(alert_snds[Random.Range(0,alert_snds.Length)]);
						}
						if(hweapon)
						{
							//Shoot waittime
							if(waittime_sh > 0) waittime_sh--;
							else
							{
								float angle = Vector3.Angle(player_pos2 - pos, transform.forward);
								if(angle < LOS_SHOOT)
								{
									waittime_sh = Random.Range(WAIT_SH_MIN, WAIT_SH_MAX);
									GetComponent<AudioSource>().PlayOneShot(shoot_snd);
									GetComponent<Light>().enabled = true;
									muzzle.enabled = true;
									StartCoroutine(ResetFire());
									DoAnim(PlayerObj.Anims.FIRE, true, true);
									if(proj_shot)
									{
										proj_point.LookAt(player_pos);
										((GameObject)GameObject.Instantiate(GameBase.inst.pr_enemy,
											proj_point.position,proj_point.rotation)).GetComponent<Rigidbody>().velocity =
											proj_point.forward * 32.0f;
									}
									else if(Random.Range(0, 2) == 0)
										MainScript.inst.Damage(Random.Range(2, 8));
									CheckAlert(transform.position, 20, 50);
								}
							}
						}
					}
				}
			}
			foreach(Collider col in Physics.OverlapSphere(pos,0.4f))
			{
				DoorScriptBase tmp = col.GetComponent<DoorScriptBase>();
				if(tmp != null && !tmp.state)
				{
					tmp.Action();
				}
			}
			NPCFleeUpdate();
		}
		
		//Scream waittime
		if(waittime_sc > 0) waittime_sc--;
		//Idle waittime
		if(!flee && waittime > 0)
		{
			waittime--;
			if(waittime <= 0) MoveNextPosition();
		}
		else if(Vector3.Distance(pos,nav_npc.destination) < DS_DIST)
		{
			if(!flee && Random.Range(0,4) == 0)
			{
				waittime = Random.Range(WAIT_TIME_MIN, WAIT_TIME_MAX);
			}
			else MoveNextPosition();
		}
		
		//Animation
		if(!nochase)
		{
			if(stayput && can_flee)
			{
				if(flee) DoAnim(PlayerObj.Anims.SPEC);
				else DoAnim(PlayerObj.Anims.IDLE);
			}
			else
			{
				if(nav_npc.velocity.magnitude > 0)
				{
					if(nav_npc.speed > SP_WALK) DoAnim(PlayerObj.Anims.RUN);
					else DoAnim(PlayerObj.Anims.WALK);
				}
				else DoAnim(PlayerObj.Anims.IDLE);
			}
		}
		else DoAnim(PlayerObj.Anims.IDLE);
	}
	
	void SetFlee(bool input)
	{
		if(flee == input) return;
		if(!flee && input)
		{
			waittime_sc = 0;
			waittime_sh = Random.Range(WAIT_SH_MIN, WAIT_SH_MAX);
		}
		flee = input;
		alert = input ? MAX_ALERT : 0;
		waittime = 0;
	}
	
	protected void MoveNextPosition()
	{
		if(stayput) return;
		nav_npc.destination = wp_normal[Random.Range(0,wp_normal.Length)].transform.position;
	}
	
	void CheckAlertDist(Vector3 pos, float min, float max)
	{
		if(flee || !gameObject.active) return;
		
		float dist = Vector3.Distance(transform.position, pos);
		if(dist < min) SetFlee(true);
		else if(dist < max)
		{
			if(alert >= MAX_ALERT) SetFlee(true);
			else alert += MIN_ALERT;
		}
	}
	
	public static void CheckAlert(Vector3 pos, float min, float max)
	{
		foreach(NPCBase n in npc_list)
		{
			if(n.enabled) n.CheckAlertDist(pos, min, max);
		}
	}
	
	public static void ClearNPCList()
	{
		npc_list.Clear();
	}
	
	public override void Damage2(int input)
	{
		HandleHit();
	}
	
	public override void Stun2(int input)
	{
		HandleHit();
	}
	
	public override void Death()
	{
		DisableNPC();
		DoAnim(Random.Range(0, 2) == 0 ? PlayerObj.Anims.DEATH : PlayerObj.Anims.DEATH2);
	}
	
	public override void Stunned()
	{
		DisableNPC();
		DoAnim(Random.Range(0, 2) == 0 ? PlayerObj.Anims.DEATH : PlayerObj.Anims.DEATH2);
	}
	
	void HandleHit()
	{
		waittime_sh = Random.Range(WAIT_SH_MIN, WAIT_SH_MAX);
		SetFlee(true);
	}
	
	void DisableNPC()
	{
		if(death_typ != NpcDeath.None)
			GetComponent<AudioSource>().PlayOneShot(death_snds[Random.Range(0,death_snds.Length)]);
		enabled = false;
		nav_npc.enabled = false;
		GetComponent<Collider>().enabled = false;
		if(hweapon)
		{
			GetComponent<Light>().enabled = false;
			muzzle.enabled = false;
		}
	}
	
	IEnumerator ResetFire()
	{
		yield return new WaitForSeconds(0.2f);
		GetComponent<Light>().enabled = false;
		muzzle.enabled = false;
	}
	
	protected abstract void SetupNPC();
	protected virtual void NPCFleeUpdate() { }
}