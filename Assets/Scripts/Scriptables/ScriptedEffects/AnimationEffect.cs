using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    [CreateAssetMenu(fileName = "AnimationEffect", menuName = "Kataklizma/Effects/Animation Effect")]
    public class AnimationEffect : ScriptedEffect {

        public string AnimationName;

        protected Animator _OwnerAnimator;
        protected Animator OwnerAnimator {
            get {
                if (Owner == null) return null;
                if (_OwnerAnimator == null)
                    _OwnerAnimator = Owner.GetComponentInChildren<Animator>();

                return _OwnerAnimator;
            }
        }

        public override bool Bind(Transform owner) {
            if (!base.Bind(owner)) return false;

            return true;
        }

        public override void Apply(Params data) {
            if (IsRemovable && IsApplied) return; // Is already applied
            base.Apply(data);

            if (OwnerAnimator != null && !string.IsNullOrEmpty(AnimationName))
                OwnerAnimator.SetTrigger(AnimationName);
        }

        public override void Remove() {
            if (!IsRemovable || !IsApplied) return; // Is not removable/applied
            base.Remove();

            if (OwnerAnimator != null && !string.IsNullOrEmpty(AnimationName))
                //OwnerAnimator.Stop(AnimationName);
                OwnerAnimator.ResetTrigger(AnimationName);
        }
    }
}
