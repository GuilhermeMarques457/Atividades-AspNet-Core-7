namespace Tests
{
    public class MyMathTest
    {
        [Fact]
        public void Test1()
        {

            //Arange
            MyMath myMath = new MyMath();
            int input1 = 10, input2 = 20;
            int expected = 30;

            //Act
            int actual = myMath.Add(input1, input2);

            //Assert
            Assert.Equal(expected, actual);

        }
    }
}