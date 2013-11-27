using UnityEngine;
using System.Collections;

// Thanks to Nick Gravelyn for this script 
// via https://gist.github.com/nickgravelyn/4385548

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticleSystem : MonoBehaviour
{
    private GameObjectPool _pool;
    
    void OnPoolCreate(GameObjectPool pool)
    {
        _pool = pool;

        particleSystem.renderer.enabled = true;
        particleSystem.time = 0;
        particleSystem.Clear(true);
        particleSystem.Play(true);
    }

    void OnPoolRelease()
    {
        particleSystem.Stop();
        particleSystem.time = 0;
        particleSystem.Clear(true);
        particleSystem.renderer.enabled = false;
    }

    void Update()
    {
        if (!particleSystem.IsAlive(true) && particleSystem.renderer.enabled)
        {
            _pool.ReleaseInstance(transform);
        }
    }
}