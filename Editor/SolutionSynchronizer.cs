namespace RSLib.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Workaround solution used to fix an issue when Unity does not synchronize the solution with the project.
    /// </summary>
    public static class SolutionSynchronizer
	{
		private static readonly object SYNCHRONIZER_OBJECT;
		private static readonly MethodInfo SYNC_SOLUTION_METHOD_INFO;
		private static readonly MethodInfo SYNCHRONIZER_SYNC_METHOD_INFO;
		
		static SolutionSynchronizer()
		{
			Type syncVSType = Type.GetType("UnityEditor.SyncVS,UnityEditor");
			FieldInfo synchronizerField = syncVSType.GetField("Synchronizer", BindingFlags.NonPublic | BindingFlags.Static);
			SYNC_SOLUTION_METHOD_INFO = syncVSType.GetMethod("SyncSolution", BindingFlags.Public | BindingFlags.Static);
			SYNCHRONIZER_OBJECT = synchronizerField.GetValue(syncVSType);
			SYNCHRONIZER_SYNC_METHOD_INFO = SYNCHRONIZER_OBJECT.GetType().GetMethod("Sync", BindingFlags.Public | BindingFlags.Instance);
		}

		[MenuItem("Assets/Sync C# Solution", priority = 1000000)]
		public static void Sync()
        {
			CleanOldFiles();
			CallSyncSolution();
			CallSynchronizerSync();
		}

		private static void CleanOldFiles()
		{
			DirectoryInfo assetsDirectoryInfo = new DirectoryInfo(Application.dataPath);
			DirectoryInfo projectDirectoryInfo = assetsDirectoryInfo.Parent;

			IEnumerable<FileInfo> files = GetFilesByExtensions(projectDirectoryInfo, "*.sln", "*.csproj");
			foreach (FileInfo file in files)
			{
				Debug.Log($"Removing old solution file: {file.Name}.");
				file.Delete();
			}
		}

		private static void CallSyncSolution()
		{
		    Debug.Log("Call method: SyncVS.Sync()");
			SYNC_SOLUTION_METHOD_INFO.Invoke(null, null);
		}

		private static void CallSynchronizerSync()
		{
			Debug.Log("Call method: SyncVS.Synchronizer.Sync()");
			SYNCHRONIZER_SYNC_METHOD_INFO.Invoke(SYNCHRONIZER_OBJECT, null);
		}

		private static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
		{
			extensions ??= new[] {"*"};
			IEnumerable<FileInfo> files = Enumerable.Empty<FileInfo>();
			foreach (string ext in extensions)
				files = files.Concat(dir.GetFiles(ext));

			return files;
		}
	}
}