using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGen {
    public class MapTilePreset : MonoBehaviour{

        public MapTileType Size;

        [Space]
        public bool UnshrinkOnSpawn = false;
        public bool UnshrinkTimeScales = true;

        public float UnshrinkTime = 1;

        protected Vector3 trackedOrigin;

        protected float currentAnimTime;

        protected bool unshrinking = false;
        protected float totalAnimTime;
        protected float endScale;
        protected Vector3 offset;

        void Start() {

            if (UnshrinkOnSpawn) {
                totalAnimTime = (UnshrinkTime * (UnshrinkTimeScales ? (int)Size : 1));
                currentAnimTime = 0;

                trackedOrigin = transform.position;

                endScale = transform.localScale.x;
                offset = (transform.forward + transform.right) * (0.5f * (int)Size);

                transform.localScale = Vector3.zero;

                unshrinking = true;
            } else
                ActivateMeshDisablers();
        }


        void Update() {

            if (unshrinking ) {
                if (currentAnimTime < totalAnimTime) {
                    currentAnimTime += Time.deltaTime;

                    var timePct = currentAnimTime / totalAnimTime;

                    Tween(timePct);
                    
                }
                else {
                    unshrinking = false;
                    ActivateMeshDisablers();
                }
                
            }
        }

        void Tween(float value) {
            transform.localScale = Vector3.one * value * endScale;
            transform.position = trackedOrigin + offset * (1.0f - value);
        }


        void ActivateMeshDisablers() {
            var disablers = GetComponentsInChildren<MeshDisable>();
            foreach (var disabler in disablers)
                if (disabler) disabler.enabled = true;
        }
                
    }
}