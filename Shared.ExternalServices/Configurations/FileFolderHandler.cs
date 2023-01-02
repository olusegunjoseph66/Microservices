using Shared.ExternalServices.Enums;

namespace Shared.ExternalServices.Configurations
{
    public class FileFolderHandler
    {
        public static string GetFolderName(UploadFolderEnum feature)
        {
            string folderName = feature switch
            {
                UploadFolderEnum.Product => "Products",
                UploadFolderEnum.Wallet => "Wallets",
                _ => "General",
            };
            return folderName;
        }
    }
}
