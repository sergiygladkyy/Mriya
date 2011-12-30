using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MriyaGallery
{
    public class GalleryItem
    {
        private string m_Thumbnail = "";
        private string m_Image = "";

        public string Thumbnail 
        { 
            get
            {
                if (m_Thumbnail.Length < 1 && m_Image.Length > 0)
                    return m_Image;
                return m_Thumbnail;
            }
            set
            {
                m_Thumbnail = value.Trim();
            }
        }

        public string Image
        {
            get
            {
                if (m_Image.Length < 1 && m_Thumbnail.Length > 0)
                    return m_Thumbnail;
                return m_Image;
            }
            set
            {
                m_Image = value.Trim();
            }
        }

        public string Video { get; set; }

        public GalleryItem()
        {
            Thumbnail = "";
            Image = "";
            Video = "";
        }

        public GalleryItem(string image)
        {
            this.Image = image;
        }

        public GalleryItem(string image, string thumbnail)
        {
            this.Image = image;
            this.Thumbnail = thumbnail;
        }

        public GalleryItem(string image, string thumbnail, string video)
        {
            this.Image = image;
            this.Thumbnail = thumbnail;
            this.Video = video;
        }

    }
}
