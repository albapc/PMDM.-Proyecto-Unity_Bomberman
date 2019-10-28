using UnityEngine;
using System.Collections;

/// <summary>
///Este script comprueba que se puede dejar una bomba en la posicion del jugador sin distorsionar el movimiento cuando el jugador se mueva.
/// Desactiva el trigger en el collider, haciendo que el objeto sea solido.
/// </summary>
public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        { // Cuando el jugador salga del zona del trigger...
            GetComponent<Collider> ().isTrigger = false; // desactivar el trigger
        }
    }
}
