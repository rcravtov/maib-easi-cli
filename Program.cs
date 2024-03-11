namespace maib_easi_cli;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;


class Program

{

    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: input certificate password output");
            Environment.Exit(1);
        }
        string input = File.ReadAllText(args[0]);
        string hash = GetHash(input, args[1], args[2]);
        File.WriteAllText(args[3], hash);
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

}