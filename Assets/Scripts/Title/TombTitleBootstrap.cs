using System;
using System.Collections;
using System.Collections.Generic;
using ByteDance.PICO.XR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Guandan.Title
{
    // THESIS: Enter the tomb through a cinematic bronze-and-jade seal.
    // OWN-WORLD: Generated antique-gold lettering, carved Chinese tomb, quiet jade controls.
    // STORY: Recognize Tomb Duel, choose Enter Tomb, arrive at the existing card table.
    // FIRST VIEWPORT: Full landscape art, central monumental title, two live lower controls.
    // FORM: User-pinned central emblem composition; continuation approved 2026-09-08.
    // REFINED: 2026-10-22 — TombButton.png texture buttons, Cinzel-Regular serif font,
    //          3-state (normal/hover/press) tint, medieval bronze audio, PICO-only hint,
    //          hover haptic 0.15/20ms, press haptic 0.4/40ms.
    public sealed class TombTitleBootstrap : MonoBehaviour
    {
        private const string GameplaySceneName = "GuandanTomb";
        private const float CoverLift = 0.32f;
        [SerializeField] private Vector3 designEyePosition = new(0f, 1.72f, -4.25f);
        [SerializeField] private Vector3 designFocusPosition = new(0f, 1.62f, 0f);
        [SerializeField] private float designEyeHeight = 1.60f;
        private Transform xrOrigin;
        private Camera headCamera;
        private Transform leftController, rightController;
        private LineRenderer leftRay, rightRay;
        private bool xrStartApplied, loading;
        private PointerState left = new(), right = new(), mouse = new(), leftHand = new(), rightHand = new();
        private readonly List<UnityEngine.XR.InputDevice> devices = new();
        private readonly List<TitleActionButton> buttons = new();
        private Canvas canvas;
        private Text instruction;
        private Image fade;
        private AudioSource audioSource;
        private int keyboardFocus = -1;
        public Vector3 DesignEyePosition => designEyePosition;
        public Vector3 DesignFocusPosition => designFocusPosition;
        public bool IsLoading => loading;

        private sealed class PointerState
        {
            public bool held;
            public TitleActionButton down;
        }

        private void Awake()
        {
            BuildRig();
            BuildTitleStage();
            TrySetFloorTracking();
            RecenterToDesignStart();
            Debug.Log("[TombTitle] Ready: TombButton bronze texture, Cinzel font, medieval audio, PICO haptics.");
        }

        private void Update()
        {
            foreach (var button in buttons) button.Hovered = false;
            if (loading) return;
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.rKey.wasPressedThisFrame) RecenterToDesignStart();
                if (keyboard.tabKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
                    keyboardFocus = (keyboardFocus + 1) % buttons.Count;
                if (keyboard.upArrowKey.wasPressedThisFrame)
                    keyboardFocus = keyboardFocus <= 0 ? buttons.Count - 1 : keyboardFocus - 1;
                if (keyboard.enterKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame)
                    buttons[Mathf.Max(0, keyboardFocus)].Activate();
                if (keyboard.escapeKey.wasPressedThisFrame) ExitGame();
            }
            if (keyboardFocus >= 0) buttons[keyboardFocus].Hovered = true;
            if (!XRSettings.enabled || !XRSettings.isDeviceActive)
            {
                ApplyDesktopCamera();
                if (Mouse.current != null)
                {
                    if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f) keyboardFocus = -1;
                    var pointer = headCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if (Mouse.current.leftButton.wasPressedThisFrame) ProcessPointer(pointer, true, mouse);
                    ProcessPointer(pointer, Mouse.current.leftButton.isPressed, mouse);
                }
                leftRay.enabled = rightRay.enabled = false;
            }
            else
            {
                if (!xrStartApplied) { PlaceXrHeadAtDesignStart(); xrStartApplied = true; }
                PollController(InputDeviceCharacteristics.Left, leftController, leftRay, left);
                PollController(InputDeviceCharacteristics.Right, rightController, rightRay, right);
                PollHand(HandType.HandLeft, leftHand);
                PollHand(HandType.HandRight, rightHand);
            }
        }

        private void OnApplicationFocus(bool focused)
        {
            if (focused) return;
            foreach (var state in new[] { left, right, mouse, leftHand, rightHand }) CancelPointer(state);
        }

        public void StartGame() { if (!loading) StartCoroutine(LoadGameplay()); }
        public void ExitGame() { if (!loading) StartCoroutine(ExitAfterFeedback()); }

        public void PlayHoverSound()
        {
            if (audioSource != null)
            {
                // Use PlayOneShot so multiple hovers don't cancel each other.
                var clip = Resources.Load<AudioClip>("GuandanUI/TombButtonHover");
                if (clip != null) audioSource.PlayOneShot(clip, 0.55f);
            }
        }

        public void PlayClickSound()
        {
            if (audioSource != null)
            {
                var clip = Resources.Load<AudioClip>("GuandanUI/TombButtonClick");
                if (clip != null) audioSource.PlayOneShot(clip, 0.80f);
            }
        }

        private IEnumerator ExitAfterFeedback()
        {
            loading = true;
            instruction.text = "LEAVING THE TOMB";
            yield return new WaitForSecondsRealtime(0.18f);
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private IEnumerator LoadGameplay()
        {
            loading = true;
            instruction.text = "OPENING THE TOMB...";
            foreach (var button in buttons) button.Interactable = false;
            yield return new WaitForSecondsRealtime(0.20f);
            AsyncOperation operation = null;
            try { operation = SceneManager.LoadSceneAsync(GameplaySceneName, LoadSceneMode.Single); }
            catch (Exception exception) { Debug.LogException(exception); }
            if (operation == null)
            {
                loading = false;
                instruction.text = "COULD NOT OPEN THE TOMB. SELECT ENTER TOMB TO RETRY.";
                foreach (var button in buttons) button.Interactable = true;
                yield break;
            }
            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f) yield return null;
            for (float t = 0; t < 0.25f; t += Time.unscaledDeltaTime)
            {
                fade.color = new Color(0.008f, 0.012f, 0.010f, t / 0.25f);
                yield return null;
            }
            Debug.Log("[TombTitle] Start confirmed; activating GuandanTomb.");
            operation.allowSceneActivation = true;
        }

        private void BuildTitleStage()
        {
            // AudioSource on the title root for UI sounds.
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0f; // 2D — UI sound, not spatialized.
            audioSource.playOnAwake = false;
            audioSource.priority = 64;

            var go = new GameObject("TitleRoot · Tripo cinematic cover", typeof(RectTransform), typeof(Canvas));
            go.transform.SetParent(transform, false);
            go.transform.position = designFocusPosition + Vector3.up * CoverLift;
            go.transform.localScale = Vector3.one * 0.004f;
            canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = headCamera;
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(1920f, 1080f);

            var artRect = Rect("Tripo Nano Banana · Tomb Duel", rect, Vector2.zero, rect.sizeDelta);
            var art = artRect.gameObject.AddComponent<RawImage>();
            art.texture = Resources.Load<Texture2D>("GuandanUI/TitleCover");
            art.raycastTarget = false;
            if (art.texture == null) Debug.LogError("[TombTitle] Missing TitleCover texture.");

            // Load shared resources once for both buttons.
            var buttonTex = Resources.Load<Texture2D>("GuandanUI/TombButton");
            if (buttonTex == null) Debug.LogError("[TombTitle] Missing TombButton texture.");
            var cinzel = Resources.Load<Font>("GuandanUI/Cinzel-Regular");
            if (cinzel == null) Debug.LogWarning("[TombTitle] Cinzel-Regular not found; falling back to built-in font.");

            CreateButton(rect, "Enter Tomb", "ENTER TOMB", new Vector2(0f, -328f), new Vector2(424f, 76f), TitleAction.Start, buttonTex, cinzel);
            CreateButton(rect, "Exit", "EXIT", new Vector2(0f, -430f), new Vector2(300f, 64f), TitleAction.Exit, buttonTex, cinzel);

            // PICO shows only controller hint; desktop build shows keyboard/mouse hint as well.
#if UNITY_ANDROID && !UNITY_EDITOR
            var hintText = "POINT AND PULL TRIGGER";
#else
            var hintText = "POINT & TRIGGER TO SELECT  /  MOUSE · TAB + ENTER";
#endif
            // Spec C secondary min at 4.26m: 4.26×0.012=5.11cm → fontSize≥19 (19×0.004×0.68=5.17cm).
            // Using 22 for safety margin.
            instruction = Label("Controls", rect, hintText, new Vector2(0f, -508f), new Vector2(1600f, 40f), 22, new Color(0.79f, 0.81f, 0.71f), cinzel);

            var fadeRect = Rect("Transition", rect, Vector2.zero, rect.sizeDelta);
            fadeRect.localPosition += Vector3.back * 5f;
            fade = fadeRect.gameObject.AddComponent<Image>();
            fade.color = Color.clear;
            fade.raycastTarget = false;
        }

        private void CreateButton(RectTransform parent, string name, string label, Vector2 position, Vector2 size, TitleAction action, Texture2D buttonTex, Font cinzel)
        {
            var hit = Rect(name, parent, position, size);
            hit.localPosition += Vector3.back * 4f;
            var collider = hit.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(size.x, size.y, 12f);
            var visual = Rect("Visual", hit, Vector2.zero, size);

            // TombButton.png: bronze-framed ancient tomb button (920×296, no text).
            // The RawImage color property is used for 3-state tinting.
            var bgImage = visual.gameObject.AddComponent<RawImage>();
            bgImage.texture = buttonTex;
            bgImage.raycastTarget = false;
            // Normal tint: slightly warm-dimmed to look restful.
            bgImage.color = new Color(0.85f, 0.78f, 0.65f, 1f);

            // Cinzel serif label: gold on top of bronze texture.
            // Spec C at d=4.26m title min: 4.26×0.028=11.9cm → need fontSize≥44 (44×0.004×0.68=12.0cm).
            // EXIT is secondary action, body spec: 4.26×0.017=7.24cm → fontSize≥28 (28×0.004×0.68=7.62cm).
            int fontSize = action == TitleAction.Start ? 46 : 34;
            var text = Label("Label", visual, label, Vector2.zero, size, fontSize, new Color(0.97f, 0.90f, 0.68f), cinzel);
            if (cinzel != null) text.fontStyle = FontStyle.Bold;

            var button = hit.gameObject.AddComponent<TitleActionButton>();
            button.Configure(this, action, visual, bgImage, text);
            buttons.Add(button);
        }

        private static RectTransform Rect(string name, Transform parent, Vector2 position, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false); rect.anchoredPosition = position; rect.sizeDelta = size;
            return rect;
        }

        private static Text Label(string name, Transform parent, string value, Vector2 position, Vector2 size, int fontSize, Color color, Font font = null)
        {
            var rect = Rect(name, parent, position, size);
            var text = rect.gameObject.AddComponent<Text>();
            text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.text = value;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = color;
            text.raycastTarget = false;
            text.fontStyle = FontStyle.Normal;
            return text;
        }

        private void BuildRig()
        {
            xrOrigin = new GameObject("Title XR Rig").transform;
            xrOrigin.SetParent(transform, false);
            if (GetComponent<PXR_Manager>() == null) gameObject.AddComponent<PXR_Manager>();
            var go = new GameObject("Title XR Head Camera", typeof(Camera), typeof(AudioListener));
            go.transform.SetParent(xrOrigin, false); go.transform.localPosition = Vector3.up * designEyeHeight; go.tag = "MainCamera";
            headCamera = go.GetComponent<Camera>();
            headCamera.clearFlags = CameraClearFlags.SolidColor;
            headCamera.backgroundColor = new Color(0.008f, 0.012f, 0.010f);
            headCamera.nearClipPlane = 0.05f; headCamera.farClipPlane = 50f;
            var driver = go.AddComponent<TrackedPoseDriver>();
            driver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            driver.updateType = TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;
            driver.positionInput = new InputActionProperty(new InputAction("Title head position", InputActionType.Value, "<XRHMD>/centerEyePosition", expectedControlType: "Vector3"));
            driver.rotationInput = new InputActionProperty(new InputAction("Title head rotation", InputActionType.Value, "<XRHMD>/centerEyeRotation", expectedControlType: "Quaternion"));
            driver.trackingStateInput = new InputActionProperty(new InputAction("Title head tracking", InputActionType.Value, "<XRHMD>/trackingState", expectedControlType: "Integer"));
            leftController = new GameObject("Left Controller").transform; leftController.SetParent(xrOrigin, false);
            rightController = new GameObject("Right Controller").transform; rightController.SetParent(xrOrigin, false);
            leftRay = MakeRay("Left Ray", new Color(0.51f, 0.87f, 0.75f));
            rightRay = MakeRay("Right Ray", new Color(1f, 0.78f, 0.40f));
        }

        private LineRenderer MakeRay(string name, Color color)
        {
            var go = new GameObject(name); go.transform.SetParent(xrOrigin, false);
            var ray = go.AddComponent<LineRenderer>(); ray.positionCount = 2; ray.useWorldSpace = true;
            ray.startWidth = .006f; ray.endWidth = .002f;
            ray.material = new Material(Shader.Find("Sprites/Default")); ray.startColor = ray.endColor = color; ray.enabled = false;
            return ray;
        }

        private void PollController(InputDeviceCharacteristics side, Transform controller, LineRenderer line, PointerState state)
        {
            devices.Clear(); InputDevices.GetDevicesWithCharacteristics(side | InputDeviceCharacteristics.Controller, devices);
            var device = devices.Count > 0 ? devices[0] : default;
            bool tracked = device.isValid, pressed = false;
            if (device.isValid)
            {
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.isTracked, out var valid)) tracked = valid;
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out var position)) controller.localPosition = position;
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out var rotation)) controller.localRotation = rotation;
                pressed = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out var trigger) && trigger;
            }
            var input = side == InputDeviceCharacteristics.Left ? XRController.leftHand : XRController.rightHand;
            if (input != null && input.isTracked.isPressed)
            {
                tracked = true; controller.localPosition = input.devicePosition.ReadValue(); controller.localRotation = input.deviceRotation.ReadValue();
                pressed |= input.TryGetChildControl<ButtonControl>("triggerPressed")?.isPressed == true;
                pressed |= (input.TryGetChildControl<AxisControl>("trigger")?.ReadValue() ?? 0f) >= 0.65f;
            }
            line.enabled = tracked;
            if (!tracked) { CancelPointer(state); return; }
            var ray = new Ray(headCamera.transform.position, controller.forward);
            var target = ProcessPointer(ray, pressed, state);
            float forward = Vector3.Dot(ray.direction, headCamera.transform.forward);
            float distance = Physics.Raycast(ray, out var visibleHit, 20f) ? visibleHit.distance : 8f;
            float start = Mathf.Max(.35f, (headCamera.nearClipPlane + .08f) / Mathf.Max(.1f, forward));
            line.enabled = forward > .1f && distance > start;
            line.SetPosition(0, ray.GetPoint(start)); line.SetPosition(1, ray.GetPoint(distance));
            if (device.isValid && device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
            {
                // Hover entry: short gentle pulse (spec: 0.15 strength, 20 ms).
                if (target != null && target.HoverStartedThisFrame)
                    device.SendHapticImpulse(0, 0.15f, 0.020f);
                // Press confirmation: stronger pulse (spec: 0.4 strength, 40 ms).
                if (pressed && target != null && target.PressStartedThisFrame)
                    device.SendHapticImpulse(0, 0.40f, 0.040f);
            }
        }

        private void PollHand(HandType hand, PointerState state)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var aim = new HandAimState();
            const HandAimStatus required = HandAimStatus.AimComputed | HandAimStatus.AimRayValid;
            if (PXR_HandTracking.GetAimState(hand, ref aim) && (aim.aimStatus & required) == required)
            {
                var direction = xrOrigin.TransformDirection(aim.aimRayPose.Orientation.ToQuat() * Vector3.forward);
                bool pressed = (aim.aimStatus & (HandAimStatus.AimIndexPinching | HandAimStatus.AimRayTouched)) != 0;
                ProcessPointer(new Ray(headCamera.transform.position, direction), pressed, state);
            }
            else CancelPointer(state);
