using UnityEngine;
using UnityEngine.UI;

public class PixelDrawer : MonoBehaviour
{
    public Image image;  // 连接到Canvas上的Image
    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(256, 256);

        // 设置纹理为白色背景
        Color[] clearPixels = new Color[texture.width * texture.height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.white;
        }
        texture.SetPixels(clearPixels);

        // 绘制网格
        DrawGrid();

        texture.Apply();

        // 确保Image的尺寸与Texture的尺寸一致
        image.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);

        // 应用到Image组件
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    // void Update()
    // {
    //     // 检测鼠标左键按下
    //     if (Input.GetMouseButtonDown(0))  // 0表示鼠标左键
    //     {
    //         // 获取鼠标在屏幕上的位置
    //         Vector2 mousePos = Input.mousePosition;

    //         // 将屏幕坐标转换为纹理坐标
    //         Vector2 texturePos = ScreenToTextureCoordinates(mousePos);

    //         // 绘制像素
    //         DrawPixel((int)texturePos.x, (int)texturePos.y, Color.red);

    //         // 应用更改
    //         texture.Apply();
    //     }
    // }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 texturePos = ScreenToTextureCoordinates(mousePos);

            // 获取 tile 的坐标
            int tileSize = 16;
            int tileX = (int)texturePos.x / tileSize;
            int tileY = (int)texturePos.y / tileSize;

            // 画 tile
            DrawTile(tileX, tileY, GenerateTilePattern());

            texture.Apply();
        }
    }

    Color[,] GenerateTilePattern()
    {
        int tileSize = 16;
        Color[,] pattern = new Color[tileSize, tileSize];

        for (int y = 0; y < tileSize; y++)
        {
            for (int x = 0; x < tileSize; x++)
            {
                // 示例：用红色填满整个 tile
                pattern[x, y] = Color.red;
            }
        }

        return pattern;
    }

    // 将屏幕坐标转换为纹理坐标
    Vector2 ScreenToTextureCoordinates(Vector2 screenPos)
    {
        // 将屏幕坐标转换为Canvas上的局部坐标
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localPos);

        // 计算纹理坐标
        float normalizedX = (localPos.x + rectTransform.rect.width * 0.5f) / rectTransform.rect.width * texture.width;
        float normalizedY = (localPos.y + rectTransform.rect.height * 0.5f) / rectTransform.rect.height * texture.height;

        // 返回转换后的纹理坐标
        return new Vector2(Mathf.FloorToInt(normalizedX), Mathf.FloorToInt(normalizedY));
    }

    // 绘制像素
    void DrawPixel(int x, int y, Color color)
    {
        // 检查坐标是否在纹理范围内
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            // 设置指定位置的像素颜色
            texture.SetPixel(x, y, color);
        }
    }

    // 绘制网格
    void DrawGrid()
    {
        // 网格颜色
        Color gridColor = Color.black;

        // 绘制每一条水平网格线
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (x % 16 == 0 || y % 16 == 0)  // 每16个像素绘制一条网格线
                {
                    texture.SetPixel(x, y, gridColor);
                }
            }
        }
    }

    void DrawTile(int tileX, int tileY, Color[,] tilePattern)
    {
        int tileSize = 16;
        for (int y = 0; y < tileSize; y++)
        {
            for (int x = 0; x < tileSize; x++)
            {
                int px = tileX * tileSize + x;
                int py = tileY * tileSize + y;

                if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    texture.SetPixel(px, py, tilePattern[x, y]);
                }
            }
        }
    }
}
