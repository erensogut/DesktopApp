using System;
namespace DesktopApp.Model
{
	public class FileModel
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public string FileContent { get; set; }
		public int FileHash { get; set; }
		public DateTime FileCreatedTime { get; set; }
	}
}

