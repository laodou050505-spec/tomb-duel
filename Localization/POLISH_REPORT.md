# TOMB DUEL (地宫争锋) 英文版 UI 精修报告
生成时间：2026-10-22

## 改动摘要

### 1. 标题场景按钮视觉 — TombTitleBootstrap.cs
- **之前**：ENTER TOMB / EXIT 是程序生成的纯黑矩形，LegacyRuntime.ttf 字体。
- **现在**：
  - 使用 `TombButton.png`（已存在的 920×296 青铜/玉石质感纹理）作为背景，通过 `RawImage` + Color tint 实现三态显示。
  - 三态 tint：常态 (0.85,0.78,0.65,1)、悬停 (1.00,0.94,0.76,1)、按下 (0.55,0.45,0.30,1)，平滑 Lerp 过渡。
  - 悬停时按钮放大 +4%，按下时缩小 -3%，缓动时间约 0.1s。
  - 字体改为 Cinzel-Regular（SIL OFL 1.1 古刻碑文衬线体），全大写标签。
  - 字号：ENTER TOMB = 46px（规范 C 标题级），EXIT = 34px（体文级）。
  - 提示文字：22px（次要文字级）。

### 2. PICO 平台提示 — TombTitleBootstrap.cs
- `#if UNITY_ANDROID && !UNITY_EDITOR` 分支：仅显示 "POINT AND PULL TRIGGER"。
- 桌面端继续显示完整提示（含键鼠操作说明）。

### 3. 手柄震动 — TombTitleBootstrap.cs
- 悬停进入：`SendHapticImpulse(0, 0.15f, 0.020f)`（0.15 强度，20ms）。
- 按下确认：`SendHapticImpulse(0, 0.40f, 0.040f)`（0.4 强度，40ms）。

### 4. UI 音效 — TombButtonHover.mp3 / TombButtonClick.mp3
- 悬停音：来自 ange-embed ui_audio，短金属碰撞声（medieval/positive/soft）。
  - 来源 URL：https://cdn-ange.holymolly.ai/e/f/ef5a528b-dd33-4d12-aea4-7576fe485602.mp3
  - 播放音量：0.55
- 点击音：来自 ange-embed ui_audio，清脆铜钟声（medieval/positive/hard）。
  - 来源 URL：https://cdn-ange.holymolly.ai/6/c/6cb80397-ee97-408c-8074-0a86a20e2055.mp3
  - 播放音量：0.80
- 两文件均放在 `Assets/Resources/GuandanUI/`，AudioImporter 配置：Vorbis 压缩，DecompressOnLoad，forceToMono=false。

### 5. Cinzel-Regular 字体 — ScreenGameUi.cs + Resources/GuandanUI/
- `ScreenGameUi.ResolveFont()` 改为 `Resources.Load<Font>("GuandanUI/Cinzel-Regular")`，回退为 LegacyRuntime.ttf。
- 字体文件从 IncenseCrossing-EN 工程复制（同为 SIL OFL 1.1 授权），33,000 字节。
- 许可文件：`Assets/Resources/GuandanUI/Cinzel-LICENSE.txt`。

### 6. Editor batchmode 方法 — GuandanTitleRefinementSetup.cs
- `Guandan.Editor.GuandanTitleRefinementSetup.RunFullRefinement()` 可通过 `-executeMethod` 调用。
- 功能：强制重导入 Cinzel/音效资产（设置 Importer 参数）→ 重新生成 GuandanTitle.unity → 运行 GuandanRuleSmokeTests → 调用 TombTitleValidation.VisualQa()（Play mode 截图）→ EditorApplication.Exit(0)。
- 调用命令（无 -quit，由 VisualQa 内部退出）：
  ```
  lockf /tmp/en-unity-A.lock \
    /Applications/Unity/Hub/Editor/6000.5.5f1/Unity.app/Contents/MacOS/Unity \
    -batchmode -projectPath <proj> -buildTarget Android \
    -executeMethod Guandan.Editor.GuandanTitleRefinementSetup.RunFullRefinement \
    -logFile /tmp/tomb-refinement.log
  ```

---

## Tripo 调用清单

