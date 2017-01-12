using UnityEngine;
using System.Collections;

public class TargetedSkill : SkillComponent {

    Unit target = null;
    SpriteRenderer sRenderer;

    //Create the skill adding all needed data, enable targeting
    public override void Create(Skill ability, Unit cast, int itemIndex)
    {
        this.item = itemIndex;
        transform.position = cast.transform.position;

        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Skills\\Reticle", typeof(Sprite)) as Sprite;

        this.skill = ability;
        this.caster = cast;
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

    //Iterate through all valid targets based on their distance from the user
    public override IEnumerator SkillSelection()
    {
        yield return null;
        //Get all targets in range, make sure a valid target exists or return control to player
        Collider2D[] targets = SplashTargets(transform.position, skill.targetRange, caster);
        if (targets.Length == 0)
        {
            Destroy(this.gameObject);
            yield return null;
        }
        else
        {
            //Allow the user to select between targets, select a target, or cancel the ability
            target = targets[0].gameObject.GetComponent<Unit>();
            int currentSelection = 0;
            while (true)
            {
                if (Input.GetButtonDown("Horizontal"))
                {
                    currentSelection += (int)Input.GetAxisRaw("Horizontal");
                    currentSelection = currentSelection >= targets.Length ? 0 : currentSelection;
                    currentSelection = currentSelection < 0 ? (targets.Length - 1) : currentSelection;
                    target = targets[currentSelection].gameObject.GetComponent<Unit>();
                    this.transform.position = target.transform.position;
                }
                if(Input.GetButtonDown("Cancel"))
                {

                    caster.StartCoroutine("StartTurn");
                    break;
                }

                if (Input.GetButtonDown("Submit"))
                {
                    //reticleDisplay = false;

                    Collider2D[] hits = SplashTargets(transform.position, skill.splashRange, caster);
                    foreach (Collider2D t in hits) SkillEffect(t.GetComponent<Unit>());

                    sRenderer.sprite = Resources.Load("Skills\\" + skill.name, typeof(Sprite)) as Sprite;

                    yield return new WaitForSeconds(skill.animationTime);
                    caster.currentMana = Mathf.Clamp(caster.currentMana - skill.cost, 0, caster.baseMonster.mana);
                    GameManager.Manager.board.EndTurn();
                    GameManager.Manager.DecrementInventory(item);
                    break;
                }

                yield return null;
            }
            Destroy(gameObject);
            yield return null;
        }
    }
}
