using UnityEngine;
using System.Collections;

public class MeleeSkill : SkillComponent
{

    //Create the skill adding all needed data, enable targeting
    public override void Create(Skill ability, Unit cast)
    {
        this.skill = ability;
        this.caster = cast;

        SpriteRenderer sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Skills\\" + skill.name, typeof(Sprite)) as Sprite;

        if (caster.currentMana >= skill.cost)
        {
            StartCoroutine("SkillSelection");
        }
        else
        {
            print("No MANA");
            caster.StartCoroutine("StartTurn");
            Destroy(gameObject);
        }
    }

    //Iterate through all valid targets based on their distance from the user (5 tiles, 5+2)
    public override IEnumerator SkillSelection()
    {
        //Get all targets in range, make sure a valid target exists or return control to player
        if (caster.direction % 2 == 1)
        {
            transform.position = caster.transform.position + Quaternion.Euler(0, 0, -45f * caster.direction) * caster.transform.right * 1.41f;
        }
        else
        {
            transform.position = caster.transform.position + Quaternion.Euler(0, 0, -45f * caster.direction) * caster.transform.right;
        }

        Collider2D[] targets = SplashTargets(transform.position, skill.splashRange, caster);

        yield return null;

        if(targets.Length == 0)
        {
            print("No targets");
        }
        else
        {
            foreach (Collider2D t in targets) SkillEffect(t.GetComponent<Unit>());
            yield return new WaitForSeconds(skill.animationTime);
        }
        caster.currentMana = Mathf.Clamp(caster.currentMana - skill.cost, 0, caster.monster.mana);
        GameManager.Manager.board.EndTurn();
        Destroy(gameObject);
        yield return null;
    }

}