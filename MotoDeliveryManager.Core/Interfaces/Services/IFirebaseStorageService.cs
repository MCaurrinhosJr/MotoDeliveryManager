using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadImageAsync(byte[] imageData);
    }
}
