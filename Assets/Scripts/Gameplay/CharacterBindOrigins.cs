using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBindOrigins : MonoBehaviour {


    public Transform Forehead;
    public Transform Mouth;
    public Transform Torso;
    public Transform LeftHand;
    public Transform RightHand;
    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform Tail;


    public Transform this[CharacterBindPoint binding] {
        get {
            switch (binding) {
                case CharacterBindPoint.Forehead: return Forehead;
                case CharacterBindPoint.Mouth: return Mouth;
                case CharacterBindPoint.Torso: return Forehead;
                case CharacterBindPoint.LeftHand: return LeftHand;
                case CharacterBindPoint.RightHand: return RightHand;
                case CharacterBindPoint.LeftFoot: return LeftFoot;
                case CharacterBindPoint.RightFoot: return RightFoot;
                case CharacterBindPoint.Tail: return Tail;

                default: return null;
            }
        }
    }
}
