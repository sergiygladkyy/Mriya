///////////////////////////////////////////////////////////////////////////////
// 
// Implements SPProfilePhotoUploader class which is deigned to update (upload) 
// profile photo. Many thanks to Peter Holpar
// http://pholpar.wordpress.com/2010/03/10/how-to-upload-a-user-profile-photo-programmatically/
// his code just help us to save many many days of efforts
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Portal;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;

namespace PBMangePhoto
{
    /// <summary>
    /// Uploads image file and thumbnails on SharePoint site
    /// See http://pholpar.wordpress.com/2010/03/10/how-to-upload-a-user-profile-photo-programmatically/
    /// for technical details
    /// </summary>
    public class SPProfilePhotoUploader
    {
        SPFolder subfolderForPictures = null;
        private ProfileImagePicker profileImagePicker = new ProfileImagePicker();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="web">SharePoint service connection</param>
        public SPProfilePhotoUploader(SPWeb web)
        {
            InitializeProfileImagePicker(web);
            subfolderForPictures = GetSubfolderForPictures();
        }

        /// <summary>
        /// Uploads image to the specified UserProfile profile
        /// </summary>
        /// <param name="p">SharePoint profile</param>
        /// <param name="file">byte array which is containing image</param>
        public void UploadPhoto(UserProfile p, byte[] file)
        {
            string fileNameWithoutExtension = GetFileNameFromAccountName(p["AccountName"].Value.ToString());

            if (subfolderForPictures != null)
            {
                // If original image is smaller (smaller side) than specified thumbnail size (which is square) image won't be created and
                // no error message will be shown
                int largeThumbnailSize = 0x90;
                int mediumThumbnailSize = 0x60;
                int smallThumbnailSize = 0x20;

                using (MemoryStream stream = new MemoryStream(file))
                {
                    using (Bitmap bitmap = new Bitmap(stream, true))
                    {
                        if (bitmap.Width < largeThumbnailSize || bitmap.Height < largeThumbnailSize)
                        {
                            if (bitmap.Width < bitmap.Height)
                                largeThumbnailSize = bitmap.Width;
                            else
                                largeThumbnailSize = bitmap.Height;
                        }
                        if (bitmap.Width < mediumThumbnailSize || bitmap.Height < mediumThumbnailSize)
                        {
                            if (bitmap.Width < bitmap.Height)
                                mediumThumbnailSize = bitmap.Width;
                            else
                                mediumThumbnailSize = bitmap.Height;
                        }
                        if (bitmap.Width < smallThumbnailSize || bitmap.Height < smallThumbnailSize)
                        {
                            if (bitmap.Width < bitmap.Height)
                                smallThumbnailSize = bitmap.Width;
                            else
                                smallThumbnailSize = bitmap.Height;
                        }
                        CreateThumbnail(bitmap, smallThumbnailSize, smallThumbnailSize, subfolderForPictures, fileNameWithoutExtension + "_SThumb.jpg");
                        CreateThumbnail(bitmap, mediumThumbnailSize, mediumThumbnailSize, subfolderForPictures, fileNameWithoutExtension + "_MThumb.jpg");
                        CreateThumbnail(bitmap, largeThumbnailSize, largeThumbnailSize, subfolderForPictures, fileNameWithoutExtension + "_LThumb.jpg");
                    }
                }
            }
        }

        /// <summary>
        /// Translates UserProfile["AccountName"] into file name
        /// </summary>
        /// <param name="profile">SharePoint profile</param>
        /// <returns>File name</returns>
        public string GetFileNameFromAccount(UserProfile profile)
        {
            return GetFileNameFromAccountName(profile["AccountName"].Value.ToString());
        }

        /// <summary>
        /// Returns SharePoint subfolder name which stores profile pictures
        /// </summary>
        /// <returns>Subfolder name</returns>
        public string GetSubfolderName()
        {
            if (subfolderForPictures == null)
                return "";

            return subfolderForPictures.Url;
        }

