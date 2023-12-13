using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Boid;
    public List<GameObject> Boids;
    public Transform Target;

    public int num;
    private void Start()
    {
        for(int i = 0; i < num; i++)
        { 
           GameObject obj= Instantiate(Boid);
          
           Boids.Add(obj); 
        }

        foreach (var obj in Boids)
        {
           Boid boid = obj.GetComponent<Boid>();
           boid._boidList = Boids;
            boid.Target = Target;
        }
    }

}
