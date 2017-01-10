using UnityEngine;
using System.Collections;

//Template for all skills
abstract public class SkillComponent : MonoBehaviour {

    public Skill skill;
    public Unit caster;
    public int item = -1;

    public abstract void Create(Skill ability, Unit caster, int item = -1);

    public abstract IEnumerator SkillSelection();

    public void SkillEffect(Unit target)
    {
        switch (skill.type)
        {
            case 0:
            case 1:
            case 2:
                ApplyDamage(target);
                break;
            case 3:
            case 4:
            case 5:
                ApplyHealing(target);
                break;
        }

        ApplyStatus(target);
    }

    public void ApplyDamage(Unit target)
    {
        if (skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int crit = (Random.Range(0, 255) + skill.critModifier) > (255 * .95f) ? 2 : 1;
            int damage = (int)((skill.modifierPower * caster.monster.stats[skill.attackType] + skill.flatPower) / target.monster.stats[skill.defenseType] * target.monster.resistances[skill.element]) * crit;
            if (crit == 2 && damage > 0)
                print(target.monster.monsterName + " takes " + damage + "!");
            else if (damage == 0)
                print("There was no effect!");
            else if (damage < 0)
                print(target.monster.monsterName + " is healed for " + -1 * damage);
            else
                print(target.monster.monsterName + " takes " + damage);

            target.currentHealth = Mathf.Clamp(target.currentHealth - damage, 0, target.monster.health);
        }
    }

    public void ApplyHealing(Unit target)
    {
        if (skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int healing = (int)(skill.modifierPower * caster.monster.stats[skill.attackType] + skill.flatPower);
            print(target.monster.monsterName + " is healed for " + healing + "!");
            target.currentHealth = Mathf.Clamp(target.currentHealth + healing, 0, target.monster.health);
        }

    }

    public void ApplyStatus(Unit target)
    {
        //To be implemented;

    }

    //Based upon the targeting type of the skill, check which units are in the attack radius and valid targets
    public Collider2D[] SplashTargets(Vector3 position, float radius, Unit caster)
    {
        radius = radius * 1.41f;
        if (skill.targetType == 3)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius, radius), 45f, 1 << 8 | 1 << 9);
        }
        else if (skill.targetType == 2)
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius, radius), 45f, 1 << caster.gameObject.layer);
        }
        else
        {
            return Physics2D.OverlapBoxAll(position, new Vector2(radius, radius), 45f, ((1 << caster.gameObject.layer) ^ (1 << 8 | 1 << 9)));
        }
    }

    public void OnDestroy()
    {
        GameManager.Manager.DecrementInventory(item);
    }

}
