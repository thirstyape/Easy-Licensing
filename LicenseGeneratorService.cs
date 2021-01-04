using System.Security.Cryptography;

namespace Easy_Licensing
{
    public class LicenseGeneratorService
    {
        public void GenerateLicense()
        {

        }

        public void GenerateLicenseRequest()
        {

        }

        public void GenerateEncryptionKeys()
        {

        }

        void Temp()
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
