using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	public GameObject explosionPrefab;
	public LayerMask levelMask;
	//variable para saber si la bomba exploto
	private bool exploded = false;

	void Start () 
	{
		//llama al metodo y espera 3 segundos para iniciarlo
		Invoke("Explode", 3f);
	}
	
	void Update () 
	{
		
	}

	void Explode() 
	{
		//lanza una explosion en la posicion de la bomba
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);

		//inicia las explosiones en todas las direcciones
		StartCoroutine(CreateExplosions(Vector3.forward));
		StartCoroutine(CreateExplosions(Vector3.right));
		StartCoroutine(CreateExplosions(Vector3.back));
		StartCoroutine(CreateExplosions(Vector3.left));

		//desactiva el MeshRenderer, haciendo que la bomba sea invisible
		GetComponent<MeshRenderer>().enabled = false;
		//indica que la bomba exploto
		exploded = true;
		//desactiva el collider, lo que permite que el jugador pueda atravesarlas
		transform.Find("Collider").gameObject.SetActive(false);
		//destruye la bomba en 3 segundos, asi nos aseguramos de que las explosiones se activen antes de destruir la bomba
		Destroy(gameObject, .3f); 
		
	}

	private IEnumerator CreateExplosions(Vector3 direction) 
	{
		//itera un bucle for por cada unidad de distancia que queremos que cubra la explosion (en este caso 2 metros)
		for(int i = 1; i < 3; i++) 
		{
			//contiene informacion sobre que y a que posicion alcanza o no el raycast
			RaycastHit hit;
			//envia un raycast desde el centro de la bomba hacia la direccion pasada en el StartCoroutine. Luego pasa el resultado 
			//al objeto RaycastHit. El parametro i determina la distancia que deberia recorrer el rayo. Por ultimo, usa el LayerMask levelMask
			//para asegurarse de que el rayo solo compruebe los bloques del nivel e ignore al jugador y al resto de colliders.
			Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit,
			i, levelMask);

			//si el raycast no colisiona con nada, la baldosa esta libre
			if(!hit.collider) 
			{
				//lanza una explosion en la posicion que indique el raycast
				Instantiate(explosionPrefab, transform.position + (i * direction),
				//cuando el raycast colisione con un bloque...
				explosionPrefab.transform.rotation);
			}
			else
			{
				//sale del bloque for, nos aseguramos de que la explosion no traspase las paredes
				break;
			}
			//espera 0.05 segundos antes de hacer una segunda iteracion del bucle. Esto hace que la explosion sea mas realista, ya que parece que
			//explosion se expande
			yield return new WaitForSeconds(.05f);
		}
	}

	/// <summary>
	/// OnTriggerEnter se llama cuando el collider entra en el trigger.
	/// </summary>
	/// <param name="other">El collider other declarado en esta colision.</param>
	public void OnTriggerEnter(Collider other)
	{
		//comprueba que la bomba no exploto y si el trigger collider tiene asignada la etiqueta "explosion"
		if(!exploded && other.CompareTag("Explosion"))
		{
			//cancela la llamada al metodo cuando se tira la bomba, para que esta no explote dos veces
			CancelInvoke("Explode");
			Explode(); //llama al metodo explode()
		}
	}
}
