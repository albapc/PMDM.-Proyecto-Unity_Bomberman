using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GlobalStateManager : MonoBehaviour
{
    //cantidad de jugadores muertos
    private int deadPlayers = 0;
    //identifica que jugador perdio
    private int deadPlayerNumber = -1;
    //numero de vidas de cada jugador
    public int P1Count, P2Count;

    //objs texto
    public Text UIText, P1Lives, P2Lives;

    void Start()
    {
        //valores iniciales
        P1Count = 3; 
        P2Count = 3;
        SetLifeCount();
        UIText.text = "";
        CheckNumberOfLives();
    }

    void Update() {
        SetLifeCount();
    }


    public void PlayerDied (int playerNumber)
    {
        deadPlayers++; //agrega un jugador al contador

        //si perdio 1 jugador o mas...
        if (deadPlayers >= 1) 
        {
            deadPlayerNumber = playerNumber; //indica el numero del jugador en cuestion
            Invoke("CheckPlayersDeath", .3f); //deja pasar 3 segundos y comprueba si el otro jugador tambien perdio 
            Invoke("CheckNumberOfLives", .5f); //deja pasar 5 segundos y comprueba el numero de vidas de cada jugador
        }  
    }

    //NOTA: si el jugador esta dentro del rango de explosion de la bomba, y esta situado en el medio de dos casillas, este recibira el impacto que abarcan
    //ambas casillas, es decir, perdera dos vidas de golpe, ya que impactaria con ambos colliders.
    void CheckPlayersDeath() 
    {
        //si perdio uno o mas jugadores...
        if (deadPlayers >= 1) 
        {
            
            //si el jugador es el numero 1...
            
            if (deadPlayerNumber == 1) 
            { 
                //le restamos una vida al jugador1
                P1Count = P1Count - 1;
                

            } 
            else if(deadPlayerNumber == 2) //si es el jugador2...
            { 
                //le restamos una vida al jugador2
                P2Count = P2Count - 1;
                
            }
        }
    }


    //asigna los valores del numero de vidas a los objetos de tipo texto
    void SetLifeCount()
    {
        P1Lives.text = P1Count.ToString();
        P2Lives.text = P2Count.ToString();
    }

    //comprueba el numero de vidas de los jugadores
    public void CheckNumberOfLives()
    {
        //si el numero de vidas del jugador 1 es menor o igual que cero...
        if (P1Count <= 0)
        {
            //mostrar que gano el jugador 2
            UIText.text = "Player 2 is the winner!";
   
        }
        else if(P2Count <= 0) //si es el jugador 2...
        {
            //mostrar que gano el jugador 1
            UIText.text = "Player 1 is the winner!";

        } else if(P1Count <= 0 && P2Count <= 0) //si perdieron los 2...
        {
            //mostrar que empataron
            UIText.text = "The game ended in a draw!";

        }
    }
    
}
