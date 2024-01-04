using System.Security.Cryptography;

namespace SimpleSolitare
{
    public interface IRngFactory
    {
        int GetInt(int min = 0, int max = int.MaxValue);

        long GetLong(long min = 0L, long max = long.MaxValue);
    }

    public class RngFactory : IRngFactory
    {
        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private readonly byte[] _data = new byte[8]; // Accommodate 64 bits

        public int GetInt(int min = 0, int max = int.MaxValue)
        {
            if (min < 0)
            {
                throw new ArgumentException("The minimum value must be greater than or equal to zero.", nameof(min));
            }

            if (min >= max)
            {
                throw new ArgumentException("The minimum value cannot be greater than or equal to the maximum value.");
            }

            _rng.GetBytes(_data);

            var result = 0;

            for (var i = 0; i < 4; i++)
            {
                result <<= 8;
                result |= _data[i];
            }

            // Make sure result is greater than or equal to zero.
            result &= 0xEFFF;

            return (result + min) % max;
        }

        public long GetLong(long min = 0L, long max = long.MaxValue)
        {
            if (min < 0)
            {
                throw new ArgumentException("The minimum value must be greater than or equal to zero.", nameof(min));
            }

            if (min >= max)
            {
                throw new ArgumentException("The minimum value cannot be greater than or equal to the maximum value.");
            }

            _rng.GetBytes(_data);

            var result = 0L;

            for (var i = 0; i < 8; i++)
            {
                result <<= 8;
                result |= _data[i];
            }

            // Make sure result is greater than or equal to zero.
            result &= 0xEFFFFFFF;

            return (result + min) % max;
        }
    }
}
