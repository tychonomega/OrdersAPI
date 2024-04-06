namespace App.Data.Models
{
    public abstract class BaseDataModel
    {
        /// <summary>
        /// All Base Data Entities should inherit from this class to get common fields across all tables/collections
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
