using System.Collections.Generic;
using UnityEngine;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Interactive Surface
    /// Interactive mesh surface with physically based system
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Modifiers/Interactive Surface")]
    public class MDM_InteractiveSurface : MonoBehaviour
    {
        public bool ppRecalculateNormals = true;
        public bool ppRecalculateBounds = true;

        public bool ppRigidbodiesAllowed = true;

        public bool ppMultithreadingSupported = false;
        [Range(1, 30)] public int ppThreadSleep = 10;
        protected Thread ppThread;

        public bool ppCustomInteractionSpeed = false;
        public bool ppContinuousEffect = false;
        public float ppInteractionSpeed = 1.5f;

        public bool ppExponentialDeformation = true;
        public float ppRadiusSoftness = 1.5f;

        public Vector3 ppDirection = new Vector3(0, -1, 0);
        public float ppRadius = 0.8f;
        public float ppRadiusMultiplier = 1.0f;
        public bool ppDetectRadiusSize = true;
        public float ppMinimumForceDetection = 0;

        private Vector3[] originalVertices;
        private Vector3[] storedVertices;
        private Vector3[] initialVertices;
        [SerializeField] public MeshFilter meshFilter;

        public bool ppRestoreSurface;
        public float ppRestoreSpeed = 0.5f;

        public bool ppCollideWithSpecificObjects = false;
        public string ppCollisionTag = "";

        private void Awake()
        {
            if (meshFilter != null) return;

            ppRecalculateBounds = MD_GlobalPreferences.AutoRecalcBounds;
            ppRecalculateNormals = MD_GlobalPreferences.AutoRecalcNormals;

            meshFilter = GetComponent<MeshFilter>();
            MD_MeshProEditor.MeshProEditor_Utilities.Util_PrepareMeshDeformationModifier(this, meshFilter, false);
            if (meshFilter.sharedMesh.vertices.Length > MD_GlobalPreferences.VertexLimit)
                ppMultithreadingSupported = true;
        }

        private void Start()
        {
            if (!Application.isPlaying) return;
            if (meshFilter == null)
            {
                MD_Debug.Debug(this, "Mesh filter doesn't exist", MD_Debug.DebugType.Error);
                return;
            }
            if (meshFilter.mesh == null)
            {
                MD_Debug.Debug(this, "Mesh doesn't exist", MD_Debug.DebugType.Error);
                return;
            }

            originalVertices = meshFilter.mesh.vertices;
            initialVertices = meshFilter.mesh.vertices;
            storedVertices = meshFilter.mesh.vertices;

            if (ppMultithreadingSupported)
            {
                Thrd_RealRot = transform.rotation;
                Thrd_RealPos = transform.position;
                Thrd_RealSca = transform.localScale;

                ppThread = new Thread(ThreadWork_ModifyMesh);
                ppThread.Start();
            }
        }

        private bool checkForUpdate_InterSpeed, checkForUpdate_Repair = false;
        private void LateUpdate()
        {
            //Returns if multithreading is enabled (it wouldn't make any sense)
            if (ppMultithreadingSupported) return;

            //Update 'custom interaction'
            if (ppCustomInteractionSpeed)
            {
                if (checkForUpdate_InterSpeed)
                {
                    int doneAll = 0;
                    if (ppContinuousEffect)
                    {
                        for (int i = 0; i < originalVertices.Length; i++)
                        {
                            if (originalVertices[i] == storedVertices[i])
                                doneAll++;
                            originalVertices[i] = Vector3.Lerp(originalVertices[i], storedVertices[i], ppInteractionSpeed * Time.deltaTime);
                        }
                        if (doneAll == originalVertices.Length)
                            checkForUpdate_InterSpeed = false;
                        meshFilter.mesh.SetVertices(originalVertices);
                    }
                    else
                    {
                        List<Vector3> Verts = new List<Vector3>();
                        Verts.AddRange(meshFilter.mesh.vertices);
                        for (int i = 0; i < Verts.Count; i++)
                        {
                            if (Verts[i] == storedVertices[i])
                                doneAll++;
                            Verts[i] = Vector3.Lerp(Verts[i], storedVertices[i], ppInteractionSpeed * Time.deltaTime);
                        }
                        if (doneAll == Verts.Count)
                            checkForUpdate_InterSpeed = false;
                        meshFilter.mesh.SetVertices(Verts);
                    }

                    if (ppRecalculateNormals) meshFilter.mesh.RecalculateNormals();
                    if (ppRecalculateBounds) meshFilter.mesh.RecalculateBounds();
                }
            }

            //Update 'repair surface'
            if (ppRestoreSurface)
            {
                if (checkForUpdate_Repair)
                {
                    int doneAll = 0;
                    for (int i = 0; i < storedVertices.Length; i++)
                    {
                        if (originalVertices[i] == storedVertices[i])
                            doneAll++;
                        storedVertices[i] = Vector3.Lerp(storedVertices[i], initialVertices[i], ppRestoreSpeed * Time.deltaTime);
                    }
                    if (doneAll == storedVertices.Length)
                        checkForUpdate_Repair = false;
                    if (!ppCustomInteractionSpeed)
                    {
                        meshFilter.mesh.SetVertices(storedVertices);
                        if (ppRecalculateNormals) meshFilter.mesh.RecalculateNormals();
                        if (ppRecalculateBounds) meshFilter.mesh.RecalculateBounds();
                    }
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!Application.isPlaying)
                return;
            if (!ppRigidbodiesAllowed)
                return;
            if (collision.contactCount == 0)
                return;
            if (ppMinimumForceDetection != 0 && collision.relativeVelocity.magnitude < ppMinimumForceDetection)
                return;
            if (ppDetectRadiusSize)
                ppRadius = collision.transform.localScale.magnitude / 4;
            for(int i = 0; i < collision.contactCount; i++)
                InteractiveSurface_ModifyMesh(collision.GetContact(i).point, ppRadius, ppDirection);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!Application.isPlaying)
                return;
            if (!ppRigidbodiesAllowed)
                return;
            if (collision.contactCount == 0)
                return;
            if (ppMinimumForceDetection != 0 && collision.relativeVelocity.magnitude < ppMinimumForceDetection)
                return;
            if (ppDetectRadiusSize)
                ppRadius = collision.transform.localScale.magnitude / 4;
            for (int i = 0; i < collision.contactCount; i++)
                InteractiveSurface_ModifyMesh(collision.GetContact(i).point, ppRadius, ppDirection);
        }

        /// <summary>
        /// Main mesh modification with specific point, size and vertice direction
        /// </summary>
        /// <param name="AtPoint">Enter point of modification</param>
        /// <param name="Radius">Enter interaction radius</param>
        /// <param name="Direction">Enter direction of the vertices</param>
        public void InteractiveSurface_ModifyMesh(Vector3 AtPoint, float Radius, Vector3 Direction)
        {
            if (ppDetectRadiusSize == false)
                Radius = ppRadius;

            Radius *= ppRadiusMultiplier;

            //If multithreading enabled, pass data to cross-thread-wrapper
            if (ppMultithreadingSupported)
            {
                Thrd_AtPoint = AtPoint;
                Thrd_Radius = Radius;
                Thrd_Dir = Direction;
                Thrd_RealPos = transform.position;
                Thrd_RealRot = transform.rotation;
                Thrd_RealSca = transform.localScale;
            }
            //Otherwise go for the default main thread
            else
            {
                for (int i = 0; i < storedVertices.Length; i++)
                {
                    Vector3 TransformedPoint = transform.TransformPoint(storedVertices[i]);
                    float distance = Vector3.Distance(AtPoint, TransformedPoint);
                    if (distance < Radius)
                    {
                        //Modify vertex in specific radius by linear or exponential distance prediction
                        Vector3 modifVertex = originalVertices[i] + (Direction * (ppExponentialDeformation ? (distance > Radius - ppRadiusSoftness ? (Radius - (distance)) : 1) : 1));
                        if (ppExponentialDeformation && ((ppDirection.y < 0 ? modifVertex.y > storedVertices[i].y : modifVertex.y < storedVertices[i].y))) continue;
                        storedVertices[i] = modifVertex;
                    }
                }
            }

            //Set vertices & continue
            if (!ppCustomInteractionSpeed || ppMultithreadingSupported)
                meshFilter.mesh.SetVertices(storedVertices);
            if (ppRecalculateNormals) meshFilter.mesh.RecalculateNormals();
            if (ppRecalculateBounds) meshFilter.mesh.RecalculateBounds();
            checkForUpdate_Repair = true;
            checkForUpdate_InterSpeed = true;
        }

        /// <summary>
        /// Reset current surface (Reset all vertices to the starting position)
        /// </summary>
        public void InteractiveSurface_ResetSurface()
        {
            for (int i = 0; i < storedVertices.Length; i++)
                storedVertices[i] = initialVertices[i];
        }

        //------External thread params----
        private Vector3 Thrd_AtPoint;
        private float Thrd_Radius;
        private Vector3 Thrd_Dir;
        private Vector3 Thrd_RealPos;
        private Vector3 Thrd_RealSca;
        private Quaternion Thrd_RealRot;
        //--------------------------------

        /// <summary>
        /// Main thread for mesh modification
        /// </summary>
        private void ThreadWork_ModifyMesh()
        {
            while (true)
            {
                for (int i = 0; i < storedVertices.Length; i++)
                {
                    Vector3 TransformedPoint = MD_MeshMathUtilities.TransformPoint(Thrd_RealPos, Thrd_RealRot, Thrd_RealSca, storedVertices[i]);
                    float distance = Vector3.Distance(new Vector3(Thrd_AtPoint.x, 0, Thrd_AtPoint.z), new Vector3(TransformedPoint.x, 0, TransformedPoint.z));
                    if (distance < Thrd_Radius)
                    {
                        Vector3 modifVertex = originalVertices[i] + (Thrd_Dir * (ppExponentialDeformation ? (distance > Thrd_Radius - ppRadiusSoftness ? (Thrd_Radius - (distance)) : 1) : 1));
                        if (ppExponentialDeformation && (modifVertex.y > storedVertices[i].y)) continue;
                        storedVertices[i] = modifVertex;
                    }
                }
                Thread.Sleep(ppThreadSleep);
            }
        }

        private void OnApplicationQuit()
        {
            //Abort thread if possible
            if (ppThread != null && ppThread.IsAlive)
                ppThread.Abort();
        }

        private void OnDestroy()
        {
            //Abort thread if possible
            if (ppThread != null && ppThread.IsAlive)
                ppThread.Abort();
        }

        /// <summary>
        /// Modify current mesh by custom RaycastEvent
        /// </summary>
        public void InteractiveSurface_ModifyMesh(MDM_RaycastEvent RayEvent)
        {
            if (!Application.isPlaying)
                return;
            if (RayEvent == null)
                return;
            if (RayEvent.hits.Length > 0 && RayEvent.hits[0].collider.gameObject != this.gameObject)
                return;
            if (ppDetectRadiusSize)
                ppRadius = RayEvent.ppPointRay ? ppRadius : RayEvent.ppSphericalRadius;

            foreach (RaycastHit hit in RayEvent.hits)
                InteractiveSurface_ModifyMesh(hit.point, ppRadius, ppDirection);
        }
    }
}

