using UnityEngine;
using System.Collections;

public class ImgSeq : MonoBehaviour
{
	public float interval = 0.0f;
	public string folder = null;
	//An array of Objects that stores the results of the Resources.LoadAll() method
	private Object[] objects;
	//Each returned object is converted to a Texture and stored in this array
	private Texture[] textures;
	//With this Material object, a reference to the game object Material can be stored
	private Material goMaterial;
	//An integer to advance frames
	private int frameCounter = 0;	

	void Awake()
	{
		//Get a reference to the Material of the game object this script is attached to
		this.goMaterial = this.GetComponent<Renderer>().material;
		//Load all textures found on the Sequence folder, that is placed inside the resources folder
		this.objects = Resources.LoadAll(folder, typeof(Texture));
		//Initialize the array of textures with the same size as the objects array
		this.textures = new Texture[objects.Length];
		//Cast each Object to Texture and store the result inside the Textures array
		for(int i = 0; i < objects.Length; i++)
			this.textures[i] = (Texture)this.objects[i];
	}

	void OnEnable()
	{
		//Call the 'PlayLoop' method as a coroutine with a 0.04 delay
        StartCoroutine("PlayLoop",interval);
	}
	
	void OnDisable()
	{
		//Stop this coroutine
		StopCoroutine("PlayLoop");
	}

    //The following methods return a IEnumerator so they can be yielded:
	//A method to play the animation in a loop
    IEnumerator PlayLoop(float delay)
    {
		while(true)
		{
	        //Wait for the time defined at the delay parameter
	        yield return new WaitForSeconds(delay);  
			//Advance one frame
			frameCounter = (++frameCounter)%textures.Length;
			//Set the material's texture to the current value of the frameCounter variable
			goMaterial.mainTexture = textures[frameCounter];
		}
    }
}