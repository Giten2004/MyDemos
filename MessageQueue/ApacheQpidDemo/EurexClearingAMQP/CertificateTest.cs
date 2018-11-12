using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EurexClearingAMQP
{
    public class CertificateTest
    {
        public void RootTest()
        {
            // 验证根证书签名  
            X509Certificate2 x509Root = new X509Certificate2(@"..\..\..\ApacheQpidClient\certificates\LiquidCapital\LCMLO_LIQSPALBB.crt");
            Console.WriteLine("Root Certificate Verified?: {0}{1}", x509Root.Verify(), Environment.NewLine);  // 根证书是自签名，所以可以通过。  
        }

        public void SubTest()
        {
            X509Certificate2 x509 = new X509Certificate2(@"C:\Users\bxu.CHINA\Desktop\HYD-801\1234\cert\ABCFR_ABCFRALMMACC1.crt");
            //X509Certificate2 x509 = new X509Certificate2(@"..\..\..\ApacheQpidClient\certificates\LiquidCapital\LCMLO_ABCFRALMMACC1.crt");

            byte[] rawdata = x509.RawData;
            Console.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
            Console.WriteLine("Friendly Name: {0}{1}", x509.FriendlyName, Environment.NewLine);
            Console.WriteLine("Certificate Verified?: {0}{1}", x509.Verify(), Environment.NewLine);
            Console.WriteLine("Simple Name: {0}{1}", x509.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
            Console.WriteLine("Signature Algorithm: {0}{1}", x509.SignatureAlgorithm.FriendlyName, Environment.NewLine);
            //    Console.WriteLine("Private Key: {0}{1}", x509.PrivateKey.ToXmlString(false), Environment.NewLine);  // cer里面并没有私钥信息
            Console.WriteLine("Public Key: {0}{1}", x509.PublicKey.Key.ToXmlString(false), Environment.NewLine);
            Console.WriteLine("Certificate Archived?: {0}{1}", x509.Archived, Environment.NewLine);
            Console.WriteLine("Length of Raw Data: {0}{1}", x509.RawData.Length, Environment.NewLine);

            Console.WriteLine("SubjectName: {0}{1}", x509.SubjectName, Environment.NewLine);
            Console.WriteLine("Subject: {0}{1}", x509.Subject, Environment.NewLine);

        }

        public void Verify()
        {
            X509Certificate2 x509 = new X509Certificate2(@"..\..\..\ApacheQpidClient\certificates\LiquidCapital\LCMLO_ABCFRALMMACC1.crt");
            //Output chain information of the selected certificate.
            X509Chain ch = new X509Chain();
            ch.Build(x509);
            Console.WriteLine("Chain Information");
            ch.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            Console.WriteLine("Chain revocation flag: {0}", ch.ChainPolicy.RevocationFlag);
            Console.WriteLine("Chain revocation mode: {0}", ch.ChainPolicy.RevocationMode);
            Console.WriteLine("Chain verification flag: {0}", ch.ChainPolicy.VerificationFlags);
            Console.WriteLine("Chain verification time: {0}", ch.ChainPolicy.VerificationTime);
            Console.WriteLine("Chain status length: {0}", ch.ChainStatus.Length);
            Console.WriteLine("Chain application policy count: {0}", ch.ChainPolicy.ApplicationPolicy.Count);
            Console.WriteLine("Chain certificate policy count: {0} {1}", ch.ChainPolicy.CertificatePolicy.Count, Environment.NewLine);
            //Output chain element information.
            Console.WriteLine("Chain Element Information");
            Console.WriteLine("Number of chain elements: {0}", ch.ChainElements.Count);
            Console.WriteLine("Chain elements synchronized? {0} {1}", ch.ChainElements.IsSynchronized, Environment.NewLine);

            //    int index = 0;
            foreach (X509ChainElement element in ch.ChainElements)
            {
                Console.WriteLine("Element subject name: {0}", element.Certificate.Subject);
                Console.WriteLine("Element issuer name: {0}", element.Certificate.Issuer);
                Console.WriteLine("Element certificate valid until: {0}", element.Certificate.NotAfter);
                Console.WriteLine("Element certificate is valid: {0}", element.Certificate.Verify());
                Console.WriteLine("Element error status length: {0}", element.ChainElementStatus.Length);
                Console.WriteLine("Element information: {0}", element.Information);
                Console.WriteLine("Number of element extensions: {0}{1}", element.Certificate.Extensions.Count, Environment.NewLine);

                string a = element.Certificate.Thumbprint;
                //     string b = ch.ChainPolicy.ExtraStore[0].Thumbprint;
                //ch.ChainPolicy.ExtraStore[index - 1].Thumbprint;

                if (ch.ChainStatus.Length > 1)
                {
                    for (int index = 0; index < element.ChainElementStatus.Length; index++)
                    {
                        Console.WriteLine(element.ChainElementStatus[index].Status);
                        Console.WriteLine(element.ChainElementStatus[index].StatusInformation);
                    }
                }
            }
        }
    }
}
