using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyController ec;
    private Collider actualCollider;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CornerLeft"))
        {
            if (ec.getPlayerCollider().bounds.Contains(other.bounds.center))
            {
                if (ec.turnDir == 0)
                {
                    ec.setCanJump(false);
                    ec.turnDir = 1;
                    ec.setCollider(other);
                    //actualCollider = other;
                }
            }
        }
        else if (other.CompareTag("CornerRight"))
        {
            if (ec.getPlayerCollider().bounds.Contains(other.bounds.center))
            {
                if (ec.turnDir == 1)
                {
                    ec.setCanJump(false);
                    ec.turnDir = 0;
                    ec.setCollider(other);
                    //actualCollider = other;
                }
            }
        }
        else if (other.CompareTag("Jump"))
        {
            ec.setJumpCollider(other);
            ec.jumpInGodMode();
        }
        else if (other.CompareTag("DoubleJump"))
        {
            ec.setJumpCollider(other);
            ec.doubleJumpInGodMode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CornerLeft"))
        {
            ec.setCanJump(true);
        }
        else if (other.CompareTag("CornerRight"))
        {
            ec.setCanJump(true);
        }
    }
}
