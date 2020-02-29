using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    public float radius = 2;
    public float width = 0.1f;

    [Range(0, 360)]
    public float arc = 90;

    // Arc is created from this point on counterclockwise
    [Range(0, 360)]
    public float orientation = 0;

    // Amount of vertices used for a circle 
    public int resolution = 360;
    public int collisionResolution = 10;

    private PolygonCollider2D polygonCollider;
    private Mesh mesh;

    // Start is called before the first frame update
    void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;        
    }

    // Update is called once per frame
    void Update()
    {
        int resolutionScaled = Mathf.Max(3, (int)(resolution / (360f / arc)));
        float radOrientation = orientation * Mathf.Deg2Rad;
        float arcResolution = (arc * Mathf.Deg2Rad) / (resolutionScaled - 1);

        // Vertices
        Vector3[] v = new Vector3[resolutionScaled * 2];
        for (int i = 0; i < resolutionScaled; i++)
        {
            float circlex = Mathf.Cos(radOrientation + arcResolution * i);
            float circley = Mathf.Sin(radOrientation + arcResolution * i);

            v[i*2] = new Vector3(circlex * radius, circley * radius, 0);
            v[i*2 + 1] = new Vector3(circlex * (radius - width), circley * (radius - width), 0);
        }

        // Triangles
        List<int> tri = new List<int>();
        int triangles = (resolutionScaled-1)*2;
        
        for (int i = 0; i < triangles; i+=2)
        {
            tri.Add(i);
            tri.Add(i+1);
            tri.Add(i+2);
            tri.Add(i+2);
            tri.Add(i+1);
            tri.Add(i+3);
        }

        // Normals (display)
        Vector3[] normals = new Vector3[v.Length];
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }

        // UVs (textures)  
        // not done yet

        // Set mesh values
        mesh.Clear();
        mesh.vertices = v;
        mesh.triangles = tri.ToArray();
        mesh.normals = normals;

        // Set Poligon collider 
        int colliderResolutionScaled = Mathf.Max(3, (int)(collisionResolution / (360f / arc)));
        float colliderArcResolution = (arc * Mathf.Deg2Rad) / (colliderResolutionScaled - 1);

        Vector2[] polygon = new Vector2[colliderResolutionScaled * 2];
        int lastPolygonIdx = polygon.Length - 1;
        for (int i = 0; i < colliderResolutionScaled; i++)
        {
            float circlex = Mathf.Cos(radOrientation + colliderArcResolution * i);
            float circley = Mathf.Sin(radOrientation + colliderArcResolution * i);

            polygon[i] = new Vector2(circlex * radius, circley * radius);
            polygon[lastPolygonIdx - i] = new Vector2(circlex * (radius - width), circley * (radius - width));
        }

        polygonCollider.SetPath(0, polygon);

    }
}
