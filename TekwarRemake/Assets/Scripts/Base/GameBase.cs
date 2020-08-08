using UnityEngine;
using System.Collections;

public sealed class GameBase : MonoBehaviour
{
	public enum Levels
	{
		NextLevel = -1,
		Intro1,
		Intro2,
		Intro3,
		Intro4,
		Menu,
		Level,
		Marty1,
		Marty2,
		Marty3,
		Marty4,
		Marty5,
		Credits
	}
	
	public enum Keys
	{
		None = 0,
		RedKey = 1,
		BlueKey = 2
	}
	
	public static GameBase inst;
	
	public const int WEAPON_RAY = ~2068;
	public const int EXPL_RAY = ~8196;
	
	public Transform player_tr;
	public HudControl hud_ctrl;
	
	//Particles
	public GameObject pr_expl;
	public GameObject pr_expl_snd;
	public GameObject pr_aexpl;
	public GameObject pr_spark;
	public GameObject pr_glass;
	public GameObject pr_lampb;
	public GameObject pr_enemy;
	
	//Musics
	public static bool music_dis = false;
	public static bool music_midi = false;
	public AudioClip[] mus_opl3;
	public AudioClip[] mus_midi;
	
	//Message
	public GUIText hud_msg;
	int hud_msg_tm;
	
	//Stat
	public bool civ_cas;
	
	void Awake()
	{
		inst = this;
		civ_cas = false;
		NPCBase.ClearNPCList();
		StartCoroutine(ResetTimer());
	}
	
	public void ShowMsg(string input)
	{
		hud_msg.text = input;
		hud_msg.enabled = true;
		hud_msg_tm = 4;
	}
	
	public AudioClip GetMusic(int idx)
	{
		return music_midi ? mus_midi[idx] : mus_opl3[idx];
	}
	
	IEnumerator ResetTimer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.0f);
			if(hud_msg_tm > 0)
			{
				hud_msg_tm--;
				if(hud_msg_tm <= 0)
				{
					hud_msg_tm = 0;
					hud_msg.enabled = false;
				}
			}
		}
	}
}