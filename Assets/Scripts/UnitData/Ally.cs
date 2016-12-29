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
        Vector3 inputVec = Vector3.zero;

        while (true)
        {
            // Check for Player Input
            inputVec = Vector3.zero;
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            // Allow the player to rotate
            if (inputVec != Vector3.zero && Input.GetButton("Cancel"))
            {
                direction = (int)(Vector3.Angle(Vector3.right, inputVec) / 45f);
                direction = inputVec.y > 0 ? 8 - direction : direction;
            }

            if(Input.GetButtonDown("Menu"))
            {
                GameManager.Manager.board.menu.StartCoroutine("CombatMenu");
                break;
            }

            //Add a fraction of delay after releasing the rotate button
            if (Input.GetButtonUp("Cancel"))
                yield return new WaitForSeconds(.05f);

            //Use the default attack
            if (Input.GetButtonDown("Submit"))
            {
                StartCoroutine(Attack());
                break;
            }

            // Check for any movement and allow movement if possible
            if (inputVec != Vector3.zero && !Input.GetButton("Cancel"))
            {
                Collider2D[] other = Physics2D.OverlapCircleAll(transform.position + inputVec, .3f, (1 << 8 | 1 << 9 | 1 << 10));

                if (other.Length == 0)
                {
                    StartCoroutine(MoveTowards(inputVec));
                    break;
                }
                else if (other[0].gameObject.layer == 9)
                {
                    //Allow for swapping with an allied unit
                    StartCoroutine(Swap(inputVec, other[0].GetComponent<Unit>()));
                    break;
                }
                else
                {
                    direction = (int)(Vector3.Angle(Vector3.right, inputVec) / 45f);
                    direction = inputVec.y > 0 ? 8 - direction : direction;
                }
            }
            yield return null;
        }
        yield return null;
    }

    public IEnumerator Attack()
    {
        UseSkill(2);
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
            case 3:
                skillComponent = skill.AddComponent<TargetedSkill>();
                skillComponent.Create(skillInfo, this);
                break;
            case 1:
            case 4:
                skillComponent = skill.AddComponent<ProjectileSkill>();
                skillComponent.Create(skillInfo, this);
                break;
            case 2:
            case 5:
                skillComponent = skill.AddComponent<MeleeSkill>();
                skillComponent.Create(skillInfo, this);
                break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        if(direction % 2 == 1)
        {
            Gizmos.DrawSphere(transform.position + Quaternion.Euler(0,0,-45f * direction) * transform.right * 1.41f, .15f);
        }
        else
        {
            Gizmos.DrawSphere(transform.position + Quaternion.Euler(0, 0, -45f * direction) * transform.right, .15f);
        }
    }
}