# Solution: 01_ShipToPhone

This branch is the fixed game. Compare it with `main` to see every change:
`git diff main solution`

| # | Bug | What you saw | Where it was | The fix |
| --- | --- | --- | --- | --- |
| 1 | No safe area | The coin counter sits under the camera hole or a rounded corner | The HUD was placed straight on the Canvas | A full-screen `SafeArea` panel with `SafeAreaFitter.cs`; the counter, box and button now live inside it |
| 2 | Constant Pixel Size | The UI is tiny on a sharp phone and huge on an old one | Canvas → Canvas Scaler → UI Scale Mode | Scale With Screen Size, Reference Resolution 1080 × 1920, Match 0.5 |
| 3 | Button too small | Hard to hit with a thumb, even after the Canvas fix | CoinButton → Rect Transform: 200 × 60 | 360 × 160 (every button at least 140 × 140) |
| 4 | Landscape orientation | A portrait game opens sideways | Player → Resolution and Presentation → Default Orientation | Portrait |
| 5 | Minimum API Level 35 | It won't install on phones with Android 14 or older | Player → Other Settings → Minimum API Level | 29 (Android 10) |
| 6 | Default package name | Your build replaces, or is blocked by, your neighbour's | Player → Other Settings → Package Name: `com.DefaultCompany.BreakAndFix` | Your own: here `com.yourstudio.breakandfix` |

The one that would hurt most if it shipped: the package name. Once a game is published, its package name can never change.
