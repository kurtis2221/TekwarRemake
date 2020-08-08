using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour
{	
	public float radius = 1.0f;
	public string map_text;
	Transform teleport;
	public int mus_no;
	
	void Awake()
	{
		teleport = transform.GetChild(0);
	}
	
	public void Trigger()
	{
		GameBase.inst.player_tr.position = teleport.position;
		GameBase.inst.player_tr.rotation = teleport.rotation;
		GameBase.inst.player_tr.GetComponent<FPSWalkerEnhanced>().ResetFall();
		MainScript.inst.PlayMusic(mus_no);
		GameBase.inst.ShowMsg(map_text);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position,radius);
	}
}