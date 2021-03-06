using System.Linq;
using Cryptography.ECDSA;
using Shackle.Core.Factories;
using Shackle.Core.Models;

namespace Shackle.Infrastructure.Factories
{
    public class AccountFactory : IAccountFactory
    {
        public Account Create(string name, long balance)
        {
            var (privateKey, publicKey, address) = Create();

            return new Account(name, privateKey, publicKey, address, balance);
        }

        private (PrivateKey PrivateKey, PublicKey PublicKey, Address Address) Create(bool compress = true)
        {
            var privateKeyBytes = Secp256K1Manager.GenerateRandomKey();
            var publicKeyBytes = Secp256K1Manager.GetPublicKey(privateKeyBytes, compress).Skip(1).ToArray();
            var publicKeyHash = Sha256Manager.GetHash(publicKeyBytes);
            var addressBytes = publicKeyHash.Skip(12).ToArray();

            return (new PrivateKey(privateKeyBytes), new PublicKey(publicKeyBytes, compress), new Address(addressBytes));
        }
    }
}