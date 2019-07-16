using System;

namespace Karmr.Common.Types
{
    public struct GeoLocation : IEquatable<GeoLocation>
    {
        public decimal Latitude { get; }

        public decimal Longitude { get; }

        public GeoLocation(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), $"Latitude ({latitude}) must be between -90 and 90");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), $"Longitude ({longitude}) must be between -180 and 180");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"{Latitude},{Longitude}";
        }

        public bool Equals(GeoLocation other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override bool Equals(object obj)
        {
            return obj is GeoLocation other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }

        public static bool operator ==(GeoLocation left, GeoLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoLocation left, GeoLocation right)
        {
            return !left.Equals(right);
        }
    }
}