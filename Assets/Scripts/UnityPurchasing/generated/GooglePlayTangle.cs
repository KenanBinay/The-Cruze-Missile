// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Tofr07KarL9ytpu7ViT6qLF/X4oC6NBzieL7S61X61a0znJbgyHh04O8arg0F3vGZt+kXmmKlYVSTqwJGuOTBGulWnC0spuRVaJUcM/vz2/KSUdIeMpJQkrKSUlIxoLbrfh3fd4ajZqSZUp/QscmuFE2VdErs2VWzptgApv3CP10v8sXcn6psVbF0cEQKqhNyqmKkL4ea14Q0046Lx2bUk7/R+wLadIL7z98d/dhWqh+tnxVi3Jejvmcdx2fkArjz1lAQmS5v+GuzpGQcCtyOfXSN+YheDNcNlyfLHjKSWp4RU5BYs4Azr9FSUlJTUhLRcwJAo569per36Pbf6/9NgjckocPLvfxyw/F5I5L40syqwWzmS7QGWlPn3h8fKNjlUpLSUhJ");
        private static int[] order = new int[] { 2,7,11,5,7,12,12,10,9,12,12,11,13,13,14 };
        private static int key = 72;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
