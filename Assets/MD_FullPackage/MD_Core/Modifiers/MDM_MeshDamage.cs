using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Mesh Damage
    /// Damage/ Distort mesh by the specific parameters
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Modifiers/Mesh Damage")]
    public class MDM_MeshDamage : MonoBehaviour
    {
        public bool ppRecalculateNormals = true;
        public bool ppRecalculateBounds = true;

        public bool ppDetectForceImpact = true;
        public float ppForceAmount = 0.15f;
        public float ppForceMultiplier = 0.075f;
        public bool ppDetectRadiusSize = false;
        public float ppRadius = 0.5f;
        public float ppRadiusMultiplier = 1.0f;
        public float ppRadiusSoftness = 1.0f;
        public float ppForceDetection = 1.5f;

        public bool ppContinousDamage = false;

        public bool ppCollisionWithSpecificTag = false;
        public string ppCollisionTag = "";

        public bool ppEnableEvent;
        public UnityEvent ppEvent;

        private Vector3[] initialVertices;
        private Vector3[] originalVertices;
        private Vector3[] workingVertices;

        [SerializeField] private MeshFilter meshFilter;

        private void Awake()
        {
            if (meshFilter != null) return;

            ppRecalculateBounds = MD_GlobalPreferences.AutoRecalcBounds;
            ppRecalculateNormals = MD_GlobalPreferences.AutoRecalcNormals;

            meshFilter = GetComponent<MeshFilter>();
            MD_MeshProEditor.MeshProEditor_Utilities.Util_PrepareMeshDeformationModifier(this, meshFilter);
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                initialVertices = meshFilter.sharedMesh.vertices;
                MeshDamage_RefreshVertices();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!Application.isPlaying)
                return;
            if (collision.contactCount == 0)
                return;
            if (ppForceDetection != 0 && collision.relativeVelocity.magnitude < ppForceDetection)
                return;
            if (ppCollisionWithSpecificTag && ppCollisionTag != collision.gameObject.tag)
                return;
            if (ppDetectForceImpact)
                ppForceAmount = collision.relativeVelocity.magnitude * ppForceMultiplier;
            if (ppDetectRadiusSize)
                ppRadius = collision.transform.localScale.magnitude / 4;
            for(int i = 0; i < collision.contactCount; i++)
                MeshDamage_ModifyMesh(collision.GetContact(i).point, ppRadius, ppForceAmount, collision.relativeVelocity, ppContinousDamage);

            if (ppEnableEvent) ppEvent?.Invoke();
        }

        /// <summary>
        /// Modify current mesh by the point, radius, force and direction
        /// </summary>
        public void MeshDamage_ModifyMesh(Vector3 point, float radius, float force, Vector3 direction, bool continuousEffect = false)
        {
            radius *= ppRadiusMultiplier;
            for (int i = 0; i < workingVertices.Length; i++)
            {
                Vector3 ppp = transform.TransformPoint(originalVertices[i]);
                float distance = Vector3.Distance(point, ppp);
                if (distance < radius)
                {
                    float radDist = (radius - distance);
                    ppp += (direction.normalized * force) * (ppRadiusSoftness == 1.0f ? radDist : Mathf.Pow(radDist, ppRadiusSoftness));
                    workingVertices[i] = transform.InverseTransformPoint(ppp);
                }
            }

            meshFilter.mesh.SetVertices(workingVertices);
            if (ppRecalculateNormals) meshFilter.mesh.RecalculateNormals();
            if (ppRecalculateBounds) meshFilter.mesh.RecalculateBounds();

            if (continuousEffect)
                MeshDamage_RefreshVertices();
        }

        /// <summary>
        /// Refresh vertices & register brand new original vertices state
        /// </summary>
        public void MeshDamage_RefreshVertices()
        {
            originalVertices = meshFilter.mesh.vertices;
            workingVertices = meshFilter.mesh.vertices;
        }

        /// <summary>
        /// Repair deformed mesh by the specified speed value
        /// </summary>
        public void MeshDamage_RepairMesh(float speed = 0.5f)
        {
            for (int i = 0; i < workingVertices.Length; i++)
                workingVertices[i] = Vector3.Lerp(workingVertices[i], initialVertices[i], speed * Time.deltaTime);

            meshFilter.mesh.SetVertices(workingVertices);
            if (ppRecalculateNormals) meshFilter.mesh.RecalculateNormals();
            if (ppRecalculateBounds) meshFilter.mesh.RecalculateBounds();
        }

        /// <summary>
        /// Modify current mesh by the custom RaycastEvent
        /// </summary>
        public void MeshDamage_ModifyMesh(MDM_RaycastEvent RayEvent)
        {
            if (!Application.isPlaying)
                return;
            if (RayEvent == null)
                return;
            if (RayEvent.hits.Length > 0 && RayEvent.hits[0].collider.gameObject != this.gameObject)
                return;
            if (ppDetectRadiusSize)
            {
                if (!RayEvent.ppPointRay)
                    ppRadius = RayEvent.ppSphericalRadius;
                else
                    ppRadius = 0.1f;
            }

            foreach (RaycastHit hit in RayEvent.hits)
                MeshDamage_ModifyMesh(hit.point, ppRadius, ppForceAmount, RayEvent.ray.direction, ppContinousDamage);
        }
    }
}

