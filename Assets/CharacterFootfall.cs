using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kataklizma.Gameplay;

public class CharacterFootfall : MonoBehaviour {

    [Header("Sound")]
    public AudioSource Sound;

    [Header("Visual")]
    public CharacterBindOrigins BindOrigins;
    public GameObject Dustcloud;
    public float DustcloudTTL = 0.5f;
    public GameObject Player;
    public float Level;
    public CarBounce[] ObjectsToBounce;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FootFall(string side)
    {
        if (Sound != null && !Sound.isPlaying) Sound.Play();
        if (Dustcloud != null )
        {
            var origin = (side == "Left" ? BindOrigins[CharacterBindPoint.LeftFoot] : BindOrigins[CharacterBindPoint.RightFoot]);

            var go = Instantiate(Dustcloud, origin.position, origin.rotation);
            Destroy(go, DustcloudTTL);
        }
        Level = Player.GetComponent<EntityAttributes>()[ValueType.CharacterLevel];
        if(Level >= 3)
        {
            Bounce();
        }
        

    }

    void Bounce()
    {
        ObjectsToBounce = FindObjectsOfType(typeof(CarBounce)) as CarBounce[];
        print(ObjectsToBounce.Length);
        foreach(CarBounce car in ObjectsToBounce)
        {
            car.Activate();
        }
    }
}
