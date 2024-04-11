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
        private readonly IConfiguration _configuration;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadImageAsync(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageData), "Os dados da imagem não podem ser nulos ou vazios.");
            }

            try
            {
                using (var stream = new MemoryStream(imageData))
                {
                    string projectId = _configuration["Firestore:ProjectId"];
                    string credentialsJson = _configuration["Firestore:CredentialsJson"];

                    GoogleCredential credential = GoogleCredential.FromJson(credentialsJson);
                    StorageClient storageClient = StorageClient.Create(credential);

                    string bucketName = $"{projectId}.appspot.com";

                    string imageName = $"{Guid.NewGuid()}.png";
                    string imagePath = $"images/{imageName}";

                    storageClient.UploadObject(bucketName, imagePath, null, stream);

                    return imagePath;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer upload da imagem para o Firebase Storage.", ex);
            }
        }
    }
}