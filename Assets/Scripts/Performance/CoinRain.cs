using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The win screen's reward: a rain of coins and gems, drawn as UI Images.
// Every 10 seconds a new wave starts and everything falls a little faster.
public class CoinRain : MonoBehaviour
{
    [SerializeField] Sprite coinSprite;
    [SerializeField] Sprite gemSprite;
    [SerializeField] int count = 600;
    [SerializeField] float itemSize = 96f;
    [SerializeField] float waveSeconds = 10f;

    readonly List<RainItem> items = new List<RainItem>();
    RectTransform area;

    public IReadOnlyList<RainItem> Items => items;
    public int Wave { get; private set; } = 1;

    void Start()
    {
        area = (RectTransform)transform;
        Rect r = area.rect;

        for (int i = 0; i < count; i++)
        {
            // Half coins, half gems, mixed so the rain looks even: coin, gem, coin, gem...
            ItemKind kind = i % 2 == 0 ? ItemKind.Coin : ItemKind.Gem;

            var go = new GameObject(kind.ToString(), typeof(RectTransform), typeof(Image), typeof(RainItem));
            var rt = (RectTransform)go.transform;
            rt.SetParent(area, false);
            rt.sizeDelta = new Vector2(itemSize, itemSize);
            rt.anchoredPosition = new Vector2(Random.Range(r.xMin, r.xMax), Random.Range(r.yMin, r.yMax));

            var image = go.GetComponent<Image>();
            image.sprite = kind == ItemKind.Coin ? coinSprite : gemSprite;
            image.raycastTarget = false;

            var item = go.GetComponent<RainItem>();
            item.Kind = kind;
            item.Speed = Random.Range(0.12f, 0.3f);
            items.Add(item);
        }
    }

    void Update()
    {
        Wave = 1 + (int)(Time.timeSinceLevelLoad / waveSeconds);
        float waveSpeed = 1f + 0.1f * Mathf.Min(Wave - 1, 10);

        Rect r = area.rect;
        float dt = Time.deltaTime;

        for (int i = 0; i < items.Count; i++)
        {
            var rt = (RectTransform)items[i].transform;
            Vector2 p = rt.anchoredPosition;
            p.y -= items[i].Speed * waveSpeed * r.height * dt;

            // Fell off the bottom: back to the top, somewhere else.
            if (p.y < r.yMin - itemSize)
                p = new Vector2(Random.Range(r.xMin, r.xMax), r.yMax + itemSize);

            rt.anchoredPosition = p;
        }
    }
}
