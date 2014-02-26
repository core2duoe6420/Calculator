using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.AlexKing.Calculator.Core;

namespace Net.AlexKing.Calculator.Test
{
    [TestClass]
    public class NormalCalculateUnitTest
    {
        [TestMethod]
        public void TestDoCalculation() {
            testExpression("0+0", 0);
            testExpression("1+1", 2);
            testExpression("1+4", 5);
            testExpression("4+1", 5);
            testExpression("40000+0.001", 40000.001);
            testExpression("0.001+40000", 40000.001);
            testExpression("2-3", -1);
            testExpression("2-3", -1);
            testExpression("3-2", 1);
            testExpression("40000-0.001", 39999.999);
            testExpression("0.001-40000", -39999.999);
            testExpression("2*3", 6);
            testExpression("2*3", 6);
            testExpression("-2*3", -6);
            testExpression("2*-3", -6);
            testExpression("-2*-3", 6);
            testExpression("6/3", 2);
            testExpression("6/3", 2);
            testExpression("1/2", 0.5);
            testExpression("-6/3", -2);
            testExpression("6/-3", -2);
            testExpression("-6/-3", 2);
            testExpression("(-3)/(-6)", 0.5);
            testExpression("2/2", 1);
            testExpression("1203/1", 1203);
            testExpression("-0/32352.689", 0);
            testExpression("1/4", 0.25);
            testExpression("1/3", 1.0 / 3);
            testExpression("2/3", 2.0 / 3);
            testExpression("1/0", Double.PositiveInfinity);
            testExpression("0/0", Double.NaN);

            /* Precision */
            testExpression("1000000000000000-1000000000000000", 0);
            testExpression("1000000000000000/1000000000000000", 1);
            testExpression("1000000000000000*0.000000000000001", 1);

            /* Order of operations */
            //testExpression("1-0.9-0.1", 0);
            testExpression("1+2*3", 7);
            testExpression("1+(2*3)", 7);
            testExpression("(1+2)*3", 9);
            testExpression("(1+2*3)", 7);
            testExpression("2*(1+1)", 4);
            testExpression("4/2*(1+1)", 4);

            /* Percentage 
            testExpression("100%", 1);
            testExpression("1%", 0.01);
            testExpression("100+1%", 101);
            testExpression("100-1%", 99);
            testExpression("100*1%", 1);
            testExpression("100/1%", 10000);
	
            */
            /* Factorial */
            testExpression("0!", 1);
            testExpression("1!", 1);
            testExpression("5!", 120);
            //testExpression("69!", 171122452428141311372468338881272839092270544893520369393648040923257279754140647424000000000000000);
            testExpression("-1!", -1);
            //testExpression("(-1)!", );
            testExpression("-(1!)", -1);

            /* Powers */
            testExpression("2^2", 4);
            testExpression("2^3", 8);
            testExpression("2^10", 1024);
            testExpression("(1+2)^2", 9);
            testExpression("abs(1-3)^2", 4);
            testExpression("0^0", 1);
            testExpression("0^0.5", 0);
            testExpression("2^0", 1);
            testExpression("2^1", 2);
            testExpression("2^2", 4);
            testExpression("2^-1", 0.5);
            testExpression("2^(-1)", 0.5);
            testExpression("-10^2", -100);
            testExpression("(-10)^2", 100);
            testExpression("-(10^2)", -100);
            //testExpression("4^3^2", 262144);
            testExpression("4^(3^2)", 262144);
            testExpression("(4^3)^2", 4096);
            testExpression("sqrt(4)", 2);
            testExpression("sqrt(2)", Math.Sqrt(2));
            testExpression("4^0.5", 2);
            testExpression("2^0.5", Math.Sqrt(2));
            //testExpression("(-8)^(1/3)", -2);
            
            testExpression("mod(0, 7)", 0);
            testExpression("mod(6, 7)", 6);
            testExpression("mod(7, 7)", 0);
            testExpression("mod(8, 7)", 1);
            //testExpression("mod(-1, 7)", 6);

            testExpression("abs(1)", 1);
            testExpression("abs(-1)", 1);

            testExpression("log(10,1)", 0);
            testExpression("log(10,2)", Math.Log10(2));
            testExpression("log(10,10)", 1);
            testExpression("log(2,2)", 1);
            testExpression("2*log(10,2)", 2 * Math.Log10(2));

            testExpression("ln(1)", 0);
            testExpression("ln(2)", Math.Log(2));
            testExpression("ln(e)", 1);
            testExpression("2*ln(2)", Math.Log(2) * 2);

            testExpression("sind(0)", 0);
            testExpression("sind(45) - 1/sqrt(2)", 0);
            testExpression("sind(20) + sind(-20)", 0);
            testExpression("sind(90)", 1);
            //testExpression("sind(180)", 0);
            testExpression("2*sind(90)", 2);
            testExpression("sind(45)^2", 0.5);

            testExpression("cosd(0)", 1);
            //testExpression("cosd(45) - 1/sqrt(2)", 0);
            testExpression("cosd(20) - cosd(-20)", 0);
            //testExpression("cosd(90)", 0);
            testExpression("cosd(180)", -1);
            testExpression("2*cosd(0)", 2);
            testExpression("cosd(45)^2", 0.5);

            testExpression("tand(0)", 0);
            testExpression("tand(10) - sind(10)/cosd(10)", 0);
            //testExpression("tand(90)", Double.PositiveInfinity);
            testExpression("tand(10)", Math.Tan(10 * Math.PI / 180));
            testExpression("tand(10)^2", Math.Pow(Math.Tan(10 * Math.PI / 180), 2));

            testExpression("arccosd(0)", 90);
            testExpression("arccosd(1)", 0);
            testExpression("arccosd(-1)", 180);
            testExpression("arccosd(1/sqrt(2))", 45);

            testExpression("arcsind(0)", 0);
            testExpression("arcsind(1)", 90);
            testExpression("arcsind(-1)", -90);
            testExpression("arcsind(1/sqrt(2))", 45);

            testExpression("sin(pi/2)", 1);
        }

        private void testExpression(string exp, double value) {
            Calculate cal = new Calculate(exp);
            Assert.AreEqual(cal.DoCalculation().ToString(), value.ToString());
        }
    }
}
