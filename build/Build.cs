using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

internal class Build : NukeBuild
{
	public static int Main() => Execute<Build>(x => x.Compile);

	[Parameter("Configuration — default Release")]
	private readonly string Configuration = "Release";

	private static AbsolutePath SourceDirectory => RootDirectory / "src";
	private static AbsolutePath OutputDirectory => RootDirectory / "out";

	private Target Clean => _ => _
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(d => d.DeleteDirectory());
			OutputDirectory.CreateOrCleanDirectory();
		});

	private Target Restore => _ => _
		.DependsOn(Clean)
		.Executes(() =>
		{
			foreach (var project in SourceDirectory.GlobFiles("*/*.csproj"))
			{
				DotNetRestore(s => s.SetProjectFile(project));
			}
		});

	private Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			foreach (var project in SourceDirectory.GlobFiles("*/*.csproj"))
			{
				var pluginName = project.Parent.Name;
				var pluginOutput = OutputDirectory / pluginName;

				DotNetBuild(s => s
					.SetProjectFile(project)
					.SetConfiguration(Configuration)
					.SetOutputDirectory(pluginOutput)
					.EnableNoRestore());
			}
		});
}