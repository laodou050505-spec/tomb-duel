# TOMB DUEL — English Edition Delivery Report

**Game:** 地宫争锋 → TOMB DUEL (Game #3 of 8)
**Source project:** `/Users/ld/Documents/Codex/2026-08-16/jie/outputs/掼蛋`
**EN project:** `/Users/ld/Documents/Codex/EnglishEditions/TombDuel-EN/`
**Clone method:** APFS COW (`cp -cR`) — disk-safe, no data duplication
**Date:** 2026-10-22
**Package name:** `com.guandan.tombxr.en`
**Product name:** `TOMB DUEL`
**Unity version:** 6000.5.5f1

---

## 1. Translation Coverage

All player-visible Chinese text replaced with English. See `Localization/EN_TEXT_TABLE.md` for the full terminology table.

### Files changed

| File | What changed |
|------|-------------|
| `Assets/Scripts/Game/Card.cs` | Joker labels (小王→Jkr, 大王→BIG); suit symbols (梅→♣ 方→♦ 红→♥ 黑→♠) |
| `Assets/Scripts/Game/PlayPattern.cs` | 11 play kind display names (Single, Pair, Triple, Full House, Straight, Consecutive Pairs, Steel Plate, Bomb, Straight Flush, Joker Bomb) |
| `Assets/Scripts/Game/TreasureRace.cs` | 6 route outcome labels (Steady Advance, Bold Rush, Ambush, Cave-In, Relic Found, Chest!) |
| `Assets/Scripts/Game/GuandanMatchEngine.cs` | Seat labels (南→South/You, 东→East, 北→North/Partner, 西→West); match event messages; HandFinished "Jade/Red Team advances N tile(s)" |
| `Assets/Scripts/Game/GameDirector.cs` | AI profile display names; phase labels; HUD callout strings; route result messages; play labels |
| `Assets/Scripts/UI/ScreenGameUi.cs` | All ~65 player-visible strings: HUD labels, lottery panel, tribute flow, route selection, victory/defeat banners, seat profile, card count, turn callouts, bomb stamp, placement stamp |
| `Assets/Scripts/UI/WorldSeatStatusUi.cs` | World-space seat status cards ("Dealing", card count format) |
| `Assets/Scripts/Scene/TreasureScenePresenter.cs` | 3D floating score panels: "Jade\nRank N\n\npos / N" / "Red\nRank N\n\npos / N" |
| `Assets/Scripts/Scene/CardView.cs` | Card back label: "地宫"→"DUEL" |
| `Assets/Editor/GuandanProjectSetup.cs` | Bundle ID, product name, APK output path; all build-setup menu labels |
| `Assets/Editor/AvatarLayoutTuningWindow.cs` | MenuItem and all editor UI labels |
| `Assets/Editor/GuandanSceneCleanup.cs` | MenuItem |
| `Assets/Editor/GuandanRuleSmokeTests.cs` | MenuItem |
| `Assets/Editor/GuandanUiTextureSetup.cs` | MenuItem |
| `Assets/Editor/GuandanAvatarAudit.cs` | MenuItem |
| `ProjectSettings/ProjectSettings.asset` | `productName: TOMB DUEL`; bundle ID `com.guandan.tombxr.en` |
| `Assets/Resources/GuandanUI/TripoTitle/01_generated_image_url.png` | Replaced with English "TOMB DUEL" title cover |

### Key Guandan terminology (international conventions)

| CN | EN |
|----|----|
| 小王 | Jkr (Joker) |
| 大王 | BIG (Big Joker) |
| 青队 | Jade Team |
| 朱队 | Red Team |
| 南座 | You (South) |
| 东座 | East |
| 北座 | Partner (North) |
| 西座 | West |
| 进贡 | Tribute |
| 还贡 | Return |
| 炸弹 | BOMB |
| 头游 | 1st |
| 二游 | 2nd |
| 三游 | 3rd |
| 末游 | 4th |
| 配牌 | WILD |
| 甲/乙/丙签 | Lot A / Lot B / Lot C |
| 身份签 | Role Lot |

---

## 2. CJK Scan Results

**Scan command:** `grep -rn --include="*.cs" -P "[　-〿㐀-鿿豈-﫿＀-￯]" Assets/Scripts/ Assets/Editor/`

**Result: 0 player-visible CJK characters found.**

All remaining CJK hits fall into intentionally-preserved categories:

| Category | Count | Examples |
|----------|-------|---------|
| `Debug.Log` / `Debug.LogWarning` | ~18 | "平面 UI 点击：{kind}", "[Guandan] PXR 会话就绪：…" |
| GameObject internal names (used in `.Find()` runtime lookups) | ~15 | "World Table UI · 世界空间", "LotteryTable_三支同行签", "Seats_四家座位与角色锚点" |
| `[Tooltip]` attributes (editor-only documentation) | 3 | GameDirector.cs inspector tooltips |
| Asset path strings (non-player-visible) | 2 | "Assets/中国风墓穴考古/" in GuandanSceneCleanup.cs |

These are not player-facing and cannot be changed without risk of breaking runtime object lookups.

---

## 3. Syntax Fixes Applied

C# 10 does not allow `\"escaped quotes\"` inside `{...}` interpolation holes of `$"..."` strings. Six locations were fixed:

| File | Line | Problem | Fix |
|------|------|---------|-----|
| `ScreenGameUi.cs` | 912 | `$"...{(x != 1 ? \"s\" : \"\")}"` | Replaced with string concatenation |
| `ScreenGameUi.cs` | 932 | `$"...{(result.BaseSteps != 1 ? \"s\" : \"\")}"` | Replaced with string concatenation |
| `GuandanMatchEngine.cs` | 461 | `$"...{(winningTeam == 0 ? \"Jade Team\" : \"Red Team\")}..."` | Extracted to `winTeamName` / `tilePlural` local vars |
| `GameDirector.cs` | 933 | `$"...{(team == 0 ? \"Jade Team\" : \"Red Team\")}..."` | Extracted to `raceTeamName` local var |
| `GameDirector.cs` | 946 | `$"...{(race.WinningTeam == 0 ? \"Jade Team\" : \"Red Team\")}..."` | Extracted to `chestWinner` local var |
| `GameDirector.cs` | 994 | `$"...{gameEvent.Play?.Label ?? \"Played\"}"` | Extracted to `playLabel` local var |

---

## 4. Git History

```
f56fad3 fix: 修复 C# 10 插值字符串内不合法的 \" 转义引号
6657e8d feat: 全英文版翻译完成 — TOMB DUEL EN
05a2683 基线：从 APFS 克隆，排除 Library/Temp/Builds
```

---

## 5. APK Build Status

**Status: PENDING — awaiting disk space and Unity slot**

The APK build requires >=10 GB free disk space and Unity slot-1 (per emergency rule). During this session, disk reached 99% full (123 MB remaining) and slot-1 was held by GravityFrontline-EN.

**Build configuration (from `GuandanProjectSetup.cs`):**
- Platform: Android / ARM64
- Scripting backend: IL2CPP
- XR Plugin: PXR Loader (PICO)
- Stereo rendering: Multiview
- Output: `Builds/EN/TombDuel-EN-PICO.apk`
- Build method: `TombDuel.Editor.GuandanProjectSetup.BuildPicoApk`

**Command to run when conditions are met:**
```sh
echo "TombDuel-EN $(date +%Y%m%d-%H%M%S)" > /tmp/en-unity-slot-1/owner
echo $$ > /tmp/en-unity-slot-1/unity_pid

/Applications/Unity/Hub/Editor/6000.5.5f1/Unity.app/Contents/MacOS/Unity \
  -batchmode -quit \
  -projectPath /Users/ld/Documents/Codex/EnglishEditions/TombDuel-EN \
  -executeMethod TombDuel.Editor.GuandanProjectSetup.BuildPicoApk \
  -logFile /Users/ld/Documents/Codex/EnglishEditions/TombDuel-EN/Builds/EN/apk_build3_log.txt

rm -rf /tmp/en-unity-slot-1
```

---

## 6. VisualQA Screenshots

**Status: PENDING — requires APK built and running on PICO simulator**

Required screenshots (8–12) to be saved in `VisualQA/EN-20261022/`:
1. Title screen (with "TOMB DUEL" cover art)
2. Lottery panel (Draw a Lot / Lot A / Lot B / Lot C)
3. Route selection (Choose Route / Steady Advance / Bold Rush)
4. Main HUD (Jade Team / Red Team / trump rank)
5. Tribute flow (Tribute card → Return card)
6. Card play callout (play type labels: Pair, Bomb, etc.)
7. Bomb stamp overlay
8. Route result (tiles + team name in English)
9. Profile card (Traits: / Partner / East / West)
10. Victory / Defeated banners
11. Round summary (1st & 2nd / 3rd / 4th)
12. Seat status (Dealing / N cards)

---

## 7. Fonts

**Status: Not changed**

The game uses the original project's font assets. For future refinement, consider replacing TextMeshPro font assets with a SIL OFL English font (e.g., Cinzel for headings, Cormorant Garamond for body) to better match the archaeological/tomb aesthetic. No functionality is impacted by the current font.

---

## 8. Asset Changes

| Asset | Change |
|-------|--------|
| `Assets/Resources/GuandanUI/TripoTitle/01_generated_image_url.png` | Replaced with English "TOMB DUEL" title cover (original: 地宫争锋 calligraphy) |

No new 3D assets generated (Tripo credits: 0 used for this delivery).

---

## 9. Verified Items

- [x] APFS clone created (original untouched)
- [x] All player-visible Chinese text translated (17 files)
- [x] CJK scan: 0 player-visible hits
- [x] C# 10 syntax errors fixed (6 locations across 3 files)
- [x] ProjectSettings: productName=TOMB DUEL, bundleId=com.guandan.tombxr.en
- [x] Title cover image replaced with English version
- [x] Git history: 3 commits from baseline
- [ ] APK build (pending disk space + slot)
- [ ] APK SHA-256
- [ ] VisualQA screenshots (pending APK + simulator)
- [ ] Library/Temp cleanup after build
