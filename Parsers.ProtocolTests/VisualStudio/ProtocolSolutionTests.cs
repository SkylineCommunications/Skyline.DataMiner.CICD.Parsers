namespace Parsers.ProtocolTests.VisualStudio
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Skyline.DataMiner.CICD.Parsers.Common.Xml;
    using Skyline.DataMiner.CICD.Parsers.Protocol.VisualStudio;

    [TestClass]
    public class ProtocolSolutionTests
    {
        [TestMethod]
        public void ProtocolSolution_Solution_Load()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dir = Path.GetFullPath(Path.Combine(baseDir, @"VisualStudio\TestFiles\Protocol\Solution1"));
            var path = Path.Combine(dir, "protocol.sln");

            var solution = ProtocolSolution.Load(path);

            Assert.IsInstanceOfType(solution, typeof(ProtocolSolution));

            Assert.AreEqual(path, solution.SolutionPath);
            Assert.AreEqual(Path.GetDirectoryName(path), solution.SolutionDirectory);
            Assert.AreEqual(5, solution.Projects.Count());

            Assert.IsInstanceOfType(solution.ProtocolDocument, typeof(XmlDocument));
            Assert.AreEqual(5, solution.QActions.Count);

            var qa1 = solution.QActions.FirstOrDefault(q => q.Id == 1);
            Assert.IsNotNull(qa1);
            Assert.AreEqual(1, qa1.Files.Count);
            Assert.IsFalse(String.IsNullOrEmpty(qa1.Files.First().Code));
            CollectionAssert.AreEquivalent(new[] { "Newtonsoft.Json.dll", "System.dll", "System.Xml.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" }, qa1.DllImports.ToArray());

            var qa2 = solution.QActions.FirstOrDefault(q => q.Id == 2);
            Assert.IsNotNull(qa2);
            Assert.AreEqual(1, qa2.Files.Count);
            Assert.IsFalse(String.IsNullOrEmpty(qa2.Files.First().Code));
            CollectionAssert.AreEquivalent(new[] { "System.dll", "[ProtocolName].[ProtocolVersion].QAction.1.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" }, qa2.DllImports.ToArray());

            var qa3 = solution.QActions.FirstOrDefault(q => q.Id == 3);
            Assert.IsNotNull(qa3);
            CollectionAssert.AreEquivalent(new[] { "QAction_3.cs", "Class1.cs", "SubDir\\Class2.cs" }, qa3.Files.Select(x => x.Name).ToArray());
            Assert.IsTrue(qa3.Files.All(x => !String.IsNullOrEmpty(x.Code)));
            CollectionAssert.AreEquivalent(new[] { "System.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" }, qa3.DllImports.ToArray());

            var qa4 = solution.QActions.FirstOrDefault(q => q.Id == 4);
            Assert.IsNotNull(qa4);
            Assert.IsTrue(qa4.Files.All(x => !String.IsNullOrEmpty(x.Code)));
            Assert.AreEqual(0, qa4.DllImports.Count);

            var qa63000 = solution.QActions.FirstOrDefault(q => q.Id == 63000);
            Assert.IsNotNull(qa63000);
            Assert.AreEqual(1, qa63000.Files.Count);
            Assert.IsFalse(String.IsNullOrEmpty(qa63000.Files.First().Code));
            CollectionAssert.AreEquivalent(new[] { "System.dll" }, qa63000.DllImports.ToArray());
        }
    }
}
