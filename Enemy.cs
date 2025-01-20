using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    public int Health = 100;

    public void Damage()
    {
        Debug.Log("damage taken");
        Health -= 10;

        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
