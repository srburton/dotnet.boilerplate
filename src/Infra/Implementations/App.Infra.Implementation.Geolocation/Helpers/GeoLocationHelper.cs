using System;

namespace App.Infra.Implementation.GeoLocation.Helpers
{
    public class GeoLocationHelper
    {
        public static bool LatitudeIsValid(double number)
            => (number <= 90 && number >= -90);

        public static bool LongitudeIsValid(double number)
            => (number <= 180 && number >= -180);

        public static double ConvertDegreesToRadians(double degrees)
            => (Math.PI / 180) * degrees;

        public static double ConvertRadiansToDegrees(double radian)
            => radian * (180.0 / Math.PI);
    }
}
