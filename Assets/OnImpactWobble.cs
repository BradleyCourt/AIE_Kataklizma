using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnImpactWobble : MonoBehaviour {

    public bool MeshModifierLoop = false;
    Mesh mesh;
    public int vertCount = 0;
    Vector3[] vertices;
    private Vector3[] vertOriginalCoordinates;

    Vector2 ScaleSet = new Vector2(0.1f, 8f);
    public Vector2 Scale = new Vector2(0f, 0f);
    public bool restoreVariable;
    // Use this for initialization
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    void Start ()
    {
        Scale = ScaleSet;
        vertices = mesh.vertices;
        //vertOriginalCoordinates = vertices;
        vertOriginalCoordinates = new Vector3[vertices.Length];
        for(int i = 0; i < vertices.Length;)
        {
            vertOriginalCoordinates[i] = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            
            i++; 
        }
    }
	
    /// <summary>
    /// Requires access to a global variable for maximum damage possible, and makes a calculation of input damage and maxDamage to get a value between 0-1
    /// When it recives information on these variables, the direction is calculated from the player to the hitPoint to get the direction.
    /// The direction then recalculates the wobble and the building wobbles according to force and direction.
    /// </summary>
    /// <param name="direction">The direction from which the impact occured.</param>
    /// <param name="damage">The hit power the building is struct by.</param>
    public void WobbleImpactPoint(Vector3 direction, float damage)
    {
        maxMoveDistance = 0; //Snap when timer runs out depending on the distance it can move variable maxMoveDistance
    }
    public Vector3 hitDirection = new Vector3(1, 0, 1);
    public float maxMoveDistance = 0;
    // Update is called once per frame
    public float hitDuration = 0.1f;
    float swap;
	void Update () {
        if(MeshModifierLoop)
        {
            swap = Time.time + hitDuration;
            MeshModifierLoop = false;
        }
        if (Time.time < swap)
        {
            Vector3 aVert;
            float angle;
            while(vertCount < vertices.Length)
            {
                aVert = vertices[vertCount];
                angle = Scale.x * vertOriginalCoordinates[vertCount].y + Time.time * Scale.y;
                vertices[vertCount] = vertOriginalCoordinates[vertCount] + Mathf.Sin(angle) * hitDirection/*Vector3.right*/;
                float newMove = Vector3.Distance(aVert, vertices[vertCount]);
                if(newMove > maxMoveDistance)
                {
                    maxMoveDistance = newMove;
                }
                vertCount++;
            }
            vertCount = 0;
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
        else
        {
            bool returnVariable = true;
            MeshModifierLoop = false;
            vertCount = 0;


            float angle;
            while (vertCount < vertices.Length)
            {
                if (vertices[vertCount] != vertOriginalCoordinates[vertCount])
                {
                    float vertCompair = Vector3.Distance(vertices[vertCount], vertOriginalCoordinates[vertCount]);
                    if (vertCompair < maxMoveDistance / 5)
                    {
                        vertices[vertCount] = vertOriginalCoordinates[vertCount];
                    }
                    else
                    {
                    returnVariable = false;
                    angle = Scale.x * vertOriginalCoordinates[vertCount].y + Time.time * Scale.y;
                    vertices[vertCount] = vertOriginalCoordinates[vertCount] + Mathf.Sin(angle) * hitDirection;
                    }


                }
                vertCount++;
            }
            vertCount = 0;
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            if (returnVariable)
            {
                restoreVariable = false;
                maxMoveDistance = 0;
            }
        }

    }


}
