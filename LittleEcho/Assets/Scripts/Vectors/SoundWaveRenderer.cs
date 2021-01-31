using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VectorGraphics;

public class SoundWaveRenderer : MonoBehaviour
{
    public Transform[] controlPoints;

    private Scene m_Scene;
    private Shape m_Path;
    private VectorUtils.TessellationOptions m_Options;
    private Mesh m_Mesh;

    private Color highlighterBlue = new Color(0, 1, 1, 0.5F);

    private SoundWave parent;

    void Start()
    {
        parent = GetComponent<SoundWave>();

        // Prepare the vector path, add it to the vector scene.
        m_Path = new Shape() {
            Contours = new BezierContour[]{ new BezierContour() { Segments = new BezierPathSegment[2] } },
            PathProps = new PathProperties() {
                Stroke = new Stroke() { Color = highlighterBlue, HalfThickness = 0.1f }
            }
        };

        m_Scene = new Scene() {
            Root = new SceneNode() { Shapes = new List<Shape> { m_Path } }
        };

        m_Options = new VectorUtils.TessellationOptions() {
            StepDistance = 1000.0f,
            MaxCordDeviation = 0.05f,
            MaxTanAngleDeviation = 0.05f,
            SamplingStepSize = 0.01f
        };

        // Instantiate a new mesh, it will be filled with data in Update()
        m_Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = m_Mesh;
    }

    void Update()
    {
        if (m_Scene == null)
            Start();

        IteratePoints();

        // Tessellate the vector scene, and fill the mesh with the resulting geometry.
        var geoms = VectorUtils.TessellateScene(m_Scene, m_Options);
        VectorUtils.FillMesh(m_Mesh, geoms, 1.0f);
    }

    void IteratePoints()
    {
        // Update the control points of the spline.
        m_Path.Contours[0].Segments[0].P0 = controlPoints[0].localPosition;
        m_Path.Contours[0].Segments[0].P1 = controlPoints[1].localPosition;
        m_Path.Contours[0].Segments[0].P2 = controlPoints[2].localPosition;
        m_Path.Contours[0].Segments[1].P0 = controlPoints[3].localPosition;

        //// Draw edge
        //if (points.Count > 1)
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        Vector3 dir = -(i < points.Count - 1 ? points[i].position - points[i + 1].position : points[i].position - points[0].position);

        //        float arcLength = 2 * Mathf.PI * (timeAlive * SoundWave.WAVE_SPEED) / parent.GetNumberOfPoints();
        //        bool stopped = points[i].stopped || (i < points.Count - 1 ? points[i + 1].stopped : points[0].stopped);
        //        bool shortEnough = dir.magnitude <= arcLength + 0.1F || (stopped && dir.magnitude <= 1);

        //        //Color color = iteration == 0 ? Color.yellow : iteration == 1 ? Color.red : iteration == 2 ? Color.magenta : Color.blue;

        //        RaycastHit2D hit = Physics2D.Raycast(points[i].position, dir, dir.magnitude, checkMask);
        //        if (shortEnough && hit.collider == null)
        //        {
        //            Debug.DrawRay(points[i].position, dir, color, deltaTime);
        //        }
        //    }
        //}
    }
}
