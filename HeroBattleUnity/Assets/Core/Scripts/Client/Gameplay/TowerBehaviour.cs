
using LiteEntitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour
{

    public SyncVar<float> hp;
    // Start is called before the first frame update
    void Start()
    {
        hp.Value = 10f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTakeDamage(float damage)
    {
        hp.Value = hp.Value - damage;
        if (hp <= 0)
        {
            //NetworkServer.Destroy(gameObject);
        }
    }
}
