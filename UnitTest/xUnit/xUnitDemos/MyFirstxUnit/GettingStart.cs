using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFirstxUnit
{
    public class GettingStart
    {
        /// Facts are tests which are always true. They test invariant conditions.
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        /// Theories are tests which are only true for a particular set of data.
        /// Although we've only written 3 test methods, the console runner actually ran 5 tests; 
        /// that's because each theory with its data set is a separate test. 
        /// Note also that the runner tells you exactly which set of data failed, because it includes the parameter values in the name of the test.
        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
