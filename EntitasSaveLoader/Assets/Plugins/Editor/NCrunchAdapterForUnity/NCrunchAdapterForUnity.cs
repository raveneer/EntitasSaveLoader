using System.Text.RegularExpressions;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;

[InitializeOnLoad]
public class NCrunchAdapterForUnity
{
    public const string NUnitUnityReference = @"<Reference Include=""nunit.framework"">
      <HintPath>.*nunit.framework.dll</HintPath>
    </Reference>";

    public const string NUnitOfficialReference = @"<Reference Include=""nunit.framework"">
      <HintPath>Assets\Plugins\Editor\NCrunchAdapterForUnity\NUnit.3.5.0\lib\net35\nunit.framework.dll</HintPath>
    </Reference>";

    static NCrunchAdapterForUnity()
    {
        ProjectFilesGenerator.ProjectFileGeneration += (string name, string content) =>
        {
            var regex = new Regex(NUnitUnityReference);
            return regex.Replace(content, NUnitOfficialReference);
        };
    }
}
