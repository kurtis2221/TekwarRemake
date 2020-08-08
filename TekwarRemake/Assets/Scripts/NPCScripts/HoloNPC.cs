using UnityEngine;

public class HoloNPC : ShootObj
{
	const float LOOK_SPD = 1;
	const int SND_WAIT = 50;
	const float AL_DIST = 20;
	const float DM_DIST = 4;
	
	public Transform proj_point;
	public AudioClip snd;
	public AudioClip snd_shot;
	public AudioClip snd_dest;
	public Animation anim_obj;
	public string anim_fire;
	public Renderer char_rend;
	int snd_wait = SND_WAIT;
	RaycastHit hit;
	bool dying;
	Material mat;
	Color col;
	
	void Awake()
	{
		health = 20;
		anim_obj[anim_fire].layer = 1;
		mat = char_rend.material;
		col = mat.color;
	}
	
	void FixedUpdate()
	{
		if(dying)
		{
			col.a -= 0.005f;
			mat.color = col;
			if(col.a <= 0.0f)
			{
				enabled = false;
				Destroy(gameObject);
			}
			return;
		}
		Vector3 pos = proj_point.position;
		Vector3 pl_pos = GameBase.inst.player_tr.position;
		float dist = Vector3.Distance(pos,pl_pos);
		if(dist < AL_DIST)
		{
			if(Physics.Linecast(pos,pl_pos,out hit))
			{
				if(hit.transform == GameBase.inst.player_tr)
				{
					pl_pos.y = pos.y;
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
						Quaternion.LookRotation(pl_pos - pos),LOOK_SPD);
					if(snd_wait <= 0)
					{
						snd_wait = SND_WAIT;
						GetComponent<AudioSource>().PlayOneShot(snd);
						anim_obj.Play(anim_fire);
						if(dist < DM_DIST)
						{
							GetComponent<AudioSource>().PlayOneShot(snd_shot);
							MainScript.inst.Damage(Random.Range(2, 8));
						}
					}
					snd_wait--;
				}
			}
		}
	}
	
	protected override void Action()
	{
		MainScript.inst.AddScore(100);
		GameBase.inst.ShowMsg("Was a hologram");
		GetComponent<AudioSource>().PlayOneShot(snd_dest);
		GetComponent<Collider>().enabled = false;
		dying = true;
	}
}