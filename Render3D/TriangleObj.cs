using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarObjects;

namespace Game1;

#pragma warning disable CA1001
public class TriangleObj : ISolarObject
{
    private readonly ISolarObject _solarObject;
    private readonly Color _color;
    private readonly VertexPositionColor[] _triangleVertices;
    private readonly VertexBuffer _vertexBuffer;

    public TriangleObj(ISolarObject solarObject, GraphicsDevice graphicsDevice, Color color)
    {
        _solarObject = solarObject;
        _color = color;

        _triangleVertices = new VertexPositionColor[3];
        _triangleVertices[0] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(0, 1, 0), _color);
        _triangleVertices[1] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(1, -1, 0), _color);
        _triangleVertices[2] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(-1, -1, 0), _color);

        _vertexBuffer = new VertexBuffer(
            graphicsDevice,
            typeof(VertexPositionColor),
            _triangleVertices.Length,
            BufferUsage.None);

        _vertexBuffer.SetData(_triangleVertices);
    }

    public Vector3 Coordinates => _solarObject.Coordinates;
    public float Mass => _solarObject.Mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        _solarObject.InteractWithAnotherObject(solarObject);
    }

    public void Update()
    {
        _solarObject.Update();

        _triangleVertices[0] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(0, 1, 0), _color);
        _triangleVertices[1] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(1, -1, 0), _color);
        _triangleVertices[2] = new VertexPositionColor(_solarObject.Coordinates + new Vector3(-1, -1, 0), _color);
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
    {
        graphicsDevice.SetVertexBuffer(_vertexBuffer);

        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _triangleVertices, 0, 1);
        }
    }
}