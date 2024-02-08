namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.Projects
{
    /// <summary>
    /// Type of the project.
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// Unable to verify the type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Legacy style.
        /// </summary>
        Legacy,

        /// <summary>
        /// SDK style.
        /// </summary>
        Sdk,
    }
}