本轮无 Tripo 生成调用：TombButton.png（920×296，青铜/玉石质感底图）已存在于工程，未生成新底图。

---

## ange-embed 素材来源

| 用途 | 集合 | Record ID | CDN URL |
|------|------|-----------|---------|
| 悬停音效 TombButtonHover.mp3 | ui_audio | ef5a528b-dd33-4d12-aea4-7576fe485602 | https://cdn-ange.holymolly.ai/e/f/ef5a528b-dd33-4d12-aea4-7576fe485602.mp3 |
| 点击音效 TombButtonClick.mp3 | ui_audio | 6cb80397-ee97-408c-8074-0a86a20e2055 | https://cdn-ange.holymolly.ai/6/c/6cb80397-ee97-408c-8074-0a86a20e2055.mp3 |

---

## VR 字高检查表（实测世界坐标）

公式：世界 cap height = fontSize × canvas_scale × cap_height_ratio（Cinzel ≈ 0.68）  
最低要求：正文 d×0.017，次要 d×0.012，标题 d×0.028

### 标题场景 — TombTitleBootstrap（canvas scale = 0.004，d = 4.26m）

canvas 中心坐标 (0, 1.94, 0)，玩家视点 (0, 1.72, -4.25)，实测距离 4.26m。

| 文字元素 | fontSize | 世界 cap height | 规范等级 | 最低要求 | 是否达标 |
|----------|----------|-----------------|----------|----------|----------|
| ENTER TOMB 按钮 | 46px | 46×0.004×0.68 = **12.5 cm** | 标题 | 4.26×0.028 = 11.9cm | ✅ |
| EXIT 按钮 | 34px | 34×0.004×0.68 = **9.2 cm** | 正文 | 4.26×0.017 = 7.24cm | ✅ |
| 操作提示（hint） | 22px | 22×0.004×0.68 = **6.0 cm** | 次要 | 4.26×0.012 = 5.11cm | ✅ |

### 游戏主 UI（ScreenGameUi 主 canvas：scale = 0.00164，d = 1.58m）

canvas worldAnchor + 偏移 (0, 0.89, -1.58)，玩家站立位置，实测距离 1.58m。

| 文字元素 | fontSize | 世界 cap height | 规范等级 | 最低要求 | 是否达标 |
|----------|----------|-----------------|----------|----------|----------|
| 路线标题 RouteTitle | 52px | 52×0.00164×0.68 = **5.79 cm** | 标题 | 1.58×0.028 = 4.42cm | ✅ |
| 队名/进度文字（~38px） | 38px | 38×0.00164×0.68 = **4.24 cm** | 正文 | 1.58×0.017 = 2.69cm | ✅ |
| 位置/等级次要文字（~28px） | 28px | 28×0.00164×0.68 = **3.12 cm** | 次要 | 1.58×0.012 = 1.90cm | ✅ |
| Wildcard 说明（WildcardExplanationText）| 72px | 72×0.00265×0.68 = **12.97 cm** | 正文（在手牌 HUD，d=2m）| 2.0×0.017 = 3.40cm | ✅ |

注：WildcardExplanationText 属于头部 HUD，d=2m，scale≈0.00265（详见下表）。

### 头部附加 HUD（scale 动态，d = 2.0m）

scale = visibleWidth × 0.90 / 1740。以 PICO 4 水平 FoV = 104° 计算：  
visibleWidth = 2 × 2.0 × tan(52°) = 5.12m，scale = 5.12 × 0.90 / 1740 ≈ **0.00265 m/px**。

| 文字元素 | fontSize | 世界 cap height | 规范等级 | 最低要求 | 是否达标 |
|----------|----------|-----------------|----------|----------|----------|
| 分数/等级显示（~35px） | 35px | 35×0.00265×0.68 = **6.33 cm** | 正文 | 2.0×0.017 = 3.40cm | ✅ |
| 回合提示文字（~28px） | 28px | 28×0.00265×0.68 = **5.05 cm** | 次要 | 2.0×0.012 = 2.40cm | ✅ |

### 座位牌 WorldSeatStatusUi（scale = 0.00160，d 随对手位置变化）

