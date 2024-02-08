using System;
using System.Collections.Generic;
using System.Text;

namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.Projects
{
    internal interface IProjectParser
    {
        ProjectType GetProjectType();

        string GetAssemblyName();

        IEnumerable<Reference> GetReferences();

        IEnumerable<ProjectReference> GetProjectReferences();

        IEnumerable<PackageReference> GetPackageReferences();

        IEnumerable<ProjectFile> GetCompileFiles();

        string GetTargetFrameworkMoniker();

        IEnumerable<ProjectFile> GetSharedProjectCompileFiles();
    }
}
