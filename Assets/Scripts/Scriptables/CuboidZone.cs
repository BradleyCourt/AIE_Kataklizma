using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "CuboidZone", menuName = "Volumes/Cuboid Zone")]
    public class CuboidZone : AbstractZone {

        public Bounds Bounds;

        public override Transform[] ZoneCast(Transform origin) {
            throw new NotImplementedException();
        }
    }
}
