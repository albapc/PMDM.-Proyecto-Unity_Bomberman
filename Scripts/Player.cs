using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //objeto del script GlobalStateManager, para usar sus metodos y variables publicas en este script
    public GlobalStateManager globalManager;
    //parametros del jugador
    [Range (1, 2)] //agrega un slider en el editor
    public int playerNumber = 1;
    //Indica el numero de jugador
    public float moveSpeed = 5f;
    public bool canDropBombs = true;
    //puede el jugador lanzar bombas?
    public bool canMove = true;
    //puede moverse el jugador=

    //Prefabs
    public GameObject bombPrefab;

    //componentes
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;
    //variable que indica si el jugador esta vivo o muerto
    private bool dead = false;
    //contadores de vidas
    private int cont1 = 0;
    private int cont2 = 0;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
}

    void Update ()
    {
        UpdateMovement ();
    }

    //se encarga del movimiento de los jugadores
    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false);

        if (!canMove)
        { //Return si el jugador no se puede mover
            return;
        }

        //Dependiendo del numero de jugador, se mueve uno u otro
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }

    /// <summary>
    /// Actualiza el movimiento del jugador uno con las teclas WASD y deja bombas con la tecla espacio
    /// </summary>
    private void UpdatePlayer1Movement ()
    {
        if (Input.GetKey (KeyCode.W))
        { //movimiento arriba
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.A))
        { //movimiento izquierda
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.S))
        { //movimiento abajo
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.D))
        { //movimiento derecha
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
        { //dejar bomba
            DropBomb ();
        }
    }

    /// <summary>
    /// Actualiza el movimiento del jugador 2 con las teclas de direccion y deja bombas con la tecla Enter
    /// </summary>
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //movimiento arriba
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //movimiento izquierda
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //movimiento abajo
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //movimiento derecha
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        { //dejar bomba. Para el jugador dos permite usar ambas teclas enter del teclado
            DropBomb ();
        }
    }

    /// <summary>
    /// Deja una bomba en la posicion del jugador
    /// </summary>
    private void DropBomb ()
    {
        if (bombPrefab)
        { //comprueba si hay asignado un prefab de la bomba
            if(bombPrefab)
            {
                //deja una bomba a los pies del jugador
                //pasa los float a int para dejar las bombas en el centro de cada baldosa
                Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), 
            bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)),
            bombPrefab.transform.rotation);  


            }
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Explosion"))
        {
            Debug.Log ("P" + playerNumber + " hit by explosion!");
            
            globalManager.PlayerDied(playerNumber); //notifica al script GlobalStateManager que el jugador x murio
            
            dead = true; //cambia la variable para controlar la vida del jugador
            
            //si el jugador1 recibe una explosion...
            if(playerNumber == 1)
            {
                cont1 = cont1 +1; //se incrementa su contador
            } else if(playerNumber ==2) //y viceversa
            {
                cont2 = cont2 +1;
            }

            //se imprime en la consola para comprobar que todo esta en orden
            Debug.Log("cont1: " + cont1);
            Debug.Log("cont2: " + cont2);

            //si uno de los jugadores recibe 3 o mas impactos...
            if (cont1 >= 3 || cont2 >= 3)
            {
                Destroy(gameObject); //se destruye el objeto en cuestion (p1 o p2)
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        
        //la variable force indica la fuerza a la que empujara el jugador la bomba
        float force = 30000;

        //si el objeto que golpeamos es una bomba
        if(c.gameObject.tag == "Bomb")
        {
            //calcula el angulo entre el punto de colision y el jugador
            Vector3 dir = c.contacts[0].point - transform.position;

            //normalizamos el vector3
            dir = dir.normalized;

            //agregamos fuerza en la direccion de dir y la multiplicamos por force. 
            // esto empujara la bomba
            bombPrefab.GetComponent<Rigidbody>().AddForce(dir * force);

        }
        
    }
}
