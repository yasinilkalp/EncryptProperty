using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EncryptProperty.Attribute;
using System;
using System.Linq;
using EncryptProperty.Provider;
using EncryptProperty.Converter;
using EncryptProperty.Config;

namespace EncryptProperty.Extension
{
    public static class BuilderExtension
    {
        public static void UseEncryption(this ModelBuilder modelBuilder, IEncryptionProvider encryptionProvider)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder), "There is not ModelBuilder object.");
            if (encryptionProvider is null)
                throw new ArgumentNullException(nameof(encryptionProvider), "You should create encryption provider.");

            var encryptionConverter = new EncryptionConverter(encryptionProvider);
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && !IsDiscriminator(property))
                    {
                        object[] attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptPropertyAttribute), false);
                        if (attributes.Any())
                            property.SetValueConverter(encryptionConverter);
                    }
                }
            }

        }

        private static bool IsDiscriminator(IMutableProperty property) => property.Name == "Discriminator" || property.PropertyInfo == null;


        public static DbContextOptionsBuilder UseEncryption(this DbContextOptionsBuilder optionsBuilder, string privateKey, bool isEncrypted)
        {
            EncryptionConfig.SetConfig(privateKey, isEncrypted);
            return optionsBuilder;
        }
    }
}
