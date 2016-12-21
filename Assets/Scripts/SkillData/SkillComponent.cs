using UnityEngine;
using System.Collections;

abstract public class SkillComponent : MonoBehaviour {

    public Skill skill;

    public abstract void Create(Skill ability, Enemy caster);

    public abstract IEnumerator SkillSelection();

    public abstract void SkillEffect(Enemy target);

}
