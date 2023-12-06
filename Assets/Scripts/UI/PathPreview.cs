using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PathPreview : PoolableObject
{
    [SerializeField] ComponentObject gridObject;
    LineRenderer lineRenderer;

    [SerializeField] private float drawTime;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Reset()
    {
        base.Reset();
        lineRenderer.positionCount = 0;
    }

    public async Task SetPath(List<Vector2Int> points, CancellationToken token)
    {
        Grid grid = (Grid)gridObject.value;
        transform.position = grid.GetCellCenterLocal((Vector3Int)points[points.Count - 1]);
        
        Vector3[] linePoints = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            linePoints[i] = grid.GetCellCenterLocal((Vector3Int)points[i]);
        }

        linePoints[^1] = Vector3.Lerp(linePoints[^2], linePoints[^1], 0.5f);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, linePoints[0]);

        for (int i = 1; i < points.Count; i++)
        {
            lineRenderer.positionCount = i + 1;
            float timer = 0;
            float timeToNextPoint = drawTime / points.Count;

            while (timer < timeToNextPoint)
            {
                timer += Time.deltaTime;
                lineRenderer.SetPosition(i, Vector3.Lerp(linePoints[i - 1], linePoints[i], timer / timeToNextPoint));
                await Task.Yield();

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            }

            lineRenderer.SetPosition(i, linePoints[i]);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(linePoints);
    }
}
