using System.Collections;
using UnityEngine.Events;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
#endif

namespace MD_Plugin
{
    /// <summary>
    /// MDM(Mesh Deformation Modifier): Sound React
    /// Simple sound reaction modifier - add another modifier to connect the sound react
    /// </summary>
    [AddComponentMenu(MD_Debug.ORGANISATION + MD_Debug.PACKAGENAME + "Modifiers/Sound React")]
    public class MDM_SoundReact : MonoBehaviour
    {
        public AudioSource ppTargetAudioSrc;
        public enum SampleDataLength { x128, x256, x512, x1024, x2048, x4096};
        public SampleDataLength ppSampleDataLength = SampleDataLength.x1024;
        public float ppTransitionSmoothness = 128.0f;
        public float ppMultiplication = 2.0f;
        public float ppMinimumOutputValue = 0.0f;
        public float ppMaximumOutputValue = 10.0f;

        public bool ppProcessOnStart = true;
        public float ppUpdateInterval = 0.01f;

        /// <summary>
        /// Main output data
        /// </summary>
        public float PpOutputData { private set; get; }
        public UnityEvent ppOutputEvent;

        private float clipRealtimeData;
        private float[] clipSampleData;

        private void Awake()
        {
            if (ppProcessOnStart)
                SoundReact_Start();
        }

        /// <summary>
        /// Start sound reaction - start receiving audio data from the target audio source
        /// </summary>
        public void SoundReact_Start()
        {
            StartCoroutine(ProcessSoundReaction());
        }

        /// <summary>
        /// Stop sound reaction - stop receiving audio data from the target audio source
        /// </summary>
        public void SoundReact_Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator ProcessSoundReaction()
        {
            int sampleData = 1024;
            switch(ppSampleDataLength)
            {
                case SampleDataLength.x128: sampleData = 128; break;
                case SampleDataLength.x256: sampleData = 256; break;
                case SampleDataLength.x512: sampleData = 512; break;
                case SampleDataLength.x1024: sampleData = 1024; break;
                case SampleDataLength.x2048: sampleData = 2048; break;
                case SampleDataLength.x4096: sampleData = 4096; break;

            }
            clipSampleData = new float[sampleData];

            while (true)
            {
                yield return new WaitForSeconds(ppUpdateInterval);
                ppTargetAudioSrc.clip.GetData(clipSampleData, ppTargetAudioSrc.timeSamples);
                clipRealtimeData = 0.0f;
                foreach (var s in clipSampleData)
                    clipRealtimeData += Mathf.Abs(s);
                clipRealtimeData /= sampleData;
                float formula = clipRealtimeData * ppMultiplication;
                PpOutputData = Mathf.Lerp(PpOutputData, Mathf.Clamp(formula, ppMinimumOutputValue, ppMaximumOutputValue), ppTransitionSmoothness * Time.deltaTime);
                ppOutputEvent?.Invoke();
            }
        }
    }
}

#if UNITY_EDITOR
namespace MD_PluginEditor
{
    [CustomEditor(typeof(MDM_SoundReact))]
    [CanEditMultipleObjects]
    public class MDM_SoundReact_Editor : MD_EditorUtilities
    {
        public override void OnInspectorGUI()
        {
            ps();

            pv();
            pl("Essential Settings", true);
            pv();
            pv();
            ppDrawProperty("ppTargetAudioSrc", "Target Audio Source", "Target audio source which holds the audio data for computation");
            pve();
            ppDrawProperty("ppProcessOnStart", "Process On Start", "If enabled, the script will start processing right after the startup");
            ppDrawProperty("ppUpdateInterval", "Update Interval (every N second)");
            pve();

            ps();

            pl("Output Settings", true);
            pv();
            ppDrawProperty("ppSampleDataLength", "Sample Data Length", "Sample data length value - the higher the value is, the more precise the audio data will result");
            ppDrawProperty("ppTransitionSmoothness", "Output Transition Smoothness", "Transition smoothness of the output data - how smooth the value will be?");
            ppDrawProperty("ppMultiplication", "Output Multiplier", "Output data value multiplier - simple amplification");
            pv();
            ppDrawProperty("ppMinimumOutputValue", "Minimum Output Value", "Minimum value of the output data");
            ppDrawProperty("ppMaximumOutputValue", "Maximum Output Value", "Maximum value of the output data");
            pve();
            pve();

            ps();

            ppDrawProperty("ppOutputEvent", "Output Event", "Unity event of the output data value - connect supported modifiers or any 3rd party methods to receive compiled output data");
            pve();

            ps();

            if (target != null) serializedObject.Update();
        }
    }
}
#endif
