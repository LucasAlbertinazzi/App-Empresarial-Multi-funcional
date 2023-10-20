namespace AppMarciusMagazine.Suporte
{
    public static class ResponsiveAuto
    {
        public static double Height(double valor)
        {
            try
            {
                double screen = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
                double iconSize = screen / valor;

                return iconSize;
            }
            catch (Exception)
            {
                return valor;
            }
        }

        public static double Width(double valor)
        {
            try
            {
                double screen = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
                double iconSize = screen / valor;

                return iconSize;
            }
            catch (Exception)
            {
                return valor;
            }
        }
    }
}