#endif
        }

        private TitleActionButton ProcessPointer(Ray ray, bool pressed, PointerState state)
        {
            TitleActionButton target = null;
            if (Physics.Raycast(ray, out var hit, 20f)) target = hit.collider.GetComponent<TitleActionButton>();
            if (target != null) target.Hovered = true;
            if (pressed && !state.held) { state.down = target; target?.SetPressed(true); }
            if (!pressed && state.held)
            {
                state.down?.SetPressed(false);
                if (target != null && target == state.down) target.Activate();
                state.down = null;
            }
            state.held = pressed;
            return target;
        }
        private static void CancelPointer(PointerState state) { state.down?.SetPressed(false); state.down = null; state.held = false; }

        public void RecenterToDesignStart()
        {
            if (XRSettings.enabled && XRSettings.isDeviceActive) { TrySetFloorTracking(); PlaceXrHeadAtDesignStart(); xrStartApplied = true; }
            else ApplyDesktopCamera();
        }
        private void ApplyDesktopCamera()
        {
            headCamera.transform.SetPositionAndRotation(designEyePosition, Quaternion.LookRotation(designFocusPosition - designEyePosition));
            headCamera.fieldOfView = Mathf.Max(58f, 2f * Mathf.Atan(3.98f / (4.25f * Mathf.Max(.6f, headCamera.aspect))) * Mathf.Rad2Deg);
        }
        private void PlaceXrHeadAtDesignStart()
        {
            var local = headCamera.transform.localPosition;
            if (local.sqrMagnitude < .0001f) local = Vector3.up * designEyeHeight;
            var forward = Vector3.ProjectOnPlane(headCamera.transform.localRotation * Vector3.forward, Vector3.up);
            var headYaw = forward.sqrMagnitude > .0001f ? Quaternion.LookRotation(forward) : Quaternion.identity;
            var designYaw = Quaternion.LookRotation(Vector3.ProjectOnPlane(designFocusPosition - designEyePosition, Vector3.up));
            var rotation = designYaw * Quaternion.Inverse(headYaw);
            xrOrigin.SetPositionAndRotation(designEyePosition - rotation * local, rotation);
        }
        private static void TrySetFloorTracking()
        {
            var systems = new List<XRInputSubsystem>(); SubsystemManager.GetSubsystems(systems);
            foreach (var system in systems) system.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        }
    }

    public enum TitleAction { Start, Exit }

    public sealed class TitleActionButton : MonoBehaviour
    {
        private TombTitleBootstrap owner;
        private TitleAction action;
        private RectTransform visual;
        private RawImage buttonBg;  // TombButton.png — tinted per state.
        private Text label;
        private float hoverLerp, pressLerp;
        private bool pressed;
        private int pressFrame = -1;
        private bool wasHovered;
        private int hoverStartFrame = -1;

        public bool Hovered { get; set; }
        public bool Interactable { get; set; } = true;
        public bool PressStartedThisFrame => pressFrame == Time.frameCount;
        // Triggered only on the first frame that hover becomes true.
        public bool HoverStartedThisFrame => hoverStartFrame == Time.frameCount;

        public void Configure(TombTitleBootstrap title, TitleAction titleAction, RectTransform target, RawImage bg, Text text)
        {
            owner = title; action = titleAction; visual = target; buttonBg = bg; label = text;
            ApplyVisual();
        }

        public void SetPressed(bool value) { pressed = value; if (value) pressFrame = Time.frameCount; }

        public void Activate()
        {
            if (!Interactable || owner == null || owner.IsLoading) return;
            pressLerp = 1f;
            owner.PlayClickSound();
            Debug.Log($"[TombTitle] Activated {action}");
            if (action == TitleAction.Start) owner.StartGame(); else owner.ExitGame();
        }

        private void LateUpdate()
        {
            // Detect hover entry this frame (for haptic and audio).
            bool currentlyHovered = Hovered && Interactable;
            if (currentlyHovered && !wasHovered)
            {
                hoverStartFrame = Time.frameCount;
                owner?.PlayHoverSound();
            }
            wasHovered = currentlyHovered;

            float t = 1f - Mathf.Exp(-18f * Time.unscaledDeltaTime);
            hoverLerp = Mathf.Lerp(hoverLerp, currentlyHovered ? 1f : 0f, t);
            pressLerp = Mathf.Lerp(pressLerp, pressed ? 1f : 0f, t);
            ApplyVisual();
        }

        private void ApplyVisual()
        {
            if (visual == null) return;

            // Scale: hover +4%, press -3% (per spec: hover 4-6%, press 3%).
            visual.localScale = Vector3.one * (1f + hoverLerp * 0.04f - pressLerp * 0.03f);

            // Button texture tint:
            // Normal  → warm-dimmed bronze (0.85, 0.78, 0.65) – restful dark jade
            // Hover   → bright warm gold  (1.00, 0.94, 0.76) – lit up
            // Press   → darkened amber    (0.55, 0.45, 0.30) – depressed into stone
            var normalTint = new Color(0.85f, 0.78f, 0.65f, 1f);
            var hoverTint  = new Color(1.00f, 0.94f, 0.76f, 1f);
            var pressTint  = new Color(0.55f, 0.45f, 0.30f, 1f);
            var tint = Color.Lerp(normalTint, hoverTint, hoverLerp);
            tint = Color.Lerp(tint, pressTint, pressLerp);
            if (buttonBg != null) buttonBg.color = tint;

            // Label: gold at rest, ivory-white on hover (same as original feel, improved contrast).
            var goldLabel  = new Color(0.97f, 0.90f, 0.68f, 1f);
            var ivoryHover = new Color(1.00f, 0.98f, 0.88f, 1f);
            var pressLabel = new Color(0.80f, 0.72f, 0.52f, 1f);
            var lblColor = Color.Lerp(goldLabel, ivoryHover, hoverLerp);
            lblColor = Color.Lerp(lblColor, pressLabel, pressLerp);
            if (label != null)
            {
                label.color = lblColor;
                if (!Interactable) label.color *= new Color(0.60f, 0.60f, 0.60f, 1f);
            }
        }
    }
}
