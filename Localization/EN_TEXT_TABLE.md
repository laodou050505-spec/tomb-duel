# TOMB DUEL — English Text Table
Generated: 2026-10-22

---

## Guandan Terminology (术语表)

International Guandan community conventions used throughout:

| Chinese | English | Notes |
|---------|---------|-------|
| 掼蛋 | Guandan / TOMB DUEL (product name) | Card game name; product branded as TOMB DUEL |
| 级牌 | Trump Rank / Level Card | The rank currently being played |
| 逢人配 | Wild Card | Card of the current trump rank that acts as any card |
| 进贡 | Tribute | Loser pays a card to the winner |
| 还贡 | Return Tribute | Winner returns a card to the loser |
| 抗贡 | Anti-Tribute | Two Big Jokers negate tribute obligation |
| 单张 | Single | One card |
| 对子 | Pair | Two cards of same rank |
| 三张 | Triple | Three cards of same rank |
| 三带二 | Full House | Triple + Pair |
| 顺子 | Straight | 5+ consecutive ranks |
| 连对 | Consecutive Pairs | 3+ consecutive pairs |
| 钢板 | Steel Plate | 3+ consecutive triples |
| 炸弹 | Bomb | 4+ cards of same rank |
| 同花顺 | Straight Flush | 5-card same-suit straight |
| 四王炸 | Joker Bomb | Two Small + Two Big Jokers |
| 小王 | Jkr (Small Joker) | Displayed on card face |
| 大王 | BIG (Big Joker) | Displayed on card face |
| 梅 (梅花) | ♣ | Suit label on card face; Card.cs uses single char "梅" |
| 方 (方块) | ♦ | Suit label on card face; Card.cs uses single char "方" |
| 红 (红桃) | ♥ | Suit label on card face; Card.cs uses single char "红" |
| 黑 (黑桃) | ♠ | Suit label on card face; Card.cs uses single char "黑" |
| 青队 | Jade Team | Team 0 |
| 朱队 | Red Team | Team 1 |
| 南 (你) | You / South | Human player seat |
| 东 | East | Opponent seat |
| 北 | Partner / North | Partner seat |
| 西 | West | Opponent seat |
| 头游 | 1st place | First to play out |
| 二游 | 2nd place | Second to play out |
| 三游 | 3rd place | Third to play out |
| 末游 | 4th place | Last to play out |
| 甲签/乙签/丙签 | Lot A / Lot B / Lot C | Role lottery lots |
| 夺宝 | Treasure Race | The tile-advancing mini-game |
| 稳当推进 | Steady Advance | Safe route choice (no risk) |
| 深入探方 | Risky Dig | Risk route choice (variable outcome) |

---

## Play Kind Labels (PlayPattern.cs)

| Key | Chinese | English | Source | Scene |
|-----|---------|---------|--------|-------|
| Single | 单张 | Single | PlayPattern.cs:Label | Hint / played card UI |
| Pair | 对子 | Pair | PlayPattern.cs:Label | Hint / played card UI |
| Triple | 三张 | Triple | PlayPattern.cs:Label | Hint / played card UI |
| FullHouse | 三带二 | Full House | PlayPattern.cs:Label | Hint / played card UI |
| Straight | 顺子 | Straight | PlayPattern.cs:Label | Hint / played card UI |
| ConsecutivePairs | 连对 | Consecutive Pairs | PlayPattern.cs:Label | Hint / played card UI |
| SteelPlate | 钢板 | Steel Plate | PlayPattern.cs:Label | Hint / played card UI |
| Bomb | {N}张炸弹 | {N}-card Bomb | PlayPattern.cs:Label | Hint / played card UI |
| StraightFlush | 同花顺 | Straight Flush | PlayPattern.cs:Label | Hint / played card UI |
| JokerBomb | 四王炸 | Joker Bomb | PlayPattern.cs:Label | Hint / played card UI |
| Invalid | 无效牌型 | Invalid | PlayPattern.cs:Label | Error feedback |

---

## Treasure Race Outcomes (TreasureRace.cs)

