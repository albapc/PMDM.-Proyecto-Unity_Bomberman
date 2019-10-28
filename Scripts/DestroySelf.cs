using UnityEngine;
using System.Collections;

/// <summary>
/// Script para destruir un objeto despues de un rato
/// </summary>
public class DestroySelf : MonoBehaviour
{
    public float Delay = 3f;
    //Retardo en segundos antes de destruir el gameobject

    void Start ()
    {
        Destroy (gameObject, Delay);
    }
}
