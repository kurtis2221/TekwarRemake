using UnityEngine;
using System.Collections.Generic;

public class BarrelScript : ExplObjScript
{
	void Awake()
	{
		radius = 5.0f;
		radius2 = 10.0f;
		dam_min = 10;
		dam_max = 40;
	}
}