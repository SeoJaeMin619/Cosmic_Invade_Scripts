using UnityEngine;

public class SupplyBuild : MonoBehaviour
{
    public int addGold = 1;
    public BuildingSO buildingData;
    public Sprite buildingSprite; 
    private SpriteRenderer spriteRenderer;
    private Collider2D _collider2D;

    private float timer = 0.0f;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        
        if (buildingData != null)
        {
            buildingSprite = buildingData.BuildingSprite;
        }

        
        spriteRenderer = GetComponent < SpriteRenderer>();

        
        spriteRenderer.sprite = buildingSprite;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            GameManager.instance.AddGold(addGold * buildingData.AddResource);
            timer = 0.0f;
        }
    }
}