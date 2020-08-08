using UnityEngine;
using System.Collections;

public class ParticleScript2 : MonoBehaviour
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
			StartCoroutine(AutoDestroy());
		}
		StartCoroutine(HideMesh());
	}
	
	IEnumerator HideMesh()
	{
		yield return new WaitForSeconds(time);
		System.Array.ForEach(gameObject.GetComponentsInChildren<Renderer>(),x => x.enabled = false);
		if(sound == null) Destroy(gameObject);
	}
	
	IEnumerator AutoDestroy()
	{
		yield return new WaitForSeconds(sound[idx].length);
		Destroy(gameObject);
	}
}