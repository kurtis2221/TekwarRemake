using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour
{
	public enum PickupType
	{
		MedKit,
		Donut,
		Pistol,
		Shrike,
		RedKey,
		BlueKey
	}
	
	string[] pick_msg =
	{
		"Medic kit",
		"MMMMMMM Donuts",
		"Pistol klip",
		"Shrike charges",
		"Red key card",
		"Blue key card"
	};
	
	public PickupType pickup;
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<MainScript>() == null) return;
		bool picked = true;
		switch(pickup)
		{
		case PickupType.MedKit:
			picked = MainScript.inst.Damage(-50);
			break;
		case PickupType.Donut:
			picked = MainScript.inst.Damage(-25);
			break;
		case PickupType.Pistol:
			picked = WeaponScript.inst.AddAmmo(1, 10);
			break;
		case PickupType.Shrike:
			picked = WeaponScript.inst.AddAmmo(2, 10);
			break;
		case PickupType.RedKey:
			MainScript.inst.AddKeycard(GameBase.Keys.RedKey);
			break;
		case PickupType.BlueKey:
			MainScript.inst.AddKeycard(GameBase.Keys.BlueKey);
			break;
		}
		if(picked)
		{
			GameBase.inst.ShowMsg(pick_msg[(int)pickup]);
			Destroy(gameObject);
		}
	}
}