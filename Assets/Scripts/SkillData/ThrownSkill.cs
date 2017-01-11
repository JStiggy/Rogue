using UnityEngine;
using System.Collections;

public class ThrownSkill : ProjectileSkill {

    public override void Create(Skill ability, Unit cast, int itemIndex = -1)
    {
        this.item = itemIndex;
        this.skill = ability;
        this.caster = cast;
        Item item = GameManager.Manager.inventory[itemIndex];

        this.skill.flatPower = item.throwFlatPower;
        this.skill.modifierPower = item.throwModPower;
        this.skill.splashRange = item.throwSplashRange;

        if(item.throwWhole == 1)
        {
            item.remainingUses = 0;
        }

        transform.position = cast.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, cast.direction * -45f);

        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Items\\" + GameManager.Manager.inventory[itemIndex].itemName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);

        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

        StartCoroutine("SkillSelection");
    }
}
