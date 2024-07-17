using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffectScript : NetworkBehaviour
{
    private void Start()
    {
        StartCoroutine(DeactivateEffect());
    }

    private IEnumerator DeactivateEffect() 
    {
        yield return new WaitForSeconds(2);
        Runner.Despawn(Object);
    }

}
