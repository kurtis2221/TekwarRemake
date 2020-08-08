using UnityEngine;
using System.Collections;

public class SwitchScript : IntObj
{
	public bool state = true;
	
	public Light[] lightsource;
	public Renderer[] meshes;
	public Renderer[] meshes_alt;
	
	public override void Action()
	{
		state = !state;
		foreach(Light l in lightsource)
			l.enabled = state;
		foreach(Renderer m in meshes)
			m.enabled = state;
		foreach(Renderer m in meshes_alt)
			m.enabled = !state;
	}
}