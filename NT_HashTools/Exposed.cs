using System;
using System.IO;

namespace BSS.HashTools
{
    public static partial class xHash
    {
        public enum Algorithm
        {
            MD5 = 0,
            SHA1 = 1,
            SHA256 = 2,
            SHA384 = 3,
            SHA512 = 4,
            RIPEMD160 = 5
        }

        //

        ///<summary>Returns the Hash-Value of a given file in lower-case</summary>
        ///<param name="filePath">File to check</param>
        ///<param name="algorithm">Valid arguments:<br/><br/>
        ///MD5<br/>
        ///RIPEMD160<br/>
        ///SHA1<br/>
        ///SHA256<br/>
        ///SHA384<br/>
        ///SHA512<br/>
        ///</param>
        ///<returns><see langword="null"/> when file not found</returns>
        ///<exception cref="FileLoadException"></exception>
        public static String GetFileHash(String filePath, Algorithm algorithm = Algorithm.SHA256)
        {
            return ComputeFileHash(ref filePath, ref algorithm);
        }

        ///<summary>Checks if a given file has a specific hash.</summary>
        ///<param name="algorithm">Valid arguments:<br/><br/>
        ///MD5<br/>
        ///RIPEMD160<br/>
        ///SHA1<br/>
        ///SHA256<br/>
        ///SHA384<br/>
        ///SHA512<br/>
        ///</param>
        ///<param name="hash">Hash to check against</param>
        ///<param name="filePath">File to check</param>
        ///<exception cref="FileLoadException"></exception>
        public static Boolean CompareHash(String filePath, String hash, Algorithm algorithm = Algorithm.SHA256)
        {
            String fileHash = ComputeFileHash(ref filePath, ref algorithm) ?? throw new FileLoadException($"Unable to calculate hash of file :{filePath}");

            if (fileHash.Equals(hash, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}