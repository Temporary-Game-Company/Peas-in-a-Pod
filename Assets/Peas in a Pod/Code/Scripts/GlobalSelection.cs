using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSelection : MonoBehaviour
{
    private Selected SelectedUnits;

    private bool dragSelect;

    private RaycastHit hit;

    private Vector3 StartLoc;

    private Vector3 EndLoc;

    private MeshCollider SelectionBox;

    private Mesh SelectionMesh;

    private Vector2[] Corners;
    
    //The vertices of our mesh collider
    private Vector3[] Verts;
    private Vector3[] Vecs;

    private void Start()
    {
        SelectedUnits = GetComponent<Selected>();
        dragSelect = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartLoc = Input.mousePosition;
            dragSelect = false;
        }

        if (Input.GetMouseButton(0))
        {
            if ((StartLoc - Input.mousePosition).magnitude > 20)
            {
                dragSelect = true;
            }
            else
            {
                dragSelect = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!dragSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(StartLoc);
                if (Physics.Raycast(ray, out hit, 50000.0f))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        SelectedUnits.AddSelected(hit.transform.gameObject);
                    }
                    else
                    {
                        SelectedUnits.DeselectAll();
                        SelectedUnits.AddSelected(hit.transform.gameObject);
                    }
                
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                    
                    }
                    else
                    {
                        SelectedUnits.DeselectAll();
                    }
                }
            }
            else
            {
                
                Vecs = new Vector3[4];
                Verts = new Vector3[4];
                int i = 0;
                EndLoc = Input.mousePosition;
                Corners = getBoundingBox(StartLoc, EndLoc);
                foreach (Vector2 corner in Corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);
                    if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
                    {
                        Verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        
                        Vecs[i] = ray.origin - hit.point;
                        Vecs[i].x = 0;
                        Vecs[i].z = 0;


                    }

                    i++;
                }
                
                //Generate Mesh
                
                SelectionMesh = generateSelectionMesh(Verts, Vecs);
                
                

                Vector3 min = SelectionMesh.vertices[2];
                Vector3 max = SelectionMesh.vertices[5];
                Vector3 centre = new Vector3((max.x + min.x) / 2, (max.y + min.y) / 2, (max.x + min.z) / 2);
                Vector3 Extents = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
                Bounds b = new Bounds(centre, Extents);
                
                foreach (GameObject r in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (b.Contains(r.transform.position))
                    {
                        SelectedUnits.AddSelected(r);
                    }
                }
                
                
                //Debug.Log(SelectionBox.bounds.ToString());
                
                

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    SelectedUnits.DeselectAll();
                }
                
                Destroy(SelectionBox, 10.0f);
                dragSelect = false;
            }
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
            {
                SelectedUnits.MoveUnits(hit.point);
            }
        }
        
    }

    private void OnGUI()
    {
        if (dragSelect)
        {
            var rect = DragBox.GetScreenRect(StartLoc, Input.mousePosition);
            DragBox.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            DragBox.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.5f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1,Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(p1, p2);
        var topRight = Vector3.Max(p1, p2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }
    
    
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
            
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        SelectedUnits.AddSelected(other.gameObject);
    }
}
