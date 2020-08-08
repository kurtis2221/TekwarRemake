using UnityEngine;
using System.Collections;

public class ParticleScript4 : MonoBehaviour
{
	public float time = 1f;
	public AudioClip[] sound;
	int idx;
	
	void Start ()
	{
		if(sound != null)
		{
			GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
			idx = Random.Range(0,sound.Length);
			GetComponent<AudioSource>().PlayOneShot(sound[idx]);
		}
		StartCoroutine(AutoDestroy());
	}
	
	IEnumerator AutoDestroy()
	{
		yield return new WaitForSeconds(sound[idx].length);
		Destroy(gameObject);
	}
}