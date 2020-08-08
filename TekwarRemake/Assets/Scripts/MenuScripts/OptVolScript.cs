using UnityEngine;
using System.Collections;

public class OptVolScript : MonoBehaviour
{
	public bool master_vol;
	public VolContScript vol;
	public int min;
	public int max;
	public int change = 10;
	string tmp;
	bool blocked;
	
	void Start()
	{
		ChangeStat();
	}
	
	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDrag()
	{
		if(blocked) return;
		
		int volume = vol.volume;
		if(volume+change >= min && volume+change <= max)
		{
			volume += change;
			vol.volume = volume;
		}
		ChangeStat();
		blocked = true;
		StartCoroutine(UnBlock());
	}
	
	IEnumerator UnBlock()
	{
		yield return new WaitForSeconds(0.1f);
		blocked = false;
	}
	
	void ChangeStat()
	{
		int volume = vol.volume;
		tmp = transform.parent.GetComponent<TextMesh>().text;
		tmp = tmp.Substring(0,tmp.LastIndexOf(':')+2) + volume.ToString();
		transform.parent.GetComponent<TextMesh>().text = tmp;
		if(master_vol) AudioListener.volume = (float)volume/100f;
	}
}