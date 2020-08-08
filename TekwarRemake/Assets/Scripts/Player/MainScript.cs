using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScript : PlayerObj
{
	public static MainScript inst;
	
	public AudioClip[] hit_snd;
	public AudioClip death_snd;
	public AudioClip item_snd;
	
	public Camera rear_cam;
	public Transform proj_point;
	public GameObject main_cam;
	public AudioSource snd_source;
	public GUITexture hud_mirr;
	public GUIText hud_time;
	public GUIText hud_score;
	public GUIText hud_abort;
	public HudBlink hud_dm;
	public HudBlink hud_cos;
	public HudBlink hud_st;
	public HudFade hud_death;
	public HudFade hud_fade;
	public HudFadeIO hud_cosfade;
	public GUITexture hud_key1;
	public GUITexture hud_key2;
	public Automap map;
	public GameBase.Keys keys;
	public MouseLook look_script;
	public Transform[] dev_teleports;
	internal bool devmode = false;
	
	int hour = 0, min = 0, sec = 0;
	bool moving = false;
	public AudioClip mirr_snd;
	Rect mirr_rect;
	bool isplaying = true;
	string colname;
	int scpoints;
	bool quitprompt = false;
	bool godmode = false;
	
	void OnApplicationFocus(bool stat)
	{
		Screen.lockCursor = true;
	}
	
	void Awake()
	{
		inst = this;
		int width = Screen.width;
		int height = Screen.height;
		Cursor.visible = false;
		Screen.lockCursor = true;
		rear_cam.rect = new Rect(
			105f/width/768f*height,
			1f-(66f/height/768f*height),
			106f/width/768f*height,
			56f/height/768f*height);
		hud_time.material.color = new Color(0f,0.5f,1f,1f);
		hud_score.material.color = hud_time.material.color;
		health = MAX_HEALTH;
		cosinous = MAX_HEALTH;
		scpoints = 0;
		keys = 0;
		StartCoroutine(ChangeTime());
		StartCoroutine(COSFade());
		hud_dm.Init(DMBlink);
		hud_cos.Init(CosBlink);
		hud_st.Init(STBlink);
		hud_death.Init(DeathFade);
		hud_fade.Init(CompleteFade);
		SetupAnimation();
	}
	
	void Start()
	{
		mirr_rect = hud_mirr.pixelInset;
	}

	void Update()
	{
		if(quitprompt)
		{
			if(Input.GetKeyDown(KeyCode.Y))
			{
				AbortMission();
				return;
			}
			else if(Input.GetKeyDown(KeyCode.N))
			{
				hud_abort.enabled = quitprompt = false;
				return;	
			}
		}
		if(Input.GetButtonDown("Use"))
		{
			foreach(Collider c in Physics.OverlapSphere(proj_point.position+proj_point.forward,1.0f))
				CheckObject(c);
		}
		else if(Input.GetButtonDown("Mirror"))
		{
			if(!moving)
			{
				if(rear_cam.enabled)
				{
					rear_cam.enabled = false;
					StartCoroutine(DoDisableCam());
				}
				else
				{
					hud_mirr.enabled = true;
					StartCoroutine(DoEnableCam());
				}
				snd_source.PlayOneShot(mirr_snd);
				moving = true;
			}
		}
		else if(Input.GetButtonDown("Music"))
		{
			if(!GameBase.music_dis)
			{
				if(isplaying) main_cam.GetComponent<AudioSource>().Stop();
				else main_cam.GetComponent<AudioSource>().Play();
				isplaying = !isplaying;
			}
		}
		else if(Input.GetButtonDown("Map"))
		{
			map.ActivateAutomap();
		}
		else if(Input.GetButton("MapZ"))
		{
			map.Zoom(Input.GetAxisRaw("MapZ"));
		}
		else if(Input.GetKeyDown(KeyCode.Escape))
		{
			hud_abort.enabled = quitprompt = true;
		}
		if(devmode)
		{
			if(Input.GetKey(KeyCode.Backspace))
			{
				if(Input.GetKeyDown(KeyCode.F1)) DEVTeleport(0);
				else if(Input.GetKeyDown(KeyCode.F2)) DEVTeleport(1);
				else if(Input.GetKeyDown(KeyCode.F3)) DEVTeleport(2);
				else if(Input.GetKeyDown(KeyCode.F4)) DEVTeleport(3);
				else if(Input.GetKeyDown(KeyCode.F5)) DEVTeleport(4);
				else if(Input.GetKeyDown(KeyCode.F6)) DEVTeleport(5);
				else if(Input.GetKeyDown(KeyCode.F11))
				{
					Damage(-MAX_HEALTH);
					AddKeycard(GameBase.Keys.RedKey);
					AddKeycard(GameBase.Keys.BlueKey);
					WeaponScript.inst.AddAmmo(1,WeaponScript.MAX_AMMO);
					WeaponScript.inst.AddAmmo(2,WeaponScript.MAX_AMMO);
					GameBase.inst.ShowMsg("Full ammo, keys, health");
				}
				else if(Input.GetKeyDown(KeyCode.F12))
				{
					godmode = !godmode;	
					GameBase.inst.ShowMsg("God mode " + (godmode ? "ON" : "OFF"));
				}
			}
		}
		else
		{
			if(Input.GetKey(KeyCode.Backspace) &&
				Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.V))
			{
				devmode = true;
				GetComponent<FPSWalkerEnhanced>().devmode = true;
				GameBase.inst.ShowMsg("Developer mode ON");
			}
		}
	}
	
	void DEVTeleport(int id)
	{
		transform.parent = null;
		transform.position = dev_teleports[id].position;
		transform.rotation = dev_teleports[id].rotation;
		GameBase.inst.ShowMsg("Teleport");
	}
	
	public void AbortMission()
	{
		Application.LoadLevel(GameBase.inst.civ_cas ?
				(int)GameBase.Levels.Marty5 :
				(int)GameBase.Levels.Marty4);
	}
	
	public void MissionComplete()
	{
		DisablePlayerControl();
		hud_fade.Fade();
	}
	
	void CheckObject(Collider hit)
	{
		IntObj tmp = hit.GetComponent<IntObj>();
		if(tmp != null) tmp.Action();
	}
	
	public override bool Damage(int input)
	{
		if(input < 0 && health >= MAX_HEALTH && cosinous >= MAX_HEALTH) return false;
		return base.Damage(input);
	}
	
	public override void Damage2(int input)
	{
		if(input > 0)
		{
			hud_dm.Blink();
			cosinous -= input + Random.Range(2, 8);
			if(cosinous < 0) cosinous = 0;
		}
		else
		{
			if(health > MAX_HEALTH) health = 100;
			cosinous -= input - Random.Range(2, 8);
			if(cosinous > MAX_HEALTH) cosinous = MAX_HEALTH;
			hud_cos.Blink();
		}
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Health, health / 100f);
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Cosinousness, cosinous / 100f);
	}
	
	public override void Death()
	{
		if(godmode) return;
		DisablePlayerControl();
		DoAnim(Random.Range(0, 2) == 0 ? PlayerObj.Anims.DEATH : PlayerObj.Anims.DEATH2);
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Health, 0);
		GetComponent<AudioSource>().PlayOneShot(death_snd);
		hud_death.Fade();
	}
	
	void DisablePlayerControl()
	{
		main_cam.GetComponent<AudioSource>().Stop();
		enabled = false;
		GetComponent<FPSWalkerEnhanced>().enabled = false;
		GetComponent<Collider>().enabled = false;
		look_script.enabled = false;
		WeaponScript.inst.enabled = false;
		WeaponScript.inst.crosshair.enabled = false;
	}
	
	public override void Stun2(int input) { }
	
	public override void Stunned()
	{
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Cosinousness, 0);
	}
	
	public void AddScore(int input)
	{
		scpoints += input;
		if(scpoints < 0) scpoints = 0;
		if(scpoints > 99999999) scpoints = 99999999;
		hud_score.text = scpoints.ToString("00000000");
	}
	
	public void AddKeycard(GameBase.Keys key)
	{
		if(key == GameBase.Keys.RedKey) hud_key1.enabled = true;
		else hud_key2.enabled = true;
		keys |= key;
		hud_st.Blink();
	}
	
	public void PlayMusic(int idx)
	{
		if(GameBase.music_dis) return;
		main_cam.GetComponent<AudioSource>().clip = GameBase.inst.GetMusic(idx);
		if(isplaying) main_cam.GetComponent<AudioSource>().Play();
	}
	
	void DMBlink()
	{
		if(Random.Range(0, 4) == 0)
			GetComponent<AudioSource>().PlayOneShot(hit_snd[Random.Range(0,hit_snd.Length)]);
	}
	
	void CosBlink()
	{
		GetComponent<AudioSource>().PlayOneShot(item_snd);
	}
	
	void STBlink()
	{
		GetComponent<AudioSource>().PlayOneShot(item_snd);
	}
	
	void DeathFade()
	{
		Application.LoadLevel((int)GameBase.Levels.Level);
	}
	
	void CompleteFade()
	{
		Screen.lockCursor = false;
		Cursor.visible = true;
		Application.LoadLevel(GameBase.inst.civ_cas ?
			(int)GameBase.Levels.Marty3 :
			(int)GameBase.Levels.Marty2);
	}
	
	IEnumerator ChangeTime()
	{
		while(hour < 24)
		{
			yield return new WaitForSeconds(1.0f);
			sec++;
			if(sec > 59)
			{
				min++;
				sec = 0;
			}
			if(min > 59)
			{
				hour++;
				min = 0;
			}
			hud_time.text = hour.ToString("00")+":"+min.ToString("00")+":"+sec.ToString("00");
		}
	}
	
	IEnumerator DoEnableCam()
	{
		while(mirr_rect.x < 0)
		{
			yield return new WaitForSeconds(0.01f);
			mirr_rect.x += 4;
			hud_mirr.pixelInset = mirr_rect;
		}
		mirr_rect.x = 0;
		hud_mirr.pixelInset = mirr_rect;
		rear_cam.enabled = true;
		moving = false;
	}
	
	IEnumerator DoDisableCam()
	{
		hud_mirr.pixelInset = mirr_rect;
		while(mirr_rect.x > -mirr_rect.width)
		{
			yield return new WaitForSeconds(0.01f);
			mirr_rect.x -= 4;
			hud_mirr.pixelInset = mirr_rect;
		}
		mirr_rect.x = -mirr_rect.width;
		hud_mirr.pixelInset = mirr_rect;
		rear_cam.enabled = false;
		hud_mirr.enabled = false;
		moving = false;
	}
	
	IEnumerator COSFade()
	{
		while(true)
		{
			yield return new WaitForSeconds(Random.Range(20, 60));
			if(cosinous <= 30) hud_cosfade.Fade();
		}
	}
}