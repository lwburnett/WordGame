using System.Collections.Generic;

namespace WordGame_Lib.Ui
{
    public interface ILightSource
    {
        List<PointLight> LightPoints { get; }
    }
}