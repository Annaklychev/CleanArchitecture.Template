using Toolbelt.Blazor.I18nText.Interfaces;

namespace CleanArchitecture.WASM.Resources;

public class en : I18nTextFallbackLanguage
{
    public Errors errors { get; set; } = new();
    public Auth auth { get; set; } = new();
    public Buttons buttons { get; set; } = new();
    public Common common { get; set; } = new();

    public string FallBackLanguage => "en";
}


