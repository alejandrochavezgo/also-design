using entities.enums;

namespace entities.models;

public class enterpriseModel
{
    public int id { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public defaultValuesModel? defaultValues { get; set; }
}