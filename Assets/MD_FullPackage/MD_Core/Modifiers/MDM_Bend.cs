using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Mesh Bend
    /// Bend mesh to the specific direction & with specific value
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Modifiers/Mesh Bend")]
    public class MDM_Bend : MonoBehaviour
    {
        public bool ppUpdateEveryFrame = false;
        public bool ppRecalculateNormals = true;
        public bool ppAlternateNormals = false;
        public float ppAlternateNormalsAngle = 90.0f;
        public bool ppRecalculateBounds = true;

        public enum Direction_ { X, Y, Z }
        public Direction_ ppBendDirection = Direction_.X;

        public float ppValue = 0;
        public bool ppMirrored = false;

        [SerializeField] private Vector3[] originalVertices;
        [SerializeField] private Vector3[] workingVertices;

        [SerializeField] private MeshFilter meshFilter;

        private void Awake()
        {
            if (meshFilter != null) return;

            ppRecalculateBounds = MD_GlobalPreferences.AutoRecalcBounds;
            ppRecalculateNormals = MD_GlobalPreferences.AutoRecalcNormals;
            ppAlternateNormals = MD_GlobalPreferences.AlternateNormalsRecalc;

            meshFilter = GetComponent<MeshFilter>();
            MD_MeshProEditor.MeshProEditor_Utilities.Util_PrepareMeshDeformationModifier(this, meshFilter);
            Bend_RegisterCurrentState();
        }

        private void Update()
        {
            if (!ppUpdateEveryFrame) return;
            if (meshFilter.sharedMesh == null) return;

            Bend_ProcessBend();
        }

        private Vector3 BendVertex(Vector3 vert, float val)
        {
            if (val == 0.0f) return vert;
            if (!ppMirrored && vert.y < 0) return vert;
            float rotExpl;
            float rotS;
            float rotC;

            switch (ppBendDirection)
            {
                case Direction_.X:
                    rotExpl = (Mathf.PI / 2) + (val * vert.z);

                    rotS = Mathf.Sin(rotExpl) * ((1 / val) + vert.x);
                    rotC = Mathf.Cos(rotExpl) * ((1 / val) + vert.x);

                    vert.z = -rotC;
                    vert.x = rotS - (1 / val);
                    break;

                case Direction_.Y:
                    rotExpl = (Mathf.PI / 2) + (val * vert.y);

                    rotS = Mathf.Sin(rotExpl) * ((1 / val) + vert.x);
                    rotC = Mathf.Cos(rotExpl) * ((1 / val) + vert.x);

                    vert.y = -rotC;
                    vert.x = rotS - (1 / val);
                    break;

                case Direction_.Z:
                    rotExpl = (Mathf.PI / 2) + (val * vert.x);

                    rotS = Mathf.Sin(rotExpl) * ((1 / val) + vert.z);
                    rotC = Mathf.Cos(rotExpl) * ((1 / val) + vert.z);

                    vert.x = -rotC;
                    vert.z = rotS - (1 / val);
                    break;
            }

            return vert;
        }

        /// <summary>
        /// Process the bend effect itself
        /// </summary>
        public void Bend_ProcessBend()
        {
            if (!ppUpdateEveryFrame) MD_MeshProEditor.MeshProEditor_Utilities.Util_CheckVerticeCount(originalVertices.Length, this.gameObject);

            for (int i = 0; i < originalVertices.Length; i++) workingVertices[i] = BendVertex(originalVertices[i], ppValue);
            meshFilter.sharedMesh.vertices = workingVertices;
            if (ppRecalculateNormals)
            {
                if (!ppAlternateNormals)
                    meshFilter.sharedMesh.RecalculateNormals();
                else
                    MD_MeshMathUtilities.AlternativeNormalCalculation.RecalculateNormals(meshFilter.sharedMesh, ppAlternateNormalsAngle);
            }
            if (ppRecalculateBounds) meshFilter.sharedMesh.RecalculateBounds();
        }

        /// <summary>
        /// Refresh & register current mesh state. This will refresh original vertices to the current state
        /// </summary>
        public void Bend_RegisterCurrentState()
        {
            originalVertices = meshFilter.sharedMesh.vertices;
            workingVertices = new Vector3[originalVertices.Length];
            System.Array.Copy(originalVertices, workingVertices, originalVertices.Length);
            ppValue = 0;
        }

        /// <summary>
        /// Bend object by the UI Slider value
        /// </summary>
        public void Bend_BendObject(Slider entry)
        {
            ppValue = entry.value;
            if (!ppUpdateEveryFrame) Bend_ProcessBend();
        }
        /// <summary>
        /// Bend object by the float value
        /// </summary>
        /// <param name="entry"></param>
        public void Bend_BendObject(float entry)
        {
            ppValue = entry;
            if (!ppUpdateEveryFrame) Bend_ProcessBend();
        }
    }
}

#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CustomEditor(typeof(MDM_Bend))]
    [CanEditMultipleObjects]
    public class MDM_Bend_Editor : MD_EditorUtilities
    {
        private MDM_Bend m;

        private void OnEnable()
        {
            m = (MDM_Bend)target;
        }

        public override void OnInspectorGUI()
        {
            ps();
            pv();
            ppDrawProperty("ppUpdateEveryFrame", "Update Every Frame");
            pplus(); 
            ppDrawProperty("ppRecalculateNormals", "Recalculate Normals");
            ppDrawProperty("ppAlternateNormals", "Alternate Normals", "Use alternate normals recalculation - results are better, but takes more performance (Great for spatial meshes)");
            if (m.ppAlternateNormals)
            {
                pplus();
                ppDrawProperty("ppAlternateNormalsAngle", "Angle");
                pminus();
            }
            ppDrawProperty("ppRecalculateBounds", "Recalculate Bounds");
            pminus();
            if (!m.ppUpdateEveryFrame)
            {
                if (pb("Update Mesh")) m.Bend_ProcessBend();
            }
            pve();
            ps();
            pv();
            ppDrawProperty("ppBendDirection", "Bend Direction");
            ppDrawProperty("ppValue", "Bend Value");
            ppDrawProperty("ppMirrored", "Mirrored", "If enabled, the bend will process on both sides of the mesh");
            pve();
            ps();
            pv();
            if (pb("Register Mesh")) m.Bend_RegisterCurrentState();
            phb("Refresh current mesh & register original vertices to the edited vertices");
            pve();
            ps(15);
            ppAddMeshColliderRefresher(m.gameObject);
            ppBackToMeshEditor(m);

            if (target != null) serializedObject.Update();
        }
    }
}
#endif

