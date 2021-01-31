using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VectorGraphics;

[ExecuteInEditMode]
public class Spline2 : MonoBehaviour
{
    public List<Transform> points;

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
            Contours = new BezierContour[]{ new BezierContour() { Segments = new BezierPathSegment[4], Closed = true } },
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

        // Update the control points of the spline.
        m_Path.Contours[0].Segments[0].P0 = points[0].localPosition;
        m_Path.Contours[0].Segments[1].P0 = points[1].localPosition;
        m_Path.Contours[0].Segments[2].P0 = points[2].localPosition;
        m_Path.Contours[0].Segments[3].P0 = points[3].localPosition;

        float perSegment = 2 * Mathf.PI / points.Count;

        float radius = 1.145F;

        float angle1 = 0.33F * perSegment;
        float angle2 = 0.66F * perSegment;

        for (int i = 0; i < points.Count; i++)
        {
            
        }

        m_Path.Contours[0].Segments[0].P1 = InterpolatePoints(radius, angle1);
        m_Path.Contours[0].Segments[1].P1 = InterpolatePoints(radius, angle1 + perSegment);
        m_Path.Contours[0].Segments[2].P1 = InterpolatePoints(radius, angle1 + perSegment * 2);
        m_Path.Contours[0].Segments[3].P1 = InterpolatePoints(radius, angle1 + perSegment * 3);


        m_Path.Contours[0].Segments[0].P2 = InterpolatePoints(radius, angle2);
        m_Path.Contours[0].Segments[1].P2 = InterpolatePoints(radius, angle2 + perSegment);
        m_Path.Contours[0].Segments[2].P2 = InterpolatePoints(radius, angle2 + perSegment * 2);
        m_Path.Contours[0].Segments[3].P2 = InterpolatePoints(radius, angle2 + perSegment * 3);

        m_Path.PathProps = Random.value > 0.5F ? props1 : props2;

        // controlPoints[3].localPosition, controlPoints[0].localPosition

        //m_Path.Contours[0].Segments[3].P2 = controlPoints[11].localPosition;

        // Tessellate the vector scene, and fill the mesh with the resulting geometry.
        var geoms = VectorUtils.TessellateScene(m_Scene, m_Options);
        VectorUtils.FillMesh(m_Mesh, geoms, 1.0f);
    }

    Vector3 InterpolatePoints(float radius, float angle)
    {
        // what the fuck
        Vector3 origin = transform.position;

        float magic = 0.552284749831F;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return new Vector3(x, y, 0);
    }
}
