﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using NUnit.Framework;
using ICSharpCode.NRefactory.VB.Parser;
using ICSharpCode.NRefactory.VB.Dom;

namespace ICSharpCode.NRefactory.VB.Tests.Dom
{
	[TestFixture]
	public class ResumeStatementTests
	{
		#region VB.NET
		[Test]
		public void VBNetResumeStatementTest()
		{
			ResumeStatement resumeStatement = ParseUtilVBNet.ParseStatement<ResumeStatement>("Resume");
		}
		#endregion
	}
}
