using UnityEngine;
using System.Collections;

//Assumes that walls exist everywhere, needs to collide to end the skill effect
public class ProjectileSkill : SkillComponent {

    bool active = true;

    SpriteRenderer sRenderer;

    //Create the skill getting info about the caster and the skill used
    public override void Create(Skill ability, Unit cast, int itemIndex = -1)
    {
        this.item = itemIndex;
        this.skill = ability;
        this.caster = cast;
        transform.position = cast.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, cast.direction * -45f);

        sRenderer = gameObject.AddComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load("Skills\\"+skill.name, typeof(Sprite)) as Sprite;

        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
        c.size = new Vector2(.3f, .3f);

        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

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

    //Fire the skill in the forward direction move forward until an overlap event with a possible target occurs
    public override IEnumerator SkillSelection()
    {
        while(active)
        {
            this.transform.position += Quaternion.Euler(0, 0, -45f * caster.direction) * Camera.main.transform.right * Time.deltaTime * 10;
            yield return null;
        }
        yield return new WaitForSeconds(skill.animationTime);
        caster.currentMana = Mathf.Clamp(caster.currentMana - skill.cost, 0, caster.monster.mana);
        GameManager.Manager.board.EndTurn();
        Destroy(gameObject);
        yield return null;
    }

    //Check for any collisons then check all hit targets applying skill effects to them
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Wall")
        {
            sRenderer.enabled = false;
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position, skill.splashRange, caster);
            if(targets.Length == 0)
            {
                print("No Targets");
                Destroy(gameObject);
            }
            else
            {
                foreach(Collider2D t in targets) SkillEffect(t.GetComponent<Unit>());
            }
        }
        else if (hit.gameObject.tag != caster.gameObject.tag && hit.gameObject != caster.gameObject)
        {
            sRenderer.enabled = false;
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position, skill.splashRange, caster);
            foreach (Collider2D t in targets)
            {
                SkillEffect(t.GetComponent<Unit>());
            }
        }
        else if (hit.gameObject.tag == caster.gameObject.tag && hit.gameObject != caster.gameObject && skill.targetType >= 2)
        {
            sRenderer.enabled = false;
            active = false;
            Collider2D[] targets = SplashTargets(hit.gameObject.transform.position, skill.splashRange, caster);
            foreach (Collider2D t in targets)
            {
                SkillEffect(t.GetComponent<Unit>());
            }
        }
    }
}