        /// <summary>
        /// Initialize ImagePicker to upload image to the SharePoint site
        /// </summary>
        /// <param name="web">SPWeb object</param>
        private void InitializeProfileImagePicker(SPWeb web)
        {
            Type profileImagePickerType = typeof(ProfileImagePicker);

            FieldInfo fi_m_objWeb = profileImagePickerType.GetField("m_objWeb", BindingFlags.NonPublic | BindingFlags.Instance);
            fi_m_objWeb.SetValue(profileImagePicker, web);

            MethodInfo mi_LoadPictureLibraryInternal = profileImagePickerType.GetMethod("LoadPictureLibraryInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi_LoadPictureLibraryInternal != null)
            {
                mi_LoadPictureLibraryInternal.Invoke(profileImagePicker, new object[] { });
            }
        }

        /// <summary>
        /// Creates thumbnails from Bitmap image
        /// </summary>
        /// <param name="original">original image Bitmap</param>
        /// <param name="idealWidth">ideal width of thumbnail (normally equal to the height)</param>
        /// <param name="idealHeight">ideal height of thumbnail (normally equal to the width)</param>
        /// <param name="folder">SharePoint folder name to store image thumbnails</param>
        /// <param name="fileName">Image file name</param>
        /// <returns></returns>
        public SPFile CreateThumbnail(Bitmap original, int idealWidth, int idealHeight, SPFolder folder, string fileName)
        {
            SPFile file = null;

            // hack to get the Microsoft.Office.Server.UserProfiles assembly
            Assembly userProfilesAssembly = typeof(UserProfile).Assembly;
            // or assuming you know all the details of the assembly
            // Assembly userProfilesAssembly = Assembly.Load(“Microsoft.Office.Server.UserProfiles, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c”);

            // UserProfilePhotos is internal class,
            // so you cannot get it directly from Visual Studio             
            Type userProfilePhotosType = userProfilesAssembly.GetType("Microsoft.Office.Server.UserProfiles.UserProfilePhotos");

            MethodInfo mi_CreateThumbnail = userProfilePhotosType.GetMethod("CreateThumbnail", BindingFlags.NonPublic | BindingFlags.Static);
            if (mi_CreateThumbnail != null)
            {
                // If account which is used to run this application is not owner of the My Sites collection
                // (Check Central Administration - Application Management - Site Collection Administrators)
                // Invoke will will except with securty exception
                file = (SPFile)mi_CreateThumbnail.Invoke(null, new object[] { original, idealWidth, idealHeight, folder, fileName });
            }

            return file;
        }

        /// <summary>
        /// Returns SharePoint folder object instance which stores profile pictures
        /// </summary>
        /// <returns>SPFolder object instance</returns>
        private SPFolder GetSubfolderForPictures()
        {
            SPFolder folder = null;

            Type profileImagePickerType = typeof(ProfileImagePicker);

            MethodInfo mi_GetSubfolderForPictures = profileImagePickerType.GetMethod("GetSubfolderForPictures", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi_GetSubfolderForPictures != null)
            {
                // If sites refer to the site collection which is no My Sites, Invoke will return NULL
                //folder = (SPFolder)GetSubfolderForPictures(profileImagePicker);
                folder = (SPFolder)mi_GetSubfolderForPictures.Invoke(profileImagePicker, new object[] { });
            }

            return folder;
        }

        private SPFolder GetSubfolderForPictures(ProfileImagePicker profileImagePicker)
        {
            Type profileImagePickerType = typeof(ProfileImagePicker);
            SPList objPicLib = null;
            System.Reflection.FieldInfo mi_objPicLib = profileImagePickerType.GetField("m_objPicLib", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi_objPicLib != null)
            {
                objPicLib = (SPList)mi_objPicLib.GetValue(profileImagePicker);
            }
            return objPicLib.RootFolder;
        }

        /// <summary>
        /// Translates UserProfile AccountName property to the picture file name
        /// </summary>
        /// <param name="accountName">UserProfile["AccountName"] property</param>
        /// <returns>Picture file name (base)</returns>
        private string GetFileNameFromAccountName(string accountName)
        {
            string result = accountName;
            string charsToReplace = @"\/:*?""<>|";
            Array.ForEach(charsToReplace.ToCharArray(), charToReplace => result = result.Replace(charToReplace, '_'));
            return result;
        }
    }
}