#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MDM_InteractiveSurface))]
    public class MDM_InteractiveLandscape_Editor : MD_EditorUtilities
    {
        private MDM_InteractiveSurface m;

        private void OnEnable()
        {
            m = (MDM_InteractiveSurface)target;
        }

        public override void OnInspectorGUI()
        {
            ps();
            pl("General Settings", true);
            pv();
            pv();
            ppDrawProperty("ppRecalculateNormals", "Recalculate Normals");
            ppDrawProperty("ppRecalculateBounds", "Recalculate Bounds");
            pve();
            ppDrawProperty("ppMultithreadingSupported", "Multithreading Supported", "If enabled, the mesh will be ready for complex operations.");
            if (m.ppMultithreadingSupported)
            {
                ppDrawProperty("ppThreadSleep", "Thread Sleep", "Overall thread sleep (in miliseconds; The lower value is, the faster thread processing will be; but more performance it may take)");
                phb("The Interactive Surface component is ready for complex meshes and will create a new separated thread");
            }
            ps(5);
            pv();
            ppDrawProperty("ppDirection", "Overall Direction", "Main direction of the vertices after interaction");
            pve();
            pv();
            ppDrawProperty("ppExponentialDeformation", "Exponential Deform", "If enabled, the mesh will be deformed expontentially (the results will be much smoother)");
            if (m.ppExponentialDeformation)
            {
                pplus();
                ppDrawProperty("ppRadiusSoftness", "Radius Softness", "If 'Exponential Deform' is enabled, vertices inside the Radius will be instantly affected. This value tells how 'soft' the radius should be to outer vertices nearly touching the inner radius");
                pminus();
            }
            pve();
            pv();
            if (m.ppRigidbodiesAllowed)
            {
                ppDrawProperty("ppDetectRadiusSize", "Detect Radius Size", "Adjust radius size by the collided objects. This will try to auto-detect the interaction radius with the collided rigidbody transform scales (recommended if 'Allow Rigidbodies' is enabled)");
                if (!m.ppDetectRadiusSize)
                    ppDrawProperty("ppRadius", "Interactive Radius", "Radius of vertices to be interacted with. Please keep in mind that if the input for mesh modify is RayEvent with Spherical Raycast, this field is no more required as the radius is received from the RayEvent - Non-Pointed Ray Radius");
            }
            else ppDrawProperty("ppRadius", "Interactive Radius", "Radius of vertices to be interacted. Please keep in mind that if the input for mesh modify is RayEvent with Spherical Raycast, this field is no more required");
            ppDrawProperty("ppRadiusMultiplier", "Radius Multiplier", "General radius multiplier. Multiplies the radius constant or auto-detected radius - default value is 1");
            pve();
            pve();

            ps(20);
            pl("Conditions", true);
            pv();
            pv();
            ppDrawProperty("ppRigidbodiesAllowed", "Allow Rigidbodies", "Allow Collision Enter & Collision Stay functions for Rigidbodies & other physically-based entities");
            if (m.ppRigidbodiesAllowed)
            {
                pplus();
                ppDrawProperty("ppMinimumForceDetection", "Force Detection Level", "Minimum rigidbody velocity detection [zero is default = without detection]");
                pv();
                ppDrawProperty("ppCollideWithSpecificObjects", "Collision With Specific Tag", "If enabled, collision will be occured only with included tag below...");
                if (m.ppCollideWithSpecificObjects)
                {
                    pplus();
                    ppDrawProperty("ppCollisionTag", "Collision Tag");
                    pminus();
                }
                pve();
                pminus();
            }
            pve();
            pve();
            if (m.ppMultithreadingSupported == false)
            {
                ps(20);
                pl("Additional Interaction Settings", true);
                pv();
                ppDrawProperty("ppCustomInteractionSpeed", "Custom Interaction Speed", "If enabled, you will be able to customize vertices speed after its interaction/ collision");
                if (m.ppCustomInteractionSpeed)
                {
                    pplus();
                    pv();
                    ppDrawProperty("ppInteractionSpeed", "Interaction Speed");
                    ppDrawProperty("ppContinuousEffect", "Enable Continuous Effect", "If enabled, interacted vertices will keep moving deeper");
                    pve();
                    pminus();
                }
                pve();

                pv();
                ppDrawProperty("ppRestoreSurface", "Restore Mesh", "Restore mesh after some time and interval");
                if (m.ppRestoreSurface)
                {
                    pplus();
                    ppDrawProperty("ppRestoreSpeed", "Restore Speed");
                    pminus();
                }
                pve();
            }
            ps(15);
            ppAddMeshColliderRefresher(m.gameObject);
            ppBackToMeshEditor(m);

            if (target != null) serializedObject.Update();
        }
    }
}
#endif
