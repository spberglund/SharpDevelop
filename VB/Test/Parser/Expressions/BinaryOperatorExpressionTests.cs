﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using NUnit.Framework;
using ICSharpCode.NRefactory.VB.Parser;
using ICSharpCode.NRefactory.VB.Dom;
using ICSharpCode.NRefactory.VB.PrettyPrinter;

namespace ICSharpCode.NRefactory.VB.Tests.Dom
{
	[TestFixture]
	public class BinaryOperatorExpressionTests
	{
		void OperatorPrecedenceTest(string strongOperator, BinaryOperatorType strongOperatorType,
		                            string weakOperator, BinaryOperatorType weakOperatorType)
		{
			string program = "a " + weakOperator + " b " + strongOperator + " c";
			BinaryOperatorExpression boe;
			boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>(program);
			Assert.AreEqual(weakOperatorType, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			boe = (BinaryOperatorExpression)boe.Right;
			Assert.AreEqual(strongOperatorType, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is IdentifierExpression);
			
			program = "a " + strongOperator + " b " + weakOperator + " c";
			boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>(program);
			Assert.AreEqual(weakOperatorType, boe.Op);
			Assert.IsTrue(boe.Right is IdentifierExpression);
			boe = (BinaryOperatorExpression)boe.Left;
			Assert.AreEqual(strongOperatorType, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is IdentifierExpression);
		}
		
		void SameOperatorPrecedenceTest(string firstOperator, BinaryOperatorType firstOperatorType,
		                                string secondOperator, BinaryOperatorType secondOperatorType)
		{
			string program = "a " + secondOperator + " b " + firstOperator + " c";
			BinaryOperatorExpression boe;
			boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>(program);
			Assert.AreEqual(firstOperatorType, boe.Op);
			Assert.IsTrue(boe.Right is IdentifierExpression);
			boe = (BinaryOperatorExpression)boe.Left;
			Assert.AreEqual(secondOperatorType, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is IdentifierExpression);
			
			program = "a " + firstOperator + " b " + secondOperator + " c";
			boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>(program);
			Assert.AreEqual(secondOperatorType, boe.Op);
			Assert.IsTrue(boe.Right is IdentifierExpression);
			boe = (BinaryOperatorExpression)boe.Left;
			Assert.AreEqual(firstOperatorType, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is IdentifierExpression);
		}
		
		#region VB.NET
		void VBNetTestBinaryOperatorExpressionTest(string program, BinaryOperatorType op)
		{
			BinaryOperatorExpression boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>(program);
			Assert.AreEqual(op, boe.Op);
			
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is IdentifierExpression);
		}
		
		[Test]
		public void VBOperatorPrecedenceTest()
		{
			OperatorPrecedenceTest("^", BinaryOperatorType.Power, "*", BinaryOperatorType.Multiply);
			SameOperatorPrecedenceTest("*", BinaryOperatorType.Multiply, "/", BinaryOperatorType.Divide);
			OperatorPrecedenceTest("/", BinaryOperatorType.Divide, "\\", BinaryOperatorType.DivideInteger);
			OperatorPrecedenceTest("\\", BinaryOperatorType.DivideInteger, "Mod", BinaryOperatorType.Modulus);
			OperatorPrecedenceTest("Mod", BinaryOperatorType.Modulus, "+", BinaryOperatorType.Add);
			SameOperatorPrecedenceTest("+", BinaryOperatorType.Add, "-", BinaryOperatorType.Subtract);
			OperatorPrecedenceTest("-", BinaryOperatorType.Subtract, "&", BinaryOperatorType.Concat);
			OperatorPrecedenceTest("&", BinaryOperatorType.Concat, "<<", BinaryOperatorType.ShiftLeft);
			SameOperatorPrecedenceTest("<<", BinaryOperatorType.ShiftLeft, ">>", BinaryOperatorType.ShiftRight);
			OperatorPrecedenceTest("<<", BinaryOperatorType.ShiftLeft, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("<>", BinaryOperatorType.InEquality, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("<", BinaryOperatorType.LessThan, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("<=", BinaryOperatorType.LessThanOrEqual, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest(">", BinaryOperatorType.GreaterThan, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest(">=", BinaryOperatorType.GreaterThanOrEqual, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("Like", BinaryOperatorType.Like, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("Is", BinaryOperatorType.ReferenceEquality, "=", BinaryOperatorType.Equality);
			SameOperatorPrecedenceTest("IsNot", BinaryOperatorType.ReferenceInequality, "=", BinaryOperatorType.Equality);
			OperatorPrecedenceTest("=", BinaryOperatorType.Equality, "And", BinaryOperatorType.BitwiseAnd);
			SameOperatorPrecedenceTest("And", BinaryOperatorType.BitwiseAnd, "AndAlso", BinaryOperatorType.LogicalAnd);
			OperatorPrecedenceTest("And", BinaryOperatorType.BitwiseAnd, "Or", BinaryOperatorType.BitwiseOr);
			SameOperatorPrecedenceTest("Or", BinaryOperatorType.BitwiseOr, "OrElse", BinaryOperatorType.LogicalOr);
			SameOperatorPrecedenceTest("Or", BinaryOperatorType.BitwiseOr, "Xor", BinaryOperatorType.ExclusiveOr);
		}
		
		[Test]
		public void VBNetPowerTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a ^ b", BinaryOperatorType.Power);
		}
		
		[Test]
		public void VBNetConcatTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a & b", BinaryOperatorType.Concat);
		}
		
		[Test]
		public void VBNetLogicalAndTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a AndAlso b", BinaryOperatorType.LogicalAnd);
		}
		[Test]
		public void VBNetLogicalAndNotLazyTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a And b", BinaryOperatorType.BitwiseAnd);
		}
		
		[Test]
		public void VBNetLogicalOrTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a OrElse b", BinaryOperatorType.LogicalOr);
		}
		[Test]
		public void VBNetLogicalOrNotLazyTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a Or b", BinaryOperatorType.BitwiseOr);
		}
		
		[Test]
		public void VBNetExclusiveOrTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a Xor b", BinaryOperatorType.ExclusiveOr);
		}
		
		
		[Test]
		public void VBNetGreaterThanTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a > b", BinaryOperatorType.GreaterThan);
		}
		
