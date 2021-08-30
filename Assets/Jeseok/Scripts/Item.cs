using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("Get Bullet");
            WeaponManager.instance.AddBullet(10);
            Destroy(gameObject);
        }

    }
}