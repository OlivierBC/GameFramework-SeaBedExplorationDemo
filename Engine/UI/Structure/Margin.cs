namespace UiIntegration.Engine.UI.Data
{
    internal class Margin
    {
        public readonly (float Left, float Top, float Right, float Bottom) Values;

        public Margin(params float[] values)
        {
            if (values == null) return;

            switch (values.Length)
            {
                case 1:
                    Values.Left = values[0];
                    Values.Top = values[0];
                    Values.Right = values[0];
                    Values.Bottom = values[0];
                    break;
                case 2:
                    Values.Left = values[1];
                    Values.Top = values[0];
                    Values.Right = values[1];
                    Values.Bottom = values[0];
                    break;
                case 3:
                    Values.Left = values[2];
                    Values.Top = values[0];
                    Values.Right = values[2];
                    Values.Bottom = values[1];
                    break;
                case 4:
                    Values.Left = values[2];
                    Values.Top = values[0];
                    Values.Right = values[3];
                    Values.Bottom = values[1];
                    break;
                default:
                    return;
            }
        }
    }
}
