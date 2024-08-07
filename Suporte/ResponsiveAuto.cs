namespace AppEmpresa.Suporte
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

        public static double FontSize(double baseFontSize)
        {
            try
            {
                double screenHeight = DeviceDisplay.MainDisplayInfo.Height;
                double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
                double screenSize = Math.Sqrt(screenHeight * screenWidth); // Média geométrica

                double responsiveFontSize = baseFontSize * (screenSize / 1000); // Ajuste proporcional

                return responsiveFontSize;
            }
            catch (Exception)
            {
                return baseFontSize;
            }
        }
    }
}
