﻿using UnityEngine;
using System.Collections;

//Template class for all Units, ally or enemy in the game
public abstract class Unit : MonoBehaviour
{

    public Monster monster;
    public int currentHealth;
    public int currentMana;
    public int direction = 2;

    public abstract void Create(Monster monsterValue);

    public abstract IEnumerator StartTurn();

    public abstract void UseSkill(int index);

    public IEnumerator MoveTowards(Vector2 TargetPosition)
    {

        yield return null;
    }

}