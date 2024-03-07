namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.Projects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Skyline.DataMiner.CICD.FileSystem;
    using Skyline.DataMiner.CICD.Parsers.Common.Exceptions;
    using Skyline.DataMiner.CICD.Parsers.Common.Extensions;

    /// <summary>
    /// Represents a Visual Studio project file.
    /// </summary>
    public class Project
    {
        private static readonly IFileSystem FileSystem = CICD.FileSystem.FileSystem.Instance;
        private readonly ICollection<ProjectFile> _files = new List<ProjectFile>();
        private readonly ICollection<Reference> _references = new List<Reference>();
        private readonly ICollection<ProjectReference> _projectReferences = new List<ProjectReference>();
        private readonly ICollection<PackageReference> _packageReferences = new List<PackageReference>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class with the specified name and project files.
        /// </summary>
        /// <param name="name">The project name.</param>
        /// <param name="files">The project files.</param>
        public Project(string name, ICollection<ProjectFile> files)
        {
            AssemblyName = name;
            _files = files;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class with the specified name, path, project files, references, package references and project references.
        /// This will not load in the data if this path would be an existing file.
        /// </summary>
        /// <param name="name">The project name.</param>
        /// <param name="path">The project file path.</param>
        /// <param name="tfm">The Target Framework Moniker.</param>
        /// <param name="projectFiles">The project files.</param>
        /// <param name="references">The references.</param>
        /// <param name="packageReferences">The package references.</param>
        /// <param name="projectReferences">The project references.</param>
        [EditorBrowsable(EditorBrowsableState.Never)] // Used by DIS
        public Project(string name, string path = null, string tfm = null, ICollection<ProjectFile> projectFiles = null, ICollection<Reference> references = null, ICollection<PackageReference> packageReferences = null, ICollection<ProjectReference> projectReferences = null)
        {
            AssemblyName = name;
            Path = path;
            TargetFrameworkMoniker = tfm;

            if (projectFiles != null)
            {
                _files = projectFiles;
            }

            if (references != null)
            {
                _references = references;
            }

            if (packageReferences != null)
            {
                _packageReferences = packageReferences;
            }

            if (_projectReferences != null)
            {
                _projectReferences = projectReferences;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        private Project()
        {
        }

        /// <summary>
        /// Gets the project name.
        /// </summary>
        /// <value>The project name.</value>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        /// <value>The project path.</value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the target framework moniker (TFM).
        /// </summary>
        /// <value>The target framework moniker (TFM).</value>
        public string TargetFrameworkMoniker { get; private set; }

        /// <summary>
        /// Gets the project files.
        /// </summary>
        /// <value>The project files.</value>
        public IEnumerable<ProjectFile> Files => _files;

        /// <summary>
        /// Gets the project references.
        /// </summary>
        /// <value>The project references.</value>
        public IEnumerable<Reference> References => _references;

        /// <summary>
        /// Gets the project references.
        /// </summary>
        /// <value>The project references.</value>
        public IEnumerable<ProjectReference> ProjectReferences => _projectReferences;

        /// <summary>
        /// Gets the package references.
        /// </summary>
        /// <value>The package references.</value>
        public IEnumerable<PackageReference> PackageReferences => _packageReferences;

        /// <summary>
        /// Gets the style of the project.
        /// </summary>
        public ProjectStyle ProjectStyle { get; private set; }

        /// <summary>
        /// Loads the projects with the specified path.
        /// </summary>
        /// <param name="path">The path of the project file to load.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <returns>The loaded project.</returns>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> does not exist.</exception>
        public static Project Load(string path, string projectName)
        {
            if (!FileSystem.File.Exists(path))
            {
                throw new FileNotFoundException("Could not find project file: " + path);
            }

            try
            {
                string projectDir = FileSystem.Path.GetDirectoryName(path);
                var xmlContent = FileSystem.File.ReadAllText(path, Encoding.UTF8);
                var document = XDocument.Parse(xmlContent);

                IProjectParser parser = ProjectParserFactory.GetParser(document, projectDir);

                string name = projectName;
                string assemblyName = parser.GetAssemblyName();
                if (!String.IsNullOrEmpty(assemblyName))
                {
                    name = assemblyName;
                }

                var project = new Project
                {
                    AssemblyName = name,
                    Path = path,
                    ProjectStyle = parser.GetProjectStyle(),
                };

                project._references.AddRange(parser.GetReferences());
                project._projectReferences.AddRange(parser.GetProjectReferences());
                project._packageReferences.AddRange(parser.GetPackageReferences());

                var files = parser.GetCompileFiles().ToList();

                project._files.AddRange(files);
                project._files.AddRange(parser.GetSharedProjectCompileFiles());

                project.TargetFrameworkMoniker = parser.GetTargetFrameworkMoniker();

                return project;
            }
            catch (Exception e)
            {
                throw new ParserException($"Failed to load project '{projectName}' ({path}).", e);
            }
        }
    }
}