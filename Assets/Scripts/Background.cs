using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 0.3f;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Scroll()
    {
        Vector2 offset = meshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
        offset.y += Time.deltaTime * scrollSpeed;

        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}