| Key | Chinese | English | Source |
|-----|---------|---------|--------|
| Double | 灵光乍现，推进翻倍 | Flash of insight — advance doubled! | TreasureRace.cs:Label |
| PlusOne | 抢先一步，多进一格 | Quick step — advance +1! | TreasureRace.cs:Label |
| Normal | 按步推进 | Steady advance | TreasureRace.cs:Label |
| MinusOne | 脚下一滑，少进一格 | A stumble — advance −1 | TreasureRace.cs:Label |
| Retreat | 触发机关，倒退一格 | Trap triggered — retreat 1 tile | TreasureRace.cs:Label |
| default | 推进 | Advance | TreasureRace.cs:Label |

---

## Card Faces (Card.cs, CardView.cs)

| Key | Chinese | English | Source | Scene |
|-----|---------|---------|--------|-------|
| SmallJoker | 小王 | Jkr | Card.cs:RankLabel | Card face |
| BigJoker | 大王 | BIG | Card.cs:RankLabel | Card face |
| SuitClubs | 梅 | ♣ | Card.cs:SuitLabel | Card face |
| SuitDiamonds | 方 | ♦ | Card.cs:SuitLabel | Card face |
| SuitHearts | 红 | ♥ | Card.cs:SuitLabel | Card face |
| SuitSpades | 黑 | ♠ | Card.cs:SuitLabel | Card face |
| CardBack | 掼\n蛋 | DUEL | CardView.cs:29 | Physical 3D card back |

---

## Match Engine Messages (GuandanMatchEngine.cs)

| Key | Chinese | English | Source |
|-----|---------|---------|--------|
| SeatSouth | 你 | You | SeatLabel() |
| SeatEast | 东家 | East | SeatLabel() |
| SeatNorth | 队友 | Partner | SeatLabel() |
| SeatWest | 西家 | West | SeatLabel() |
| HandStart | 第 {N} 局，级牌 {R} | Round {N} · Trump Rank {R} | HandStarted event |
| TributeHighest | 这张牌不是当前可进贡的最大牌 | That card is not your highest tribute card | Error |
| TributePaid | {seat}完成进贡 | {seat} paid tribute | Event |
| ReturnInvalid | 这张牌不能用于还贡 | That card cannot be used for Return Tribute | Error |
| ReturnPaid | {seat}向{payer}还贡 | {seat} returned tribute to {payer} | Event |
| NotYourTurn | 还没轮到该玩家 | Not your turn yet | Error |
| NoValidPlay | 没有选择有效牌型 | No valid play selected | Error |
| HandChanged | 手牌状态已经变化 | Hand has changed — please retry | Error |
| CantBeat | 所选牌不能压过桌面牌型 | Those cards cannot beat the current play | Error |
| Played | {seat}打出{label} | {seat} played {label} | Event |
| Finished | {seat}获得第 {N} 名 | {seat} finished #{N} | Event |
| CantPass | 当前不能不出 | Cannot pass right now | Error |
| Passed | {seat}不出 | {seat} passed | Event |
| NewLead | 新一轮由{seat}领出 | New trick — {seat} leads | Event |
| AntiTribute | 两张大王抗贡，本局免贡 | Two Big Jokers — Anti-Tribute: no tribute this round | Event |
| TributeAssigned | {payer}向{recipient}进贡 | {payer} paid tribute to {recipient} | Event |
| HandFinished | {team}推进 {N} 格 | {team} advances {N} tile(s) | Event |
| JadeTeam | 青队 | Jade Team | Team name |
| RedTeam | 朱队 | Red Team | Team name |

---

## World Seat Status (WorldSeatStatusUi.cs)

| Key | Chinese | English | Source |
|-----|---------|---------|--------|
| FinishedNth | 第 {N} 名 | #{N} | WorldSeatStatusUi.cs |
| CardCountUnknown | ?张 | ? | WorldSeatStatusUi.cs |
| CardCount | {N}张 | {N} | WorldSeatStatusUi.cs |
| CurrentAction | 当前 · {action} | Now: {action} | WorldSeatStatusUi.cs |
| ReturningTribute | 正在还贡… | Returning tribute… | WorldSeatStatusUi.cs |
| Thinking | 思考中… | Thinking… | WorldSeatStatusUi.cs |
| Waiting | 等待行动 | Waiting… | WorldSeatStatusUi.cs |

