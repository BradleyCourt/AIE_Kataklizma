using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundRandomizer : MonoBehaviour {

    protected AudioSource _Source;
    public AudioSource Source {
        get {
            if (_Source == null)
                _Source = GetComponent<AudioSource>();

            return _Source;
        }
    }
    public bool Shuffle = true;

    [Space]
    public List<AudioClip> Clips;

    protected int CurrentIndex = -1;

    protected AudioClip NextClip {
        get {
            if (Clips.Count == 0) return null;

            if (Shuffle)
                CurrentIndex = Random.Range(0, Clips.Count);
            else
                CurrentIndex = (int)Mathf.Repeat(++CurrentIndex, Clips.Count);

            return Clips[CurrentIndex];
        }
    }

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Source != null && !Source.isPlaying)
        {
            var clip = NextClip;
            Source.PlayOneShot(clip);
        }
        
	}
}
