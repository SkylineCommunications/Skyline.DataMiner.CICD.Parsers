namespace Parsers.AutomationTests
{
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.CICD.FileSystem;

    [TestClass]
    public class AssemblyInitialize
    {
	    [AssemblyInitialize]
	    public static void AssemblyInit(TestContext context)
	    {
		    // This is needed because certain tools will look at all csproj files in the entire repository.

		    var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			//FileSystem.Instance.Directory.AllowWritesOnDirectory(baseDir);

		    foreach (var zipFile in Directory.GetFiles(baseDir, "*.zip", SearchOption.AllDirectories))
		    {
			    string dir = Path.Combine(Path.GetDirectoryName(zipFile), "TestFiles");

			    if (Directory.Exists(dir))
			    {
				    // Has been extracted already before (different target framework run)
			    }

                ZipFile.ExtractToDirectory(zipFile, dir);
		    }
	    }
    }
}