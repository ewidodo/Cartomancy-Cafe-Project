using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The data component of anything that will affect the final fortune
public class Ingredient : MonoBehaviour
{
    public string name;
    public string description;
    public Vector2 fortuneOffset = new();



    private void ApplyFortuneOffset()
    {
        //Barista.Instance.currentCustomer.ApplyFortuneOffset(fortuneOffset);
    }
}
