namespace maib_easi_cli;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;


class Program

{

    static void Main(string[] args)
    {

        if (args.Length == 5 && args[0] == "hash")
        {

            string input = File.ReadAllText(args[1]);
            string hash = GetHash(input, args[2], args[3]);
            File.WriteAllText(args[4], hash);
            Environment.Exit(0);
        }

        if (args.Length == 3 && args[0] == "base64")
        {
            string input = File.ReadAllText(args[1]);
            string inputBase64 = GetBase64(input);
            File.WriteAllText(args[2], inputBase64);
            Environment.Exit(0);
        }

        PrintUsageAndExit();
    }

    static void PrintUsageAndExit()
    {
        Console.WriteLine("Usage: hash <input> <certificate> <password> <output>");
        Console.WriteLine("Usage: base64 <input> <output>");
        Environment.Exit(1);
    }

    static string GetHash(string data, string certFilePath, string certPass)
    {
        System.Security.Cryptography.X509Certificates.X509Certificate2 certificate =
            new X509Certificate2(certFilePath, certPass, X509KeyStorageFlags.MachineKeySet);

        System.IO.MemoryStream stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(data));
        byte[] messageBytes = stream.ToArray();
        System.Security.Cryptography.Pkcs.ContentInfo contentInfo = new ContentInfo(messageBytes);
        System.Security.Cryptography.Pkcs.SignedCms signedCms = new SignedCms(contentInfo, true);
        System.Security.Cryptography.Pkcs.CmsSigner cmsSigner = new CmsSigner(certificate);
        cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;
        signedCms.ComputeSignature(cmsSigner);
        byte[] signedData = signedCms.Encode();

        return Convert.ToBase64String(signedData);
    }

    static string GetBase64(string message)
    {
        System.IO.MemoryStream stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(message));
        byte[] messageBytes = stream.ToArray();
        return Convert.ToBase64String(messageBytes);
    }

}