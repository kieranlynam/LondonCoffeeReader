using System;

namespace CoffeeClientPrototype.ViewModel.List
{
    public static class DistanceCalculator
    {
        public static double GetDistanceBetween(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double num = latitude1 * 0.017453292519943295;
            double num2 = longitude1 * 0.017453292519943295;
            double num3 = latitude2 * 0.017453292519943295;
            double num4 = longitude2 * 0.017453292519943295;
            double num5 = num4 - num2;
            double num6 = num3 - num;
            double num7 = Math.Pow(Math.Sin(num6 / 2.0), 2.0) + Math.Cos(num) * Math.Cos(num3) * Math.Pow(Math.Sin(num5 / 2.0), 2.0);
            double num8 = 2.0 * Math.Atan2(Math.Sqrt(num7), Math.Sqrt(1.0 - num7));
            return 6376500.0 * num8;
        }
    }
}
