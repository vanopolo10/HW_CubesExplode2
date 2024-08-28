using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private int _hundredPercent = 100;
    
    public event Action<Cube> Split;
    public event Action<Vector3, Vector3> Clicked; 
    
    public int CloneChance { get; private set; } = 100;
    
    private void OnMouseDown()
    {
        if (Random.Range(1, _hundredPercent + 1) <= CloneChance)
        {
            Split?.Invoke(this);
        }
        
        Clicked?.Invoke(transform.position, transform.lossyScale);

        Clicked = null;
        
        Destroy(gameObject);
    }

    public void Init(int cloneChance, Vector3 size)
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
        
        CloneChance = cloneChance;

        gameObject.transform.localScale = size;
    }
}
