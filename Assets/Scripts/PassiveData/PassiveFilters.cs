using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveFilters {

    public delegate bool StatFilters(Unit unit, int value);
    public delegate bool SkillFilters(Skill skill, int value);
    public delegate bool DamageFilter(Unit attacker, Unit defender, Skill skill, int value);
    public List<StatFilters> statFilters;
    public List<SkillFilters> skillFilters;
    public List<DamageFilter> damageFilters;

    public void Create()
    {
        statFilters = new List<StatFilters>();
        statFilters.Add(AboveHealth); statFilters.Add(BelowHealth); statFilters.Add(EqualHealth);
        statFilters.Add(HasStatus); statFilters.Add(DoesNotHaveStatus); statFilters.Add(HasNoStatus);

    }

    //Can be called directly
    public bool GenerateRandom(int min)
    {
        return Random.Range(0, 101) > min;
    }

    //Delegates for StatFilter
    public bool AboveHealth(Unit unit, int percent)
    {
        return (unit.currentHealth / unit.baseMonster.health) * 100 >= percent;
    }

    public bool BelowHealth(Unit unit, int percent)
    {
        return (unit.currentHealth / unit.baseMonster.health) * 100 <= percent;
    }

    public bool EqualHealth(Unit unit, int percent)
    {
        return (unit.currentHealth / unit.baseMonster.health) * 100 == percent;
    }

    public bool HasStatus(Unit unit, int status)
    {
        return unit.status[status] == 1;
    }

    public bool DoesNotHaveStatus(Unit unit, int status)
    {
        return unit.status[status] != 1;
    }

    public bool HasNoStatus(Unit unit, int val)
    {
        bool tmp = true;
        for(int i = 4; i < unit.status.Length; ++i)
        {
            tmp = tmp && unit.status[i] != 1;
        }
        return tmp;
    }

    //Delegates for SkillFilter
    public bool IsElement(Skill skill, int value)
    {
        return skill.element == value;
    }

    public bool NotElement(Skill skill, int value)
    {
        return skill.element != value;
    }

    public bool IsType(Skill skill, int value)
    {
        return skill.type == value;
    }

    public bool IsDamagingType(Skill skill, int value)
    {
        return (skill.type == 0) || (skill.type == 1) || (skill.type == 2) || (skill.type == 6);
    }

    public bool IsHealingType(Skill skill, int value)
    {
        return (skill.type == 4) || (skill.type == 5) || (skill.type == 3) || (skill.type == 7);
    }

}
