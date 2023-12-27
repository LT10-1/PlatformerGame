using UnityEngine;
using UnityEngine.UIElements;

public enum FruitType
{
    apple,
    banana,
    cherries,
    kiwi,
    melon,
    orange,
    pineapple,
    strawberry
}

public class Fruit_Apple : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    public FruitType myFruitType;
    [SerializeField] private Sprite[] fruitImage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().fruit++;
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(((int)myFruitType), 1);

    }

    private void OnValidate()
    {
        sr.sprite = fruitImage[((int)myFruitType)];
    }
}
