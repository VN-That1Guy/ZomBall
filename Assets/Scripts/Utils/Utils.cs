using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //== Bézier Curves ============================================================\\

    /// <summary>
    /// While most Bezier curves are 3 or 4 points, it is possible to have any number of points using this recursive function.
    /// </summary>
    /// <param name="u">The amount of interpolation [0..1]</param>
    /// <param name="points">Any Array of Vector3s to interpolate</param>
    static public Vector3 Bezier( float u, params Vector3[] points )
    {
        // Set up the array
        Vector3[,] vArr = new Vector3[points.Length, points.Length];
        // Fill the last row of vArr with the elements of vList
        int r = points.Length - 1;
        for ( int c = 0; c < points.Length; c++ )
        {
            vArr[r, c] = points[c]; // vList should be replaced with points on the line inside the first for loop (https://book.prototools.net/chapter-32-space-shmup-p2-3e/)
        }

        // Iterate over all remaining rows and interpolate points at each one
        for (r--; r >= 0; r--)
        {
            for (int c = 0; c <= r; c++)
            {
                vArr[r, c] = Vector3.LerpUnclamped(vArr[r + 1, c], vArr[r + 1, c + 1], u);
            }

        }

        // When complete, vArr[0,0] holds the final interpolated value
        return vArr[0, 0];
    }

    //== Materials Functions =======================================================\\
    /// <summary>
    /// Returns a list of all Materials on this GameObject and its children
    /// </summary>
    /// <param name="go">The GameObject on which to search for Renderers</param>
    /// <returns></returns>
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        Material[] mats = new Material[rends.Length];
        for (int i=0; i<rends.Length; i++)
        {
            mats[i] = rends[i].material;
        }

        return mats;
    }

    //== Bounds =======================================================\\
    /// <summary>
    /// Returns a random point from a specified bound
    /// Note: this is a local point within the gameObject.
    /// </summary>
    /// <param name="bounds">The bounds that will be read and set a return value based on it's size</param>
    /// <returns></returns>
    static public Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float minX = bounds.size.x * -0.5f;
        float minY = bounds.size.y * -0.5f;
        float minZ = bounds.size.z * -0.5f;

        return new Vector3(
            Random.Range(minX,-minX), 
            Random.Range(minY,-minY), 
            Random.Range(minZ,-minZ)
            );
    }
}
