using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VectorGraphics;

[ExecuteInEditMode]
public class Spline2 : MonoBehaviour
{
    public List<Transform> points;

    public int numberOfPoints = 8;

    private Scene m_Scene;
    private Shape m_Path;
    private VectorUtils.TessellationOptions m_Options;
    private Mesh m_Mesh;

    private Color highlighterBlue = new Color(0, 1, 1, 0.5F);
    private Color highlighterRed = new Color(1, 0, 0, 0.5F);

    private PathProperties props1;
    private PathProperties props2;

    void Start()
    {
        props1 = new PathProperties()
        {
            Stroke = new Stroke() { Color = highlighterBlue, HalfThickness = 0.1f }
        };

        props2 = new PathProperties()
        {
            Stroke = new Stroke() { Color = highlighterRed, HalfThickness = 0.1f }
        };

        // Prepare the vector path, add it to the vector scene.
        m_Path = new Shape() {
            Contours = new BezierContour[]{ new BezierContour() { Segments = new BezierPathSegment[numberOfPoints], Closed = true } },
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

        float perSegment = 2 * Mathf.PI / points.Count;

        float radius = 1.03F;

        float angle1 = 0.33F * perSegment;
        float angle2 = 0.66F * perSegment;

        for (int i = 0; i < points.Count; i++)
        {
            m_Path.Contours[0].Segments[i].P0 = points[i].localPosition;
            m_Path.Contours[0].Segments[i].P1 = InterpolatePoints(radius, angle1 + perSegment * i);
            m_Path.Contours[0].Segments[i].P2 = InterpolatePoints(radius, angle2 + perSegment * i);
        }

        m_Path.PathProps = Random.value > 0.5F ? props1 : props2;

        // Tessellate the vector scene, and fill the mesh with the resulting geometry.
        var geoms = VectorUtils.TessellateScene(m_Scene, m_Options);
        VectorUtils.FillMesh(m_Mesh, geoms, 1.0f);
    }

    Vector3 InterpolatePoints(float radius, float angle)
    {
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return new Vector3(x, y, 0);
    }
}
