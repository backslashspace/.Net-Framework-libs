using System;
using System.IO;
using System.Reflection;

namespace BSS.EmbedExtractor
{
    ///<summary>Extracts embedded files.</summary>
    public static partial class xExtractor
    {
        public static void SaveResourceToDisk(String filePath, String assemblyResourcePath, Assembly assembly, Boolean useAbsoluteResourcePath = false)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath was null");
            }

            if (assemblyResourcePath == null)
            {
                throw new ArgumentNullException("assemblyResourcePath was null");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly was null");
            }

            (String path, String fileName) = ValidatingPathSplit(ref filePath);

            if (!useAbsoluteResourcePath)
            {
                GetTopLevelAssemblyNamespace(ref assembly,out String _namespace);

                assemblyResourcePath = $"{_namespace}.{assemblyResourcePath}";
            }

            ValidateResource(ref assemblyResourcePath, ref assembly);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using FileStream fileStream = File.Create($"{path}\\{fileName}");

            assembly.GetManifestResourceStream(assemblyResourcePath).CopyTo(fileStream);
        }

        public static String[] GetResourceNames(Assembly assembly, Boolean stripeTopLevelNamespace = false)
        {
            return GetResourceNames(ref assembly, stripeTopLevelNamespace);
        }
    }
}