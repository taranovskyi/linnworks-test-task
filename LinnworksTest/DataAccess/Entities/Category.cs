using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinnworksTest.DataAccess
{
    public class Category: IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        protected bool Equals(Category other)
        {
            return Id.Equals(other.Id) && CategoryName == other.CategoryName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Category) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (CategoryName != null ? CategoryName.GetHashCode() : 0);
            }
        }

        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
