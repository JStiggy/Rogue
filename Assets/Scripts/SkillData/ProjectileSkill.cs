using UnityEngine;
using System.Collections;

public class ProjectileSkill : SkillComponent {

    bool active = true;

    SpriteRenderer sRenderer;
    Unit caster;

    //Create the skill getting info about the caster and the skill used
    public override void Create(Skill ability, Unit cast)
    {
        this.skill = ability;
        this.caster = cast;
        transform.position = cast.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, cast.direction * -22.5f);

        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Skills\\"+skill.name, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);

        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

        StartCoroutine("SkillSelection");
    }

    //Fire the skill in the forward direction move forward until an overlap event with a possible target occurs
    public override IEnumerator SkillSelection()
    {
        while(active)
        {
            this.transform.Translate(transform.right * Time.deltaTime * 10);
            yield return null;
        }
        yield return new WaitForSeconds(skill.animationTime);
        //GameManager.Manager.EndTurn();
        Destroy(gameObject);
        yield return null;
    }

    //Check for all applicable targets after a hit occurs
    Collider2D[] SplashTargets(Vector3 position)
    {
        new Vector2(skill.splashRange, skill.splashRange);
        if (skill.targetType == 3)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(skill.splashRange+1, skill.splashRange+1), 45f, 1 << 8 | 1 << 9);
        }
        else if (skill.targetType == 2)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(skill.splashRange + 1, skill.splashRange + 1), 45f, 1 << caster.gameObject.layer);
        }
        else
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(skill.splashRange + 1, skill.splashRange + 1), 45f, ((1 << caster.gameObject.layer) ^ (1 << 8 | 1 << 9)));
        }
    }

    //Check for any collisons then check all hit targets applying skill effects to them
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Wall")
        {
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position);
            if(targets.Length == 0)
            {
                print("No Targets");
                Destroy(gameObject);
            }
            else
            {
                foreach(Collider2D t in targets) SkillEffect(t.GetComponent<Unit>());
            }
        }
        else if (hit.gameObject.tag != caster.gameObject.tag && hit.gameObject != caster.gameObject)
        {
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position);
            foreach (Collider2D t in targets)
            {
                SkillEffect(t.GetComponent<Unit>());
            }
        }
        else if (hit.gameObject.tag == caster.gameObject.tag && hit.gameObject != caster.gameObject && skill.targetType >= 2)
        {
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position);
            foreach (Collider2D t in targets)
            {
                SkillEffect(t.GetComponent<Unit>());
            }
        }

    }

    //Apply skill effects to the targets
    public override void SkillEffect(Unit target)
    {
        sRenderer.enabled = false;
        if (skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int crit = (Random.Range(0, 255) + skill.critModifier) > (255 * .95f)  ? 2 : 1;
            int damage = (int) ((skill.modifierPower * caster.monster.stats[skill.attackType] + skill.flatPower) / target.monster.stats[skill.defenseType] * target.monster.resistances[skill.element]) * crit ;
            if (crit == 2)
                print(target.monster.monsterName + " takes " + damage + "!");
            else
                print(target.monster.monsterName + " takes " + damage);
        }
    }
}