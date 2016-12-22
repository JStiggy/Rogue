using UnityEngine;
using System.Collections;

public class TargetedSkill : SkillComponent {

    Texture reticle;
    Unit target = null;
    Unit caster;
    bool reticleDisplay = true;

    //Create the skill adding all needed data, enable targeting
    public override void Create(Skill ability, Unit cast)
    {
        transform.position = cast.transform.position;

        reticle = Resources.Load("Skills\\Reticle", typeof(Texture)) as Texture;

        this.skill = ability;
        this.caster = cast;

        StartCoroutine("SkillSelection");
    }

    //Based upon the targeting type of the skill, check which units are in the attack radius and valid targets
    Collider2D[] SplashTargets(Vector3 position, float radius)
    {
        if (skill.targetType == 3)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius + 1, radius + 1), 45f, 1 << 8 | 1 << 9);
        }
        else if (skill.targetType == 2)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius + 1, radius + 1), 45f, 1 << caster.gameObject.layer);
        }
        else
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius + 1, radius + 1), 45f, ((1 << caster.gameObject.layer) ^ (1 << 8 | 1 << 9)));
        }
    }

    //Iterate through all valid targets based on their distance from the user (5 tiles, 5+2)
    public override IEnumerator SkillSelection()
    {
        //Get all targets in range, make sure a valid target exists or return control to player
        Collider2D[] targets = SplashTargets(transform.position, 6f);
        if(targets.Length == 0)
        {
            print("No targets, return control to character");
            Destroy(this.gameObject);
            yield return null;
        }

        //Allow the user to select between targets, select a target, or cancel the ability
        target = targets[0].gameObject.GetComponent<Unit>();
        int currentSelection = 0;
        while(true)
        {
            if(Input.GetButtonDown("Horizontal"))
            {
                currentSelection += (int)Input.GetAxisRaw("Horizontal");
                currentSelection = currentSelection >= targets.Length ? 0 : currentSelection;
                currentSelection = currentSelection < 0 ? (targets.Length - 1) : currentSelection;
                target = targets[currentSelection].gameObject.GetComponent<Unit>();
                print(target.gameObject.name);
            }

            if (Input.GetButtonDown("Submit"))
            {
                reticleDisplay = false;
                this.transform.position = target.transform.position;

                Collider2D[] hits = SplashTargets(transform.position, skill.splashRange);
                foreach(Collider2D t in hits) SkillEffect(t.GetComponent<Unit>());

                SpriteRenderer p = gameObject.AddComponent<SpriteRenderer>();
                p.sprite = Resources.Load("Skills\\" + skill.name, typeof(Sprite)) as Sprite;

                break;
            }

            yield return null;
        }
        yield return new WaitForSeconds(skill.animationTime);
        //GameManager.Manager.EndTurn();
        Destroy(gameObject);
        yield return null;
    }

    //Calculate the damage dealt by the skill. Accuracy is applied here.
    public override void SkillEffect(Unit target)
    {
        //If the attack has any damage, calculate the damage
        if (skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int crit = (Random.Range(0, 255) + skill.critModifier) > (255 * .95f) ? 2 : 1;
            int damage = (int)((skill.modifierPower * caster.monster.stats[skill.attackType] + skill.flatPower) / target.monster.stats[skill.defenseType] * target.monster.resistances[skill.element]) * crit;
            if (crit == 2)
                print(target.monster.monsterName + " takes " + damage + "!");
            else
                print(target.monster.monsterName + " takes " + damage);
        }
    }

    //Print the targeting reticle if applicable
    void OnGUI()
    {
        if (target == null || !reticleDisplay) return;
        Vector2 v = Camera.main.WorldToScreenPoint(target.transform.position);
        v = new Vector2(v.x - 16, Screen.height - v.y -16);
        Graphics.DrawTexture(new Rect(v, new Vector2(32,32)) , reticle);
    }
}
