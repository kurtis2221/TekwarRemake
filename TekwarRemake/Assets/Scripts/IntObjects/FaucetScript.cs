using UnityEngine;
using System.Collections;

public class FaucetScript : IntObj {
	
	GameObject rwater;
	
	void Awake()
	{
		rwater = transform.Find("rwater").gameObject;
	}
	
	public override void Action()
	{
		rwater.GetComponent<ParticleEmitter>().emit = !rwater.GetComponent<ParticleEmitter>().emit;
		if(rwater.GetComponent<ParticleEmitter>().emit) GetComponent<AudioSource>().Play();
		else GetComponent<AudioSource>().Stop();
	}
}