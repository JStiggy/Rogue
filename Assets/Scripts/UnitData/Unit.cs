using UnityEngine;
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

    public IEnumerator MoveTowards(Vector3 deltaPosition, GameObject other)
    {
        direction = (int)(Vector3.Angle(Vector3.right, deltaPosition) / 45f);
        direction = deltaPosition.y > 0 ? 8 - direction : direction;
        Vector3 targetPosition = this.transform.position + deltaPosition;
        while (this.transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3 * Time.deltaTime);
            yield return null;
        }
        GameManager.Manager.board.EndTurn();
        yield return null;
    }

    public IEnumerator MoveTowards(Vector3 deltaPosition)
    {
        direction = (int) (Vector3.Angle(Vector3.right, deltaPosition) / 45f);
        direction = deltaPosition.y > 0 ? 8 - direction : direction;
        Vector3 targetPosition = this.transform.position + deltaPosition;
        while (this.transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3*Time.deltaTime);
            yield return null;
        }
        GameManager.Manager.board.EndTurn();
        yield return null;
    }

    public IEnumerator Swap(Vector3 deltaPosition, Unit other)
    {
        direction = (int)(Vector3.Angle(Vector3.right, deltaPosition) / 45f);
        direction = deltaPosition.y > 0 ? 8 - direction : direction;

        other.direction = (int)(Vector3.Angle(Vector3.right, -deltaPosition) / 45f);
        other.direction = -deltaPosition.y > 0 ? 8 - direction : direction;

        Vector3 targetPosition = this.transform.position + deltaPosition;
        Vector3 targetPosition2 = other.transform.position - deltaPosition;

        while (this.transform.position != targetPosition && other.transform.position != targetPosition2)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3 * Time.deltaTime);
            other.transform.position = Vector3.MoveTowards(other.transform.position, targetPosition2, 3 * Time.deltaTime);
            yield return null;
        }
        GameManager.Manager.board.EndTurn();
        yield return null;
    }

}