using UnityEngine;
using System.Collections;

public class ProjectileSkill : SkillComponent {

    bool active = true;

    Enemy caster;

    public override void Create(Skill ability, Enemy cast)
    {
        this.skill = ability;
        this.caster = cast;
        
        transform.rotation = Quaternion.Euler(0, 0, cast.direction * -22.5f);
        SpriteRenderer p = gameObject.AddComponent<SpriteRenderer>();
        p.sprite = Resources.Load("Skills\\"+skill.name, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);

        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

        StartCoroutine("SkillSelection");
    }

    public override IEnumerator SkillSelection()
    {
        while(active)
        {
            this.transform.Translate(transform.right * Time.deltaTime * 10);
            yield return null;
        }
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        print(gameObject);

        if (hit.gameObject.tag == "Wall")
        {
            active = false;
            print("The attack missed");
            Destroy(gameObject);
        }
        else if (hit.gameObject.tag != caster.gameObject.tag && hit.gameObject != caster.gameObject)
        {
            active = false;
            SkillEffect(hit.gameObject.GetComponent<Enemy>());
        }
        else if (hit.gameObject.tag == caster.gameObject.tag && hit.gameObject != caster.gameObject && skill.targetType == 2)
        {
            active = false;
            SkillEffect(hit.gameObject.GetComponent<Enemy>());
        }
        
    }

    public override void SkillEffect(Enemy target)
    {
        if(skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int crit = (Random.Range(0, 255) + skill.critModifier) > (255 * .95f)  ? 2 : 1;
            int damage = (int) ((skill.modifierPower * caster.monster.stats[skill.attackType] + skill.flatPower) / target.monster.stats[skill.defenseType] * target.monster.resistances[skill.element]) * crit ;
            if (crit == 2)
                print("Damage!: " + damage);
            else
                print("Damage:" + damage);
        }
        //GameManager.Manager.EndTurn();
        Destroy(gameObject);
    }
}