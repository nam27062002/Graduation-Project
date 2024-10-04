using System.IO;

namespace Alkawa.Core
{
    public static class FileSystemInfoExtensions
    {
        public static void DeleteReadOnly(this FileSystemInfo fileSystemInfo)
        {
            if (!fileSystemInfo.Exists)
                return;

            //Handle directory case
            var directoryInfo = fileSystemInfo as DirectoryInfo;
            if (directoryInfo != null)
            {
                foreach (FileSystemInfo childInfo in directoryInfo.GetFileSystemInfos())
                {
                    childInfo.DeleteReadOnly();
                }
            }

            fileSystemInfo.Attributes = FileAttributes.Normal;
            fileSystemInfo.Delete();
        }

        public static void RemoveReadOnlyFlag(this FileSystemInfo fileSystemInfo)
        {
            if (!fileSystemInfo.Exists)
                return;

            //Handle directory case
            var directoryInfo = fileSystemInfo as DirectoryInfo;
            if (directoryInfo != null)
            {
                foreach (FileSystemInfo childInfo in directoryInfo.GetFileSystemInfos())
                {
                    childInfo.RemoveReadOnlyFlag();
                }
            }

            fileSystemInfo.Attributes &= ~FileAttributes.ReadOnly;
        }
    }
}