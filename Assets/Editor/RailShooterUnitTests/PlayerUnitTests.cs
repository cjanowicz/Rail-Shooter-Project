using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest {
    [TestFixture]
    [Category("Sample Tests")]
    internal class PlayerUnitTests {

        [Test]
        [Category("Banking test")]
        public void BankingTest_ExpectedValue() {
            //Pass if the player's z rotation increments  correctly.
            //Arrange
            PlayerAimMovement testScript_BankingMethod = new PlayerAimMovement();
            float test_currentX = 10;
            float test_targetX = 15;
            float test_BankingZStr = 10;
            float test_value = 0f;
            float expected_z_rotation = (test_targetX - test_currentX) * -test_BankingZStr;

            //act
            test_value = testScript_BankingMethod.Banking(test_targetX, test_currentX, test_BankingZStr);
            Debug.Log("test_Value = " + test_value);
            //Assert
            if (expected_z_rotation == test_value)
                Assert.Pass("With a difference of " + (test_targetX - test_currentX) + ", rotation is " + test_value);
            else
                Assert.Fail("Expected Z Rotation is " + expected_z_rotation + ", result was: " + test_value);
        }
    }
}
