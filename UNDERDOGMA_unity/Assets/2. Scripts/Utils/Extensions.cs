using System.IO;
using System.Threading.Tasks;

public static class Extensions
{
    public static async Task<long> CopyAsync(string FromPath, string ToPath)
    {
        using (var fromStream = new FileStream(FromPath, FileMode.Open))
        {
            long totalCopied = 0;

            using (var toStream = new FileStream(ToPath, FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                int nRead = 0;
                while ((nRead = await fromStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    await toStream.WriteAsync(buffer, 0, nRead);
                    totalCopied += nRead;
                }
            }

            return totalCopied;
        }
    }
}