---

## Game Director (GameDirector.cs)

### AI Profiles (line 1514–1519, 145)

All 5 fields per profile: Name, Title (type), Description (long), Note (interaction hint), Traits (array).

| Key | Chinese (原文精确) | English | Source |
|-----|--------------------|---------|--------|
| Player_Name | 你 | You | GameDirector.cs:145 |
| Player_Title | 真人发掘领队 | Human Excavation Leader | GameDirector.cs:145 |
| Player_Desc | 与你的队友一起完成牌局并打开中央宝箱。 | Complete the card hands with your teammate and unlock the central treasure chest. | GameDirector.cs:145 |
| Player_Note | 稳住牌权，先看清桌面。 | Hold the lead; read the table first. | GameDirector.cs:145 |
| Player_Traits | 真人, 发掘领队 | Human, Leader | GameDirector.cs:145 |
| ShenYan_Name | 沈砚 | Shen Yan | GameDirector.cs:1514 |
| ShenYan_Title | 谨慎的计算派 | The Careful Calculator | GameDirector.cs:1514 |
| ShenYan_Desc | 习惯先看搭档的牌路，能稳就不冒险。复杂局面会思考很久，但很少冲动出炸。 | Reads the partner's style first; avoids risk when stable. Thinks long in complex positions, but rarely blitzes with a Bomb. | GameDirector.cs:1514 |
| ShenYan_Note | 会轻轻敲桌，再把牌推到中央。 | Gently taps the table, then slides cards to center. | GameDirector.cs:1514 |
| ShenYan_Traits | 沉稳, 重配合, 偏稳当 | Composed, Team-focused, Conservative | GameDirector.cs:1514 |
| TangGuo_Name | 唐果 | Tang Guo | GameDirector.cs:1515 |
| TangGuo_Title | 爱冒险的气氛组 | The Thrill-Seeker | GameDirector.cs:1515 |
| TangGuo_Desc | 出牌节奏快，落后时尤其喜欢赌一把。收到鲜花会认真接住。 | Fast-paced; loves to gamble when behind. Catches flowers thrown at them with flair. | GameDirector.cs:1515 |
| TangGuo_Note | 会先看向宝箱，再迅速拍下手里的牌。 | Glances at the treasure chest, then slaps cards down fast. | GameDirector.cs:1515 |
| TangGuo_Traits | 活泼, 敢冒险, 爱鲜花 | Lively, Risk-taker, Flower lover | GameDirector.cs:1515 |
| QinZheng_Name | 秦峥 | Qin Zheng | GameDirector.cs:1516 |
| QinZheng_Title | 强硬的压制派 | The Dominant Aggressor | GameDirector.cs:1516 |
| QinZheng_Desc | 喜欢抢回牌权，炸弹出手干脆。赢下一轮后会向对面扬眉。 | Loves reclaiming the lead; Bombs come out decisively. Raises an eyebrow at opponents after winning a trick. | GameDirector.cs:1516 |
| QinZheng_Note | 被西红柿砸中后，大概率会立刻扔回来。 | Hit by a tomato? Almost certainly throws one right back. | GameDirector.cs:1516 |
| QinZheng_Traits | 强硬, 好胜, 爱压制 | Firm, Competitive, Domineering | GameDirector.cs:1516 |
| SuHe_Name | 苏禾 | Su He | GameDirector.cs:1517 |
| SuHe_Title | 温和的协作派 | The Gentle Cooperator | GameDirector.cs:1517 |
| SuHe_Desc | 会主动给搭档留出牌权，不轻易盖住队友。 | Willingly yields the lead to a partner; never buries a teammate's play. | GameDirector.cs:1517 |
| SuHe_Note | 收到鲜花时会别在衣襟上，保持到本小局结束。 | Pins a flower to their lapel and keeps it through the round. | GameDirector.cs:1517 |
| SuHe_Traits | 温和, 顾搭档, 有耐心 | Gentle, Partner-minded, Patient | GameDirector.cs:1517 |
| LuoFang_Name | 罗放 | Luo Fang | GameDirector.cs:1518 |
| LuoFang_Title | 爱热闹的挑衅派 | The Playful Provocateur | GameDirector.cs:1518 |
| LuoFang_Desc | 牌桌动作很多，喜欢用表情扰乱气氛，但真正出牌并不莽撞。 | Many table gestures; likes stirring the atmosphere with expressions — but never plays recklessly. | GameDirector.cs:1518 |
| LuoFang_Note | 最容易躲西红柿，也最容易捡起来扔回去。 | Easiest to dodge tomatoes, and most likely to pick one up and return fire. | GameDirector.cs:1518 |
| LuoFang_Traits | 幽默, 爱互动, 会反击 | Humorous, Interactive, Retaliatory | GameDirector.cs:1518 |
| YeLan_Name | 叶岚 | Ye Lan | GameDirector.cs:1519 |
| YeLan_Title | 沉默的快打派 | The Silent Speed Player | GameDirector.cs:1519 |
| YeLan_Desc | 喜欢尽快清理手牌，局势明朗时会突然提速。 | Aims to clear the hand quickly; accelerates suddenly when the game is clear. | GameDirector.cs:1519 |
| YeLan_Note | 很少说话，但会用一张漂亮的顺子回应。 | Rarely speaks, but answers with a beautiful Straight. | GameDirector.cs:1519 |
| YeLan_Traits | 安静, 快打, 看残局 | Quiet, Fast-player, Endgame reader | GameDirector.cs:1519 |

