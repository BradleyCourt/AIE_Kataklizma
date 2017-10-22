using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "AttributeEffect", menuName = "Kataklizma/Effects/AttributeEffect")]
    public class AttributeEffect : ScriptedEffect {



        //public ValueSubtype AffectType;
        public bool AffectsSelf = false;
        public bool AffectsOthers = true;
        public List<ValueCollection.Value> Effects;

        protected Gameplay.EntityAttributes _SelfAttributes;
        protected Gameplay.EntityAttributes SelfAttributes {
            get {
                if (Owner == null) return null;
                if ( _SelfAttributes == null ) {
                    _SelfAttributes = Owner.GetComponent<Gameplay.EntityAttributes>();
                }
                return _SelfAttributes;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void Apply(Params data) {
            if (Owner == null) return;

            if (AffectsSelf) {
                SelfAttributes.ApplyEffects(Effects);
            }

            if (AffectsOthers && data.Hits != null) {
                foreach (var hit in data.Hits) {
                    var attribs = hit.GetComponent<Gameplay.EntityAttributes>();
                    if (attribs == null) continue; // No Attribute set to affect

                    attribs.ApplyEffects(Effects);
                }
            }
        }
    }
}
