using WorldTravel.Enums;

namespace WorldTravel.Dtos.Files.ViewModels
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
    }
}
