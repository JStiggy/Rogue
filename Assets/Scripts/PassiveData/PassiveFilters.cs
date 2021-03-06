﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveFilters {

    public delegate bool StatFilters(Unit unit, int value);
    public delegate bool SkillFilters(Skill skill, int value);
    public delegate bool DamageFilter(Unit defender, Skill skill, int value);
    public List<StatFilters> statFilters;
    public List<SkillFilters> skillFilters;
    public List<DamageFilter> damageFilters;

    public void Create()
    {
        statFilters = new List<StatFilters>();
        statFilters.Add(AboveHealth); statFilters.Add(BelowHealth); statFilters.Add(EqualHealth);
        statFilters.Add(HasStatus); statFilters.Add(DoesNotHaveStatus); statFilters.Add(HasNoStatus);

        skillFilters = new List<SkillFilters>();
        skillFilters.Add(IsElement); skillFilters.Add(NotElement); skillFilters.Add(IsType);
        skillFilters.Add(IsDamagingType); skillFilters.Add(IsHealingType); skillFilters.Add(IsPhysical);

        damageFilters = new List<DamageFilter>();
        damageFilters.Add(TargetAboveHealth); damageFilters.Add(TargetBelowHealth);
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

    public bool HasNoStatus(Unit unit, int value)
    {
        bool tmp = true;
        for(int i = 4; i < unit.status.Length; ++i)
        {
            tmp = tmp && unit.status[i] != 1;
        }
        return tmp;
    }

    //Delegates for SkillFilter, these can be used to apply statust alterations
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

    public bool IsPhysical(Skill skill, int value)
    {
        return IsElement(skill, 0) && IsDamagingType(skill, 0);
    }

    //Delegates for DamageFilters these alter damage dealt
    public bool TargetAboveHealth(Unit target, Skill skill, int value)
    {
        return (target.currentHealth / target.baseMonster.health) * 100 >= value;
    }

    public bool TargetBelowHealth(Unit target, Skill skill, int value)
    {
        return (target.currentHealth / target.baseMonster.health) * 100 <= value;
    }

}
