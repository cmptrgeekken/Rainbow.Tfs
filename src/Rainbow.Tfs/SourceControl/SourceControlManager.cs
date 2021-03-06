using System;
using System.Web;
using Sitecore.Configuration;

namespace Rainbow.Tfs.SourceControl
{
	public class SourceControlManager : ISourceControlManager
	{
		public ISourceControlSync SourceControlSync { get; private set; }
		public bool AllowFileSystemClear { get { return SourceControlSync.AllowFileSystemClear; } }

		private string _username;
		private string _password;
		private string _domain;
		private string _workspacePath;
		private bool? _useImpersonation;

		private const string UsernameKey = "Rainbow.Tfs.Login";
		private const string PasswordKey = "Rainbow.Tfs.Password";
		private const string DomainKey = "Rainbow.Tfs.Domain";
		private const string WorkspacePathKey = "Rainbow.Tfs.WorkspacePath";
		private const string UseImpersonationKey = "Rainbow.Tfs.UseImpersonationCredentials";

		protected string Username
		{
			get
			{
				if (!string.IsNullOrEmpty(_username)) return _username;

				var configSetting = Settings.GetSetting(UsernameKey);
				_username = configSetting;

				return _username;
			}
		}

		protected string Password
		{
			get
			{
				if (!string.IsNullOrEmpty(_password)) return _password;

				var configSetting = Settings.GetSetting(PasswordKey);
				_password = configSetting;

				return _password;
			}
		}

		protected string Domain
		{
			get
			{
				if (!string.IsNullOrEmpty(_domain)) return _domain;

				var configSetting = Settings.GetSetting(DomainKey);
				_domain = configSetting;

				return _domain;
			}
		}

		protected string WorkspacePath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_workspacePath)) return _workspacePath;

				var configSetting = Settings.GetSetting(WorkspacePath);
				_workspacePath = configSetting;

				return _workspacePath;
			}
		}

		protected bool UseImpersonation
		{
			get
			{
				if (_useImpersonation.HasValue) return _useImpersonation.Value;

				var configSetting = Settings.GetBoolSetting(UseImpersonationKey, false);
				_useImpersonation = configSetting;

				return configSetting;
			}
		}

		private ScmSettings GetSettings()
		{
			return new ScmSettings()
			{
				Domain = Domain,
				Password = Password,
				Username = Username,
				WorkspacePath = string.IsNullOrWhiteSpace(WorkspacePath)
					? HttpContext.Current.Server.MapPath("/")
					: WorkspacePath,
				UseImpersonation = UseImpersonation
			};
		}

		public SourceControlManager()
		{
			var settings = GetSettings();

			SourceControlSync = new FileSyncTfs(settings);
		}

		public SourceControlManager(ISourceControlSync sourceControlSync)
		{
			SourceControlSync = sourceControlSync;
		}

		public bool FileExistsInSourceControl(string filename)
		{
			return SourceControlSync.FileExistsInSourceControl(filename);
		}

		public bool EditPreProcessing(string filename)
		{
			bool success = SourceControlSync.EditPreProcessing(filename);
			if (success) return true;

			throw new Exception("[Rainbow] Edit pre-processing failed for " + filename);
		}

		public bool EditPostProcessing(string filename)
		{
			bool success = SourceControlSync.EditPostProcessing(filename);
			if (success) return true;

			throw new Exception("[Rainbow] Edit post-processing failed for " + filename);
		}

		public bool DeletePreProcessing(string filename)
		{
			bool success = SourceControlSync.DeletePreProcessing(filename);
			if (success) return true;

			throw new Exception("[Rainbow] Delete pre-processing failed for " + filename);
		}
	}
}