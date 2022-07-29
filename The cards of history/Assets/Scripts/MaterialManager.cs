using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialManager : MonoBehaviour
{

    public static void SetMaterial(Renderer render, string name)
    {
        render.material = Resources.Load<Material>("MapMaterials/" + name);
    }

    public static Material getMaterial(string name)
    {
        return Resources.Load<Material>("MapMaterials/" + name);
    }

    public static Material SetCurrentMaterial(string name)
    {
        return Resources.Load<Material>("MapMaterials/" + name);
    }
}