		[Test]
		public void VBNetGreaterThanOrEqualTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a >= b", BinaryOperatorType.GreaterThanOrEqual);
		}
		
		[Test]
		public void VBNetEqualityTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a = b", BinaryOperatorType.Equality);
		}
		
		[Test]
		public void VBNetInEqualityTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a <> b", BinaryOperatorType.InEquality);
		}
		
		[Test]
		public void VBNetLessThanTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a < b", BinaryOperatorType.LessThan);
		}
		
		[Test]
		public void VBNetLessThanOrEqualTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a <= b", BinaryOperatorType.LessThanOrEqual);
		}
		
		[Test]
		public void VBNetAddTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a + b", BinaryOperatorType.Add);
		}
		
		[Test]
		public void VBNetSubtractTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a - b", BinaryOperatorType.Subtract);
		}
		
		[Test]
		public void VBNetMultiplyTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a * b", BinaryOperatorType.Multiply);
		}
		
		[Test]
		public void VBNetDivideTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a / b", BinaryOperatorType.Divide);
		}
		
		[Test]
		public void VBNetDivideIntegerTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a \\ b", BinaryOperatorType.DivideInteger);
		}
		
		[Test]
		public void VBNetModulusTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a Mod b", BinaryOperatorType.Modulus);
		}
		
		[Test]
		public void VBNetShiftLeftTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a << b", BinaryOperatorType.ShiftLeft);
		}
		
		[Test]
		public void VBNetShiftRightTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a >> b", BinaryOperatorType.ShiftRight);
		}
		
		[Test]
		public void VBNetISTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a is b", BinaryOperatorType.ReferenceEquality);
		}
		
		[Test]
		public void VBNetISNotTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a IsNot b", BinaryOperatorType.ReferenceInequality);
		}
		
		[Test]
		public void VBNetLikeTest()
		{
			VBNetTestBinaryOperatorExpressionTest("a Like b", BinaryOperatorType.Like);
		}
		
		[Test]
		public void VBNetNullCoalescingTest()
		{
			VBNetTestBinaryOperatorExpressionTest("If(a, b)", BinaryOperatorType.NullCoalescing);
		}
		
		[Test]
		public void VBNetDictionaryAccess()
		{
			BinaryOperatorExpression boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>("a!b");
			Assert.AreEqual(BinaryOperatorType.DictionaryAccess, boe.Op);
			Assert.IsTrue(boe.Left is IdentifierExpression);
			Assert.IsTrue(boe.Right is PrimitiveExpression);
		}
		
		[Test]
		public void VBNetWithDictionaryAccess()
		{
			BinaryOperatorExpression boe = ParseUtilVBNet.ParseExpression<BinaryOperatorExpression>("!b");
			Assert.AreEqual(BinaryOperatorType.DictionaryAccess, boe.Op);
			Assert.IsTrue(boe.Left.IsNull);
			Assert.IsTrue(boe.Right is PrimitiveExpression);
		}
		#endregion
		
		#region AddIntegerTests
		string AddIntegerToBoe(string input, int number)
		{
			return AddInteger<BinaryOperatorExpression>(input, number);
		}
		
		string AddInteger<T>(string input, int number) where T : Expression
		{
			Expression e = ParseUtilVBNet.ParseExpression<T>(input);
			e = Expression.AddInteger(e, number);
			VBNetOutputVisitor v = new VBNetOutputVisitor();
			e.AcceptVisitor(v, null);
			return v.Text;
		}
		
		[Test]
		public void AddInteger()
		{
			Assert.AreEqual("a + 2", AddIntegerToBoe("a + 1", 1));
			Assert.AreEqual("a + 2", AddIntegerToBoe("a + 3", -1));
			Assert.AreEqual("a + b + c + 2", AddIntegerToBoe("a + b + c + 1", 1));
			Assert.AreEqual("a", AddIntegerToBoe("a + 1", -1));
			Assert.AreEqual("2", AddInteger<PrimitiveExpression>("1", 1));
			Assert.AreEqual("-1", AddInteger<PrimitiveExpression>("1", -2));
			Assert.AreEqual("0", AddInteger<PrimitiveExpression>("1", -1));
			Assert.AreEqual("a + 1", AddInteger<IdentifierExpression>("a", 1));
		}
		
		[Test]
		public void AddIntegerWithNegativeResult()
		{
			Assert.AreEqual("a - 1", AddIntegerToBoe("a + 1", -2));
			Assert.AreEqual("a - 2", AddIntegerToBoe("a - 1", -1));
			Assert.AreEqual("a + b + c - 2", AddIntegerToBoe("a + b + c + 2", -4));
			Assert.AreEqual("a + b + c - 6", AddIntegerToBoe("a + b + c - 2", -4));
			Assert.AreEqual("a + b + c", AddIntegerToBoe("a + b + c + 2", -2));
			Assert.AreEqual("a", AddIntegerToBoe("a - 1", 1));
			Assert.AreEqual("a + 1", AddIntegerToBoe("a - 2", 3));
			Assert.AreEqual("a - 1", AddInteger<IdentifierExpression>("a", -1));
		}
		#endregion
	}
}