### 3D World Buttons and Labels

| Key | Chinese (原文精确) | English | Source | Scene |
|-----|--------------------|---------|--------|-------|
| Btn3D_Play | 出牌 | Play | GameDirector.cs:1200 | 3D button |
| Btn3D_Pass | 不出 | Pass | GameDirector.cs:1201 | 3D button |
| Btn3D_Hint | 提示 | Hint | GameDirector.cs:1202 | 3D button |
| Btn3D_Continue | 下一局 | Next Round | GameDirector.cs:1203 | 3D button |
| Btn3D_GoRace | 前往夺宝 | Go to Race | GameDirector.cs:1206 | 3D button |
| Btn3D_Steady | 稳当探方 | Steady | GameDirector.cs:1204 | 3D route button |
| Btn3D_Risky | 激进探方 | Risky | GameDirector.cs:1205 | 3D route button |
| RaceLabel_Init | 夺宝 · 青 0 / 12    朱 0 / 12 | Race · Jade 0 / 12   Red 0 / 12 | GameDirector.cs:1199 | 3D race HUD |
| RaceLabel_Live | 夺宝 · 青 {J} / {max}    朱 {R} / {max} | Race · Jade {J} / {max}   Red {R} / {max} | GameDirector.cs:1369 | 3D race HUD |

### Seat Suffix Labels (GameDirector.cs:1361)

| Key | Chinese (原文精确) | English | Source |
|-----|--------------------|---------|--------|
| SeatSuffix_Place | · 第 {N} 名 | · #{N} | GameDirector.cs:1361 |
| SeatSuffix_Unknown | · ?张 | · ? | GameDirector.cs:1361 |
| SeatSuffix_Count | · {N}张 | · {N} | GameDirector.cs:1361 |

### Phase Labels (GameDirector.cs:1403–1407)

