using UnityEngine;

public class Guard : MonoBehaviour
{
    public GuardHand guardHand;

    public int guardDamage;

    private void Start()
    {
        guardHand.damage = guardDamage;
    }
}
