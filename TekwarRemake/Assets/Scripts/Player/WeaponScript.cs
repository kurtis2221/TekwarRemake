using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
	[System.Serializable]
	public class Weapon
	{
		public GameObject weapon;
		public AudioClip fire_snd;
		public ParticleEmitter muzzle;
		public Object proj;
		public GameObject particle;
		public GameObject particle2;
		public bool is_proj;
		public bool stun;
		public int ammo;
		public int min_dm;
		public int max_dm;
		public int min_ns;
		public int max_ns;
	}
	
	public const int MAX_AMMO = 100;
	const int WAIT_TIME = 200;
	
	public static WeaponScript inst;

	public Transform hand;
	public Transform proj_point;
	public GUITexture crosshair;
	public bool weapon_drawn;
	public Renderer skin_weapon;
	public Renderer skin_muzzle; 
	
	public Weapon[] weapons;
	int weaponid;
	Weapon curr_weapon;
	Weapon stun_weapon;
	
	Quaternion normal_pos;
	Quaternion shoot_pos;
	Quaternion holster_pos;
	
	bool blocked = false;
	bool isdown = false;
	bool changing = false;
	bool firing = false;
	RaycastHit hit = new RaycastHit();
	GameObject tmp;
	
	int waittime;
	
	void Start()
	{
		inst = this;
		waittime = WAIT_TIME;
		normal_pos = hand.localRotation;
		shoot_pos = normal_pos * Quaternion.Euler(new Vector3(-10,0,0));
		holster_pos = normal_pos * Quaternion.Euler(new Vector3(50,0,0));
		hand.localRotation = holster_pos;
		blocked = true;
		weaponid = 1;
		curr_weapon = weapons[weaponid];
		stun_weapon = weapons[0];
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Ammo,(float)curr_weapon.ammo/MAX_AMMO);
	}
	
	void Update()
	{
		float mwheel = Input.GetAxisRaw("Mouse ScrollWheel");
		if(mwheel != 0)
		{
			if(mwheel < 0)
			{
				weaponid++;
				if(weaponid >= weapons.Length) weaponid = 0;
			}
			else
			{
				weaponid--;
				if(weaponid < 0) weaponid = weapons.Length - 1;
			}
			ChangeWeapon(weaponid);
		}
	}
	
	void FixedUpdate()
	{
		if(!changing)
		{
			//Weapon change
			if(!firing)
			{
				if(Input.GetButton("Wp1") && weaponid != 0) ChangeWeapon(0);
				else if(Input.GetButton("Wp2") && weaponid != 1) ChangeWeapon(1);
				else if(Input.GetButton("Wp3") && weaponid != 2) ChangeWeapon(2);
			}
			//Fire
			if(!blocked)
			{
				if(Input.GetButton("Fire1") && curr_weapon.ammo > 0)
				{
					MainScript.inst.DoAnim(PlayerObj.Anims.FIRE, true, true);
					GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
					GetComponent<AudioSource>().PlayOneShot(curr_weapon.fire_snd);
					GetComponent<Light>().enabled = true;
					skin_muzzle.enabled = true;
					hand.localRotation = shoot_pos;
					if(curr_weapon == stun_weapon) curr_weapon.ammo -= 20;
					else curr_weapon.ammo--;
					GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Ammo,(float)curr_weapon.ammo/MAX_AMMO);
					if(!curr_weapon.is_proj)
					{
						curr_weapon.muzzle.emit = true;
						if(Physics.Raycast(proj_point.position,proj_point.forward,out hit,256.0f,GameBase.WEAPON_RAY))
							CheckHit(ref hit, true, weaponid);
					}
					else
						((GameObject)GameObject.Instantiate(curr_weapon.proj,
							proj_point.position,proj_point.rotation)).GetComponent<Rigidbody>().velocity = proj_point.forward * 32.0f;
					blocked = true;
					firing = true;
					NPCBase.CheckAlert(transform.position,curr_weapon.min_ns,curr_weapon.max_ns);
					StartCoroutine(ResetWeapon());
				}
			}
		}
		
		//Recharge stun gun
		if(stun_weapon.ammo < MAX_AMMO)
		{
			if(waittime > 0) waittime--;
			else
			{
				waittime = WAIT_TIME;
				stun_weapon.ammo += 20;
				if(stun_weapon.ammo > MAX_AMMO) stun_weapon.ammo = MAX_AMMO;
				if(curr_weapon == stun_weapon)
					GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Ammo,(float)curr_weapon.ammo/MAX_AMMO);
			}
		}
		
		//Equip weapon
		if(Input.GetButton("Fire1") && !weapon_drawn || Input.GetButton("Holster"))
		{
			if(!isdown && !changing)
			{
				blocked = weapon_drawn;
				weapon_drawn = !weapon_drawn;
				skin_weapon.enabled = weapon_drawn;
				skin_muzzle.enabled = false;
				crosshair.enabled = weapon_drawn;
				if(weapon_drawn)
				{
					hand.gameObject.active = true;
					curr_weapon.weapon.SetActiveRecursively(true);
				}
				hand.parent.GetComponent<WeaponSway>().enabled = weapon_drawn;
				changing = true;
				isdown = true;
			}
		}
		else isdown = false;
		
		//Changing animation
		if(changing)
		{
			hand.localRotation = Quaternion.RotateTowards(
				hand.localRotation,
				(weapon_drawn) ? normal_pos : holster_pos,
				4.0f);
			if(((weapon_drawn) ? normal_pos : holster_pos) == hand.localRotation)
			{
				if(!weapon_drawn)
					hand.gameObject.SetActiveRecursively(false);
				changing = false;
			}
		}
	}
	
	public bool CheckHit(ref RaycastHit hit, bool imp, int wid)
	{
		Weapon curr_weapon = weapons[wid];
		if(!curr_weapon.stun)
		{
			int damage = Random.Range(curr_weapon.min_dm, curr_weapon.max_dm);
			ShootObj tmp2 = hit.collider.GetComponent<ShootObj>();
			if(tmp2 != null) tmp2.Damage(damage);
		}
		PlayerObj tmp = hit.collider.GetComponent<PlayerObj>();
		if(tmp != null)
		{
			int damage = Random.Range(curr_weapon.min_dm, curr_weapon.max_dm);
			if(curr_weapon.stun) 
			{
				if(tmp is AndroidNPC) return false;
				tmp.Stun(damage);
			}
			else tmp.Damage(damage);
			if(imp) GameObject.Instantiate(curr_weapon.particle2,hit.point,default(Quaternion));
			return true;
		}
		else
		{
			if(imp && !hit.collider.GetComponent<HoloNPC>())
				GameObject.Instantiate(curr_weapon.particle,hit.point,default(Quaternion));
			return false;
		}
	}
	
	public bool AddAmmo(int wid, int ammo)
	{
		Weapon weapon = weapons[wid];
		if(weapon.ammo >= MAX_AMMO) return false;
		weapon.ammo += ammo;
		if(weapon.ammo > MAX_AMMO) weapon.ammo = MAX_AMMO;
		if(curr_weapon == weapon) GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Ammo,(float)curr_weapon.ammo/MAX_AMMO);
		MainScript.inst.hud_st.Blink();
		return true;
	}
	
	void ChangeWeapon(int wid)
	{
		if(weapon_drawn)
		{
			blocked = weapon_drawn;
			weapon_drawn = !weapon_drawn;
			skin_weapon.enabled = weapon_drawn;
			skin_muzzle.enabled = false;
			crosshair.enabled = weapon_drawn;
			hand.parent.GetComponent<WeaponSway>().enabled = weapon_drawn;
			changing = true;
		}
		weaponid = wid;
		curr_weapon = weapons[wid];
		GameBase.inst.hud_ctrl.ChangeData(HudControl.HudEnum.Ammo,(float)curr_weapon.ammo/MAX_AMMO);
	}
	
	IEnumerator ResetWeapon()
	{
		yield return new WaitForSeconds(0.1f);
		GetComponent<Light>().enabled = false;
		skin_muzzle.enabled = false;
		if(!curr_weapon.is_proj)
			curr_weapon.muzzle.emit = false;
		hand.localRotation = normal_pos;
		StartCoroutine(ResetWeapon2());
	}
	
	IEnumerator ResetWeapon2()
	{
		yield return new WaitForSeconds(0.2f);
		if(weapon_drawn)
		{
			blocked = false;
			firing = false;
		}
	}
}