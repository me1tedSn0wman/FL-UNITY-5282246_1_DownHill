using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Reference Configurator", menuName = "Scriptable Objects/Car Management/Car Reference Configurator", order = 1)]
public class CarReferenceConfigurator : ScriptableObject
{
    public List<CarReference> listOfCarReferences;
}
