namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.SolutionParser.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a project in a solution file.
    /// </summary>
    internal class LegacySlnProject : SlnProject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacySlnProject"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        /// <param name="name">The project name.</param>
        /// <param name="path">The project path.</param>
        /// <param name="guid">The project GUID.</param>
        public LegacySlnProject(Guid typeGuid, string name, string path, Guid guid) : base(typeGuid, name, path, guid)
        {
        }

        /// <summary>
        /// Gets the project sections.
        /// </summary>
        /// <value>The project sections.</value>
        public ICollection<LegacySlnProjectSection> ProjectSections { get; } = new List<LegacySlnProjectSection>();
    }
}