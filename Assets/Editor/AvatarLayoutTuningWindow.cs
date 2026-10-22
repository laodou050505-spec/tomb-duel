using System.IO;
using Guandan.Scene;
using UnityEditor;
using UnityEngine;

namespace Guandan.EditorTools
{
    public sealed class AvatarLayoutTuningWindow : EditorWindow
    {
        private const string AssetPath = "Assets/Resources/AvatarLayoutSettings.asset";
        private AvatarLayoutSettings settings;
        private Vector2 scroll;

        [MenuItem("TombDuel/Avatar Layout Tuning")]
        public static void Open()
        {
            var window = GetWindow<AvatarLayoutTuningWindow>("Avatar Layout");
            window.minSize = new Vector2(420f, 610f);
            window.Show();
        }

        private void OnEnable()
        {
            settings = EnsureSettingsAsset();
        }

        private void OnGUI()
        {
            if (settings == null) settings = EnsureSettingsAsset();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Table Avatar Layout", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                Application.isPlaying
                    ? "Live preview: changes apply immediately to all three avatars and the seat box."
                    : "These parameters do not move or save the tomb scene. Enter Play mode to preview; values persist until next run.",
                MessageType.Info);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(settings, "Adjust TombDuel Avatar Layout");
            settings.globalScale = EditorGUILayout.Slider("Global Scale", settings.globalScale, 0.75f, 1.45f);
            settings.chestToTableOffset = EditorGUILayout.Slider("Chest-to-Table Offset", settings.chestToTableOffset, -0.45f, 0.45f);
            settings.chairToHipGap = EditorGUILayout.Slider("Seat-to-Hip Gap", settings.chairToHipGap, 0.02f, 0.28f);
            DrawSeat("East · Right Opponent", settings.east);
            DrawSeat("North · Partner Across", settings.north);
            DrawSeat("West · Left Opponent", settings.west);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                ApplyToRunningAvatars();
            }

            EditorGUILayout.Space(14f);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Apply to Running Avatars", GUILayout.Height(34f))) ApplyToRunningAvatars();
                if (GUILayout.Button("Reset Recommended", GUILayout.Height(34f))) ResetRecommended();
            }
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Position offsets use scene metres. Y = up/down, Yaw = avatar rotation.", EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.EndScrollView();
        }

        private static void DrawSeat(string label, AvatarSeatLayout seat)
        {
            EditorGUILayout.Space(12f);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                seat.positionOffset = EditorGUILayout.Vector3Field("Position Offset", seat.positionOffset);
                seat.scale = EditorGUILayout.Slider("Avatar Scale", seat.scale, 0.65f, 1.65f);
                seat.yawOffset = EditorGUILayout.Slider("Yaw Offset", seat.yawOffset, -45f, 45f);
                seat.chairHeightMultiplier = EditorGUILayout.Slider("Seat Height Mult", seat.chairHeightMultiplier, 0.65f, 1.45f);
            }
        }

        private void ApplyToRunningAvatars()
        {
            if (!Application.isPlaying) return;
            foreach (var bindings in Object.FindObjectsByType<RuntimeGameplayBindings>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                bindings.ApplyLayoutSettings(settings);
            SceneView.RepaintAll();
        }

        private void ResetRecommended()
        {
            Undo.RecordObject(settings, "Reset Avatar Layout to Recommended");
            settings.globalScale = 1f;
            settings.chestToTableOffset = 0f;
            settings.chairToHipGap = 0.08f;
            ResetSeat(settings.east);
            ResetSeat(settings.north);
            ResetSeat(settings.west);
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            ApplyToRunningAvatars();
        }

        private static void ResetSeat(AvatarSeatLayout seat)
        {
            seat.positionOffset = Vector3.zero;
            seat.scale = 1f;
            seat.yawOffset = 0f;
            seat.chairHeightMultiplier = 1f;
        }

        private static AvatarLayoutSettings EnsureSettingsAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<AvatarLayoutSettings>(AssetPath);
            if (asset != null) return asset;
            Directory.CreateDirectory("Assets/Resources");
            asset = CreateInstance<AvatarLayoutSettings>();
            AssetDatabase.CreateAsset(asset, AssetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
