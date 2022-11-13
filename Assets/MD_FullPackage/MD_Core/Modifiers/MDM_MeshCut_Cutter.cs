using UnityEngine;

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Mesh Cut - Cutter source
    /// Apply this component to any planar object that will represent a cut transform with upwards-cut-direction
    /// </summary>
    public class MDM_MeshCut_Cutter : MonoBehaviour
    {
        [Space]
        [Tooltip("Virtual cutter size vector. You can naturally re-scale the object.")]
        public Vector2 cutterSize = new Vector2(2, 2);
        [Tooltip("Quality of the raycast - how many iterations will proceed?")]
        [Range(4, 128)] public int cutterIterations = 8;
        [Space]
        public bool showIterations = true;
        [Tooltip("Cutter color - Editor only.")]
        public Color cutterColor = Color.cyan;

        internal Vector3 position { get { return transform.position; } }
        internal Vector3 scale { get { return transform.localScale * cutterSize; } }

        internal Vector3 GetHorizontalVec { get { return (((-transform.forward * 0.5f) * transform.localScale.z) * cutterSize.x) * transform.localScale.z; } }
        internal Vector3 GetVerticalVec(int index, float step, bool reversed = false)
        {
            return (((((reversed  ? -transform.up : transform.up) * 0.5f)) * (step * (index + 1))) * transform.localScale.y);
        }
        internal Vector3 GetHorizontalDirection { get { return transform.forward * GetHorizontalMultiplication; } }
        internal float GetHorizontalMultiplication { get { return transform.localScale.z * cutterSize.x * transform.localScale.z; } }

        private void OnDrawGizmos()
        {
            Gizmos.color = cutterColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.0f, cutterSize.y * transform.localScale.y, cutterSize.x * transform.localScale.z));
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.0f, cutterSize.y * transform.localScale.y - 0.3f, cutterSize.x * transform.localScale.z - 0.3f));

            cutterSize.x = Mathf.Abs(cutterSize.x);
            cutterSize.y = Mathf.Abs(cutterSize.y);

            if (!showIterations) return;

            int iteraction = cutterIterations < 4 ? 4 : cutterIterations;
            int half = iteraction / 2;
            float y = (cutterSize.y * transform.localScale.y) / half;

            // Ray Visualization
            //------------------------------
            for (int i = 0; i < half; i++)
                Debug.DrawRay(transform.position + GetHorizontalVec + GetVerticalVec(i,y), GetHorizontalDirection);
            Debug.DrawRay(transform.position + GetHorizontalVec, GetHorizontalDirection);
            for (int i = 0; i < half; i++)
                Debug.DrawRay(transform.position + GetHorizontalVec + GetVerticalVec(i, y, true), GetHorizontalDirection);
            //------------------------------
        }

        /// <summary>
        /// Check if the current Mesh Cutter intersects the specific object
        /// </summary>
        /// <returns>Returns true if the intersection proceeds successfully</returns>
        public bool HitObject(GameObject obj)
        {
            int iteraction = cutterIterations < 4 ? 4 : cutterIterations;
            int half = iteraction / 2;
            float y = (cutterSize.y * transform.localScale.y) / half;
            bool res;

            for (int i = 0; i < half; i++)
            {
                res = CastRay(transform.position + GetHorizontalVec + GetVerticalVec(i, y), GetHorizontalDirection, obj);
                if (res) return true;
            }
            res = CastRay(transform.position + GetHorizontalVec, GetHorizontalDirection, obj);
            if (res) return true;

            for (int i = 0; i < half; i++)
            {
                res = CastRay(transform.position + GetHorizontalVec + GetVerticalVec(i, y, true), GetHorizontalDirection, obj);
                if (res) return true;
            }

            return res;
        }

        private bool CastRay(Vector3 p, Vector3 d, GameObject obj)
        {
            Ray r = new Ray(p, d.normalized);

            if (Physics.Raycast(r, out RaycastHit h, GetHorizontalMultiplication))
            {
                if (h.collider && h.collider.gameObject == obj)
                    return true;
            }
            return false;
        }
    }
}