using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Builds the 02_Performance exercise in one click: Tools → Break and Fix → Create 02_Performance scene.
// It draws the art (a background, a coin and a gem), imports it, builds the scene and opens it.
// The art is made by code so the repo stays small and every student gets exactly the same scene.
public static class PerformanceSceneBuilder
{
    const string GeneratedFolder = "Assets/Generated";   // in .gitignore: made on each computer, never committed
    const string ScenePath = GeneratedFolder + "/02_Performance.unity";
    const string ArtFolder = GeneratedFolder + "/02_Performance";

    [MenuItem("Tools/Break and Fix/Create 02_Performance scene")]
    static void Create()
    {
        if (File.Exists(ScenePath))
        {
            // 0 = Open it, 1 = Cancel (also Esc or closing the window), 2 = Build again.
            int choice = EditorUtility.DisplayDialogComplex("02_Performance",
                "The scene already exists. Open it, or build it again from scratch?\n\nBuilding again resets your changes to the scene and its art.",
                "Open it", "Cancel", "Build again");
            if (choice == 1)
                return;
            if (choice == 0)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(ScenePath);
                return;
            }
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        try
        {
            EditorUtility.DisplayProgressBar("02_Performance", "Drawing the art…", 0.1f);
            if (!AssetDatabase.IsValidFolder(GeneratedFolder))
                AssetDatabase.CreateFolder("Assets", "Generated");
            if (!AssetDatabase.IsValidFolder(ArtFolder))
                AssetDatabase.CreateFolder(GeneratedFolder, "02_Performance");
            Sprite background = MakeSprite("background", 4096, BackgroundPixel, SetBackgroundImport);
            EditorUtility.DisplayProgressBar("02_Performance", "Drawing the art…", 0.7f);
            Sprite coin = MakeSprite("coin", 128, CoinPixel, SetItemImport);
            Sprite gem = MakeSprite("gem", 128, GemPixel, SetItemImport);

            EditorUtility.DisplayProgressBar("02_Performance", "Building the scene…", 0.9f);
            BuildScene(background, coin, gem);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    // ---------- import settings ----------

    static void SetBackgroundImport(TextureImporter importer)
    {
        importer.spritePixelsPerUnit = 100;
        importer.maxTextureSize = 4096;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = true;
        importer.isReadable = true;
    }

    static void SetItemImport(TextureImporter importer)
    {
        importer.spritePixelsPerUnit = 128;
        importer.maxTextureSize = 128;
        importer.mipmapEnabled = false;
        importer.isReadable = false;
    }

    // ---------- the scene ----------

    static void BuildScene(Sprite background, Sprite coin, Sprite gem)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var cameraGo = new GameObject("Main Camera");
        cameraGo.tag = "MainCamera";
        cameraGo.transform.position = new Vector3(0f, 0f, -10f);
        var camera = cameraGo.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(30, 20, 60, 255);

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Background: one picture that covers the screen.
        Canvas backCanvas = MakeCanvas("Background Canvas", 0);
        var backImage = MakeImage("Background", backCanvas.transform, background);
        Stretch(backImage.rectTransform);
        var fitter = backImage.gameObject.AddComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        fitter.aspectRatio = 1f;

        // The coin rain, on its own Canvas.
        Canvas rainCanvas = MakeCanvas("Rain Canvas", 1);
        var rainGo = new GameObject("Coin Rain", typeof(RectTransform));
        rainGo.transform.SetParent(rainCanvas.transform, false);
        Stretch((RectTransform)rainGo.transform);
        var rain = rainGo.AddComponent<CoinRain>();
        Wire(rain, "coinSprite", coin);
        Wire(rain, "gemSprite", gem);

        // HUD: the counts at the top, the FPS counter in the corner.
        Canvas hudCanvas = MakeCanvas("HUD Canvas", 2);

        Text hudText = MakeText("HUD Label", hudCanvas.transform, font, 46, TextAnchor.MiddleCenter);
        var hudRect = hudText.rectTransform;
        hudRect.anchorMin = new Vector2(0f, 1f);
        hudRect.anchorMax = new Vector2(1f, 1f);
        hudRect.pivot = new Vector2(0.5f, 1f);
        hudRect.anchoredPosition = new Vector2(0f, -160f);
        hudRect.sizeDelta = new Vector2(-80f, 90f);
        hudText.text = "Coins: 0   Gems: 0   Wave 1";
        var hud = hudText.gameObject.AddComponent<HudLabel>();
        Wire(hud, "label", hudText);
        Wire(hud, "rain", rain);

        Text fpsText = MakeText("FPS Counter", hudCanvas.transform, font, 40, TextAnchor.LowerLeft);
        var fpsRect = fpsText.rectTransform;
        fpsRect.anchorMin = Vector2.zero;
        fpsRect.anchorMax = Vector2.zero;
        fpsRect.pivot = Vector2.zero;
        fpsRect.anchoredPosition = new Vector2(40f, 60f);
        fpsRect.sizeDelta = new Vector2(600f, 70f);
        fpsText.text = "-- fps";
        var fps = fpsText.gameObject.AddComponent<FpsCounter>();
        Wire(fps, "label", fpsText);

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.Refresh();
        Debug.Log("02_Performance is ready. Press Play, then read the symptom cards in the README.");
    }

    static Canvas MakeCanvas(string name, int sortingOrder)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        var scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    static Image MakeImage(string name, Transform parent, Sprite sprite)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.raycastTarget = false;
        return image;
    }

