using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Raycast Event
    /// Raycast behaviour with customizable events
    /// </summary>
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Modifiers/Raycast Event")]
    public class MDM_RaycastEvent : MonoBehaviour
    {
        public bool ppUpdateRayPerFrame = true;

        public float ppRayLength = 5.0f;
        public bool ppPointRay = true;
        public float ppSphericalRadius = 0.2f;
        public bool ppLocalRay = true;
        public Vector3 ppGlobalRayDir = new Vector3(0, -1, 0);

        public LayerMask ppRayLayer = ~0;
        public bool ppRaycastWithSpecificTag = false;
        public string ppRaycastTag = "";

        public UnityEvent ppEventOnRaycast;
        public UnityEvent ppEventOnRaycastExit;

        public RaycastHit[] hits;
        public Ray ray;

        private void OnDrawGizmosSelected()
        {
            if (!ppPointRay) Gizmos.DrawWireSphere(transform.position + (ppLocalRay ? transform.forward : ppGlobalRayDir.normalized) * ppRayLength, ppSphericalRadius);
            Gizmos.DrawLine(transform.position, transform.position + (ppLocalRay ? transform.forward : ppGlobalRayDir.normalized) * ppRayLength);
        }

        private void Update()
        {
            if (ppUpdateRayPerFrame)
                RayEvent_UpdateRaycastState();
        }

        /// <summary>
        /// Get Raycast state
        /// </summary>
        public bool RayEvent_IsRaycasting()
        {
            return raycastingState;
        }

        internal bool raycastingState = false;

        /// <summary>
        /// Update current Raycast (If 'Update Ray Per Frame' is disabled)
        /// </summary>
        public void RayEvent_UpdateRaycastState()
        {
            ray = new Ray(transform.position, ppLocalRay ? transform.forward : ppGlobalRayDir.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, ppRayLength, ppRayLayer))
            {
                if (raycastingState) ppEventOnRaycastExit?.Invoke();
                raycastingState = false;
                return;
            }
            else if (ppRaycastWithSpecificTag && hit.collider.tag != ppRaycastTag)
            {
                if (raycastingState) ppEventOnRaycastExit?.Invoke();
                raycastingState = false;
                return;
            }

            raycastingState = true;

            hits = new RaycastHit[0];
            if (ppPointRay)
                hits = Physics.RaycastAll(ray, ppRayLength, ppRayLayer);
            else
                hits = Physics.SphereCastAll(ray, ppSphericalRadius, ppRayLength, ppRayLayer);

            if (hits.Length > 0) ppEventOnRaycast?.Invoke();
        }
    }
}
#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CustomEditor(typeof(MDM_RaycastEvent))]
    [CanEditMultipleObjects]
    public class MDM_RaycastEvent_Editor : MD_EditorUtilities
    {
        private MDM_RaycastEvent m;
        private void OnEnable()
        {
            m = (MDM_RaycastEvent)target;
        }

        public override void OnInspectorGUI()
        {
            ps();
            pv();
            ppDrawProperty("ppUpdateRayPerFrame", "Update Ray Per Frame", "If disabled, you are able to invoke your own method to Update ray state");
            ps();
            pv();
            ppDrawProperty("ppRayLength", "Ray Length");
            ppDrawProperty("ppPointRay", "Pointed Ray", "If disabled, raycast will be generated as a 'Spherical Ray'");
            if (!m.ppPointRay)
                ppDrawProperty("ppSphericalRadius", "Radius");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppLocalRay", "Local Direction","If disabled, the ray's direction will be related to the world-space");
            if (m.ppLocalRay == false)
                ppDrawProperty("ppGlobalRayDir", "Global Ray Direction");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppRayLayer", "Allowed Layer", "Allowed layer list for the ray");
            ppDrawProperty("ppRaycastWithSpecificTag", "Raycast Specific Tag", "If disabled, raycast will accept every object with collider");
            if (m.ppRaycastWithSpecificTag)
            {
                pplus();
                ppDrawProperty("ppRaycastTag", "Raycast Tag");
                pminus();
            }
            pve();
            pve();
            ps();
            ppDrawProperty("ppEventOnRaycast", "Event Raycast Hit", "Event on raycast enter");
            ppDrawProperty("ppEventOnRaycastExit", "Event Raycast Exit", "Event on raycast exit");

            if (target != null) serializedObject.Update();
        }
    }
}
#endif
