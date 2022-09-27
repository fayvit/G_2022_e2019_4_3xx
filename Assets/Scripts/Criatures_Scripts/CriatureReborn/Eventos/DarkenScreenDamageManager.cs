using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021;

public class DarkenScreenDamageManager : DarkenScreenDamageBase
{

    [SerializeField] private Transform returnDarkPosition;
   
    protected override void Posicione()
    {
        Moved.position = returnDarkPosition.position;
        Moved.GetComponent<CharacterManager>().ActivePet.transform.position = Moved.position - 2 * Moved.forward;
    }

}

