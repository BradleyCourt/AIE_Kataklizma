using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Extensions {

	public static Collider[] OverlapCone( this Collider[] lhs, Vector3 position, Vector3 direction, float radius ) {

        var discrim = Mathf.Cos(radius);

        return lhs.Where(m => Vector3.Dot((m.transform.position - position).normalized, direction) <= discrim).ToArray();

    }
}
