using CaveTester.Core;
using FluentAssertions;
using Xunit;

namespace CaveTester.Tests
{
    public class IdGeneratorTests
    {
        private readonly IdGenerator _generator = new IdGenerator();

        [Fact]
        public void ShouldGenerateSequentialIds()
        {
            _generator.New<int>().Should().Be(1);
            _generator.New<int>().Should().Be(2);
        }

        [Fact]
        public void ShouldGenerateIdsForMultipleTypes()
        {
            _generator.New<int>().Should().Be(1);
            _generator.New<float>().Should().Be(1);
            _generator.New<int>().Should().Be(2);
            _generator.New<float>().Should().Be(2);
        }

        [Fact]
        public void ShouldGenerateNonDefaultStartValue()
        {
            var generator = new IdGenerator(314);

            generator.New<int>().Should().Be(314);
            generator.New<int>().Should().Be(315);
        }

        [Fact]
        public void ShouldResetSequence()
        {
            _generator.New<int>().Should().Be(1);
            _generator.Reset<int>();
            _generator.New<int>().Should().Be(1);
        }

        [Fact]
        public void ShouldResetAllSequences()
        {
            _generator.New<int>().Should().Be(1);
            _generator.New<float>().Should().Be(1);
            _generator.Reset();
            _generator.New<int>().Should().Be(1);
            _generator.New<float>().Should().Be(1);
        }
    }
}