using SimpleSolitare;

namespace SimpleSolitareTests
{
    public class RngFactoryTests
    {
        [Fact]
        public void When_getint_called_with_defaults_then_return_random_number()
        {
            var sut = new RngFactory();

            var result = sut.GetInt();

            result.Should().BeInRange(0, int.MaxValue);
        }

        [Fact]
        public void When_getint_called_with_specified_min_and_default_max_then_return_random_number()
        {
            var sut = new RngFactory();

            var result = sut.GetInt(0, 100);

            result.Should().BeInRange(0, 100);
        }
    }
}