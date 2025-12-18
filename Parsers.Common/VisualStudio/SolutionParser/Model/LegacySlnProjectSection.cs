namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.SolutionParser.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a project section of a legacy solution file.
    /// </summary>
    internal class LegacySlnProjectSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacySlnProjectSection"/> class.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <param name="sectionType">The section type.</param>
        public LegacySlnProjectSection(string name, LegacySlnProjectSectionType sectionType)
        {
            Name = name;
            Type = sectionType;
            Entries = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the name of the project section.
        /// </summary>
        /// <value>The project section name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the project section.
        /// </summary>
        /// <value>The project section type.</value>
        public LegacySlnProjectSectionType Type { get; }

        /// <summary>
        /// Gets the entries of the project section.
        /// </summary>
        /// <value>The project section entries.</value>
        public IDictionary<string, string> Entries { get; }
    }
}