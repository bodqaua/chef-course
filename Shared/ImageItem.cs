using Chef.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Shared
{
    public class ImageItem
    {
        public string ListId { get; set; }
        public string Value { get; set; }

        public int Id { get; set; }

        public static ImageItem CreateImageItem(string value)
        {
            return new ImageItem
            {
                ListId = Guid.NewGuid().ToString(),
                Value = value
            };
        }

        public static ImageItem CreateImageItemWithId(int id, string value)
        {
            return new ImageItem
            {
                Id = id,
                ListId = Guid.NewGuid().ToString(),
                Value = value
            };
        }

        public static List<DbImage> PrepareImages(List<string> images)
        {
            List<DbImage> preparedImages = new List<DbImage>();

            for (int i = 0; i < images.Count; i++)
            {
                preparedImages.Add(new DbImage
                {
                    Id = i,
                    Content = images[i]
                });
            }

            return preparedImages;
        }

        public static List<DbImage> PrepareImagesWithId(List<ImageItem> images)
        {
            List<DbImage> preparedImages = new List<DbImage>();

            for (int i = 0; i < images.Count; i++)
            {
                preparedImages.Add(new DbImage
                {
                    Id = images[i].Id,
                    Content = images[i].Value
                });
            }

            return preparedImages;
        }
    }

}
