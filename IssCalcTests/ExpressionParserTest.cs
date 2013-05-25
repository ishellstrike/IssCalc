using System.Collections;
using System.Linq;
using IssCalcCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using IssCalcCore.Tokens;
using System.Collections.Generic;

namespace IssCalcTests
{
    
    
    /// <summary>
    ///Это класс теста для ExpressionParserTest, в котором должны
    ///находиться все модульные тесты ExpressionParserTest
    ///</summary>
    [TestClass()]
    public class ExpressionParserTest
    {
        /// <summary>
        ///Тест для GetTokenizedRPNString
        ///</summary>
        [TestMethod()]
        public void GetTokenizedRPNStringTest()
        {
            string s = "10+20";
            string expected = "10 20 +"; 
            string actual;
            actual = ExpressionParser.GetTokenizedRPNString(s);
            Assert.AreEqual(expected, actual);
        }
    }
}
