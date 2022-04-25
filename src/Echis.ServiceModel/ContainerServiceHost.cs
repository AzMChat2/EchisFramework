using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace System.ServiceModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ContainerServiceHost : ServiceHostBase 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implementedContracts"></param>
		/// <returns></returns>
		protected override System.ServiceModel.Description.ServiceDescription CreateDescription(out IDictionary<string, System.ServiceModel.Description.ContractDescription> implementedContracts)
		{
			throw new NotImplementedException();
		}
	}

}
