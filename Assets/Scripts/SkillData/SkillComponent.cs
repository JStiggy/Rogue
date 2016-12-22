using UnityEngine;
using System.Collections;

//Template for all skills
abstract public class SkillComponent : MonoBehaviour {

    public Skill skill;

    public abstract void Create(Skill ability, Unit caster);

    public abstract IEnumerator SkillSelection();

    public abstract void SkillEffect(Unit target);

}
