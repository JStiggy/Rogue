using UnityEngine;
using System.Collections;

public class ItemComponent : MonoBehaviour {

    public Item item;
    SpriteRenderer sRenderer;

    public void Create(int itemIndex)
    {
        gameObject.layer = 11;
        item = GameManager.Manager.gameLibrary.itemData.Items[itemIndex].clone();
        item.remainingUses = Random.Range(1, item.stackSize+1);

        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Items\\" + item.itemName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);
    }

    public void Create(Item i)
    {
        gameObject.layer = 11;
        item = i;
        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Items\\" + item.itemName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);
    }

    public Item Swap(Item newItem)
    {
        Item temp = item.clone();
        item = newItem;
        sRenderer.sprite = Resources.Load("Items\\" + newItem.itemName, typeof(Sprite)) as Sprite;
        return temp;
    }

}
