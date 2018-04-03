using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Kernel.Dal
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Id = Guid.NewGuid();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RowId { get; set; }

        [Index(IsUnique = true)]
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    [Table("EnumTypes")]
    public class EnumTypeModel : BaseModel
    {
        public EnumTypeModel()
        {
        }

        protected EnumTypeModel(string type, ICollection<EnumValueModel> values)
        {
            Type = type;
            Values = values;
        }

        public static EnumTypeModel Create(string type, ICollection<EnumValueModel> values)
        {
            return new EnumTypeModel(type, values);
        }

        public static EnumTypeModel Create(string type)
        {
            return new EnumTypeModel(type, Enumerable.Empty<EnumValueModel>().ToList());
        }

        public string Type { get; private set; }
        public virtual ICollection<EnumValueModel> Values { get; }
    }

    [Table("EnumValues")]
    public class EnumValueModel : BaseModel
    {
        public EnumValueModel()
        {
        }

        protected EnumValueModel(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static EnumValueModel Create(string name, int value)
        {
            return new EnumValueModel(name, value);
        }

        public virtual EnumTypeModel Type { get; set; }
        public string Name { get; private set; }
        public int Value { get; private set; }
    }
}
