using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public Monster monster;
    public int currentHealth;
    public int currentMana;
    public int direction = 2;

    public void Create(Monster monsterValue)
    {
        monster = monsterValue;
        gameObject.name = monsterValue.monsterName;
        currentHealth = monsterValue.health;
        currentMana = monsterValue.mana;
        gameObject.tag = "Enemy";
        SpriteRenderer p = gameObject.AddComponent<SpriteRenderer>();
        p.sprite = Resources.Load("Units\\" + monsterValue.monsterName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);
    }

    public void UseSkill(int index)
    {
        Skill skillInfo = GameManager.Manager.gameLibrary.skillData.Skills[index];
        GameObject skill = new GameObject();
        SkillComponent skillComponent;
        switch (skillInfo.type)
        {
            case 0:

                break;
            case 1:
                skillComponent = skill.AddComponent<ProjectileSkill>();
                skillComponent.Create(skillInfo, this);
                break;
            case 2:

                break;
        }

    }

    public void SkillOffensiveProjectile(Skill CurrentSkill)
    {

    }
}
