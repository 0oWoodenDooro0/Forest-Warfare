using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !Game.Player1God)
        {
            Game.Player1Blood = GetDamage(Game.Player1Blood);
        }
        else if (other.gameObject.CompareTag("Player2") && !Game.Player2God)
        {
            Game.Player2Blood = GetDamage(Game.Player2Blood);
        }
    }

    private int GetDamage(int blood)
    {
        var remain = blood;
        remain -= damage;

        return remain;
    }
}