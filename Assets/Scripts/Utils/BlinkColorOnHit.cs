using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class BlinkColorOnHit : MonoBehaviour
{
    private static float    blinkDuration  = 0.1f; // # seconds to show damage
    private static Color    blinkColor     = Color.red;

    [Header("Dynamic")]
    public bool             showingColor = false;
    public float            blinkCompleteTime;
    public bool             ignoreOnCollisionEnter = false;

    private Material[]      materials;
    private Color[]         originalColors;

    void Awake()
    {
        // Get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials( gameObject );
        originalColors = new Color[materials.Length];
        for (int i=0;  i<materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showingColor && Time.time > blinkCompleteTime) RevertColors();        
    }

    /// <summary>
    /// Sets the Albedo color (i.e., the main color) of all materials in the
    ///     materials array to blinkColor, sets showingColor to true, and sets the 
    ///     time that the colors should be reverted.
    /// </summary>
    public void SetColors()
    {
        if (this.gameObject.IsDestroyed())
            return;
        foreach (Material m in materials)
        {
            m.color = blinkColor;
        }
        showingColor = true;
        blinkCompleteTime = Time.time + blinkDuration;
    }

    /// <summary>
    /// Reverts all materials in the materials array back to their original color
    ///     and sets showingColor to false.
    /// </summary>
    void RevertColors()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = originalColors[i];
        }
        showingColor = false;
    }
}
