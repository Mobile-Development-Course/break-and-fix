# Break it, fix it

One Unity project full of small broken games. Each scene is one exercise with planted bugs: find them before your players do.

## Open it

1. Clone this repo.
2. Unity Hub → **Add** → **Add project from disk** → pick the cloned `break-and-fix` folder. Open it with the same Unity version you use in class.
3. Open the exercise's scene from `Assets/Scenes`.

| Scene | Exercise |
| --- | --- |
| `01_ShipToPhone` | Six bugs that only show up on real phones |

## 01_ShipToPhone

A tiny game: tap the green button to earn a coin. Every coin makes the orange box spin faster.

1. Open `Assets/Scenes/01_ShipToPhone.unity`.
2. Look at the game in the **Device Simulator** (Window → General → Device Simulator). Try a phone with a camera hole and an old 16:9 phone.
3. Build it to a real Android phone (File → Build Profiles → Android → Build And Run).

There are six bugs. Each card below tells you what a player would see, and how to see it yourself. Your job is to find the cause.

### 1. The coin counter disappears

On a phone with a camera hole or a notch, the coin counter at the top is hidden behind the camera.

**See it:** Device Simulator, pick a phone with a camera hole.

### 2. The game looks different on every phone

On one phone the button and the text are big, on another they are tiny.

**See it:** Device Simulator, switch between a small old phone and a big new one. Watch the button.

### 3. Your thumb misses the button

Even when it looks fine on the screen, the button is hard to hit.

**See it:** on a real phone, tap the button 10 times fast with your thumb. How many taps missed?

### 4. The game opens sideways

On the phone, the game starts in landscape, and turning the phone doesn't help.

**See it:** build it to a real phone.

### 5. It installs on one phone, but not the other

The build works on a new phone and fails to install on an older one.

**See it:** build to both phones in your pair. Check each phone's Android version (Settings → About phone). If both run Android 15 or newer you won't see this one, so borrow an older phone from another pair.

### 6. Your neighbour's game kills yours

Install your build, then your neighbour's build, on the same phone. Only one game is left: the second one replaces the first, or fails to install.

**See it:** two laptops, one phone.

### Write it down

For each card:

| Card | Which setting or line causes it | The fix |
| --- | --- | --- |
| 1 | | |

Fix all six, build again, and show the fixed game running on a phone.

**After the exercise:** the fixed game and the answers are on the `solution` branch (`git checkout solution`, then read `SOLUTION.md`). Try it yourself first.
