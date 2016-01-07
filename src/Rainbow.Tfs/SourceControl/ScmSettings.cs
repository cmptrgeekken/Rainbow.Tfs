namespace Rainbow.Tfs.SourceControl
{
	public class ScmSettings
	{
		public string Username { get; set; }
		public string Domain { get; set; }
		public string Password { get; set; }
		public string WorkspacePath { get; set; }
	    public bool UseImpersonation { get; set; }
	}
}