| Key | Chinese (原文精确) | English | Source |
|-----|--------------------|---------|--------|
| Phase_TributeReturn | 还贡 · 请选择一张牌 | Return Tribute · Select a card | GameDirector.cs:1403 |
| Phase_Playing | 掼蛋 · 级牌 {R} · {seat}行动 | Guandan · Rank {R} · {seat} to act | GameDirector.cs:1404 |
| Phase_HandComplete | 夺宝 · 选择探方路线 | Treasure Race · Choose route | GameDirector.cs:1405 |
| Phase_MatchComplete | 地宫宝箱已开启 | Tomb Chest Opened | GameDirector.cs:1406 |
| Phase_Default | 墓室牌局 | Tomb Card Table | GameDirector.cs:1407 |

### Deal Announcement

| Key | Chinese (原文精确) | English | Source | Scene |
|-----|--------------------|---------|--------|-------|
| WildDeal | 红桃 {R} 为逢人配 | ♥ {R} is the Wild Card | GameDirector.cs | Deal announcement |
| Lottery_Draw | 抽签中 | Drawing lots… | GameDirector.cs | Lottery |
| StraightFlushDemo | 同花顺 · 演出预览 | Straight Flush · Demo | GameDirector.cs:417 | Debug key |

---

## Screen Game UI (ScreenGameUi.cs)

