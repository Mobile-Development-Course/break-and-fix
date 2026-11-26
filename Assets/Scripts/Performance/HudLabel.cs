using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// The HUD line at the top: how many coins and gems are falling, and the wave.
public class HudLabel : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] CoinRain rain;

    void Update()
    {
        RainItem[] items = rain.GetComponentsInChildren<RainItem>();
        List<RainItem> coins = items.Where(item => item.Kind == ItemKind.Coin).ToList();
        int gems = items.Count(item => item.Kind == ItemKind.Gem);

        label.text = "Coins: " + coins.Count + "   Gems: " + gems + "   Wave " + rain.Wave;
    }
}
