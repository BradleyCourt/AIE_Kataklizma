using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static partial class Extensions {

	public static Collider[] OverlapCone( this Collider[] lhs, Vector3 position, Vector3 direction, float radius ) {

        var discrim = Mathf.Cos(radius);

        return lhs.Where(m => Vector3.Dot((m.transform.position - position).normalized, direction) <= discrim).ToArray();

    }

    public static IEnumerator DelayedAction(this MonoBehaviour obj, float delay, System.Action action) {

        yield return new WaitForSeconds(delay);

        if (action != null) action();

    }

    public static T FindType<T>(this object[] lhs) where T : class {
        T result = null;
        foreach (var element in lhs) {
            result = element as T;
            if (result != null) break;
        }

        return result;
    }
}
