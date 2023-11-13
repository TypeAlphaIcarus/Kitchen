using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    public virtual void Interact(Player player)
    {
        Debug.LogError("基类的互动方法");
    }
}