#if UNITY_5_6_OR_NEWER

namespace Sirenix.Serialization.Internal
{
    using Sirenix.Serialization;
    using UnityEditor;
    using UnityEditor.Build;
    using System.IO;
    using System;

#if UNITY_2018_1_OR_NEWER

    using UnityEditor.Build.Reporting;

#endif

#pragma warning disable CS0618 // Type or member is obsolete
    public class PreBuildAOTAutomation : IPreprocessBuild
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public int callbackOrder
        {
            get
            {
                return -1000;
            }
        }

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            if (AOTGenerationConfig.Instance.AutomateBeforeBuilds
                && AOTGenerationConfig.Instance.AutomateForPlatforms != null
                && AOTGenerationConfig.Instance.AutomateForPlatforms.Contains(target))
            {
                AOTGenerationConfig.Instance.ScanProject();
                AOTGenerationConfig.Instance.GenerateDLL();
            }
        }

#if UNITY_2018_1_OR_NEWER

        public void OnPreprocessBuild(BuildReport report)
        {
            this.OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
        }

#endif
    }

#pragma warning disable CS0618 // Type or member is obsolete
    public class PostBuildAOTAutomation : IPostprocessBuild
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public int callbackOrder
        {
            get
            {
                return -1000;
            }
        }

        public void OnPostprocessBuild(BuildTarget target, string path)
        {
            if (AOTGenerationConfig.Instance.AutomateBeforeBuilds
                && AOTGenerationConfig.Instance.AutomateForPlatforms != null
                && AOTGenerationConfig.Instance.AutomateForPlatforms.Contains(target)
                && AOTGenerationConfig.Instance.DeleteDllAfterBuilds)
            {
                Directory.Delete(AOTGenerationConfig.Instance.AOTFolderPath, true);
                File.Delete(AOTGenerationConfig.Instance.AOTFolderPath.TrimEnd('/', '\\') + ".meta");
                AssetDatabase.Refresh();
            }
        }

#if UNITY_2018_1_OR_NEWER

        public void OnPostprocessBuild(BuildReport report)
        {
            this.OnPostprocessBuild(report.summary.platform, report.summary.outputPath);
        }

#endif
    }
}

#endif