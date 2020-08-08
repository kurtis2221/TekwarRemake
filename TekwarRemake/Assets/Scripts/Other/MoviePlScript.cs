using UnityEngine;
using System.Collections;

public class MoviePlScript : MonoBehaviour
{
	public GameBase.Levels level = GameBase.Levels.NextLevel;
	public MovieTexture mov;
	public GameObject loadsc;
	bool skip;
	
	void Start()
	{
		Cursor.visible = false;
		Screen.lockCursor = true;
		skip = false;
		GetComponent<AudioSource>().clip = mov.audioClip;
		mov.Play();
		GetComponent<AudioSource>().Play();
		StartCoroutine(MovieWait());
	}
	
	void FixedUpdate()
	{
		if(skip || Input.GetKey(KeyCode.Escape))
		{
			StopAllCoroutines();
			enabled = false;
			if(loadsc != null)
			{
				loadsc.SetActiveRecursively(true);
			}
			Application.LoadLevel(level == GameBase.Levels.NextLevel ? Application.loadedLevel + 1 : (int)level);
		}
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),mov,ScaleMode.ScaleToFit);
	}
	
	IEnumerator MovieWait()
	{
		yield return new WaitForSeconds(mov.duration + 1.0f);
		skip = true;
	}
}