#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CustomEditor(typeof(MDM_MeshDamage))]
    [CanEditMultipleObjects]
    public class MDM_MeshDamage_Editor : MD_EditorUtilities
    {
        private MDM_MeshDamage m;

        private void OnEnable()
        {
            m = (MDM_MeshDamage)target;
        }

        public override void OnInspectorGUI()
        {
            ps();

            pv();
            ppDrawProperty("ppRecalculateNormals", "Recalculate Normals");
            ppDrawProperty("ppRecalculateBounds", "Recalculate Bounds");
            pve();

            ps(20);

            pl("General Settings", true);
            pv();
            pv();
            ppDrawProperty("ppDetectRadiusSize", "Detect Radius Size", "Adjust radius size by the collided objects. This will try to auto-detect the interaction radius with the collided rigidbody transform scales");
            if (!m.ppDetectRadiusSize)
            {
                EditorGUI.indentLevel++;
                ppDrawProperty("ppRadius", "Interaction Radius");
                EditorGUI.indentLevel--;
            }
            ppDrawProperty("ppRadiusMultiplier","Radius Multiplier", "General radius multiplier. Multiplies the radius constant or auto-detected radius - default value is 1");
            ppDrawProperty("ppRadiusSoftness", "Radius Softness", "General radius softness - the higher the value is, the softer the results are. Default value is 1 - if the value is different, takes more performance");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppDetectForceImpact", "Detect Force Impact", "If enabled (Recommended), the system will try to detect a force impact automatically with the collided rigidbodies");
            if (!m.ppDetectForceImpact)
                ppDrawProperty("ppForceAmount", "Impact Force", "Constant force value applied to the interacted vertices");
            else
            {
                pplus();
                ppDrawProperty("ppForceMultiplier", "Force Multiplier", "Multiplier of the applied force from the rigidbody");
                pminus();
            }
            pve();
            pve();

            ps(20);

            pl("Conditions", true);
            pv();
            pv();
            ppDrawProperty("ppForceDetection", "Force Detection Level", "Minimum relative velocity impact detection level - what should be the minimum velocity for the rigidbodies to damage this mesh?");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppContinousDamage", "Continuous Effect", "If enabled, vertices of the mesh will be able to move beyond their initial location");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppCollisionWithSpecificTag", "Collision With Specific Tag", "If enabled, collision will be allowed for objects with the tag below");
            if (m.ppCollisionWithSpecificTag)
            {
                pplus();
                ppDrawProperty("ppCollisionTag", "Collision Tag");
                pminus();
            }
            pve();
            pve();

            ps(20);

            pv();
            ppDrawProperty("ppEnableEvent", "Enable Event System");
            if (m.ppEnableEvent)
                ppDrawProperty("ppEvent", "Event On Collision", "Event will be proceeded after successful collision");
            pve();

            ps(15);

            ppAddMeshColliderRefresher(m.gameObject);
            ppBackToMeshEditor(m);

            if (target != null) serializedObject.Update();
        }
    }
}
#endif
