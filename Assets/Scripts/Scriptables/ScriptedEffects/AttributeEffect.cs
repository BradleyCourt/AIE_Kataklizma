using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Kataklizma.Gameplay;

namespace Scriptables {

    [CreateAssetMenu(fileName = "AttributeEffect", menuName = "Kataklizma/Effects/AttributeEffect")]
    public class AttributeEffect : ScriptedEffect {



        //public ValueSubtype AffectType;
        public bool AffectsSelf = false;
        public bool AffectsOthers = true;
        public List<ValueCollection.Value> Effects;

        protected EntityAttributes _SelfAttributes;
        protected EntityAttributes SelfAttributes {
            get {
                if (Owner == null) return null;
                if ( _SelfAttributes == null ) {
                    _SelfAttributes = Owner.GetComponent<EntityAttributes>();
                }
                return _SelfAttributes;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void Apply(Params data) {
            if (IsRemovable && IsApplied) return; // Is already applied
            base.Apply(data);

            if (Owner == null) return;

            if (AffectsSelf) {
                SelfAttributes.ApplyEffects(Effects);
            }

            if (AffectsOthers && data.Hits != null) {
                foreach (var hit in data.Hits) {
                    var attribs = hit.GetComponent<EntityAttributes>();
                    if (attribs == null) continue; // No Attribute set to affect

                    attribs.ApplyEffects(Effects);
                }
            }
        }

        public override void Remove() {
            if (!IsRemovable || !IsApplied) return; // Is not removable/applied
            base.Remove();
        }
    }
}
