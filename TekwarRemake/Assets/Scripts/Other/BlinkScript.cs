using UnityEngine;
using System.Collections;

public class BlinkScript : MonoBehaviour
{
	public Light[] lights;
	public Renderer[] meshes;
	public Renderer[] meshes_alt;
	bool blnk = false;
	
	void OnEnable()
	{
		StartCoroutine(Blink());
	}
	
	void OnDisable()
	{
		StopCoroutine("Blink");
	}
	
	IEnumerator Blink()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.0f);
			blnk = !blnk;
			foreach(Light l in lights)
				l.enabled = blnk;
			for(int i = 0; i < meshes.Length; i++)
			{
				meshes[i].enabled = blnk;
				meshes_alt[i].enabled = !blnk;
			}
		}
	}
}