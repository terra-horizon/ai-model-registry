using Cite.Tools.FieldSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terra.AiModelRegistry.App.Service.Version
{
	public interface IVersionInfoService
	{
		Task<List<Model.VersionInfo>> CurrentAsync(IFieldSet fields = null);
	}
}
