using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : NetworkBehaviour
{
    [SyncVar]
    public float hp = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
