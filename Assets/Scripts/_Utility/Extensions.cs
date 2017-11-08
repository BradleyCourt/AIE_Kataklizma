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

    public static Quaternion GetRotation( this Matrix4x4 lhs ) {
        //// Sourced from: http://answers.unity3d.com/questions/11363/converting-matrix4x4-to-quaternion-vector3.html
        //// > Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
        //Quaternion q = new Quaternion();
        //q.w = Mathf.Sqrt(Mathf.Max(0, 1 + lhs[0, 0] + lhs[1, 1] + lhs[2, 2])) / 2;
        //q.x = Mathf.Sqrt(Mathf.Max(0, 1 + lhs[0, 0] - lhs[1, 1] - lhs[2, 2])) / 2;
        //q.y = Mathf.Sqrt(Mathf.Max(0, 1 - lhs[0, 0] + lhs[1, 1] - lhs[2, 2])) / 2;
        //q.z = Mathf.Sqrt(Mathf.Max(0, 1 - lhs[0, 0] - lhs[1, 1] + lhs[2, 2])) / 2;
        //q.x *= Mathf.Sign(q.x * (lhs[2, 1] - lhs[1, 2]));
        //q.y *= Mathf.Sign(q.y * (lhs[0, 2] - lhs[2, 0]));
        //q.z *= Mathf.Sign(q.z * (lhs[1, 0] - lhs[0, 1]));
        //return q;


        // Sourced from: http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
        return Quaternion.LookRotation(
            lhs.GetColumn(2),
            lhs.GetColumn(1));
    }

    public static Vector3 GetPosition( this Matrix4x4 lhs ) {
        // Sourced from: http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
        return lhs.GetColumn(3);
    }

    public static Vector3 GetScale(this Matrix4x4 lhs ) {
        // Sourced from: http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
        return new Vector3(
            lhs.GetColumn(0).magnitude,
            lhs.GetColumn(1).magnitude,
            lhs.GetColumn(2).magnitude);
    }
}
