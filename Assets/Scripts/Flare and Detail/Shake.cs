using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

    
    public float Displacement = 1.0f;
    public float Speed = 0.05f;
    public float DurationOnDamage = 0.5f;

    private Vector3 trackedOrigin;

    private int TweenId;

    [Space]
    public bool _Shake = false;
    protected int _ShakeRefCount = 0;
    
    public bool Shaking {
        get { return _Shake || (_ShakeRefCount > 0); }
        set {
            _ShakeRefCount += value ? 1 : -1;
        }
    }

    protected Kataklizma.Gameplay.EntityAttributes Attributes;

    // Use this for initialization
    void Start () {
        trackedOrigin = transform.position;

        Attributes = GetComponent<Kataklizma.Gameplay.EntityAttributes>();
        Attributes.ValueChanged += Attributes_ValueChanged;
    }

    private void Attributes_ValueChanged(Object sender, ValueType type, ValueSubtype suybtype, float oldValue) {
        
        if ( type == ValueType.Health) {
            Shaking = true;

            if (Attributes[ValueType.Health] > 0)
                StartCoroutine(this.DelayedAction(DurationOnDamage, () => { Shaking = false; }));
        }
    }

    void Update() {
        if (Shaking && !(IsInvoking("Tween") || LeanTween.isTweening(TweenId)))
            Tween();
    }


    void Tween()
    {
        if (!Shaking) {


            TweenId = LeanTween.value(gameObject, (value) => {

                var posn = transform.position;
                posn.x = value.x;
                posn.z = value.y;

                transform.position = posn;

            }, new Vector2(transform.position.x, transform.position.z), new Vector2(trackedOrigin.x, trackedOrigin.z), Speed).id;

            return;
        }

        //LeanTween.moveX(gameObject, 1, 0.2f);
        var current = new Vector2(transform.position.x, transform.position.z);
        var target = new Vector2(trackedOrigin.x , trackedOrigin.z) + Random.insideUnitCircle * Displacement;

        TweenId = LeanTween.value(gameObject, (value) =>
        {

            var posn = transform.position;
            posn.x = value.x;
            posn.z = value.y;

            transform.position = posn;

        }, current, target, Speed)
        .setOnComplete(() => { Invoke("Tween", 0); }).id;
        
    }

    void TurnOff() {
        enabled = false;
    }
}
