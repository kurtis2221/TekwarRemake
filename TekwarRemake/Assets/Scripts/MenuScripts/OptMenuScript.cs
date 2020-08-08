using UnityEngine;
using System.Collections;

public class OptMenuScript : MonoBehaviour
{
	public GameObject main_cam;
	public AudioClip menu_snd;
	public GameObject main_menu;
	public ConfigLoader cfg;
	
	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.red;
		main_cam.GetComponent<AudioSource>().PlayOneShot(menu_snd);
	}
	
	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDown()
	{
		GetComponent<Renderer>().material.color = Color.white;
		cfg.DoSave();
		main_menu.SetActiveRecursively(true);
		transform.parent.gameObject.SetActiveRecursively(false);
	}
}