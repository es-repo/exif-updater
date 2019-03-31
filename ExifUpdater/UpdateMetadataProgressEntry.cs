namespace ExifUpdater
{
	public class UpdateMetadataProgressEntry
	{
		public int FileIndex { get; set; }
		public string File { get; set; }
		public ImageMetadataUpdateResult UpdateResult { get; set; }
	}
}
