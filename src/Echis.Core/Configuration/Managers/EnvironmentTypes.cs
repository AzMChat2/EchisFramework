
using System.Diagnostics.CodeAnalysis;
namespace System.Configuration.Managers
{
	/// <summary>
	/// Represents the types of environments for an application.
	/// </summary>
	public enum EnvironmentTypes
	{
		/// <summary>
		/// Environment used for Production
		/// </summary>
		Production,
		/// <summary>
		/// Second or Backup Environment used for Production
		/// </summary>
		ProductionBackup,
		/// <summary>
		/// Environment used for Customer Acceptance Testing
		/// </summary>
		CustomerAcceptanceTesting,
		/// <summary>
		/// Second or Backup Environment used for Customer Acceptance Testing
		/// </summary>
		CustomerAcceptanceBackup,
		/// <summary>
		/// Environment used for User Acceptance Testing
		/// </summary>
		UserAcceptanceTesting,
		/// <summary>
		/// Second or Backup Environment used for User Acceptance Testing
		/// </summary>
		UserAcceptanceBackup,
		/// <summary>
		/// Environment used for Quality Assurance Testing
		/// </summary>
		QualityAssuranceTesting,
		/// <summary>
		/// Second or Backup Environment used for Quality Assurance Testing
		/// </summary>
		QualityAssuranceBackup,
		/// <summary>
		/// Environment used for Integration Testing
		/// </summary>
		IntegrationTesting,
		/// <summary>
		/// Second or Backup Environment used for Integration Testing
		/// </summary>
		IntegrationBackup,
		/// <summary>
		/// Environment used for Development
		/// </summary>
		Development,
		/// <summary>
		/// Second or Backup Environment used for Development
		/// </summary>
		DevelopmentBackup,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Alpha,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Bravo,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Charlie,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Delta,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Echo,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Foxtrot,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Golf,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Hotel,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Indio,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Juliet,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Kilo,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Lima,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Mike,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		November,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Oscar,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Papa,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Quebec,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Romeo,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Sierra,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Tango,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Uniform,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Victor,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Whiskey,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
			Justification = "XRay (exray) is intentionally misspelled to represent the letter X in the alphabet.")]
		Xray,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Yankee,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Zulu,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Zero,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		One,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Two,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Three,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Four,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Five,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Six,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Seven,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Eight,
		/// <summary>
		/// Other or user defined environment.
		/// </summary>
		Nine
	}
}
