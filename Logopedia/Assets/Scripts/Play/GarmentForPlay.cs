using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GarmentForPlay : MonoBehaviour
{

    public class Factory : PlaceholderFactory<string, GarmentForPlay>
    {

    }
}
