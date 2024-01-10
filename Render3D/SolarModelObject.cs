using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarObjects;

namespace Game1;

public class SolarModelObject : ISolarObject
{
    private readonly ISolarObject _solarObject;
    private readonly Model _model;

    public SolarModelObject(ISolarObject solarObject, Model model)
    {
        _solarObject = solarObject;
        _model = model;
    }

    public Vector3 Coordinates => _solarObject.Coordinates;
    public int Mass => _solarObject.Mass;

    public void InteractWithAnotherObject(ISolarObject solarObject)
    {
        _solarObject.InteractWithAnotherObject(solarObject);
    }

    public void Update()
    {
        _solarObject.Update();
    }

    public void Draw(Matrix viewMatrix, Matrix worldMatrix, Matrix projectionMatrix)
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.View = viewMatrix;
                effect.World = worldMatrix * Matrix.CreateTranslation(Coordinates);
                effect.Projection = projectionMatrix;
            }

            mesh.Draw();
        }
    }
}