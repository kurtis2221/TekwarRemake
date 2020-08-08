using UnityEngine;
using System.Collections;

public class KeypadScript : IntObj
{
	public IntObj door;
	public Texture alttext;
	Texture origtext;
	bool block = false;
	public GameBase.Keys keytype;
	
	void Awake()
	{
		origtext = GetComponent<Renderer>().material.mainTexture;
	}
	
	public override void Action()
	{
		if(!block)
		{
			if((MainScript.inst.keys & keytype) != keytype)
			{
				GameBase.inst.ShowMsg("Passage requires " +
					(keytype == GameBase.Keys.RedKey ? "red" : "blue") + " keycard");
				return;
			}
			block = true;
			GetComponent<AudioSource>().Play();
			GetComponent<Renderer>().material.mainTexture = alttext;
			door.GetComponent<IntObj>().Action();
			StartCoroutine(ResetSwitch());
		}
	}
	
	IEnumerator ResetSwitch()
	{
		yield return new WaitForSeconds(0.5f);
		GetComponent<Renderer>().material.mainTexture = origtext;
		block = false;
	}
}