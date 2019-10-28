using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {
    
    Transform player;
	NavMeshAgent enemy;
    Animator animator;
	public float proximidad;

	void Awake()
	{
        //asignacion del player a su etiqueta y posicion
		player = GameObject.FindGameObjectWithTag("Player").transform;
        //asignacion del enemigo
		enemy = GetComponent<NavMeshAgent>();
        //asignacion del component animator del enemigo
        animator = enemy.GetComponent<Animator>();
    }

	void Update()
	{
        //mueve el enemigo hacia donde esta el jugador
		enemy.SetDestination(player.position);

        //si la distancia que hay del enemigo al jugador es menor a la proximidad marcada...
		if(!enemy.pathPending && enemy.remainingDistance < proximidad) 
		{
            //imprime en la consola
			Debug.Log("Peligro");
            animator.SetBool("estarEnPeligro", true); //el enemigo cambia de color a rojo

        } 
		else //si pasa lo contrario...
		{
            //mensaje en la consola
			Debug.Log("Tranqui");
            animator.SetBool("estarEnPeligro", false); //el enemigo cambia de color a rojo
        }
	}
    


}