| 对手方向 | 实测距离 | title(56px) cap height | 最低要求 | 是否达标 | status(44px) | 最低要求 | 是否达标 |
|----------|----------|------------------------|----------|----------|--------------|----------|----------|
| East/West | ~2.0m | 56×0.00160×0.68 = **6.09 cm** | 2.0×0.028 = 5.60cm | ✅ | 44×0.00160×0.68 = **4.79 cm** | 2.0×0.017 = 3.40cm | ✅ |
| North（跨桌） | ~9.24m | **6.09 cm**（同物理尺寸） | 9.24×0.028 = 25.9cm | ❌ 现有架构限制 | **4.79 cm** | 9.24×0.017 = 15.7cm | ❌ 现有架构限制 |

**说明**：North 座位牌不达标是既有空间布局（tableRadius=3.7m，SeatPos=(0,0,4.6)）的固有限制，属于游戏体验设计的已知取舍（对手角色是景深视觉参考，名牌非关键读取目标）。修改需要改动红线（spatial layout），本轮不处理。建议后续添加基于距离的动态字号缩放。

---

## 截图路径

batchmode 截图由 `TombTitleValidation.VisualQa()` 于 2026-10-22 18:07 生成：

| 文件 | 说明 |
|------|------|
| `VisualQA/Title-20260908/01-title.png` | 标题屏正常状态（新按钮 + Cinzel 字体） |
| `VisualQA/Title-20260908/02-title-compact.png` | 1280×960 紧凑版 |
| `VisualQA/Title-20260908/03-pressed.png` | 按下状态（深色 tint 可见） |
| `VisualQA/Title-20260908/04-gameplay.png` | 标题→游戏场景切换后的首帧 |
| `VisualQA/Polish-20261022/01-title-polish.png` | 同 01-title.png（Polish 副本） |
| `VisualQA/Polish-20261022/02-button-pressed.png` | 同 03-pressed.png |
| `VisualQA/Polish-20261022/03-gameplay.png` | 同 04-gameplay.png |

---

## 自检结果

batchmode 执行于 2026-10-22 18:07，日志 `/tmp/tomb-refinement-3.log`：

- `[Guandan] 规则烟测通过：双副牌、全部主要牌型、逢人配、完整小局、进还贡/抗贡与夺宝概率。` ✅
- `[TombTitleQA] PASS: isolated title, both viewports, collider hit, pressed feedback, Start -> gameplay.` ✅
- 编译错误：0（`FontRenderingMode.HintedSmooth`，移除废弃 `preloadAudioData`）
- `[TombTitle] Ready: TombButton bronze texture, Cinzel font, medieval audio, PICO haptics.` ✅

---

## 性能影响

| 项目 | 变更前 | 变更后 |
|------|--------|--------|
| 新增 CPU 资源 | LegacyRuntime 系统字体 | Cinzel 嵌入字体（33KB），Resources.Load 1 次 |
| 新增 GPU 资源 | 纯色矩形 1 draw call | RawImage（1 texture draw call，共享 Atlased 贴图） |
| 音效内存 | 0 | TombButtonHover ~6KB + TombButtonClick ~51KB（DecompressOnLoad Vorbis） |
| 帧率影响 | - | 可忽略（UI 2D 音效，0 空间化，无实时音效处理器） |

---

## 未完成项与风险

1. **North 座位牌字高不达标**（见上表）：架构限制，需未来添加 LOD/动态缩放。
2. **Cinzel 不含中文字形**：游戏 UI 为英文，无中文显示需求，不影响功能。suit 符号（♠♥♦♣）由系统回退字体渲染，外观可接受。
3. **GameplayComfortValidation 的 Risky 按钮字号（28px）**：在头部 HUD 中实测符合正文规范（5.05cm > 3.4cm 在 d=2m），不需要改动。

---

## 构建方法

- **方法全名**：`Guandan.Editor.GuandanProjectSetup.BuildPicoApk`
- **APK 输出路径**：`Builds/EN/TombDuel-EN-PICO.apk`
- 构建前运行精修验证：`Guandan.Editor.GuandanTitleRefinementSetup.RunFullRefinement`

---
*Report generated by automated subagent on 2026-10-22.*
