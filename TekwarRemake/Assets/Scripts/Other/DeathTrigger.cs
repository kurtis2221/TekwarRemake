using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		PlayerObj tmp = col.GetComponent<PlayerObj>();
		if(tmp != null)
		{
			tmp.Death();
		}
	}
}