    static Text MakeText(string name, Transform parent, Font font, int size, TextAnchor alignment)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        var text = go.GetComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        return text;
    }

    static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    // Sets a private [SerializeField] the way the Inspector would.
    static void Wire(UnityEngine.Object target, string field, UnityEngine.Object value)
    {
        var so = new SerializedObject(target);
        so.FindProperty(field).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    // ---------- the art ----------

    static Sprite MakeSprite(string name, int size, Func<int, int, int, Color32> pixel, Action<TextureImporter> settings)
    {
        string path = $"{ArtFolder}/{name}.png";

        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                pixels[y * size + x] = pixel(x, y, size);

        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply();
        File.WriteAllBytes(path, texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.alphaIsTransparency = true;
        settings(importer);
        importer.SaveAndReimport();

        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    // A sunset: sky gradient, a sun, two rows of hills.
    static Color32 BackgroundPixel(int x, int y, int size)
    {
        float u = x / (float)size;
        float v = y / (float)size;

        float hillsBack = 0.30f + 0.05f * Mathf.Sin(u * 7f + 1f) + 0.02f * Mathf.Sin(u * 23f);
        float hillsFront = 0.18f + 0.04f * Mathf.Sin(u * 5f + 3f) + 0.015f * Mathf.Sin(u * 31f + 2f);
        if (v < hillsFront) return new Color32(34, 22, 58, 255);
        if (v < hillsBack) return new Color32(70, 40, 96, 255);

        float dx = u - 0.68f, dy = v - 0.42f;
        if (dx * dx + dy * dy < 0.011f) return new Color32(255, 214, 120, 255);

        Color sky = Color.Lerp(new Color32(255, 150, 100, 255), new Color32(46, 30, 92, 255), Mathf.InverseLerp(0.3f, 1f, v));
        return sky;
    }

    static Color32 CoinPixel(int x, int y, int size)
    {
        float c = (size - 1) * 0.5f;
        float d = Mathf.Sqrt((x - c) * (x - c) + (y - c) * (y - c));
        byte alpha = (byte)(255 * Mathf.Clamp01(60f - d));
        if (d > 50f) return new Color32(205, 140, 20, alpha);
        if (d > 36f && d < 40f) return new Color32(230, 170, 30, 255);
        return new Color32(255, 205, 50, 255);
    }

    static Color32 GemPixel(int x, int y, int size)
    {
        float c = (size - 1) * 0.5f;
        float d = Mathf.Abs(x - c) + Mathf.Abs(y - c) * 0.8f;
        byte alpha = (byte)(255 * Mathf.Clamp01(58f - d));
        if (d > 48f) return new Color32(30, 140, 220, alpha);
        return x < c ? new Color32(90, 215, 255, 255) : new Color32(60, 185, 245, 255);
    }
}
