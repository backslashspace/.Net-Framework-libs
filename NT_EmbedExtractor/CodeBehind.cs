using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BSS.EmbedExtractor
{
    public static partial class xExtractor
    {
        private static (String path, String fileName) ValidatingPathSplit(ref String filePath)
        {
            String path = "";
            String fileName = "";

            String[] pathParts = filePath.Split('\\');

            if (pathParts.Length < 2)
            {
                throw new ArgumentException("input was not a filePath?");
            }

            for (UInt16 i = 0; i < pathParts.Length - 1; ++i)
            {
                if (i < pathParts.Length - 2)
                {
                    path += pathParts[i] + "\\";
                }
                else
                {
                    path += pathParts[i];
                }
            }

            fileName = pathParts[pathParts.Length - 1];

            return (path, fileName);
        }

        private static void ValidateResource(ref String assemblyResourcePath, ref Assembly assembly)
        {
            String[] resourceNames = GetResourceNames(ref assembly);

            for (UInt32 i = 0; i < resourceNames.Length; ++i)
            {
                if (resourceNames[i] == assemblyResourcePath)
                {
                    return;
                }
            }

            throw new InvalidDataException("resource not found in assembly");
        }

        private static Boolean GetTopLevelAssemblyNamespace(ref Assembly assembly, out String TLNamespace)
        {
            try
            {
                String _namespace = assembly.DefinedTypes.First().FullName;

                TLNamespace = _namespace.Split('.')[0];

                return true;
            }
            catch
            {
                TLNamespace = null;
                return false;
            }
        }

        private static String[] GetResourceNames(ref Assembly assembly, Boolean stripeTopLevelNamespace = false)
        {
            String[] names = assembly.GetManifestResourceNames();

            if (stripeTopLevelNamespace)
            {
                for (UInt32 i = 0; i < names.Length; i++)
                {
                    names[i] = names[i].Split(new Char[] { '.' }, 2, StringSplitOptions.None)[1];
                }
            }

            return names;
        }
    }
}