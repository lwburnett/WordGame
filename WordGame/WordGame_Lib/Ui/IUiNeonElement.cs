using System.Collections.Generic;

namespace WordGame_Lib.Ui
{
    public interface IUiNeonElement : IUiElement
    {
        List<PointLight> LightPoints { get; }

        // bool IsOn { get; }
        // void TurnOn();
        // void TurnOff();
    }
}