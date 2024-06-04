using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bunker : MonoBehaviour
{
    public Texture2D Splat;

    private Texture2D _originalTexture;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _originalTexture = _spriteRenderer.sprite.texture;

        ResetBunker();
    }

    public void ResetBunker()
    {

        CopyTexture(_originalTexture);

        gameObject.SetActive(true);
    }

    private void CopyTexture(Texture2D source)
    {
        Texture2D copy = new Texture2D(source.width, source.height, source.format, false)
        {
            filterMode = source.filterMode,
            anisoLevel = source.anisoLevel,
            wrapMode = source.wrapMode
        };

        copy.SetPixels32(source.GetPixels32());
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, _spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), _spriteRenderer.sprite.pixelsPerUnit);
        _spriteRenderer.sprite = sprite;
    }

    public bool CheckCollision(BoxCollider2D other, Vector3 hitPoint)
    {
        Vector2 offset = other.size / 2;
        return Hit(hitPoint) ||
               Hit(hitPoint + (Vector3.down * offset.y)) ||
               Hit(hitPoint + (Vector3.up * offset.y)) ||
               Hit(hitPoint + (Vector3.left * offset.x)) ||
               Hit(hitPoint + (Vector3.right * offset.x));
    }

    private bool Hit(Vector3 hitPoint)
    {

        if (!CheckPoint(hitPoint, out int px, out int py)) {
            return false;
        }

        Texture2D texture = _spriteRenderer.sprite.texture;

        px -= Splat.width / 2;
        py -= Splat.height / 2;

        int startX = px;

        for (int y = 0; y < Splat.height; y++)
        {
            px = startX;

            for (int x = 0; x < Splat.width; x++)
            {
                Color pixel = texture.GetPixel(px, py);
                pixel.a *= Splat.GetPixel(x, y).a;
                texture.SetPixel(px, py, pixel);
                px++;
            }

            py++;
        }

        texture.Apply();

        return true;
    }

    private bool CheckPoint(Vector3 hitPoint, out int px, out int py)
    {
        Vector3 localPoint = transform.InverseTransformPoint(hitPoint);

        localPoint.x += _collider.size.x / 2;
        localPoint.y += _collider.size.y / 2;

        Texture2D texture = _spriteRenderer.sprite.texture;

        px = (int)(localPoint.x / _collider.size.x * texture.width);
        py = (int)(localPoint.y / _collider.size.y * texture.height);

        return texture.GetPixel(px, py).a != 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            gameObject.SetActive(false);
        }
    }

}
