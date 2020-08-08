using UnityEngine;
using System.Collections;

public class TurretNPC : ShootObj
{
	const int WAIT_TIME = 50;
	const float DH_MOVE = 0.5f;
	
	public Transform proj_point;
	public AudioClip shot_snd;
	public AudioClip death_snd;
	public Transform dead_tr;
	RaycastHit hit;
	int wait;
	bool dying;
	
	void Awake()
	{
		health = 50;
	}
	
	void FixedUpdate()
	{
		//Fall "animation"
		if(dying)
		{
			Vector3 dead_pos = dead_tr.position;
			transform.position = Vector3.MoveTowards(transform.position,dead_pos,DH_MOVE);
			transform.rotation = Quaternion.RotateTowards(transform.rotation,dead_tr.rotation,DH_MOVE);
			if(Vector3.Distance(transform.position,dead_pos) < 0.1f)
			{
				transform.position = dead_pos;
				transform.rotation = dead_tr.rotation;
				enabled = false;
			}
			return;
		}
		//Seek mode
		Transform player = GameBase.inst.player_tr;
		Vector3 pos = proj_point.position;
		Vector3 pl_pos = player.position;
		if(Vector3.Distance(transform.position,pl_pos) < 20.0f)
		{
			proj_point.LookAt(pl_pos);
			transform.localRotation =
				Quaternion.Euler(0.0f,0.0f,270f-proj_point.localRotation.eulerAngles.y);
			if(Physics.Linecast(pos,pl_pos,out hit))
			{
				if(hit.transform == player)
				{
					if(wait == 0)
					{
						GetComponent<AudioSource>().PlayOneShot(shot_snd);
						((GameObject)GameObject.Instantiate(GameBase.inst.pr_enemy,
							proj_point.position,proj_point.rotation)).GetComponent<Rigidbody>().velocity = proj_point.forward * 32.0f;
						wait = WAIT_TIME;
					}
				}
			}
		}
		if(wait > 0) wait--;
	}
	
	protected override void Action()
	{
		MainScript.inst.AddScore(50);
		GameObject.Instantiate(GameBase.inst.pr_expl,transform.position,transform.rotation);
		dying = true;
		GetComponent<Collider>().enabled = false;
		GetComponent<AudioSource>().PlayOneShot(death_snd);
	}
}