# TOMB DUEL EN — Visual & Experience Polish Report

**轮次：** 2026-10-22 打磨轮（继 Build 8 英文翻译版之后）
**负责人：** 子代理（打磨专项）
**基线 APK：** `Builds/EN/TombDuel-EN-PICO.apk`（Build 8，SHA-256: `bbd54629e3acd0971e3e86e33168ff16e1bc1d19b61d30b677ac5bd6615e4cfb`）

---

## 一、改动清单

### 1. 封面升级（TitleCover.png）

| 项目 | 改前 | 改后 |
|------|------|------|
| 生成模型 | banana (2026-09-08) | banana2 (2026-10-22) |
| 尺寸 | 2752×1536（16:9）| 2752×1536（16:9）|
| "TOMB DUEL" 字效 | 铜绿/青铜质感，中等发光 | 深度金石刻字，暖金色晕光更显著 |
| 宝箱光 | 较淡的背景光 | 大幅增强的暖金光束从后方门廊射出 |
| 画面杂散字符 | 面板上有少量杂散字符痕迹 | 干净，面板只有龙纹浮雕 |
| 气氛 | 适度 | 光柱中细腻尘埃粒子，对比度更强 |
| 拼写核验 | TOMB DUEL ✓ | TOMB DUEL ✓ |
| 水印 | 无 | 无 |
| 副标题 | 无 | 无 |

**文件：** `Assets/Resources/GuandanUI/TitleCover.png`，`Assets/Resources/GuandanUI/TripoTitle/01_generated_image_url.png`
**Task ID：** `ddad84ff-b4e0-438c-8a1d-10f2eaab5d93`（消耗 10 积分）

另有一次 1:1 草稿（task_id: `e031224a-7adc-4c4d-841d-ae66e99cb144`，10 积分），保存于 VisualQA/Polish-20261022 仅供参考，未部署。

---

### 2. 炸弹 / 同花顺视觉区分（ScreenGameUi.cs）

**改动：** 增加 `PlayStraightFlushImpact()` 方法，同花顺打出时使用不同配色与印章文案。

| 要素 | 普通炸弹 | 同花顺 |
|------|----------|--------|
| 背景遮罩 | 深红 `(0.42, 0.025, 0.012)` | 深翠绿 `(0.02, 0.18, 0.16)` |
| 震荡环 A | 橙红 | 翠绿 |
| 震荡环 B | 金黄 | 淡翠绿 |
| 震荡环 C | 绿松石 | 金黄 |
| 印章底色 | 朱红 | 墨绿 |
| 印章文字 | BOMB / {牌型} | STRAIGHT / FLUSH |

**GameDirector.cs 配合：** 当 `PlayKind.StraightFlush` 时调用 `PlayStraightFlushImpact()`，其余炸弹仍用 `PlayBombImpact()`。

**红线验证：** 未改变任何掼蛋判定逻辑。

---

### 3. 结算面板入场演出（ScreenGameUi.cs）

**改动：** 新增 `ResultEntranceRoutine(bool won)` 协程，在 `matchResultPanel` 首次从 inactive→active 切换时触发。

- **胜利：** 24 颗金色火花从屏幕中心向外爆发（0→820px），附带一圈金色闪环；运动时间 1.4 秒；粒子颜色循环 4 种金调
- **失败：** 20 颗银蓝火花，半径 560px，偏冷调
- **守卫：** `matchResultWasActive` 字段确保每局只触发一次，`Refresh()` 里重置避免重开时重复

---

### 4. 夺宝路线推进动效增强（ScreenGameUi.cs）

**改动：** `TreasureMoveRoutine()` 中宝箱开启（`chestOpened=true`）路径：
- 面板扩大至 760×130（原 620×106），字号 42（原 38）
- 新增 20 颗金色火花粒子从面板向外发散（350° 全角，范围至 430px），持续 1.7 秒
- 粒子颜色：三色金调循环（深金、橙金、亮金黄）

普通前进（`chestOpened=false`）路径：面板位置微调至 y=-430（原 y=-454），视觉上更靠近游戏区中心。

---

### 5. 夺宝路线格子光标脉动（ScreenGameUi.cs）

**改动：** `RenderRouteTrack()` 中"前沿格"（当前团队下一格，即 `index == position`）添加 ~3Hz 正弦脉动：

```csharp
var pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 3.4f);
Color.Lerp(activeColor, Color.white, 0.18f + pulse * 0.22f)
```

作用：引导玩家视线关注进度；不影响已通过格和未通过格的颜色。

---

### 6. Play 按钮激活时脉动（ScreenGameUi.cs）

**改动：** 在 `Refresh()` 中，当 `director.CanSubmitSelection` 时，Play 按钮 `normalColor` 以约 4Hz 在基础朱红与亮橙红之间脉动。失去选牌合法性时恢复静态基色。

---

### 7. 选牌状态文本颜色编码（ScreenGameUi.cs）

**改动：** `RenderHand()` 中 `selectionText.color` 根据状态动态变色：

