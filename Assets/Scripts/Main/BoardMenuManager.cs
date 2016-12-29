using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardMenuManager : MonoBehaviour {

    public Image[] unitHUD;
    public Image[] unitImage;
    public Text[] unitText;

    public Image[] combatMenu;
    public Text[] combatMenuText;

    public Text skillDescription;
    float menuDelay = .15f;

    public void UpdateHUD()
    {
        List<Unit> allies = GameManager.Manager.board.allies;
        for (int i = 0; i < 4; i++)
        {
            if(allies.Count <= i)
            {
                unitHUD[i].enabled = unitImage[i].enabled = unitText[i].enabled = false;
            }
            else
            {
                unitHUD[i].enabled = unitImage[i].enabled = unitText[i].enabled = true;
                unitImage[i].sprite = allies[i].GetComponent<SpriteRenderer>().sprite;
                unitText[i].text = allies[i].currentHealth.ToString().PadRight(3) + "/" + allies[i].monster.health.ToString().PadRight(3) + "\n" +
                                   allies[i].currentMana.ToString().PadRight(3) + "/" + allies[i].monster.mana.ToString().PadRight(3) + "\nHealthy";
            }
        }
    }

    public IEnumerator CombatMenu()
    {
        yield return null;
        combatMenu[0].gameObject.SetActive(true);
        int moveSelection = 0;
        combatMenuText[moveSelection].fontStyle = FontStyle.Italic;
        while (true)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                combatMenuText[moveSelection].fontStyle = FontStyle.Normal;
                GameManager.Manager.board.currentUnit.StartCoroutine("StartTurn");
                break;
            }

            if (Input.GetButtonDown("Submit"))
            {
                combatMenuText[moveSelection].fontStyle = FontStyle.Normal;
                this.StartCoroutine("SpellMenu");
                break;
            }

            if (Input.GetAxisRaw("Vertical") != 0)
            {
                combatMenuText[moveSelection].fontStyle = FontStyle.Normal;
                moveSelection -= (int)Input.GetAxisRaw("Vertical");
                moveSelection = moveSelection >= combatMenuText.Length ? 0 : moveSelection;
                moveSelection = moveSelection < 0 ? (combatMenuText.Length - 1) : moveSelection;
                combatMenuText[moveSelection].fontStyle = FontStyle.Italic;
                yield return new WaitForSeconds(menuDelay);
            }
            yield return null;
        }
        combatMenu[0].gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator SpellMenu()
    {
        yield return null;
        combatMenu[1].gameObject.SetActive(true);

        int moveSelection = 0;

        Unit cUnit = GameManager.Manager.board.currentUnit;
        print(cUnit.monster.monsterName);
        List<Skill> skills = GameManager.Manager.gameLibrary.skillData.Skills;

        string[] element = { "Phys", "Arcane", "Fire", "Ice", "Elec" };

        skillDescription.text = skills[cUnit.monster.skills[moveSelection]].name + "\n" + skills[cUnit.monster.skills[moveSelection]].cost + " Mana\nElement: " + element[skills[cUnit.monster.skills[moveSelection]].element] + "\n\n" + skills[cUnit.monster.skills[moveSelection]].description;
        while (true)
        {

            if (Input.GetButtonDown("Cancel"))
            {
                GameManager.Manager.board.menu.StartCoroutine("CombatMenu");
                break;
            }

            if(Input.GetButtonUp("Menu"))
            {
                GameManager.Manager.board.currentUnit.StartCoroutine("StartTurn");
                break;
            }

            if(Input.GetButtonDown("Submit"))
            {
                GameManager.Manager.board.currentUnit.UseSkill(cUnit.monster.skills[moveSelection]);
                break;
            }

            if (Input.GetAxisRaw("Vertical") != 0)
            {
                moveSelection -= (int)Input.GetAxisRaw("Vertical");
                moveSelection = moveSelection >= cUnit.monster.skills.Length ? 0 : moveSelection;
                moveSelection = moveSelection < 0 ? (cUnit.monster.skills.Length - 1) : moveSelection;

                skillDescription.text = skills[cUnit.monster.skills[moveSelection]].name + "\n" + skills[cUnit.monster.skills[moveSelection]].cost + " Mana\nElement: " + element[skills[cUnit.monster.skills[moveSelection]].element] + "\n\n" + skills[cUnit.monster.skills[moveSelection]].description;

                yield return new WaitForSeconds(menuDelay);
            }
            yield return null;
        }
        combatMenu[1].gameObject.SetActive(false);
        yield return null;
    }
}
