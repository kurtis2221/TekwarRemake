using UnityEngine;
using System.Collections;

public class TriggerScript : MonoBehaviour
{
	public IntObj scr;
	
	void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<MainScript>() == null) return;
		scr.Action();
	}
}