using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AshBehavior : MonoBehaviour
{
    //comme il aura plusieurs couleurs, autant avoir un tableau
    public Material [] materials;
    private NavMeshAgent agent;
    private CubeBehavior cubeDestination;
    public int colorIndex;
    
    void Start()
    {
        if(materials == null || materials.Length < 4)
        {
            Debug.LogError("This component need 4 materials", gameObject); 
            // pour prévenir l'utilisateur de ce qu'il faut et de quel objet en question
        }
        else 
        {
            int index = Random.Range(0,3);
            GetComponent<Renderer>().material = materials[colorIndex];
        }

        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(CubeBehavior cube)
    {
        agent.SetDestination(cube.transform.position);
        cubeDestination = cube;

    }
    
    private void ChangeColor()
    {
        int exchange = colorIndex;
        colorIndex = cubeDestination.colorIndex;
        cubeDestination.colorIndex = exchange;

        GetComponent<Renderer>().material = materials[colorIndex]; 
        cubeDestination.GetComponent<Renderer>().material = materials[cubeDestination.colorIndex];
        //avant changement:
        //Material mat = GetComponent<Renderer>().material; // je mémorise la couleur qui est sur moi
        //GetComponent<Renderer>().material = cubeDestination.GetComponent<Renderer>().material; // puis je dis que la couleur plus haut est égale à celle du cube que je prends
        //cubeDestination.GetComponent<Renderer>().material = mat; // et le cube dont j'ai pris la couleur obtient la couleur du matériel au début
        // algorithme de transmission
    }
    void Update()
    {
        if(cubeDestination != null)
        {
            if(Vector3.Distance(cubeDestination.transform.position,transform.position) < 0.5f)
            {
                ChangeColor(); // echange ma couleur avec le cube que je touche
                cubeDestination = null; // pour changer la condition qui était toujours vrai
            }
        }

    }
}