| 状态 | 颜色 | 含义 |
|------|------|------|
| N selected · Ready | 软绿 `(0.62, 0.96, 0.72)` | 合法手牌可出 |
| N selected · Invalid | 橙红 `(0.96, 0.52, 0.38)` | 当前选牌无效 |
| Select cards / Waiting | 原米白 | 等待状态 |
| Tribute 阶段 | 偏金 `(0.95, 0.88, 0.60)` | 区分还贡 |

---

### 8. 轮到你时提示文本脉动（ScreenGameUi.cs）

**改动：** `RenderTurnCallouts()` 中，当 `PlayerSeat.South` 且处于出牌阶段，`turnCalloutSouth.color` 以 ~2.8Hz 在基础金色和亮琥珀之间脉动，吸引玩家注意。

---

### 9. 手牌选中瞬时金光（ScreenGameUi.cs）

**改动：** `HandleTarget()` 中 `UiHitKind.Card` 分支添加 `CardSelectFlashRoutine()`：
- 在手牌区位置（y=-468）创建一条 1740×18px 的金色横条
- 0.22 秒内淡入(0→0.55 alpha)再淡出，提供即时手感反馈
- 对规则/逻辑无影响

---

## 二、Tripo 调用清单

| 序号 | Task ID | 模型 | 用途 | 积分 | 状态 |
|------|---------|------|------|------|------|
| 1 | `e031224a-7adc-4c4d-841d-ae66e99cb144` | banana2 | 封面草稿（1:1，未部署） | 10 | 保存于 VisualQA/Polish-20261022 |
| 2 | `ddad84ff-b4e0-438c-8a1d-10f2eaab5d93` | banana2 16:9 | 封面正式版（已部署） | 10 | ✓ 已替换 TitleCover.png |
| 基线 | `bd2af20b-c863-4be3-bb1c-56057917089e` | banana | Build 8 封面（已覆盖） | 5 | 历史记录 |

---

## 三、性能前后对比

> **注：** 本轮纯代码 + 图片资产打磨，不增加任何新 GameObject、粒子系统或 Canvas 层；所有效果使用已有 `effectRoot` Canvas 内的程序化矩形图和 Coroutine，帧内峰值开销极低。

| 指标 | 改前（Build 8）| 改后（当前代码）|
|------|---------------|----------------|
| 新 MonoBehaviour | 0 | 0 |
| 新实时光 | 0 | 0 |
| 新粒子系统 | 0 | 0 |
| 帧内额外 UI Draw Call | ~0 常态（效果仅在事件触发后持续约 1–2s 存在） | ~0 常态 |
| 封面图尺寸 | 5.7 MB PNG（2752×1536）| 6.0 MB PNG（2752×1536）|
| 效果协程 | BombImpact, FlyCardArc, PlacementStamp, TreasureMove | 以上 + ResultEntrance, CardSelectFlash，以及 StraightFlushImpact（复用 BombImpact 路径）|
| 72fps 预算影响 | 基线通过 | 预计不影响（效果短暂，无持久化对象） |

---

## 四、对比图路径

| 画面 | 改前（参考） | 改后 |
|------|------------|------|
| 封面（原版 Build 8） | `VisualQA/Title-20260909-Raised/` | `VisualQA/Polish-20261022/01_generated_image_url_2.png` |
| 现用封面正式版 | — | `Assets/Resources/GuandanUI/TitleCover.png` |

---

## 五、红线验证

- [x] 掼蛋判定、配牌点数、队伍、夺宝流程：**未修改**（只动 UI 层）
- [x] 实体模型牌：**未恢复**
- [x] 头部骨骼番茄反馈：**未修改**（胸前轻反馈原样保留）
- [x] 牌桌 / 人物 / 墓穴场景布局：**未修改**
- [x] 对手出牌单行叠牌：**未修改**（`RenderPlayZone` 未动）
- [x] 结算文字组上下留白对称：**未修改**（`BuildMatchResultPanel` 偏移值不变）
- [x] 封面布局和按钮位置：**未修改**（按钮是代码叠层，封面只替换背景图）

---

## 六、未完成项与阻塞说明

### ⛔ 构建 APK（硬阻塞）
- **阻塞原因：** 磁盘剩余 ~13 GB，规范要求 ≥15 GB 才启动 Unity；`Library/Bee`（4.6 GB）删除权限被拒绝（Permission denied），按规范不绕过
- **当前 APK：** Build 8（`bbd54629...`）仍是设备上已安装的最新版本
- **结果：** 本轮所有代码 / 资产改动均已提交 git，待磁盘释放后下次构建时生效

### ⚠️ 截图验收（次要）
- 规范 2026-10-22 13:40 追加已降低截图要求为"可选项"
- VisualQA/EN-20261022/ 的一张封面截图 (`desktop_snapshot_01.png`) 为 Build 8 运行画面

### ⚠️ GameplayComfortValidation / GuandanRuleSmokeTests
- 规范要求继续通过
- 本轮未修改游戏规则逻辑，现有测试断言不受影响
- 由于无法构建，无法在本轮实测验证；待下次构建后验证

---

## 七、CJK 扫描（打磨范围）

本轮改动仅涉及 `ScreenGameUi.cs` 和 `GameDirector.cs`。新增/修改字符串全为英文。
```
grep -n "[一-鿿]" 新增行 → 0 hits
```

---

*报告生成时间：2026-10-22*
