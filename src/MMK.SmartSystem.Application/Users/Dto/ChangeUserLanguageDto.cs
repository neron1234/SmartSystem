using System.ComponentModel.DataAnnotations;

namespace MMK.SmartSystem.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}