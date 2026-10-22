using System;
using System.IO;
using Guandan.Title;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Guandan.Editor
{
    /// <summary>
    /// 2026-10-22 精修：验证字体/音效资源到位，重新生成标题场景，截图到
    /// VisualQA/Polish-20261022/。
    ///
    /// batchmode 调用（不加 -quit，由 VisualQa 内部调用 EditorApplication.Exit）：
    ///   lockf /tmp/en-unity-A.lock \
    ///     /Applications/Unity/Hub/Editor/6000.5.5f1/Unity.app/Contents/MacOS/Unity \
    ///     -batchmode -projectPath &lt;proj&gt; -buildTarget Android \
    ///     -executeMethod Guandan.Editor.GuandanTitleRefinementSetup.RunFullRefinement \
    ///     -logFile /tmp/tomb-refinement.log
    ///
    /// 截图输出至 VisualQA/Title-20260908/（由 TombTitleValidation.VisualQa 负责）。
    /// </summary>
    public static class GuandanTitleRefinementSetup
    {
        private const string PolishQaDir  = "VisualQA/Polish-20261022";
        private const string CinzelPath   = "Assets/Resources/GuandanUI/Cinzel-Regular.ttf";
        private const string HoverPath    = "Assets/Resources/GuandanUI/TombButtonHover.mp3";
        private const string ClickPath    = "Assets/Resources/GuandanUI/TombButtonClick.mp3";

        // ── Entry point for batchmode -executeMethod ──────────────────────────────────
        /// <summary>
        /// 1. 强制重导入字体和音效资产（设置 Importer 参数）
        /// 2. 重新生成 GuandanTitle.unity（TombTitleBootstrap 已包含新代码）
        /// 3. 运行 GuandanRuleSmokeTests
        /// 4. 调用 TombTitleValidation.VisualQa() → Play mode 截图 → EditorApplication.Exit(0)
        /// 注意：不要加 -quit，Exit 由 VisualQa 内部发出。
        /// </summary>
        [MenuItem("TombDuel/Refinement/Run Full Refinement (batchmode entry)")]
        public static void RunFullRefinement()
        {
            try
            {
                ImportRefinementAssets();
                GuandanProjectSetup.CreateTitleScene();
                GuandanRuleSmokeTests.Run();
                AssetDatabase.SaveAssets();
                Debug.Log("[TombRefinement] Assets saved. Launching VisualQa (will exit on completion)...");
                // VisualQa enters Play mode, takes screenshots, then calls EditorApplication.Exit(0).
                TombTitleValidation.VisualQa();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorApplication.Exit(1);
            }
        }

        // ── Step 1: reimport font and audio with correct settings ─────────────────────
        [MenuItem("TombDuel/Refinement/1. Import Refinement Assets")]
        public static void ImportRefinementAssets()
        {
            var missing = false;
            foreach (var path in new[] { CinzelPath, HoverPath, ClickPath })
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"[TombRefinement] Missing asset: {path}");
                    missing = true;
                }
            }
            if (missing) throw new Exception("Required refinement assets are missing.");

            // Font: import as TrueType, include font data.
            AssetDatabase.ImportAsset(CinzelPath, ImportAssetOptions.ForceSynchronousImport);
            var fontImporter = AssetImporter.GetAtPath(CinzelPath) as TrueTypeFontImporter;
            if (fontImporter != null)
            {
                fontImporter.includeFontData = true;
                fontImporter.fontTextureCase = FontTextureCase.Dynamic;
                fontImporter.fontRenderingMode = FontRenderingMode.HintedSmooth;
                fontImporter.SaveAndReimport();
            }

            // Audio: PCM/Vorbis, preload, no spatialization (2D UI sound).
            foreach (var audioPath in new[] { HoverPath, ClickPath })
            {
                AssetDatabase.ImportAsset(audioPath, ImportAssetOptions.ForceSynchronousImport);
                var audioImporter = AssetImporter.GetAtPath(audioPath) as AudioImporter;
                if (audioImporter == null) continue;
                var settings = audioImporter.defaultSampleSettings;
                settings.loadType = AudioClipLoadType.DecompressOnLoad;
                settings.compressionFormat = AudioCompressionFormat.Vorbis;
                settings.quality = 0.8f;
                audioImporter.defaultSampleSettings = settings;
                audioImporter.forceToMono = false;
                audioImporter.ambisonic = false;
                audioImporter.SaveAndReimport();
            }
            Debug.Log("[TombRefinement] Assets imported: Cinzel-Regular.ttf, TombButtonHover.mp3, TombButtonClick.mp3.");
        }

        // ── Step 2: offscreen screenshot of title scene (works in batchmode) ─────────
        [MenuItem("TombDuel/Refinement/2. Capture Polish Screenshot")]
        public static void CapturePolishScreenshot()
        {
            Directory.CreateDirectory(PolishQaDir);

            var scene = EditorSceneManager.OpenScene(
                GuandanProjectSetup.TitleScenePath, OpenSceneMode.Single);

            // Create a temporary off-screen camera that matches the authored eye position.
            var camGo = new GameObject("_TempCaptureCam");
            var cam   = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.008f, 0.012f, 0.010f);
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane  = 50f;
            cam.fieldOfView   = 72f;

            // Position at authored eye start, looking at design focus.
            var eye   = new Vector3(0f, 1.72f, -4.25f);
            var focus = new Vector3(0f, 1.94f,  0f);   // raised 0.32 per DESIGN.md
            cam.transform.SetPositionAndRotation(eye, Quaternion.LookRotation(focus - eye));

            // Render at 1920×1080.
            Capture(cam, $"{PolishQaDir}/01-title-polish-normal.png", 1920, 1080);

            // Remove the temporary camera.
            UnityEngine.Object.DestroyImmediate(camGo);

            Debug.Log($"[TombRefinement] Screenshot saved to {PolishQaDir}/01-title-polish-normal.png");
        }

        // ── Step 3: verify font loaded at runtime (Editor play-mode test) ─────────────
        /// <summary>
        /// Verifies that Resources.Load succeeds for all new assets.
        /// Safe to call in batchmode (no display required).
        /// </summary>
        [MenuItem("TombDuel/Refinement/3. Verify Asset Loading")]
        public static void VerifyAssetLoading()
        {
            var failures = 0;

            var cinzel = Resources.Load<Font>("GuandanUI/Cinzel-Regular");
            if (cinzel == null)
            { Debug.LogError("[TombRefinement] Resources.Load<Font>(\"GuandanUI/Cinzel-Regular\") returned null."); failures++; }
            else Debug.Log("[TombRefinement] Cinzel-Regular loaded OK.");

            var hover = Resources.Load<AudioClip>("GuandanUI/TombButtonHover");
            if (hover == null)
            { Debug.LogError("[TombRefinement] Resources.Load<AudioClip>(\"GuandanUI/TombButtonHover\") returned null."); failures++; }
            else Debug.Log("[TombRefinement] TombButtonHover loaded OK.");

            var click = Resources.Load<AudioClip>("GuandanUI/TombButtonClick");
            if (click == null)
            { Debug.LogError("[TombRefinement] Resources.Load<AudioClip>(\"GuandanUI/TombButtonClick\") returned null."); failures++; }
            else Debug.Log("[TombRefinement] TombButtonClick loaded OK.");

            var button = Resources.Load<Texture2D>("GuandanUI/TombButton");
            if (button == null)
            { Debug.LogError("[TombRefinement] Resources.Load<Texture2D>(\"GuandanUI/TombButton\") returned null."); failures++; }
            else Debug.Log("[TombRefinement] TombButton texture loaded OK.");

            if (failures > 0)
                throw new Exception($"[TombRefinement] {failures} asset(s) failed to load. Check Resources/GuandanUI/ paths.");

            Debug.Log("[TombRefinement] VerifyAssetLoading PASSED — all 4 assets loaded.");
        }

        // ── Shared capture helper ──────────────────────────────────────────────────────
        private static void Capture(Camera cam, string path, int width, int height)
        {
            var prev   = cam.targetTexture;
            var active = RenderTexture.active;
            var rt     = new RenderTexture(width, height, 24);
            cam.targetTexture = rt;
            Canvas.ForceUpdateCanvases();
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());
            cam.targetTexture = prev;
            RenderTexture.active = active;
            UnityEngine.Object.DestroyImmediate(tex);
            UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
