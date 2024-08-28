using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _minCubeAmount = 2;
    [SerializeField] private int _maxCubeAmount = 6;

    [SerializeField] private Cube _scenedCube;
    
    private int _cloneChanceReduce = 2;
    private int _cloneScaleReduce = 2;
    
    public event Action<List<Cube>> CubesSpawned;

    private void OnEnable()
    {
        _scenedCube.Split += SpawnCubes;
    }

    private void OnDisable()
    {
        _scenedCube.Split -= SpawnCubes;
    }

    private void SpawnCubes(Cube cube)
    {
        cube.Split -= SpawnCubes;

        int cloneChance = cube.CloneChance / _cloneChanceReduce;

        int cubeAmount = Random.Range(_minCubeAmount, _maxCubeAmount + 1);
        
        Vector3 newSize = cube.transform.localScale /= _cloneScaleReduce;

        List<Cube> cubesSpawned = new();
        
        for (int i = 0; i < cubeAmount; i++)
        {
            var clone = Instantiate(cube, cube.transform.position, new Quaternion());
        
            clone.Init(cloneChance, newSize);
            
            cubesSpawned.Add(clone);
            
            clone.Split += SpawnCubes;
        }
        
        CubesSpawned?.Invoke(cubesSpawned);
    }
}
