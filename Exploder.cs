using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private Cube _scenedCube;
    [SerializeField] private Spawner _spawner;
    
    [SerializeField] private float _explosionRadius = 50;
    [SerializeField] private float _maxExplosionForce = 1000;
    [SerializeField] private float _sizeExplodeModifier = 200;
    
    private void OnEnable()
    {
        _scenedCube.Clicked += Explode;
        _spawner.CubesSpawned += SubscribeToCubes;
    }
    
    private void OnDisable()
    {
        _scenedCube.Clicked -= Explode;
        _spawner.CubesSpawned -= SubscribeToCubes;
    }

    private void SubscribeToCubes(List<Cube> cubes)
    {
        foreach (Cube cube in cubes)
        {
            cube.Clicked += Explode;
        }
    }

    private void Explode(Vector3 explosionPosition, Vector3 cubeSize)
    {
        float baseExplosionForce = _maxExplosionForce - cubeSize.x * _sizeExplodeModifier;

        Debug.Log($"Base explosion force: {baseExplosionForce}");

        foreach (Rigidbody cube in GetExplodableObjects(explosionPosition))
        {
            float distanceToExplosion = Vector3.Distance(cube.position, explosionPosition);
            float distanceFactor = Mathf.Clamp01(1 - distanceToExplosion / _explosionRadius);
            float explosionForce = baseExplosionForce * distanceFactor;

            Debug.Log($"Applying explosion force: {explosionForce} to object at distance: {distanceToExplosion}");

            cube.AddExplosionForce(explosionForce, explosionPosition, _explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects(Vector3 explosionPosition)
    {
        Collider[] hits = Physics.OverlapSphere(explosionPosition, _explosionRadius);

        List<Rigidbody> cubes = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody)
                cubes.Add(hit.attachedRigidbody);

        return cubes;
    }
}
