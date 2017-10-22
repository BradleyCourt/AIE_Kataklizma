using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class AnchoredEffect : ScriptedEffect {
        

        public CharacterBindPoint Anchor;

        protected CharacterBindOrigins _OwnerOrigins;
        protected CharacterBindOrigins OwnerOrigins {
            get {
                if (Owner == null) return null;
                if ( _OwnerOrigins == null )
                    _OwnerOrigins = Owner.GetComponentInChildren<CharacterBindOrigins>();

                return _OwnerOrigins;
            }
        }        

    }
}
