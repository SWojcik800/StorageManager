using Dapper;
using StorageManager.App.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageManager.App.Features.Storage
{
    public sealed class StorageItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string AddedBy { get; private set; }
        public DateTime AddDate { get; private set; }
        public Image? DisplayImage
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                    return null;

                byte[] bytes = Convert.FromBase64String(Image);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                    return image;
                }
            }
        }

        public static List<StorageItem> ReadItems()
        {
            using (var connect = DbConnectionFactory.Create())
            {
                var result = connect.Query<StorageItem>("SELECT * FROM [dbo].[StorageItems] ORDER BY AddDate DESC");
                return result.ToList();
            }
        }

        public void Delete()
        {
            using (var connection = DbConnectionFactory.Create())
            {
                connection.Execute("DELETE FROM [dbo].[StorageItems] WHERE Id = @Id", new
                {
                    Id = Id
                });
            }

        }
        public StorageItem Read(int id)
        {
            using (var connect = DbConnectionFactory.Create())
            {
                var sql = @"SELECT * FROM [dbo].[StorageItems] WHERE Id = @Id";
                var result = connect.QueryFirst<StorageItem>(sql, new
                {
                    Id = id
                });

                Id = result.Id;
                Name = result.Name;
                Description = result.Description;
                Category = result.Category;
                Image = result.Image;
                AddedBy = result.AddedBy;
                AddDate = result.AddDate;

                return result;
            }
        }
        public void Save()
        {
            using (var connection = DbConnectionFactory.Create())
            {
                if (Id == 0)
                {
                    var sql = @"
                    INSERT INTO [dbo].[StorageItems]
                   ([Name]
                   ,[Description]
                   ,[Category]
                   ,[Image]
                   ,[AddedBy]
                   ,[AddDate])
                    OUTPUT INSERTED.Id
                     VALUES
                           (@Name
                           ,@Description
                           ,@Category
                           ,@Image
                           ,SUSER_NAME(),
                           getdate())
                        ";

                    var id = connection.QuerySingle<int>(sql, this);

                    if (id > 0)
                        Id = id;
                }
                else
                {
                    var sql = @"
                    UPDATE [dbo].[StorageItems]
                       SET [Name] = @Name,
                           [Description] = @Description,
                           [Category] = @Category,
                           [Image] = @Image
                     WHERE Id = @Id";

                    connection.Execute(sql, this);
                }

            }
        }
        public void ResetToDefault()
        {
            Id = 0;
            Name = "";
            Description = "";
            Category = "";
            Image = "";
            AddedBy = "";
            AddDate = default;
        }
    }
}
