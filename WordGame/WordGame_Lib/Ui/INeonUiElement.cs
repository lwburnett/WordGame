using System.Collections.Generic;

namespace WordGame_Lib.Ui
{
    public interface INeonUiElement : IUiElement
    {
        List<PointLight> LightPoints { get; }
    }
}