| Key | Chinese | English | Source | Scene |
|-----|---------|---------|--------|-------|
| LotteryTitle | 先抽同行人 | Draw a Lot | ScreenGameUi.cs:276 | Lottery panel |
| LotteryHint | 三支身份签中，一支成为队友，其余两支成为对手 | One lot becomes your partner. The other two become opponents. | ScreenGameUi.cs:277 | Lottery panel |
| MainTitle | 掼蛋夺宝 · 第 {N} 小局 | TOMB DUEL · Round {N} | ScreenGameUi.cs:291 | HUD |
| JadeLevel | 青队 · 打 {R} | Jade · Rank {R} | ScreenGameUi.cs:292 | HUD |
| RedLevel | 朱队 · 打 {R} | Red · Rank {R} | ScreenGameUi.cs:293 | HUD |
| TrumpRank | 本局级牌 {R} | Trump Rank: {R} | ScreenGameUi.cs:298 | HUD |
| PhaseTribute | 正在进贡… | Tribute in progress… | ScreenGameUi.cs:301 | Turn text |
| PhaseAssign | 正在确认进贡归属… | Assigning tribute… | ScreenGameUi.cs:302 | Turn text |
| PhaseReturn_You | 请选择一张牌还贡 | Choose return tribute card | ScreenGameUi.cs:303 | Turn text |
| PhaseReturn_AI | 正在还贡… | Returning tribute… | ScreenGameUi.cs:303 | Turn text |
| PhaseHandComplete | 本局名次已确认 | Placements confirmed | ScreenGameUi.cs:304 | Turn text |
| PhaseMatchComplete | 宝藏出土 | Treasure Unearthed | ScreenGameUi.cs:305 | Turn text |
| SoundOn | 声音 | Sound | ScreenGameUi.cs:309 | Button |
| SoundMuted | 静音 | Muted | ScreenGameUi.cs:309 | Button |
| BtnPlay_Tribute | 还贡 | Return | ScreenGameUi.cs:313 | Button |
| BtnPlay_Normal | 出牌 | Play | ScreenGameUi.cs:313 | Button |
| FlyCard_Tribute | 进贡 | Tribute | ScreenGameUi.cs:368/373 | Fly card label |
| BtnPass | 过牌 | Pass | ScreenGameUi.cs:629 | Button |
| BtnPlay | 出牌 | Play | ScreenGameUi.cs:630 | Button |
| BtnNorth | 北家 | North | ScreenGameUi.cs:632 | Profile button |
| BtnEast | 东家 | East | ScreenGameUi.cs:633 | Profile button |
| BtnWest | 西家 | West | ScreenGameUi.cs:634 | Profile button |
| RouteTitle | 选择发掘路线 | Choose Route | ScreenGameUi.cs:656 | Route panel |
| EnterTreasure | 进入夺宝场景 | Enter Treasure Race | ScreenGameUi.cs:662 | Button |
| BtnSteady | 稳当推进 | Steady Advance | ScreenGameUi.cs:663 | Button |
| BtnRisky | 深入探方 | Risky Dig | ScreenGameUi.cs:664 | Button |
| BtnContinue | 返回牌桌 · 下一局 | Back to Table · Next Round | ScreenGameUi.cs:667 | Button |
| BtnRestart | 重新抽签 | Draw Again | ScreenGameUi.cs:668 | Button |
| BtnProfileClose | 关闭人物资料 | Close | ScreenGameUi.cs:693 | Button |
| VariantTitle | 选择逢人配的牌型解释 | Choose Wild Card Role | ScreenGameUi.cs:697 | Wild card panel |
| BtnCancel | 取消 | Cancel | ScreenGameUi.cs:700 | Button |
| Placement | 你 · 第 {N} 名 | You · #{N} | ScreenGameUi.cs:771 | Hand info |
| Dealing | 正在发牌 {V} / 27 | Dealing {V} / 27 | ScreenGameUi.cs:772 | Hand info |
| HandCount | 手牌 {N} | Hand: {N} | ScreenGameUi.cs:772 | Hand info |
| RevealingRank | 正在揭示本局级牌 | Revealing trump rank… | ScreenGameUi.cs:774 | Selection text |
| CardsArriving | 牌正在依次到手 | Cards arriving… | ScreenGameUi.cs:774 | Selection text |
| ConfirmReturn | 确认要还出的牌 | Confirm return tribute card | ScreenGameUi.cs:776 | Selection text |
| SelectReturn | 请选择一张牌还贡 | Select a card to return | ScreenGameUi.cs:776 | Selection text |
| ReadyToPlay | 已选 {N} 张 · 可以出牌 | {N} selected · Ready | ScreenGameUi.cs:778 | Selection text |
| InvalidPattern | 已选 {N} 张 · 牌型不成立 | {N} selected · Invalid | ScreenGameUi.cs:778 | Selection text |
| SelectCards | 请选择要出的牌 | Select cards to play | ScreenGameUi.cs:780 | Selection text |
| WaitingOthers | 等待其他牌手 | Waiting for others | ScreenGameUi.cs:780 | Selection text |
| TurnCallout_Your | 轮到你出牌 | Your turn to play | ScreenGameUi.cs:812 | Turn callout |
| TurnCallout_Thinking | 思考中… | Thinking… | ScreenGameUi.cs:812 | Turn callout |
| TurnCallout_TributePay | 正在进贡… | Paying tribute… | ScreenGameUi.cs:813 | Turn callout |
| TurnCallout_TributeAssign | 确认进贡归属… | Assigning tribute… | ScreenGameUi.cs:814 | Turn callout |
| TurnCallout_ReturnYou | 请选择还贡牌 | Choose return tribute | ScreenGameUi.cs:815 | Turn callout |
| TurnCallout_ReturnAI | 正在还贡… | Returning tribute… | ScreenGameUi.cs:815 | Turn callout |
| LastStep | 最后一步 · 宝藏即将揭晓 | Final Step · Treasure incoming! | ScreenGameUi.cs:899 | Route panel |
| Victory | 恭喜获胜 | VICTORY! | ScreenGameUi.cs:902 | Result |
| Defeat | 惜败此局 | DEFEATED | ScreenGameUi.cs:902 | Result |
| WonDetail | 你与队友率先取得地宫宝藏\n青队 {J} 格 · 朱队 {R} 格 | You and your partner claimed the tomb treasure!\nJade {J} · Red {R} tiles | ScreenGameUi.cs:905 | Result |
| LostDetail | 对手率先取得宝藏 · 再战一局\n青队 {J} 格 · 朱队 {R} 格 | Opponents claimed the treasure — play again!\nJade {J} · Red {R} tiles | ScreenGameUi.cs:906 | Result |
| RoundRecord | 本局发掘记录 · {team} {copy} · 基础前进 {N} 格 | Round · {team} {copy} · Advance {N} tile(s) | ScreenGameUi.cs:912 | Round summary |
| SteadyDynamic | 稳当推进\n确定前进 {N} 格 | Steady Advance\n+{N} tile(s) | ScreenGameUi.cs:932 | Button |
| RiskyDynamic | 深入探方\n翻倍 10%... | Risky Dig\n×2 (10%) +1 (15%) Normal (55%)\n−1 (12%) Retreat (8%) | ScreenGameUi.cs:933 | Button |
| ChooseRoute | 选择发掘路线 | Choose Route | ScreenGameUi.cs:936 | Route title |
| OpponentChoosing | {name}正在选择探方 | {name} is choosing… | ScreenGameUi.cs:936 | Route title |
| RoundDone | 本局结算 · 前往夺宝 | Round done · To Race | ScreenGameUi.cs:936 | Route title |
| PartnerRisky | 队友建议：可以深入探一步 | Partner: dig one step deeper. | ScreenGameUi.cs:939 | Advice |
| PartnerSteady | 队友建议：先保住这次发现 | Partner: secure this advance. | ScreenGameUi.cs:939 | Advice |
| TapToSwitch | 点击上方按钮切换到夺宝路线视角 | Tap the button above to switch to the Race view. | ScreenGameUi.cs:940 | Advice |
| WaitingOpponent | 等待对手队选择… | Waiting for opponents… | ScreenGameUi.cs:942 | Route result |
| JadeWins | 青队取得宝藏 | Jade Team Wins! | ScreenGameUi.cs:946 | Route title |
| RedWins | 朱队取得宝藏 | Red Team Wins! | ScreenGameUi.cs:946 | Route title |
| ChestOpened | 中央出土宝箱已经开启 | The central chest has been opened. | ScreenGameUi.cs:947 | Advice |
| AdvanceComplete | 本次推进完成 | Advance Complete | ScreenGameUi.cs:951 | Route title |
| ContinuePlay | 宝箱仍在地宫深处，继续下一局 | The chest remains in the tomb depths — play on! | ScreenGameUi.cs:952 | Advice |
| Finish_1st2nd | 头游 · 二游 | 1st & 2nd | ScreenGameUi.cs:971 | Round summary |
| Finish_1st3rd | 头游 · 三游 | 1st & 3rd | ScreenGameUi.cs:972 | Round summary |
| Finish_1st4th | 头游 · 末游 | 1st & 4th | ScreenGameUi.cs:973 | Round summary |
| WildBanner | 配作 {meanings} | Wild → {meanings} | ScreenGameUi.cs:1027 | Wild card banner |
| WildBadge | 配\n{rank} | Wild\n→{rank} | ScreenGameUi.cs:1054 | Card badge |
| TeamRail_J | 青 | J | ScreenGameUi.cs:1141 | Score rail |
| TeamRail_R | 朱 | R | ScreenGameUi.cs:1141 | Score rail |
| BombStamp | 炸弹\n{label} | BOMB\n{label} | ScreenGameUi.cs:1243 | Bomb effect |
| PlacementStamp | {seat}\n第 {N} 名 | {seat}\n#{N} | ScreenGameUi.cs:1362 | Finish stamp |
| FlyCard_Return | 还贡 | Return | ScreenGameUi.cs:1381 | Fly card |
| FlyCard_Tribute2 | 进贡 | Tribute | ScreenGameUi.cs:1381 | Fly card |
| TreasureChest | {team}抵达第 {N} 格\n宝箱开启 | {team} reaches tile {N}!\nChest Opened! | ScreenGameUi.cs:1409 | Treasure move |
| TreasureAdvance | {team}推进\n第 {from} 格 → 第 {to} 格 | {team} advances\nTile {from} → Tile {to} | ScreenGameUi.cs:1410 | Treasure move |
| ProfileHeader | {seat} · 人物资料 | {seat} · Profile | ScreenGameUi.cs:1440 | Profile panel |
| ProfileTraits | 性格：{traits} | Traits: {traits} | ScreenGameUi.cs:1444 | Profile panel |
| SealLotA | 身份签 · 甲 | Role Lot · A | ScreenGameUi.cs:1600 | Lottery seal |
| SealLotB | 身份签 · 乙 | Role Lot · B | ScreenGameUi.cs:1600 | Lottery seal |
| SealLotC | 身份签 · 丙 | Role Lot · C | ScreenGameUi.cs:1600 | Lottery seal |
| PlaceNth | 第 {N} 名 | #{N} | ScreenGameUi.cs:1701 | Seat count |
| Dealing_State | 收牌中 | Dealing | ScreenGameUi.cs:1703 | Seat count |
| UnknownCount | ?张 | ? | ScreenGameUi.cs:1705 | Seat count |
| KnownCount | {N}张 | {N} | ScreenGameUi.cs:1705 | Seat count |
| SeatName_You | 你 | You | ScreenGameUi.cs:1710 | SeatName() |
| SeatName_East | 东家 | East | ScreenGameUi.cs:1711 | SeatName() |
| SeatName_Partner | 队友 | Partner | ScreenGameUi.cs:1712 | SeatName() |
| SeatName_West | 西家 | West | ScreenGameUi.cs:1713 | SeatName() |
| CardLabel_Wild | \n配 | \nWILD | ScreenGameUi.cs:1728 | Card label |

