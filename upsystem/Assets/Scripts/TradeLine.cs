using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TradeLine : MonoBehaviour {
    LineRenderer lineRenderer;
    List<GameObject> objs = new List<GameObject>();
    public Material material;
    public float width;
    Vector3 lastPos = Vector3.zero;
    bool savePosition = false;
	// Use this for initialization
	void Start () 
    {
        lineRenderer = this.GetComponent<LineRenderer>() as LineRenderer;
        GameStateManager.Instance.TurnEnded += ClearAllLines;
    }
	
    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
        Vector3[] points = new Vector3[0];
        lineRenderer.SetPositions(points);
    }

    public void MakePermLine()
    {
        GameObject obj = new GameObject();
        objs.Add(obj);
        LineRenderer lineRenderer = obj.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = material;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = 2;
        lineRenderer.sortingLayerName = "Bear";

        Vector3 worldPos = lastPos;
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = GameStateManager.Instance.transferShip2.gameObject.transform.position;
        points[1] = worldPos;

        Material mat = lineRenderer.material;
        mat.SetFloat("_Theta", Bearing(points[0].x, points[0].y, points[1].x, points[1].y));
        Color color = new Color(0.8867f, 0.7697f, 0.3388f, 0.6f);
        mat.SetColor("_Color", color);
        lineRenderer.SetPositions(points);
    }

    void ClearAllLines()
    {
        foreach(GameObject obj in objs)
        {
            Destroy(obj);
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if(GameStateManager.Instance.GetState() == GameStateManager.GameState.transfer)
        {
            savePosition = true;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos = new Vector3(worldPos.x, worldPos.y, 0);
            lineRenderer.positionCount = 2;
            Vector3[] points = new Vector3[2];
            points[0] = GameStateManager.Instance.transferShip2.gameObject.transform.position;
            points[1] = worldPos;
            Material mat = lineRenderer.material;
            mat.SetFloat("_Theta", Bearing(points[0].x, points[0].y, points[1].x, points[1].y));
            lineRenderer.SetPositions(points);
        }
        else if(savePosition)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos = new Vector3(worldPos.x, worldPos.y, 0);
            lastPos = worldPos;
            savePosition = false;
        }
    }

    // Computes the bearing in degrees from the point A(a1,a2) to
    // the point B(b1,b2).
    float Bearing(float a1, float a2, float b1, float b2)
    {
        const float TWOPI = 6.283185307f;
        if (a1 == b1 && a2 == b2)
        {
            return 0;
        }
        float theta = Mathf.Atan2(b1 - a1, a2 - b2);
        theta += 1.570796f;
        if (theta < 0.0)
            theta += TWOPI;
        return theta;
    }
}
