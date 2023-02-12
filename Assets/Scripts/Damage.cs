using UnityEngine;

public class Damage : MonoBehaviour
{
    public Player.DamageType damageType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Game.Player1Blood = GetDamage(damageType, Game.Player1Blood);
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            Game.Player2Blood = GetDamage(damageType, Game.Player2Blood);
        }
    }

    private int GetDamage(Player.DamageType damageType, int blood)
    {
        var remain = blood;
        if (damageType == Player.DamageType.KeyCodeJ)
        {
            remain -= 5;
        }
        else if(this.damageType== Player.DamageType.KeyCodeK)
        {
            remain -= 15;
        }
        else if(this.damageType== Player.DamageType.KeyCodeL)
        {
            remain -= 10;
        }
        else if(this.damageType== Player.DamageType.KeyCodeI)
        {
            remain -= 25;
        }

        return remain;
    }
}