---

## Treasure Scene Presenter (TreasureScenePresenter.cs)

| Key | Chinese | English | Source | Scene |
|-----|---------|---------|--------|-------|
| FlagScore_Init | 青/朱队\n打 2\n\n0 / 12 格 | Jade/Red\nRank 2\n\n0 / 12 | TreasureScenePresenter.cs:289 | 3D floating score panel |
| FlagScore_Update | 青/朱队\n打 {R}\n\n{pos} / 12 格 | Jade/Red\nRank {R}\n\n{pos} / 12 | TreasureScenePresenter.cs:300-302 | 3D floating score panel |

---

## CJK Scan Exclusions

The following items contain CJK characters and are intentionally preserved because they are NOT player-visible (per spec: "日志、对象名、枚举、路径、存档键不属于玩家可见文字，可以保留"):

| Location | Content | Reason |
|----------|---------|--------|
| ScreenGameUi.cs:386,395 | "World Table UI · 世界空间" | Scene hierarchy path used in Find() — must match scene .unity file |
| ScreenGameUi.cs:451 | "Head HUD · 顶部战况" | Scene hierarchy path |
| GuandanXRBootstrap.cs:568-569 | [Guandan] log messages | Debug.LogWarning — not player-visible |
| GuandanXRBootstrap.cs:670-671 | "左","右" in LogPointer | Internal debug method — not player-visible |
| GameDirector.cs:1227-1228 | "LotteryTable_三支同行签" | GameObject.Find path — must match scene |
| GameDirector.cs:1321-1327 | "Seats_四家座位与角色锚点", "Seat_*" paths | GameObject.Find paths |
| GameDirector.cs:1439 | "World Table UI · 世界空间" | child.name comparison |
| TombRoomBuilder.cs (all) | Object names | Generated scene object names — not player-visible |
| TreasureScenePresenter.cs:256,258 | "FlagScore_青队/朱队" | GameObject names |
| TreasureScenePresenter.cs:322,428 | Object names | GameObject names |
| RuntimeGameplayBindings.cs:62,144,151,228-230,268,362,740,774 | Object names and .name lookups | Internal object naming |
| RuntimeGameplayBindings.cs:834-837 | SeatLabel() | Used only for object names (chair.name, avatar.name) |
| TreasureCelebration.cs:21 | "VictoryConfetti · 双侧礼花" | GameObject name |
| AuthoredAvatarPresentation.cs:66 | "坐姿动画代理" | Debug proxy object name |
| GuandanSceneCleanup.cs:20-24 | "Assets/中国风墓穴考古/" | Asset folder paths — must match actual folders |
| GuandanRuleSmokeTests.cs:15-51 | Chinese assertion error strings | Editor/debug only — console output |
| GuandanProjectSetup.cs:27,64-65,163-201 | RootName, object names, build error messages | Scene object name and editor/build-time only |
| All *.cs comment lines | Various | Comments — explicitly excluded per spec |
