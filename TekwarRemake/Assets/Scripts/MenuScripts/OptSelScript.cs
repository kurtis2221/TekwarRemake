using UnityEngine;
using System.Collections;

public class OptSelScript : MonoBehaviour
{
	public bool type = false;
	public bool stat = false;
	public int stat2 = 0;
	public bool stat3 = false;
	public ConfigLoader cl;
	string tmp;
	
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
	
	void OnMouseDown()
	{
		if(type)
		{
			stat2++;
			stat2%=3;
		}
		else
			stat = !stat;
		ChangeStat();
	}
	
	void ChangeStat()
	{
		if(type)
		{
			tmp = GetComponent<TextMesh>().text;
			tmp = tmp.Substring(0,tmp.LastIndexOf(':')+2);
			if(stat2 == 0)
				tmp += "LO";
			else if(stat2 == 1)
				tmp += "ME";
			else if(stat2 == 2)
				tmp += "HI";
			GetComponent<TextMesh>().text = tmp;
		}
		else
		{
			tmp = GetComponent<TextMesh>().text;
			tmp = tmp.Substring(0,tmp.LastIndexOf(':')+2) + (stat ? "ON" : "OFF");
			GetComponent<TextMesh>().text = tmp;
			if(stat3) cl.ChangeMusic(stat);
		}
	}
}