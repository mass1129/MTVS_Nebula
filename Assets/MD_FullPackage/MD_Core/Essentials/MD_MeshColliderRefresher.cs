using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MD(Mesh Deformation) Essential Component: Mesh Collider Refresher
    /// Essential component for general mesh-collider refreshing
    /// </summary>
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Mesh Collider Refresher")]
    public class MD_MeshColliderRefresher : MonoBehaviour
    {
        public enum RefreshType { Once, PerFrame, Interval, Never };
        public RefreshType ppRefreshType = RefreshType.Once;

        public float ppIntervalSeconds = 1f;
        public bool ppConvexMeshCollider = false;
        public MeshColliderCookingOptions ppCookingOptions = ~MeshColliderCookingOptions.None;

        public bool ppIgnoreRaycast = false;

        public Vector3 ppColliderOffset = Vector3.zero;

        private MeshCollider mcCache;
        private MeshFilter selfRenderCache;

        private void Awake()
        {
            if (!selfRenderCache) selfRenderCache = GetComponent<MeshFilter>();
            if (!mcCache) mcCache = GetComponent<MeshCollider>();

            if (ppRefreshType == RefreshType.Never) return;
            MeshCollider_UpdateMeshCollider();
        }

        private float intervalTimer = 0;
        private void LateUpdate()
        {
            if (ppRefreshType == RefreshType.PerFrame)
                MeshCollider_UpdateMeshCollider();
            else if (ppRefreshType == RefreshType.Interval)
            {
                intervalTimer += Time.deltaTime;
                if (intervalTimer > ppIntervalSeconds)
                {
                    MeshCollider_UpdateMeshCollider();
                    intervalTimer = 0;
                }
            }
        }

        /// <summary>
        /// Update current mesh collider
        /// </summary>
        public void MeshCollider_UpdateMeshCollider()
        {
            if (ppRefreshType == RefreshType.Never) return;

            if (ppIgnoreRaycast) gameObject.layer = 2;

            if (!selfRenderCache) selfRenderCache = GetComponent<MeshFilter>();

            if (!selfRenderCache || (selfRenderCache && !selfRenderCache.sharedMesh))
            {
                MD_Debug.Debug(this, "Object " + this.name + " doesn't contain any Mesh Renderer Component. Mesh Collider Refresher could not be proceeded", MD_Debug.DebugType.Error);
                return;
            }

            if (!mcCache) mcCache = GetComponent<MeshCollider>(); 
            if (!mcCache) mcCache = gameObject.AddComponent<MeshCollider>();

            mcCache.sharedMesh = selfRenderCache.sharedMesh;
            mcCache.convex = ppConvexMeshCollider;
            mcCache.cookingOptions = ppCookingOptions;

            if (ppRefreshType != RefreshType.Once)
                return;
            if (ppColliderOffset == Vector3.zero)
                return;

            Mesh newMeshCol = new Mesh();
            newMeshCol.vertices = mcCache.sharedMesh.vertices;
            newMeshCol.triangles = mcCache.sharedMesh.triangles;
            newMeshCol.normals = mcCache.sharedMesh.normals;
            Vector3[] verts = newMeshCol.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] += ppColliderOffset;
            newMeshCol.vertices = verts;
            mcCache.sharedMesh = newMeshCol;
        }
    }
}

#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CustomEditor(typeof(MD_MeshColliderRefresher))]
    [CanEditMultipleObjects]
    public class MD_MeshColliderRefresher_Editor : MD_EditorUtilities
    {
        private MD_MeshColliderRefresher m;

        private void OnEnable()
        {
            m = (MD_MeshColliderRefresher)target;
        }

        public override void OnInspectorGUI()
        {
            ps();
            Color c;
            ColorUtility.TryParseHtmlString("#9fe6b2", out c);
            GUI.color = c;
            pv();
            pv();
            ppDrawProperty("ppRefreshType", "Collider Refresh Type","Once - Refreshes the collider just once in the startup. Per Frame - Refreshes the collider every frame after start. Interval - Refreshes the collider in the specific interval after start. Never - Never refreshes the collider.");
            if (m.ppRefreshType == MD_MeshColliderRefresher.RefreshType.Interval)
                ppDrawProperty("ppIntervalSeconds", "Interval (every N second)", "Set the interval value for mesh collider refreshing in seconds");
            else if (m.ppRefreshType == MD_MeshColliderRefresher.RefreshType.Once)
                ppDrawProperty("ppColliderOffset", "Collider Offset", "Specific offset of the mesh collider generated after start");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppConvexMeshCollider", "Convex Mesh Collider");
            ppDrawProperty("ppCookingOptions", "Cooking Options", "Specify the mesh collider in higher details by choosing proper cooking options");
            pve();
            ps(5);
            pv();
            ppDrawProperty("ppIgnoreRaycast", "Ignore Raycast", "If enabled, the objects layer mask will be set to 2 [Ignore raycast]. Otherwise the masks will be untouched");
            pve();
            if (pb("Add Mesh Collider Now"))
                m.gameObject.AddComponent<MeshCollider>();
            pve();

            if (target != null) serializedObject.Update();
        }
    }
}
#endif
