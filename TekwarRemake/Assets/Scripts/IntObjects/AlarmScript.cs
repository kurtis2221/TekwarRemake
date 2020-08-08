using UnityEngine;
using System.Collections;

public class AlarmScript : IntObj
{
	int sec;
	AudioSource src;
	
	void Awake()
	{
		src = GetComponent<AudioSource>();
		StartCoroutine(Reset());
	}
	
	public override void Action()
	{
		if(sec > 0) return;
		sec = 5;
		src.Play();
	}
	
	IEnumerator Reset()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.0f);
			if(sec > 0)
			{
				sec--;
				if(sec == 0)
					src.Stop();	
			}
		}
	}
}