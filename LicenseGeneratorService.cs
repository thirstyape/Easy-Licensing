using System;
using System.Security.Cryptography;

namespace Easy_Licensing
{
    public class LicenseGeneratorService
    {
        void Shutup()
        {
            using var dh = new ECDiffieHellmanCng
            {
                KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
                HashAlgorithm = CngAlgorithm.Sha256
            };

            var pk = dh.PublicKey.ToByteArray();

            //dh.DeriveKeyMaterial()
        }
    }
}
