namespace Parsers.ProtocolTests.VisualStudio
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Skyline.DataMiner.CICD.FileSystem;
    using Skyline.DataMiner.CICD.Parsers.Protocol.VisualStudio;

    [TestClass]
    public class ProtocolSolutionTests
    {
        [TestMethod]
        public void ProtocolSolution_Solution_Load_Legacy()
        {
            var baseDir = FileSystem.Instance.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dir = FileSystem.Instance.Path.GetFullPath(FileSystem.Instance.Path.Combine(baseDir, @"VisualStudio\TestFiles\Protocol\Solution1"));
            var path = FileSystem.Instance.Path.Combine(dir, "Protocol.sln");

            var solution = ProtocolSolution.Load(path);

            ValidateSolution(solution, dir, path, false);
        }

        [TestMethod]
        public void ProtocolSolution_Solution_Load_Slnx()
        {
            var baseDir = FileSystem.Instance.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dir = FileSystem.Instance.Path.GetFullPath(FileSystem.Instance.Path.Combine(baseDir, @"VisualStudio\TestFiles\Protocol\Solution1"));
            var path = FileSystem.Instance.Path.Combine(dir, "Protocol.slnx");

            var solution = ProtocolSolution.Load(path);

            ValidateSolution(solution, dir, path, true);
        }

        private static void ValidateSolution(ProtocolSolution solution, string dir, string path, bool isSlnx)
        {
            solution.Should().NotBeNull();
            solution.SolutionPath.Should().BeEquivalentTo(path);
            solution.SolutionDirectory.Should().BeEquivalentTo(FileSystem.Instance.Path.GetDirectoryName(path));

            solution.Folders.Should().HaveCount(3);
            solution.Projects.Should().HaveCount(6);
            solution.QActions.Should().HaveCount(5);

            // Validate folders.
            // Internal folder.
            var internalFolder = solution.Folders.FirstOrDefault(f => f.Name == "Internal");
            Assert.IsNotNull(internalFolder);
            Assert.AreEqual("Internal", internalFolder.RelativePath);
            Assert.AreEqual(Path.Combine(dir, "Internal"), internalFolder.AbsolutePath);

            if (!isSlnx)
            {
                Assert.AreEqual(Guid.Parse("07EA3993-AA2E-4C59-9110-BF25DB8450BE"), internalFolder.Guid);
            }

            Assert.AreEqual(1, internalFolder.Children.Count());
            Assert.AreEqual(0, internalFolder.Files.Count());
            Assert.AreEqual(1, internalFolder.SubProjects.Count());
            Assert.AreEqual(0, internalFolder.SubFolders.Count());
            Assert.IsNull(internalFolder.Parent);

            Assert.AreEqual("QAction_Helper", internalFolder.Children.First().Name);

            Assert.AreEqual("QAction_Helper", internalFolder.SubProjects.First().Name);
            Assert.AreEqual(Path.Combine(dir, "QAction_Helper", "QAction_Helper.csproj"), internalFolder.SubProjects.First().AbsolutePath);
            Assert.AreEqual(@"QAction_Helper\QAction_Helper.csproj", internalFolder.SubProjects.First().RelativePath);
            Assert.AreEqual(internalFolder, internalFolder.SubProjects.First().Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("31B1EF6A-2E94-4F70-9B05-F297AB3B6C69"), internalFolder.SubProjects.First().Guid);
            }

            // QActions folder.
            var qactionsFolder = solution.Folders.FirstOrDefault(f => f.Name == "QActions");
            Assert.IsNotNull(qactionsFolder);
            Assert.IsNull(qactionsFolder.Parent);
            Assert.AreEqual("QActions", qactionsFolder.RelativePath);
            Assert.AreEqual(Path.Combine(dir, "QActions"), qactionsFolder.AbsolutePath);

            if (!isSlnx)
            {
                Assert.AreEqual(Guid.Parse("F32C5E72-BF2C-45CF-AAB1-DADCC040E51C"), qactionsFolder.Guid);
            }

            Assert.AreEqual(4, qactionsFolder.Children.Count());
            Assert.AreEqual(0, qactionsFolder.Files.Count());
            Assert.AreEqual(4, qactionsFolder.SubProjects.Count());
            Assert.AreEqual(0, qactionsFolder.SubFolders.Count());

            var qactionProjects = qactionsFolder.SubProjects.ToList();

            var qaction1 = solution.Projects.FirstOrDefault(p => p.Name == "QAction_1");

            Assert.IsNotNull(qaction1);
            Assert.AreEqual(Path.Combine(dir, "QAction_1", "QAction_1.csproj"), qaction1.AbsolutePath);
            Assert.AreEqual(@"QAction_1\QAction_1.csproj", qaction1.RelativePath);
            Assert.AreEqual(qactionsFolder, qaction1.Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("20481214-4655-4C51-97AA-5DA92296CBCF"), qaction1.Guid);
            }

            var qaction2 = solution.Projects.FirstOrDefault(p => p.Name == "QAction_2");

            Assert.IsNotNull(qaction2);
            Assert.AreEqual(Path.Combine(dir, "QAction_2", "QAction_2.csproj"), qaction2.AbsolutePath);
            Assert.AreEqual(@"QAction_2\QAction_2.csproj", qaction2.RelativePath);
            Assert.AreEqual(qactionsFolder, qaction2.Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("B5ED3E0A-72ED-42ED-8375-65925907F2D9"), qaction2.Guid);
            }

            var qaction3 = solution.Projects.FirstOrDefault(p => p.Name == "QAction_3");

            Assert.IsNotNull(qaction3);
            Assert.AreEqual("QAction_3", qaction3.Name);
            Assert.AreEqual(Path.Combine(dir, "QAction_3", "QAction_3.csproj"), qaction3.AbsolutePath);
            Assert.AreEqual(@"QAction_3\QAction_3.csproj", qaction3.RelativePath);
            Assert.AreEqual(qactionsFolder, qaction3.Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("35F7A839-1F8D-4932-8850-D6B9FD17A2E8"), qaction3.Guid);
            }

            var qaction63000 = solution.Projects.FirstOrDefault(p => p.Name == "QAction_63000");

            Assert.IsNotNull(qaction63000);
            Assert.AreEqual("QAction_63000", qaction63000.Name);
            Assert.AreEqual(Path.Combine(dir, "QAction_63000", "QAction_63000.csproj"), qaction63000.AbsolutePath);
            Assert.AreEqual(@"QAction_63000\QAction_63000.csproj", qaction63000.RelativePath);
            Assert.AreEqual(qactionsFolder, qaction63000.Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("113FC56A-2732-420E-B365-3915558AFD45"), qaction63000.Guid);
            }

            // Solution items folder.
            var solutionItemsFolder = solution.Folders.FirstOrDefault(f => f.Name == "Solution Items");
            Assert.IsNotNull(solutionItemsFolder);
            Assert.IsNull(solutionItemsFolder.Parent);
            Assert.AreEqual("Solution Items", solutionItemsFolder.RelativePath);
            Assert.AreEqual(Path.Combine(dir, "Solution Items"), solutionItemsFolder.AbsolutePath);

            if (!isSlnx)
            {
                Assert.AreEqual(Guid.Parse("13998E16-2F02-4D47-BEB1-4ED034DD72D8"), solutionItemsFolder.Guid);
            }

            Assert.AreEqual(0, solutionItemsFolder.Children.Count(), "Children");
            Assert.AreEqual(1, solutionItemsFolder.Files.Count(), "Files");
            Assert.AreEqual(0, solutionItemsFolder.SubProjects.Count(), "Subprojects");
            Assert.AreEqual(0, solutionItemsFolder.SubFolders.Count(), "Subfolders");

            Assert.AreEqual("protocol.xml", solutionItemsFolder.Files.First().FileName);
            Assert.AreEqual(Path.Combine(dir, "protocol.xml"), solutionItemsFolder.Files.First().AbsolutePath);

            // Check test project.
            var testProject = solution.Projects.FirstOrDefault(p => p.Name == "QAction_3Tests");
            Assert.IsNotNull(testProject);
            Assert.AreEqual("QAction_3Tests", testProject.Name);
            Assert.AreEqual(Path.Combine(dir, "QAction_3Tests", "QAction_3Tests.csproj"), testProject.AbsolutePath);
            Assert.AreEqual(@"QAction_3Tests\QAction_3Tests.csproj", testProject.RelativePath);
            Assert.IsNull(testProject.Parent);

            if (!isSlnx)
            {
                Assert.AreEqual(new Guid("83620144-DF9F-49E2-8376-AF72CD68B55C"), testProject.Guid);
            }

            // Verify QActions.
            var qa1 = solution.QActions.FirstOrDefault(q => q.Id == 1);
            qa1.Should().NotBeNull();
            qa1.Files.Should().HaveCount(1);
            qa1.Files[0].Code.Should().NotBeNullOrEmpty();
            qa1.DllImports.Should().BeEquivalentTo(new[] { "Newtonsoft.Json.dll", "System.dll", "System.Xml.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" });

            var qa2 = solution.QActions.FirstOrDefault(q => q.Id == 2);
            qa2.Should().NotBeNull();
            qa2.Files.Should().HaveCount(1);
            qa2.Files[0].Code.Should().NotBeNullOrEmpty();
            qa2.DllImports.Should().BeEquivalentTo(new[] { "System.dll", "[ProtocolName].[ProtocolVersion].QAction.1.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" });

            var qa3 = solution.QActions.FirstOrDefault(q => q.Id == 3);
            qa3.Should().NotBeNull();
            qa3.Files.Should().HaveCount(3);
            qa3.Files.All(x => !String.IsNullOrEmpty(x.Code)).Should().BeTrue();
            qa3.Files[0].Code.Should().NotBeNullOrEmpty();
            qa3.Files[1].Code.Should().NotBeNullOrEmpty();
            qa3.Files[2].Code.Should().NotBeNullOrEmpty();
            qa3.DllImports.Should().BeEquivalentTo(new[] { "System.dll", "[ProtocolName].[ProtocolVersion].QAction.63000.dll" });

            var qa4 = solution.QActions.FirstOrDefault(q => q.Id == 4);
            qa4.Should().NotBeNull();
            qa4.Files.All(x => !String.IsNullOrEmpty(x.Code)).Should().BeTrue();
            qa4.DllImports.Should().BeEmpty();

            var qa63000 = solution.QActions.FirstOrDefault(q => q.Id == 63000);
            qa63000.Should().NotBeNull();
            qa63000.Files.Should().HaveCount(1);
            qa63000.Files[0].Code.Should().NotBeNullOrEmpty();
            qa63000.DllImports.Should().BeEquivalentTo(new[] { "System.dll" });
        }
    }
}
