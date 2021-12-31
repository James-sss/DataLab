using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.ViewModels.Data
{
    public class AddDataVM : IValidatableObject
    {
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        [Display(Name = "Datetime")]
        public DateTime? Datetime { get; set; }

        [Required]
        [Display(Name = "SensorType")]
        public int SensorTypeId { get; set; }

        [Required]
        [Display(Name = "Value")]
        public decimal Value { get; set; }

        [NotMapped]
        public string CustomerName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SensorTypeId.ToString().Contains("1") && Value >= 101)
            {
                yield return new ValidationResult("Temperature value must be between -50 and 100 degrees", new[] { "Value" });
            }
            if (SensorTypeId.ToString().Contains("1") && Value <= -51)
            {
                yield return new ValidationResult("Temperature value must be between -50 and 100 degrees", new[] { "Value" });
            }

            if (SensorTypeId.ToString().Contains("2") && Value >= 501)
            {
                yield return new ValidationResult("RainFall value must be between 0 and 500", new[] { "Value" });
            }
            if (SensorTypeId.ToString().Contains("2") && Value <= 0)
            {
                yield return new ValidationResult("RainFall value must be between 0 and 500", new[] { "Value" });
            }

            if (SensorTypeId.ToString().Contains("3") && Value >= 15)
            {
                yield return new ValidationResult("pH value must be between 0 and 14", new[] { "Value" });
            }
            if (SensorTypeId.ToString().Contains("3") && Value <= 0)
            {
                yield return new ValidationResult("pH value must be between 0 and 14", new[] { "Value" });
            }

            if (!new string[] { "1", "2", "3" }.Any(s => SensorTypeId.ToString().Contains(s)) && Value >= 601)
            {
                yield return new ValidationResult("Default sensor value must be between 0 and 600", new[] { "Value" });
            }
            if (!new string[] { "1", "2", "3" }.Any(s => SensorTypeId.ToString().Contains(s)) && Value <= 0)
            {
                yield return new ValidationResult("Default sensor value must be between 0 and 600", new[] { "Value" });
            }
        }
    }
}
