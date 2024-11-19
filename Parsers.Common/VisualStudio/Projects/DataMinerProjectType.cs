namespace Skyline.DataMiner.CICD.Parsers.Common.VisualStudio.Projects
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DataMiner project type.
    /// </summary>
    public enum DataMinerProjectType
    {
        /// <summary>
        /// Represents a DataMiner Install Package project.
        /// </summary>
        Package,

        /// <summary>
        /// Represents a DataMiner Automation Script project.
        /// </summary>
        AutomationScript,

        /// <summary>
        /// Represents a DataMiner Ad Hoc Data Source project.
        /// </summary>
        AdHocDataSource
    }

    /// <summary>
    /// Class to convert from and to the <see cref="DataMinerProjectType"/> enum.
    /// </summary>
    public static class DataMinerProjectTypeConverter
    {
        private static Dictionary<string, DataMinerProjectType> stringToEnum = new Dictionary<string, DataMinerProjectType>()
        {
            ["Package"] = DataMinerProjectType.Package,
            ["AutomationScript"] = DataMinerProjectType.AutomationScript,
            ["ad-hoc-data-source"] = DataMinerProjectType.AdHocDataSource
        };

        private static Dictionary<DataMinerProjectType, string> enumToString = new Dictionary<DataMinerProjectType, string>()
        {
            [DataMinerProjectType.Package] = "Package",
            [DataMinerProjectType.AutomationScript] = "AutomationScript",
            [DataMinerProjectType.AdHocDataSource] = "ad-hoc-data-source"
        };

        /// <summary>
        /// Tries to convert the specified value to the <see cref="DataMinerProjectType"/> enum. Will return null when unable to convert to enum.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value when successful, null when not.</returns>
        public static DataMinerProjectType? ToEnum(string value)
        {
            if (stringToEnum.TryGetValue(value, out DataMinerProjectType t))
            {
                return t;
            }

            return null;
        }

        /// <summary>
        /// Tries to convert the specified <see cref="DataMinerProjectType"/> value to the string version for the project. Will return null when unable to convert to enum.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value when successful, null when not.</returns>
        public static string ToString(DataMinerProjectType value)
        {
            if (enumToString.TryGetValue(value, out string s))
            {
                return s;
            }

            return null;
        }
    }
}