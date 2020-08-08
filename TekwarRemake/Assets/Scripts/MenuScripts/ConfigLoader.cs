using UnityEngine;
using System.Collections;
using ini_hndl;

public class ConfigLoader : MonoBehaviour
{
	public AudioSource maincam;
	public AudioClip music;
	public AudioClip altmusic;
	public Transform menu_opt;
	public VolContScript vol;
	public VolContScript vol_mus;
	public VolContScript mouse_sens;
	public VolContScript fov;
	OptSelScript[] options;
	
	public IniHandler cfg;
	public const string filename = "config.ini";
	
	void Awake()
	{
		Screen.lockCursor = false;
		Cursor.visible = true;
		options = menu_opt.GetComponentsInChildren<OptSelScript>(true);
		cfg = new IniHandler(filename);
		cfg.HeadComments = "#TekWar config file";
		cfg.LoadIni();
		//Volume
		float tmp = 1.0f;
		if(float.TryParse(cfg["Volume"],out tmp))
		{
			vol.volume = (int)(tmp * 100);
			AudioListener.volume = tmp;
		}
		if(float.TryParse(cfg["MVolume"],out tmp))
		{
			vol_mus.volume = (int)(tmp * 100);
		}
		if(float.TryParse(cfg["MouseSens"],out tmp))
		{
			mouse_sens.volume = (int)(tmp * 10);
		}
		if(float.TryParse(cfg["FOV"],out tmp))
		{
			fov.volume = (int)tmp;
		}
		//MIDI
		if(cfg["MIDI"] == "0") maincam.clip = altmusic;
		maincam.Play();
		//Other
		foreach(OptSelScript o in options)
		{
			//PostQuality
			if(o.type)
			{
				int tmp2;
				int.TryParse(cfg["PostQuality"],out tmp2);
				if(tmp2 < 0) tmp2 = 0;
				else if(tmp2 > 2) tmp2 = 2;
				o.stat2 = tmp2;
			}
			else o.stat = cfg[o.name] != "0";
		}
	}
	
	public void DoSave()
	{
		cfg["Volume"] = ((float)vol.volume / 100).ToString("0.0");
		cfg["MVolume"] = ((float)vol_mus.volume / 100).ToString("0.0");
		cfg["MouseSens"] = ((float)mouse_sens.volume / 10).ToString("0.0");
		cfg["FOV"] = fov.volume.ToString();
		foreach(OptSelScript o in options)
		{
			if(o.type) cfg[o.name] = o.stat2.ToString();
			else cfg[o.name] = o.stat ? "1" : "0";
		}
		cfg.SaveIni();
	}
	
	public void ChangeMusic(bool midi)
	{
		maincam.clip = midi ? music : altmusic;
		maincam.Play();
	}
}