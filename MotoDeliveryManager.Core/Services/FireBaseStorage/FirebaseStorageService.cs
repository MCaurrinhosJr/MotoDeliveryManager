using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using MotoDeliveryManager.Domain.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MotoDeliveryManager.Domain.Services.FirebaseStorage
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfigurationSection _firebaseConfig;

        public FirebaseStorageService(IConfigurationSection firebaseConfig)
        {
            _firebaseConfig = firebaseConfig;
        }

        public async Task<string> UploadImageAsync(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageData), "Os dados da imagem não podem ser nulos ou vazios.");
            }

            try
            {
                // Obter as configurações do Firebase
                var apiKey = _firebaseConfig["ApiKey"];
                var bucket = _firebaseConfig["Bucket"];
                var authEmail = _firebaseConfig["AuthEmail"];
                var authPassword = _firebaseConfig["AuthPassword"];

                // Autenticar com o Firebase
                var credential = GoogleCredential.FromJson($"{{\"apiKey\":\"{apiKey}\",\"authDomain\":\"{bucket}.firebaseapp.com\",\"projectId\":\"{bucket}\"}}");
                var storage = StorageClient.Create(credential);

                // Upload da imagem
                using (var stream = new MemoryStream(imageData))
                {
                    var objectName = Guid.NewGuid().ToString(); // Nome do objeto no Firebase Storage
                    await storage.UploadObjectAsync(bucket, objectName, null, stream);
                    return $"https://storage.googleapis.com/{bucket}/{objectName}"; // URL do objeto no Firebase Storage
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer upload da imagem para o Firebase Storage.", ex);
            }
        }
    }
}