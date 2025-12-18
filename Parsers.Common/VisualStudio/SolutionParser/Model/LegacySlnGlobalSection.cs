namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.SolutionParser.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a global section in a legacy solution file.
    /// </summary>
    internal class LegacySlnGlobalSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacySlnGlobalSection"/> class.
        /// </summary>
        /// <param name="name">The section name.</param>
        /// <param name="sectionType">The section type.</param>
        public LegacySlnGlobalSection(string name, LegacySlnGlobalSectionType sectionType)
        {
            Name = name;
            Type = sectionType;
            Entries = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>The section name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the type of the section.
        /// </summary>
        /// <value>The section type.</value>
        public LegacySlnGlobalSectionType Type { get; }

        /// <summary>
        /// Gets or sets the section entries.
        /// </summary>
        /// <value>The section entries.</value>
        public IDictionary<string, string> Entries { get; }
    }
}
