using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Api.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("CourseID")]
        [JsonIgnore]
        public string? CourseID { get; set; } 

        [Required]
        //[MinLength(20)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        //[MinLength(100)]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        [Required]
        public string Technology { get; set; }

        [Required]
        public string LaunchUrl { get; set; }
    }
}
