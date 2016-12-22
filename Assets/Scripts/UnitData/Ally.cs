using UnityEngine;
using System.Collections;

public class Ally : Unit
{

    //Create an enemy of specified base stats and any needed values
    public override void Create(Monster monsterValue)
    {
        monster = monsterValue;
        gameObject.name = monsterValue.monsterName;
        currentHealth = monsterValue.health;
        currentMana = monsterValue.mana;

        gameObject.tag = "Ally";
        gameObject.layer = 9;

        SpriteRenderer p = gameObject.AddComponent<SpriteRenderer>();
        p.sprite = Resources.Load("Units\\" + monsterValue.monsterName, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);
    }

    public override IEnumerator StartTurn()
    {
        yield return null;
    }

    //Use a skill of the specified index. This will typical use skills in monster.skills. However any skill can be called technically
    public override void UseSkill(int index)
    {
        Skill skillInfo = GameManager.Manager.gameLibrary.skillData.Skills[index];
        GameObject skill = new GameObject();
        SkillComponent skillComponent;
        switch (skillInfo.type)
        {
            case 0:
                skillComponent = skill.AddComponent<TargetedSkill>();
                skillComponent.Create(skillInfo, this);
                break;
            case 1:
                skillComponent = skill.AddComponent<ProjectileSkill>();
                skillComponent.Create(skillInfo, this);
                break;
            case 2:
                // Other skill types need to be added
                break;
        }
    }
}