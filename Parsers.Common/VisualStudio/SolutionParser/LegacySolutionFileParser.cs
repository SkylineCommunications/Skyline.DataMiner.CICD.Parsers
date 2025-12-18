namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.SolutionParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.SolutionParser.Model;

    /// <summary>
    /// Represents a solution file parser.
    /// </summary>
    internal class LegacySolutionFileParser
    {
        private static readonly Regex _projectPattern = new Regex(@"Project\(\""(?<typeGuid>.*?)\""\)\s+=\s+\""(?<name>.*?)\"",\s+\""(?<path>.*?)\"",\s+\""(?<guid>.*?)\""(?<content>.*?)\bEndProject\b", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        private static readonly Regex _globalPattern = new Regex(@"GlobalSection\((?<name>[\w]+)\)\s+=\s+(?<type>(?:post|pre)Solution)(?<content>.*?)EndGlobalSection", RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        private static readonly Regex _sectionPattern = new Regex(@"ProjectSection\((?<name>.*?)\)\s+=\s+(?<type>.*?)\s+(?<entries>.*?)\bEndProjectSection\b", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex _entryPattern = new Regex(@"^\s*(?<key>.*?)=(?<value>.*?)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        private readonly string _solutionContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacySolutionFileParser"/> class.
        /// </summary>
        /// <param name="solutionContents">The solution contents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="solutionContents"/> is <see langword="null"/>.</exception>
        public LegacySolutionFileParser(string solutionContents)
        {
            _solutionContents = solutionContents ?? throw new ArgumentNullException(nameof(solutionContents));
        }

        /// <summary>
        /// Parses the projects of the solution file.
        /// </summary>
        /// <returns>The projects of the solution.</returns>
        public IEnumerable<LegacySlnProject> ParseProjects()
        {
            var matches = _projectPattern.Matches(_solutionContents);

            foreach (Match match in matches)
            {
                yield return ParseProject(match);
            }
        }

        /// <summary>
        /// Parses the global sections of the solution file.
        /// </summary>
        /// <returns>The global sections of the solution file.</returns>
        public IEnumerable<LegacySlnGlobalSection> ParseGlobalSections()
        {
            var matches = _globalPattern.Matches(_solutionContents);

            foreach (Match match in matches)
            {
                yield return ParseGlobalSection(match);
            }
        }

        private static LegacySlnProject ParseProject(Match match)
        {
            var typeGuid = new Guid(match.Groups["typeGuid"].Value);
            var guid = new Guid(match.Groups["guid"].Value);
            string name = match.Groups["name"].Value;
            string path = match.Groups["path"].Value;
            var content = match.Groups["content"].Value.Trim();

            var project = new LegacySlnProject(typeGuid, name, path, guid);

            if (!String.IsNullOrWhiteSpace(content))
            {
                var sectionMatches = _sectionPattern.Matches(content);

                foreach (Match sectionMatch in sectionMatches)
                {
                    var projectSection = ParseProjectSection(sectionMatch);
                    project.ProjectSections.Add(projectSection);
                }
            }

            return project;
        }

        private static LegacySlnProjectSection ParseProjectSection(Match match)
        {
            var projectType = (match.Groups["type"].Value == "preProject")
                              ? LegacySlnProjectSectionType.PreProject
                              : LegacySlnProjectSectionType.PostProject;
            string name = match.Groups["name"].Value;
            string entries = match.Groups["entries"].Value;

            var section = new LegacySlnProjectSection(name, projectType);

            if (!String.IsNullOrWhiteSpace(entries))
            {
                var entryMatches = _entryPattern.Matches(entries);

                foreach (Match entryMatch in entryMatches)
                {
                    var entryKey = entryMatch.Groups["key"].Value.Trim();
                    var entryValue = entryMatch.Groups["value"].Value.Trim();
                    section.Entries[entryKey] = entryValue;
                }
            }

            return section;
        }

        private static LegacySlnGlobalSection ParseGlobalSection(Match match)
        {
            var sectionType = (match.Groups["type"].Value == "preSolution")
                                  ? LegacySlnGlobalSectionType.PreSolution
                                  : LegacySlnGlobalSectionType.PostSolution;
            string name = match.Groups["name"].Value;
            var content = match.Groups["content"].Value;

            var section = new LegacySlnGlobalSection(name, sectionType);

            if (!String.IsNullOrWhiteSpace(content))
            {
                var entryMatches = _entryPattern.Matches(content);

                foreach (Match entryMatch in entryMatches)
                {
                    var entryKey = entryMatch.Groups["key"].Value.Trim();
                    var entryValue = entryMatch.Groups["value"].Value.Trim();
                    section.Entries[entryKey] = entryValue;
                }
            }

            return section;
        }
    }
}
