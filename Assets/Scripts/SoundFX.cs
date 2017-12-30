using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClass {
	public string NameSFX;
	public AudioClip SFX;
    public AudioSource Source;
}

public class SoundFX : MonoBehaviour {
	private static SoundFX _Instance;
	public static SoundFX Instance {
		get 
		{
			return _Instance;
		}
	}

	public List <AudioClass> AudioList;

	private AudioSource _Audio;
	// Use this for initialization

	void Awake (){
		_Instance = this;
	}

    public void PlaySFX(string name)
    {
        AudioClass _audio = getSFX(name);
        if (_audio != null)
        {
            _audio.Source.clip = _audio.SFX;
            _audio.Source.Play();
        }
    }

    AudioClass getSFX(string name)
    {
        foreach (AudioClass audio in AudioList)
        {
            if (audio.NameSFX == name)
                return audio;
        }
        return null;
    }



    //public void PlaySFX (string name, bool isEnd = false){
    //    bool isFind = false;
    //    bool notPlay = false;

    //    if (!isEnd){
    //        _Audio.Stop();
    //        for (int i = 0 ; i < AudioList.Count && !isFind; i++){
    //            if (AudioList[i].NameSFX == name){
    //                isFind = true;
    //                _Audio.clip = AudioList[i].SFX;
    //                _Audio.Play();
    //            }
    //        }
    //    }
    //    else{
    //        for (int i = 0 ; i < AudioList.Count && !isFind; i++){
    //            if (AudioList[i].NameSFX == name){
    //                isFind = true;
    //                for (int x = 0; x < Audio.Count && !notPlay;x++){
    //                    if (!Audio[x].isPlaying){
    //                        Audio[x].Stop();
    //                        Audio[x].clip = AudioList[i].SFX;
    //                        Audio[x].Play();
    //                        notPlay = true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
