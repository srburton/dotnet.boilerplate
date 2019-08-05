using System;
using Helper = App.Infra.Implementation.GeoLocation.Helpers.GeoLocationHelper;

namespace App.Infra.Implementation.GeoLocation
{
    /**
     * Base Article
     * http://janmatuschek.de/LatitudeLongitudeBoundingCoordinates
     * 
     * Represents a point on the surface of a sphere. (The Earth is almost
     * spherical.)
     *
     * This code was originally published at
     * http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates#Java
     * 
     * @author Jan Philip Matuschek
     * @version 22 September 2010
     * @converted to C# by Anthony Zigenbine on 19th October 2010
     * 
     * This code was originally published at
     * https://github.com/anthonyvscode/LonelySharp/blob/master/LonelySharp/GeoLocation.cs    
     */
    public class GeoLocation
    {
        private double _radLat;  // latitude in radians
        private double _radLon;  // longitude in radians

        private double _degLat;  // latitude in degrees
        private double _degLon;  // longitude in degrees

        private static double MIN_LAT = Helper.ConvertDegreesToRadians(-90d);  // -PI/2
        private static double MAX_LAT = Helper.ConvertDegreesToRadians(90d);   //  PI/2
        private static double MIN_LON = Helper.ConvertDegreesToRadians(-180d); // -PI
        private static double MAX_LON = Helper.ConvertDegreesToRadians(180d);  //  PI

        private const double EARTH_RADIUS = 6371.01;

        private GeoLocation()
        {
        }

        /// <summary>
        /// Return GeoLocation from Degrees
        /// </summary>
        /// <param name="latitude">The latitude, in degrees.</param>
        /// <param name="longitude">The longitude, in degrees.</param>
        /// <returns>GeoLocation in Degrees</returns>
        public static GeoLocation FromDegrees(double latitude, double longitude)
        {
            GeoLocation result = new GeoLocation
            {
                _radLat = Helper.ConvertDegreesToRadians(latitude),
                _radLon = Helper.ConvertDegreesToRadians(longitude),
                _degLat = latitude,
                _degLon = longitude
            };
            result.CheckBounds();
            return result;
        }

        /// <summary>
        /// Return GeoLocation from Radians
        /// </summary>
        /// <param name="latitude">The latitude, in radians.</param>
        /// <param name="longitude">The longitude, in radians.</param>
        /// <returns>GeoLocation in Radians</returns>
        public static GeoLocation FromRadians(double latitude, double longitude)
        {
            GeoLocation result = new GeoLocation
            {
                _radLat = latitude,
                _radLon = longitude,
                _degLat = Helper.ConvertRadiansToDegrees(latitude),
                _degLon = Helper.ConvertRadiansToDegrees(longitude)
            };

            result.CheckBounds();
            return result;
        }

        private void CheckBounds()
        {
            if (_radLat < MIN_LAT || _radLat > MAX_LAT || _radLon < MIN_LON || _radLon > MAX_LON)
                throw new Exception("Arguments are out of bounds");
        }

        /// <summary>
        /// the latitude, in degrees.
        /// </summary>
        /// <returns></returns>
        public double getLatitudeInDegrees()
            => _degLat;

        /// <summary>
        /// the longitude, in degrees.
        /// </summary>
        /// <returns></returns>
        public double getLongitudeInDegrees()
            => _degLon;

        /// <summary>
        /// the latitude, in radians.
        /// </summary>
        /// <returns></returns>
        public double getLatitudeInRadians()
           => _radLat;

        /// <summary>
        ///  the longitude, in radians.
        /// </summary>
        /// <returns></returns>
        public double getLongitudeInRadians()
            => _radLon;

        /// <summary>
        /// Computes the great circle distance between this GeoLocation instance and the location argument.
        /// </summary>
        /// <param name="location">Location to act as the centre point</param>
        /// <returns>the distance, measured in the same unit as the radius argument.</returns>
        public double DistanceTo(GeoLocation location)
        {
            return Math.Acos(Math.Sin(_radLat) * Math.Sin(location._radLat) +
                    Math.Cos(_radLat) * Math.Cos(location._radLat) *
                    Math.Cos(_radLon - location._radLon)) * EARTH_RADIUS;
        }
       
        /// <summary>
        /// Computes the bounding coordinates of all points on the surface
        /// of a sphere that have a great circle distance to the point represented
        /// by this GeoLocation instance that is less or equal to the distance
        /// argument.
        /// For more information about the formulae used in this method visit
        /// http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates
        /// </summary>
        /// <param name="distance">The distance from the point represented by this 
        /// GeoLocation instance. Must me measured in the same unit as the radius argument.
        /// </param>
        /// <returns>An array of two GeoLocation objects such that:
        /// 
        /// a) The latitude of any point within the specified distance is greater
        /// or equal to the latitude of the first array element and smaller or
        /// equal to the latitude of the second array element.
        /// If the longitude of the first array element is smaller or equal to
        /// the longitude of the second element, then
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element and smaller or
        /// equal to the longitude of the second array element.
        /// 
        /// b) If the longitude of the first array element is greater than the
        /// longitude of the second element (this is the case if the 180th
        /// meridian is within the distance), then
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element
        /// or smaller or equal to the longitude of the second
        /// array element.</returns>
        public GeoLocation[] BoundingCoordinates(double distance)
        {
            if (distance < 0d)
                throw new Exception("Distance cannot be less than 0");

            // angular distance in radians on a great circle
            double radDist = distance / EARTH_RADIUS;

            double minLat = _radLat - radDist;
            double maxLat = _radLat + radDist;

            double minLon, maxLon;
            if (minLat > MIN_LAT && maxLat < MAX_LAT)
            {
                double deltaLon = Math.Asin(Math.Sin(radDist) /
                    Math.Cos(_radLat));
                minLon = _radLon - deltaLon;
                if (minLon < MIN_LON) minLon += 2d * Math.PI;
                maxLon = _radLon + deltaLon;
                if (maxLon > MAX_LON) maxLon -= 2d * Math.PI;
            }
            else
            {
                // a pole is within the distance
                minLat = Math.Max(minLat, MIN_LAT);
                maxLat = Math.Min(maxLat, MAX_LAT);
                minLon = MIN_LON;
                maxLon = MAX_LON;
            }

            return new GeoLocation[]
            {               
               FromRadians(minLat, minLon),
               FromRadians(maxLat, maxLon)
            };
        }

        public override string ToString()
           => $"({_degLat}\u00B0{_degLon}\u00B0) = ({_radLat} rad, {_radLon} rad)";
    }
}
