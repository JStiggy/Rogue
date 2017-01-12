using UnityEngine;
using System.Collections;

//Template for all skills
abstract public class SkillComponent : MonoBehaviour {

    public Skill skill;
    public Unit caster;
    public int item = -1;

    public abstract void Create(Skill ability, Unit caster, int item = -1);

    public abstract IEnumerator SkillSelection();

    public void ApplyPassives()
    {
        skill = skill.Clone();
        foreach (int i in caster.stats.passives)
        {
            Passive p = GameManager.Manager.gameLibrary.passiveData.Passives[i];
            if (p.type == 1 && (p.requirement == -1 || GameManager.Manager.gameLibrary.passiveFilters.skillFilters[p.requirement](skill, p.requirementParameter)))
            {
                for (int j = 0; j < skill.status.Length; ++j)
                {
                    skill.status[j] += p.statusBonus[j];
                }
            }
        }
    }

    public int ApplyDamagePassives(int damage, Unit target)
    {
        int baseDamage = damage;
        foreach (int i in caster.stats.passives)
        {
            Passive p = GameManager.Manager.gameLibrary.passiveData.Passives[i];
            if (p.type == 2 && (p.requirement == -1 || GameManager.Manager.gameLibrary.passiveFilters.damageFilters[p.requirement](target, skill, p.requirementParameter)))
            {
                print(p.name + " activated");
                damage += (int)(baseDamage * p.valueBonus);
            }
        }
        return damage;
    }

    public void SkillEffect(Unit target)
    {
        ApplyPassives();
        switch (skill.type)
        {
            case 0:
            case 1:
            case 2:
            case 6:
                ApplyDamage(target);
                break;
            case 3:
            case 4:
            case 5:
            case 7:
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
            int damage = (int)((skill.modifierPower * caster.stats.stats[skill.attackType] * Mathf.Pow(1.25f, caster.status[skill.attackType]) + skill.flatPower) / 
                               (target.stats.stats[skill.defenseType]* Mathf.Pow(1.25f, target.status[skill.defenseType])) * target.stats.resistances[skill.element]) * crit;

            damage = ApplyDamagePassives(damage, target);

            if (crit == 2 && damage > 0)
                print(target.baseMonster.monsterName + " takes " + damage + "!");
            else if (damage == 0)
                print("There was no effect!");
            else if (damage < 0)
                print(target.baseMonster.monsterName + " is healed for " + -1 * damage);
            else
                print(target.baseMonster.monsterName + " takes " + damage);

            target.currentHealth = Mathf.Clamp(target.currentHealth - damage, 0, target.baseMonster.health);
        }
    }

    public void ApplyHealing(Unit target)
    {
        if (skill.modifierPower > 0 || skill.flatPower > 0)
        {
            int healing = (int)(skill.modifierPower * caster.stats.stats[skill.attackType] * Mathf.Pow(1.25f, caster.status[skill.attackType]) + skill.flatPower);
            print(target.stats.monsterName + " is healed for " + healing + "!");
            target.currentHealth = Mathf.Clamp(target.currentHealth + healing, 0, target.baseMonster.health);
        }

    }

    public void ApplyStatus(Unit target)
    {
        for(int i = 0; i < skill.status.Length; ++i)
        {
            //Attempt to remove status increase buffs
            if(skill.status[i] < 0)
            {
                if(Random.Range(0,256) <= -skill.status[i]) { 
                
                    if(i<4)
                    {
                        target.status[i] = Mathf.Clamp(++target.status[i], -3, 3);
                    }
                    else
                    {
                        target.status[i] = Mathf.Clamp(--target.status[i], 0, 1);
                    }
                    print("Status applied: " + target.status[i]);
                }
            }
            //Attempt to inflict status
            else if(skill.status[i] > 0)
            {
                if (Random.Range(0, 256) <= skill.status[i]-target.stats.statusResistances[i])
                {
                    if (i < 4)
                    {
                        target.status[i] = Mathf.Clamp(--target.status[i], -3, 3);
                    }
                    else
                    {
                        target.status[i] = Mathf.Clamp(++target.status[i], 0, 1);
                    }
                    print("Status inflicted: " + target.status[i]);
                }
            }
        }

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
}
