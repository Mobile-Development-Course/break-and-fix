# Solutions

This branch has the fixed games. Compare it with `main` to see every change:
`git diff main solution`

## 01_ShipToPhone

| # | Bug | What you saw | Where it was | The fix |
| --- | --- | --- | --- | --- |
| 1 | No safe area | The coin counter sits under the camera hole or a rounded corner | The HUD was placed straight on the Canvas | A full-screen `SafeArea` panel with `SafeAreaFitter.cs`; the counter, box and button now live inside it |
| 2 | Constant Pixel Size | The UI is tiny on a sharp phone and huge on an old one | Canvas → Canvas Scaler → UI Scale Mode | Scale With Screen Size, Reference Resolution 1080 × 1920, Match 0.5 |
| 3 | Button too small | Hard to hit with a thumb, even after the Canvas fix | CoinButton → Rect Transform: 200 × 60 | 360 × 160 (every button at least 140 × 140) |
| 4 | Landscape orientation | A portrait game opens sideways | Player → Resolution and Presentation → Default Orientation | Portrait |
| 5 | Minimum API Level 35 | It won't install on phones with Android 14 or older | Player → Other Settings → Minimum API Level | 29 (Android 10) |
| 6 | Default package name | Your build replaces, or is blocked by, your neighbour's | Player → Other Settings → Package Name: `com.DefaultCompany.BreakAndFix` | Your own: here `com.yourstudio.breakandfix` |

The one that would hurt most if it shipped: the package name. Once a game is published, its package name can never change.

## 02_Performance

On this branch the scripts and the scene builder are fixed. If you built the scene on `main`, run **Tools → Break and Fix → Create 02_Performance scene** again and choose **Build again**: the scene and art in `Assets/Generated` were made the broken way.

| # | Bug | What you saw | Where it was | The fix | Before → after |
| --- | --- | --- | --- | --- | --- |
| 1 | Garbage every frame | A hitch every few seconds; Profiler → GC Alloc is never 0 | `HudLabel.cs`, `Update`: `GetComponentsInChildren`, LINQ `Where().ToList()` and `Count()`, and a new string, 60 times a second | Count the list `CoinRain` already keeps with a plain `for` loop, and set `label.text` only when a number changes | Several KB a frame → 0 B |
| 2 | Batches broken | Stats → Batches is far higher than two kinds of picture should need; the Frame Debugger says the next draw uses a different texture | `CoinRain.cs`, `Start`: coin, gem, coin, gem from two separate textures. Wherever a gem overlaps a coin, the Canvas has to start a new batch | Spawn all the coins, then all the gems: 2 batches. Or pack `coin` and `gem` into one Sprite Atlas: 1 batch, in any order | Dozens → 2 (1 with an atlas) |
| 3 | Texture too big | Memory is high for a tiny game, and it starts slowly | `background.png` import settings: Max Size 4096, no compression, mipmaps on, Read/Write on | Max Size 2048, Android override ASTC 6x6, mipmaps off, Read/Write off | About 85 MB → about 2 MB |

The one that would hurt most if it shipped: the texture. A slow frame makes a game feel bad, but running out of memory closes it, and old phones run out first.
