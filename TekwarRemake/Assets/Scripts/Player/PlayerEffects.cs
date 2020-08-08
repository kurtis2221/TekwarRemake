using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour
{
	public GameObject part_splash;
	bool isinwater = false;
	RaycastHit hit = new RaycastHit();
	
	void FixedUpdate()
	{
		if(Physics.Raycast(transform.position,-transform.up,out hit,1.0f,16))
		{
			if(hit.collider.name.StartsWith("water"))
			{
				if(!isinwater)
				{
					GameObject.Instantiate(part_splash,hit.point,new Quaternion(0,0,0,0));					
					isinwater = true;	
				}
			}
			else
				isinwater = false;
		}
		else isinwater = false;
	}
}