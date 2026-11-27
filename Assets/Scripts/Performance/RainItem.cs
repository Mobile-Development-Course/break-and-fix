using UnityEngine;

public enum ItemKind { Coin, Gem }

// One falling coin or gem. CoinRain moves it; this only remembers what it is.
public class RainItem : MonoBehaviour
{
    public ItemKind Kind;
    public float Speed;   // screen heights per second
}
