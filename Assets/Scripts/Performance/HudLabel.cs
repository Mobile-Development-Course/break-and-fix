using UnityEngine;
using UnityEngine.UI;

// The HUD line at the top: how many coins and gems are falling, and the wave.
// Fixed: no garbage per frame. It counts the list CoinRain already keeps, with a plain loop,
// and builds a new string only when a number on the screen changes.
public class HudLabel : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] CoinRain rain;

    int shownCoins = -1;
    int shownGems = -1;
    int shownWave = -1;

    void Update()
    {
        int coins = 0;
        int gems = 0;
        var items = rain.Items;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Kind == ItemKind.Coin)
                coins++;
            else
                gems++;
        }

        if (coins == shownCoins && gems == shownGems && rain.Wave == shownWave)
            return;

        shownCoins = coins;
        shownGems = gems;
        shownWave = rain.Wave;
        label.text = "Coins: " + coins + "   Gems: " + gems + "   Wave " + rain.Wave;
    }
}
