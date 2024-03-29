namespace Parsers.CommonTests.VisualStudio.Projects
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.Projects;

    [TestClass]
    public class LegacyStyleParserTests
    {
        [TestMethod]
        public void GetCompileFilesTest()
        {
            // Arrange
            string path = Path.GetFullPath(@".\VisualStudio\TestFiles\ProjectsForTesting\Files\Files_UnknownFile.csproj");
            string projectDir = Path.GetDirectoryName(path);
            var xmlContent = File.ReadAllText(path, Encoding.UTF8);
            var document = XDocument.Parse(xmlContent);
            LegacyStyleParser legacyStyleParser = new LegacyStyleParser(document, projectDir);

            // Act
            Action action = () => _ = legacyStyleParser.GetCompileFiles().ToList();
            
            // Assert
            action.Should().Throw<FileNotFoundException>();
        }
    }
}