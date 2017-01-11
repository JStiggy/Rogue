using UnityEngine;
using System.Collections;

public class ItemComponent : MonoBehaviour {

    public Item item;

    public void Create(int itemIndex)
    {
        item = GameManager.Manager.gameLibrary.itemData.Items[itemIndex];

        SpriteRenderer sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Items\\" + item.itemName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);
    }

}
