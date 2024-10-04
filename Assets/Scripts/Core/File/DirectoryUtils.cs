using System;
using System.IO;

namespace Alkawa.Core
{
    [Flags]
    public enum EFileOperationFlag
    {
        None = 0,
        Recursive = 1 << 1,
        ClearDestination = 1 << 2
    }
    public static class DirectoryUtils
    {
        public static void DeleteDirectory(string path)
        {
            try
            {
                var fileSystemInfo = new DirectoryInfo(path);
                fileSystemInfo.DeleteReadOnly();
            }
            catch
            {

            }
        }

        public static void EmptyDirectory(string _path)
        {
            var directoryInfo = new DirectoryInfo(_path);
            if (directoryInfo.Exists)
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        public static void RemoveReadOnlyFlagOnDirectory(string path)
        {
            try
            {
                var fileSystemInfo = new DirectoryInfo(path);
                fileSystemInfo.RemoveReadOnlyFlag();
            }
            catch
            {

            }
        }

        public static bool TryRenameDirectory(string sourceDirPath,string destinationDirPath)
        {
            try
            {
                if (Exists(sourceDirPath))
                    Directory.Move(sourceDirPath, destinationDirPath);
            }
            catch
            {
                return false;
            }

            return Exists(destinationDirPath);
        }

        public static bool TryCreateDirectory(string _path)
        {
            try
            {
                Directory.CreateDirectory(_path);
            }
            catch
            {

            }

            return Exists(_path);
        }
        
        public static bool Exists(string _path)
        {
            try
            {
                return Directory.Exists(_path);
            }
            catch
            {
                return false;
            }
        }

        public static void CopyDirectory(string _source, string _dest, EFileOperationFlag _flags = EFileOperationFlag.Recursive)
        {
            CopyDirectoryInternal(_source, _dest, _flags);
        }

        private static void CopyDirectoryInternal(string _source, string _dest, EFileOperationFlag _flags)
        {
            var dir = new DirectoryInfo(_source);
            
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            
            DirectoryInfo[] dirs = dir.GetDirectories();
            
            if (_flags.HasFlag(EFileOperationFlag.ClearDestination) && Directory.Exists(_dest))
            {
                Directory.Delete(_dest, true);
            }
            
            if(!Directory.Exists(_dest))
                Directory.CreateDirectory(_dest);
            
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(_dest, file.Name);
                file.CopyTo(targetFilePath);
            }
            
            if (_flags.HasFlag(EFileOperationFlag.Recursive))
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(_dest, subDir.Name);
                    CopyDirectoryInternal(subDir.FullName, newDestinationDir, _flags);
                }
            }
        }

#if UNITY_EDITOR
        public static void CopyDirectoryConditional(string _source, string _dest, Predicate<string> _acceptFileCB, EFileOperationFlag _flags = EFileOperationFlag.Recursive)
        {
            CopyDirectoryInternalConditional(_source, _dest, _acceptFileCB, _flags);
        }

        private static void CopyDirectoryInternalConditional(string _source, string _dest, Predicate<string> _acceptFileCB, EFileOperationFlag _flags)
        {
            var dir = new DirectoryInfo(_source);
            
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            
            DirectoryInfo[] dirs = dir.GetDirectories();
            
            if (_flags.HasFlag(EFileOperationFlag.ClearDestination) && Directory.Exists(_dest))
            {
                Directory.Delete(_dest, true);
            }
            
            if(!Directory.Exists(_dest))
                Directory.CreateDirectory(_dest);
            
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(_dest, file.Name);
                if (_acceptFileCB(targetFilePath))
                    file.CopyTo(targetFilePath);
            }
            
            if (_flags.HasFlag(EFileOperationFlag.Recursive))
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(_dest, subDir.Name);
                    CopyDirectoryInternalConditional(subDir.FullName, newDestinationDir, _acceptFileCB, _flags);
                }
            }
        }
#endif
        
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_ANDROID || UNITY_IOS)//alf
        public static string CleanInvalidCharsFolderName(string folderName)
        {
            if (string.IsNullOrEmpty(folderName)) return folderName;

            foreach (var c in Path.GetInvalidFileNameChars())
                folderName = folderName.Replace(c.ToString(), "_");

            foreach (var c in Path.GetInvalidPathChars())
                folderName = folderName.Replace(c.ToString(), "_");

            return folderName;
        }
#endif
    }
}