using System;

namespace IC.DotNet.Interview.Core.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public Guid UserCreatedId { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserLastUpdatedId { get; set; }
        public DateTime LastUpdated { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseModel))
                return false;

            BaseModel model = (BaseModel)obj;
            return Id == model.Id;
        }
    }
}